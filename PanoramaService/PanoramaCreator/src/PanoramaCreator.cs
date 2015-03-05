using System;
using System.IO;
using System.Net;

namespace DimitriVranken.PanoramaCreator {
    static class PanoramaCreator {
        static readonly Options Options = new Options();
        static IPAddress _ipAddress;
        static FileInfo _outputFile;

        static void Main(string[] args) {
            try {
                ParseOptions(args);

                if (Options.Verbose)
                {
                    Console.WriteLine(Options.IpAddress);
                    Console.WriteLine(Options.Output);
                    Console.WriteLine(Options.ImageCount);
                    Console.WriteLine(Options.Verbose);
                }
                Console.ReadLine();

                var temporaryFolder = Path.Combine(_outputFile.Directory.FullName, @"temp\");



                // Setup folders
                Common.CheckDirectory(_outputFile.Directory.FullName);
                Common.CheckDirectory(temporaryFolder);


                var camera = new CameraControl(_ipAddress);

                // Setup camera
                camera.Rotate(CameraDirection.Home);
                // TODO: Set rotation distance

                // Take images
                for (var imageIndex = 1; imageIndex <= Options.ImageCount; imageIndex++) {
                    var fileName = String.Format("image_{0}.jpg", imageIndex);
                    var destinationFile = Path.Combine(temporaryFolder, fileName);

                    camera.TakeImage(destinationFile);
                    // Don't rotate anymore after the last image was taken
                    if (imageIndex < Options.ImageCount) {
                        camera.Rotate(CameraDirection.Right);
                    }
                }

                // Create panorama


                // Delete temp images
                // TODO:
            }
            catch (Exception exception) {
                // TODO: handle
            }
        }

        private static void ParseOptions(string[] options) {
            // Parse options
            CommandLine.Parser.Default.ParseArgumentsStrict(options, Options);


            // Validate "ip-address"
            if (!IPAddress.TryParse(Options.IpAddress, out _ipAddress)) {
                
            }

            // Validate "output"
            _outputFile = new FileInfo(Options.Output);
            if (_outputFile.Exists) {

            }

            // Validate "image-count"
            if (Options.ImageCount > 5) {

            }
            else if (Options.ImageCount < 1) {

            }

            // "verbose" doesn't need to be validated
        }
    }
}
