using System;
using System.Globalization;
using System.IO;
using System.Net;

namespace DimitriVranken.PanoramaCreator
{
    class CameraControl
    {
        // TODO: Use custom logger

        const string UrlProtocol = "http://";
        const string UrlCommandFolder = "/cgi-bin/";
        const string UrlTakeImageCommand = "video.jpg";
        const string UrlRotationCommand = "camctrl.cgi?move=";
        const string UrlPanSpeedCommand = "camctrl.cgi?speedpan=";


        public IPAddress IpAddress { get; private set; }

        public WebProxy Proxy { get; private set; }


        public CameraControl(IPAddress ipAddress)
        {
            IpAddress = ipAddress;
        }

        public CameraControl(IPAddress ipAddress, WebProxy proxy)
            : this(ipAddress)
        {
            Proxy = proxy;
        }


        private HttpWebResponse ExecuteCommand(string commandUrl, int waitTime)
        {
#if DEBUG
            return null;
#endif

            var requestUrl = UrlProtocol + IpAddress + UrlCommandFolder + commandUrl;

            Logger.Default.Info("Camera: Executing request '{0}'", requestUrl);
            var request = WebRequest.Create(requestUrl);
            request.Timeout = 15 * 1000;

            // TODO: Implement proxy
            if (false)
            {
                var address = "http://172.20.10.24:3128";
                var username = "username";
                var password = "password";
                var proxy = new WebProxy();

                proxy.Address = new Uri(address);
                proxy.Credentials = new NetworkCredential(username, password);
                request.Proxy = proxy;
            }

            var response = (HttpWebResponse)request.GetResponse();
            Logger.Default.Debug("Camera: Response received (HTTP {0})", response.StatusCode);

            System.Threading.Thread.Sleep(waitTime);
            return response;
        }

        private bool ExecuteCommand(string commandUrl, string destinationFile)
        {
#if DEBUG
            return false;
#endif

            using (var response = ExecuteCommand(commandUrl, 3 * 1000))
            {
                // Check if the response is valid
                if (response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Moved ||
                    response.StatusCode == HttpStatusCode.Redirect)
                {
                    // Download the response to a file
                    using (var inputStream = response.GetResponseStream())
                    using (var outputStream = File.OpenWrite(destinationFile))
                    {
                        var buffer = new byte[4096];
                        int bytesRead;

                        do
                        {
                            bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                            outputStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead != 0);
                        Logger.Default.Error("Camera: Downloaded response of command '{0}'", commandUrl);
                    }

                    // The file was downloaded successfully
                    return true;
                }
            }

            // The file couldn't be downloaded
            Logger.Default.Error("Camera: Couldn't download response of command '{0}'", commandUrl);
            return false;
        }


        public void TakeImage(string destinationFile)
        {
            if (string.IsNullOrEmpty(destinationFile))
            {
                throw new ArgumentNullException("destinationFile");
            }

            // Build command URL
            var commandUrl = UrlTakeImageCommand;

            // Execute command
            Logger.UserInterface.Debug("Camera: Taking an image");
            ExecuteCommand(commandUrl, destinationFile);
        }

        public void Rotate(CameraDirection direction)
        {
            // Build command URL
            var commandUrl = UrlRotationCommand;
            switch (direction)
            {
                case CameraDirection.Home:
                    commandUrl += "home";
                    break;
                case CameraDirection.Up:
                    commandUrl += "up";
                    break;
                case CameraDirection.Down:
                    commandUrl += "down";
                    break;
                case CameraDirection.Left:
                    commandUrl += "left";
                    break;
                case CameraDirection.Right:
                    commandUrl += "right";
                    break;
                default:
                    throw new ArgumentException("Unknown enum value encountered.");

            }

            // Set wait time
            var waitTime = (direction == CameraDirection.Home)
                ? 6 * 1000
                : 3 * 1000;

            // Execute command
            Logger.UserInterface.Info("Rotating the camera {0}", direction.ToString().ToLower());
            ExecuteCommand(commandUrl, waitTime).Dispose();
        }

        public void SetPanSpeed(int speed)
        {
            if (speed < -5)
            {
                throw new ArgumentOutOfRangeException(speed.ToString(CultureInfo.InvariantCulture));
            }
            if (speed > 5)
            {
                throw new ArgumentOutOfRangeException(speed.ToString(CultureInfo.InvariantCulture));
            }

            // Build command URL
            var commandUrl = UrlPanSpeedCommand + speed;

            // Execute command
            Logger.UserInterface.Debug("Setting the camera pan speed to {0}", speed);
            ExecuteCommand(commandUrl, (int)(0.5 * 1000));
        }

    }
}
