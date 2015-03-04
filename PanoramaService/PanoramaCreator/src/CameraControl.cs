using System;
using System.IO;
using System.Net;

namespace DimitriVranken.PanoramaCreator
{
    class CameraControl
    {
        // TODO: Add logging

        const string UrlProtocol = "http://";
        const string UrlFolder = "/cgi-bin/";
        const string UrlRotationCommand = "camctrl.cgi?move=";
        const string UrlImageCommand = "video.jpg";


        public IPAddress CameraIpAddress { get; private set; }


        public CameraControl(IPAddress cameraIpAddress)
        {
            CameraIpAddress = cameraIpAddress;
        }


        private HttpWebResponse ExecuteCommand(string commandUrl)
        {
            var requestUrl = UrlProtocol + CameraIpAddress + UrlFolder + commandUrl;
            var request = WebRequest.Create(requestUrl);
            return (HttpWebResponse)request.GetResponse();
        }

        private bool ExecuteCommand(string commandUrl, string destinationFile)
        {
            var response = ExecuteCommand(commandUrl);

            // Check if the response is valid
            if (response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Moved ||
                response.StatusCode == HttpStatusCode.Redirect)
            {
                // Download the response to a file
                using (var inputStream = response.GetResponseStream())
                {
                    using (var outputStream = File.OpenWrite(destinationFile))
                    {
                        var buffer = new byte[4096];
                        int bytesRead;

                        do
                        {
                            bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                            outputStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead != 0);
                    }
                }

                // The file was downloaded successfully
                return true;
            }

            // The file couldn't be downloaded
            return false;
        }


        public void TakeImage(string destinationFile)
        {
            if (string.IsNullOrEmpty(destinationFile))
            {
                throw new ArgumentNullException("destinationFile");
            }

            // Build command URL
            string commandUrl = UrlImageCommand;

            // Execute command
            ExecuteCommand(commandUrl, destinationFile);
        }

        public void Rotate(CameraDirection direction)
        {
            // Build command URL
            string commandUrl = UrlRotationCommand;
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

            // Execute command
            ExecuteCommand(commandUrl);
        }
    }
}
