using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace DimitriVranken.PanoramaCreator {
    static class PanoramaCreator {
        static readonly Options Options = new Options();
        static IPAddress _ipAddress;
        static FileInfo _outputFile;

        static void Main(string[] args) {
            try {
                if (!ParseOptions(args)) {
                    Environment.Exit(1);
                }

                PrintHeading();
                Console.WriteLine("Camera IP address: {0}", _ipAddress);

                // Setup
                Common.CheckDirectory(_outputFile.Directory.FullName);

                var camera = new CameraControl(_ipAddress);
                camera.Rotate(CameraDirection.Home);
                // TODO: Set rotation distance

                // Take images
                Console.WriteLine();

                var imageFiles = new List<string>();
                for (var imageIndex = 1; imageIndex <= Options.ImageCount; imageIndex++) {
                    // Take image
                    var imageFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".jpg");
                    imageFiles.Add(imageFile);

                    Logger.UserInterface.Info("Taking picture {0}/{1}.", imageIndex, Options.ImageCount);
                    camera.TakeImage(imageFile);

                    // Rotate camera (not after the last image was taken)
                    if (imageIndex < Options.ImageCount) {
                        Logger.UserInterface.Debug("Rotating the camera to the right.");
                        camera.Rotate(CameraDirection.Right);
                    }
                }

                // Create panorama
                Console.WriteLine();

                // TODO:

                // Delete temporary files
                Logger.UserInterface.Debug("Deleting temporary files");
                foreach (var imageFile in imageFiles) {
                    Common.TryDeleteFile(imageFile);
                }

                Console.ReadLine();
            }
            catch (Exception exception) {
                // TODO: handle
                Logger.UserInterface.Fatal("A fatal error occurred. See the log file for more information.");
                Logger.Default.Fatal("Fatal error", exception);

                Environment.Exit(1);
            }
        }


        private static bool ParseOptions(string[] options) {
            // Parse options
            CommandLine.Parser.Default.ParseArgumentsStrict(options, Options);


            // Log raw options
            Logger.UserInterface.Debug("ip-address: {0}", Options.IpAddress);
            Logger.UserInterface.Debug("output: {0}", Options.Output);
            Logger.UserInterface.Debug("force: {0}", Options.Force);
            Logger.UserInterface.Debug("image-count: {0}", Options.ImageCount);
            Logger.UserInterface.Debug("verbose: {0}", Options.Verbose);


            // Validate options
            var optionInvalid = false;

            if (!IPAddress.TryParse(Options.IpAddress, out _ipAddress)) {
                optionInvalid = true;
                Logger.UserInterface.Error("Error: The specified ip-address is invalid.");
            }

            _outputFile = new FileInfo(Options.Output);
            if (Options.Force == false && _outputFile.Exists) {
                optionInvalid = true;
                Logger.UserInterface.Error("Error: The output file already exists. Use -f to force an overwrite.");
            }

            // "force" doesn't need to be validated

            const int minimumImageCount = 1;
            const int maximumImageCount = 5;
            if (Options.ImageCount < 1) {
                optionInvalid = true;
                Logger.UserInterface.Error("Error: The image-count may not be lower than {0}", minimumImageCount);
            }
            else if (Options.ImageCount > 5) {
                optionInvalid = true;
                Logger.UserInterface.Error("Error: The image-count may not be greater than {0}", maximumImageCount);
            }

            // "verbose" doesn't need to be validated


            // Return
            return !optionInvalid;
        }

        private static void PrintHeading() {
            var assemblyInfo = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
            Console.WriteLine("{0} {1}", assemblyInfo.ProductName, assemblyInfo.ProductVersion);
            Console.WriteLine("{0}", assemblyInfo.LegalCopyright);

            Console.WriteLine();
        }
    }
}
