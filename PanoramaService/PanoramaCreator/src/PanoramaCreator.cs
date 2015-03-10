using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace DimitriVranken.PanoramaCreator
{
    static class PanoramaCreator
    {
        // TODO: Don't use DEBUG to detect cam

        static readonly Options Options = new Options();
        static IPAddress _ipAddress;
        static FileInfo _outputFile;

        static void Main(string[] args)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                // Parse parameters
                if (!ParseOptions(args))
                {
                    Environment.Exit(1);
                }


                // Print info
                PrintHeading();
                Console.WriteLine("Camera IP address: {0}", _ipAddress);

                // Capture images
                var imageFiles = TakeImages();
#if DEBUG
                imageFiles.Clear();
                for (var imageFileIndex = 1; imageFileIndex <= Options.ImageCount; imageFileIndex++)
                {
                    imageFiles.Add(String.Format(@"C:\temp\img{0}.jpg", imageFileIndex));
                }
#endif

                // Generate panoramic image
                GeneratePanoramicImage(imageFiles);


#if DEBUG
#else
                // Delete temporary files
                Logger.UserInterface.Debug("Deleting temporary files");
                foreach (var imageFile in imageFiles)
                {
                    Common.TryDeleteFile(imageFile);
                }
#endif

                // Print done
                stopwatch.Stop();
                var executionTime = Math.Round((decimal)stopwatch.ElapsedMilliseconds / 1000, 2);
                Console.WriteLine("Done ({0} seconds)", executionTime);
#if DEBUG
                Console.ReadLine();
#endif
            }
            catch (Exception exception)
            {
                Logger.UserInterface.Fatal("A fatal error occurred. See the log file for more information.");
                Logger.Default.FatalException("", exception);

                Environment.Exit(1);
            }
        }


        private static bool ParseOptions(string[] options)
        {
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

            if (!IPAddress.TryParse(Options.IpAddress, out _ipAddress))
            {
                optionInvalid = true;
                Logger.UserInterface.Error("Error: The specified ip-address is invalid.");
            }

            _outputFile = new FileInfo(Options.Output);
            if (Options.Force == false && _outputFile.Exists)
            {
                optionInvalid = true;
                Logger.UserInterface.Error("Error: The output file already exists. Use -f to force an overwrite.");
            }

            // "force" doesn't need to be validated

            const int minimumImageCount = 2;
            const int maximumImageCount = 10;
            if (Options.ImageCount < minimumImageCount)
            {
                optionInvalid = true;
                Logger.UserInterface.Error("Error: The image-count may not be lower than {0}", minimumImageCount);
            }
            else if (Options.ImageCount > maximumImageCount)
            {
                optionInvalid = true;
                Logger.UserInterface.Error("Error: The image-count may not be greater than {0}", maximumImageCount);
            }

            // "verbose" doesn't need to be validated


            // Return
            return !optionInvalid;
        }

        private static void PrintHeading()
        {
            var assemblyInfo = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
            Console.WriteLine("{0} {1}", assemblyInfo.ProductName, assemblyInfo.ProductVersion);
            Console.WriteLine("{0}", assemblyInfo.LegalCopyright);

            Console.WriteLine();
        }

        private static List<string> TakeImages()
        {
            Console.WriteLine();

            // Setup camera
            var camera = new CameraControl(_ipAddress);
            // TODO: Check if home can be set programatically
            camera.Rotate(CameraDirection.Home);
            camera.SetPanSpeed(-3);
            // TODO: Set rotation distance

            // Take images
            var imageFiles = new List<string>();
            for (var imageIndex = 1; imageIndex <= Options.ImageCount; imageIndex++)
            {
                // Take image
                var imageFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".jpg");
                imageFiles.Add(imageFile);

                Logger.UserInterface.Info("Taking picture {0}/{1}", imageIndex, Options.ImageCount);
                camera.TakeImage(imageFile);

                // Rotate camera (not after the last image was taken)
                if (imageIndex < Options.ImageCount)
                {
                    camera.Rotate(CameraDirection.Right);
                }
            }

            return imageFiles;
        }

        private static void GeneratePanoramicImage(IEnumerable<string> imageFiles)
        {
            Console.WriteLine();

            Common.CheckDirectory(_outputFile.Directory.FullName);
            PanoramaGenerator.GeneratePanoramicImage(imageFiles, _outputFile.FullName);
        }
    }
}
