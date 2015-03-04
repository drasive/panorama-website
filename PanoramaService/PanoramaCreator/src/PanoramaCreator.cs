using System;
using System.IO;
using System.Net;

namespace DimitriVranken.PanoramaCreator
{
    class PanoramaCreator
    {
        static void Main(string[] args)
        {
            // TODO: Parse args using: Install-Package CommandLineParser
            var ipAddress = "127.0.0.1";
            var destinationFolder = @"C:\temp\panorama\";
            var temporaryFolder = Path.Combine(destinationFolder, @"temp\");
            var imageCount = 5;



            // Setup folders
            Common.CheckDirectory(destinationFolder);
            Common.CheckDirectory(temporaryFolder);


            var camera = new CameraControl(IPAddress.Parse(ipAddress));

            // Setup camera
            camera.Rotate(CameraDirection.Home);
            // TODO: Set rotation distance

            // Take images
            for (var imageIndex = 1; imageIndex <= imageCount; imageIndex++)
            {
                var fileName = String.Format("image_{0}.jpg", imageIndex);
                var destinationFile = Path.Combine(destinationFolder, fileName);

                camera.TakeImage(destinationFile);
                // Don't rotate anymore after the last image was taken
                if (imageIndex < imageCount)
                {
                    camera.Rotate(CameraDirection.Right);
                }
            }

            // Create panorama
            

            // Delete temp images
            // TODO:
        }
    }
}
