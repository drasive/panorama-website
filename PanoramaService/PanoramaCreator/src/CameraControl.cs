﻿using System;
using System.IO;
using System.Net;

namespace DimitriVranken.PanoramaCreator
{
    class CameraControl
    {
        // TODO: Test

        const string UrlProtocol = "http://";
        const string UrlFolder = "/cgi-bin/";
        const string UrlPanSpeedCommand = "camctrl.cgi?speedpan=";
        const string UrlRotationCommand = "camctrl.cgi?move=";
        const string UrlImageCommand = "video.jpg";


        public IPAddress CameraIpAddress { get; private set; }


        public CameraControl(IPAddress cameraIpAddress)
        {
            CameraIpAddress = cameraIpAddress;
        }


        private HttpWebResponse ExecuteCommand(string commandUrl, int waitTime)
        {
#if DEBUG
            return null;
#endif

            var requestUrl = UrlProtocol + CameraIpAddress + UrlFolder + commandUrl;

            Logger.Default.Info("Camera: Executing request '{0}'", requestUrl);
            var request = WebRequest.Create(requestUrl);
            request.Timeout = 15 * 1000;

            var proxy = new WebProxy();
            var adress = "http://172.20.10.24:3128";
            var username = "un";
            var password = "pw";
            proxy.Address = new Uri(adress);
            proxy.Credentials = new NetworkCredential(username, password);
            request.Proxy = proxy;

            var response = (HttpWebResponse)request.GetResponse();
            Logger.Default.Debug("Response received (HTTP {0})", response.StatusCode);

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
            var commandUrl = UrlImageCommand;

            // Execute command
            ExecuteCommand(commandUrl, destinationFile);
        }

        public void SetPanSpeed(int speed)
        {
            // TODO: Validate params
            // TODO: Log?

             // Build command URL
            var commandUrl = UrlPanSpeedCommand + speed;

            // Execute command
            ExecuteCommand(commandUrl, (int)(0.5 * 1000));
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
            int waitTime;
            if (direction == CameraDirection.Home)
            {
                waitTime = 5 * 1000;
            }
            else
            {
                waitTime = 3 * 1000;
            }

            // Execute command
            Logger.UserInterface.Info("Rotating the camera: " + direction.ToString());
            ExecuteCommand(commandUrl, waitTime).Dispose();
        }
    }
}
