using System;
using System.IO;
using System.Net;

namespace DimitriVranken.PanoramaCreator
{
    /// <summary>
    /// A LevelOne FCS-1060 or WCS-2060 network camera.
    /// </summary>
    class Camera
    {
        /// <summary>
        /// The ip address of the camera.
        /// </summary>
        public IPAddress IpAddress { get; private set; }

        /// <summary>
        /// The web proxy to use for accessing the camera.
        /// </summary>
        public WebProxy Proxy { get; private set; }


        /// <summary>
        /// Creates a new instance of the <see cref="Camera"/> class.
        /// </summary>
        /// <param name="ipAddress">The ip address of the camera.</param>
        public Camera(IPAddress ipAddress)
        {
            if (ipAddress == null)
            {
                throw new ArgumentNullException("ipAddress");
            }

            IpAddress = ipAddress;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Camera"/> class.
        /// </summary>
        /// <param name="ipAddress">The ip address of the camera.</param>
        /// <param name="proxy">The web proxy to use for accessing the camera.</param>
        public Camera(IPAddress ipAddress, WebProxy proxy)
            : this(ipAddress)
        {
            Proxy = proxy;
        }


        private HttpWebResponse ExecuteCommand(string commandUrl, int waitTime)
        {
            if (string.IsNullOrEmpty(commandUrl))
            {
                throw new ArgumentNullException("commandUrl");
            }
            if (waitTime < 0 || waitTime > 1000 * 60 * 5)
            {
                throw new ArgumentOutOfRangeException("waitTime");
            }

            var requestUrl = String.Format("http://{0}/cgi-bin/{1}", IpAddress, commandUrl);

            Logger.Default.Trace("Camera: Executing request '{0}'", requestUrl);
            var request = WebRequest.Create(requestUrl);
            request.Timeout = 20 * 1000;
            if (Proxy != null)
            {
                request.Proxy = Proxy;
            }

            try
            {

                var response = (HttpWebResponse)request.GetResponse();
                Logger.Default.Trace("Camera: Response received (HTTP {0})", response.StatusCode);

                System.Threading.Thread.Sleep(waitTime);
                return response;
            }
            catch (WebException exception)
            {
                if (exception.Status == WebExceptionStatus.ConnectFailure)
                {
                    throw new Exception("Unable to connect to the network camera.", exception);
                }

                throw;
            }
        }

        private bool ExecuteCommand(string commandUrl, FileInfo destinationFile)
        {
            if (string.IsNullOrEmpty(commandUrl))
            {
                throw new ArgumentNullException("commandUrl");
            }
            if (destinationFile == null)
            {
                throw new ArgumentNullException("destinationFile");
            }

            using (var response = ExecuteCommand(commandUrl, 2 * 1000))
            {
                // Check if the response is valid
                if (response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Moved ||
                    response.StatusCode == HttpStatusCode.Redirect)
                {
                    // Download the response to a file
                    using (var inputStream = response.GetResponseStream())
                    using (var outputStream = File.OpenWrite(destinationFile.FullName))
                    {
                        var buffer = new byte[4096];
                        int bytesRead;

                        do
                        {
                            bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                            outputStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead != 0);
                        Logger.Default.Trace("Camera: Downloaded response of command '{0}'", commandUrl);
                    }

                    // The file was downloaded successfully
                    return true;
                }
            }

            // The file couldn't be downloaded
            Logger.Default.Error("Camera: Couldn't download response of command '{0}'", commandUrl);
            return false;
        }


        /// <summary>
        /// Takes an image using the camera and saves it to the specified file.
        /// </summary>
        /// <param name="destinationFile">The file to save the image to. The parent folder has to exist.</param>
        public void CaptureImage(FileInfo destinationFile)
        {
            if (destinationFile == null)
            {
                throw new ArgumentNullException("destinationFile");
            }

            // Build command URL
            const string commandUrl = "video.jpg";

            // Execute command
            Logger.UserInterface.Debug("Camera: Taking an image");
            if (!ExecuteCommand(commandUrl, destinationFile))
            {
                throw new Exception("Camera: Failed to take an image");
            }
        }

        /// <summary>
        /// Rotate the camera.
        /// </summary>
        /// <param name="direction">The direction to turn the camera in.</param>
        /// <param name="speed">The speed to turn the camera with.</param>
        public void Rotate(CameraDirection direction, int speed = 0)
        {
            if (speed < -5 || speed > 5)
            {
                throw new ArgumentOutOfRangeException("speed");
            }

            // Build command URL
            var directionString = string.Empty;
            switch (direction)
            {
                case CameraDirection.Home:
                    directionString += "home";
                    break;
                case CameraDirection.Up:
                    directionString += "up";
                    break;
                case CameraDirection.Down:
                    directionString += "down";
                    break;
                case CameraDirection.Left:
                    directionString += "left";
                    break;
                case CameraDirection.Right:
                    directionString += "right";
                    break;
                default:
                    throw new Exception("Unknown enum value encountered.");

            }

            var commandUrl = String.Format("camctrl.cgi?move={0}&speed={1}", directionString, speed);

            // Set wait time
            var waitTime = (direction == CameraDirection.Home)
                ? (int)(4.5 * 1000)
                : (int)(2.5 * 1000);

            // Execute command
            Logger.UserInterface.Debug("Rotating the camera {0} with speed {1}", direction.ToString().ToLower(), speed);

            using (ExecuteCommand(commandUrl, waitTime))
            {
                // using-block makes sure the return value gets disposed properly in all cases
            }
        }
    }
}
