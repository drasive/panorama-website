using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace DimitriVranken.PanoramaCreator
{
    static class PanoramaCreator
    {
        // TODO: Change -o parameter to folder, use for example images

        static readonly Options Options = new Options();
        static Camera _camera;

        private static void PrintHeading()
        {
            var assemblyInfo = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
            Console.WriteLine("{0} {1}", assemblyInfo.ProductName, assemblyInfo.ProductVersion);
            Console.WriteLine("{0}", assemblyInfo.LegalCopyright);

            Console.WriteLine();
        }

        private static bool ParseOptions(string[] options)
        {
            // Parse options
            CommandLine.Parser.Default.ParseArgumentsStrict(options, Options);

            if (Options.Verbose)
            {
                Logger.UserInterface.UpdateLogLevel(NLog.LogLevel.Debug);
            }

            // Log raw options
            Logger.UserInterface.Debug("ip-address: {0}", Options.IpAddress);
            Logger.UserInterface.Debug("output: {0}", Options.Output);
            Logger.UserInterface.Debug("force: {0}", Options.Force);
            Logger.UserInterface.Debug("image-count: {0}", Options.ImageCount);

            Logger.UserInterface.Debug("proxy-address: {0}", Options.ProxyAddress);
            Logger.UserInterface.Debug("proxy-username: {0}", Options.ProxyUsername);
            Logger.UserInterface.Debug("proxy-password: {0}", Options.ProxyPassword);

            Logger.UserInterface.Debug("verbose: {0}", Options.Verbose);
            Logger.UserInterface.Debug("no-network: {0}", Options.NoNetwork);


            // Validate options
            var optionInvalid = false;

            // ---ip-address
            IPAddress ipAddressParsed;
            if (IPAddress.TryParse(Options.IpAddress, out ipAddressParsed))
            {
                Options.IpAddressParsed = ipAddressParsed;
            }
            else
            {
                optionInvalid = true;
                Logger.UserInterface.Error("Error: The specified ip-address is invalid");
            }

            // ---output
            Options.OutputParsed = new FileInfo(Options.Output);
            if (Options.Force == false && Options.OutputParsed.Exists)
            {
                optionInvalid = true;
                Logger.UserInterface.Error("Error: The output file already exists. Use -f to force an overwrite");
            }

            // ---proxy-address
            if (!string.IsNullOrEmpty(Options.ProxyAddress))
            {
                Uri proxyAddressParsed;

                if (Uri.TryCreate(Options.ProxyAddress, UriKind.Absolute, out proxyAddressParsed))
                {
                    Options.ProxyAddressParsed = proxyAddressParsed;
                }
                else
                {
                    optionInvalid = true;
                    Logger.UserInterface.Error("Error: The specified proxy-address is invalid");
                }
            }

            // ---proxy-username
            if (Options.ProxyUsername != null && Options.ProxyAddressParsed == null)
            {
                Logger.UserInterface.Warn("Warning: The specified proxy-username is ignored because no proxy-address is set");
            }
            else if (Options.ProxyUsername == null && Options.ProxyAddress != null)
            {
                optionInvalid = true;
                Logger.UserInterface.Error("Error: proxy-username needs to be set too when using proxy-address");
            }

            // ---proxy-password
            if (Options.ProxyPassword != null && Options.ProxyAddress == null)
            {
                Logger.UserInterface.Warn("Warning: The specified proxy-password is ignored because no proxy-address is set");
            }
            else if (Options.ProxyPassword == null && Options.ProxyAddress != null)
            {
                optionInvalid = true;
                Logger.UserInterface.Error("Error: proxy-password needs to be set too when using proxy-address");
            }

            // ---image-count
            const int minimumImageCount = 2;
            const int maximumImageCount = 25;
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

            // ---verbose doesn't need to be parsed

            // ---no-network doesn't need to be parsed


            return !optionInvalid;
        }

        private static void SetupCamera()
        {
            if (_camera != null)
            {
                return;
            }

            if (Options.ProxyAddressParsed == null)
            {
                _camera = new Camera(Options.IpAddressParsed, Options.NoNetwork);
            }
            else
            {
                var proxy = new WebProxy(Options.ProxyAddressParsed, true);
                proxy.Credentials = new NetworkCredential(Options.ProxyUsername, Options.ProxyPassword);

                _camera = new Camera(Options.IpAddressParsed, proxy, Options.NoNetwork);
            }
        }

        private static List<string> TakeImages()
        {
            Console.WriteLine();

            SetupCamera();

            // Move into starting position
            // TODO: (Networking) Test if increased pan speed saves time when turning to starting position
            _camera.Rotate(CameraDirection.Home);
            var stepsToTheLeft = Math.Floor((decimal)Options.ImageCount / 2);
            for (var stepsExecuted = 0; stepsExecuted < stepsToTheLeft; stepsExecuted++)
            {
                _camera.Rotate(CameraDirection.Left);
            }

            // Take images
            // TODO: (Networking) Test out other pan speed values and the panoramic image quality
            _camera.SetPanSpeed(-3);

            var imageFiles = new List<string>();
            // TODO: (Networking) Test out amount of images required for 180 degrees
            for (var imageIndex = 1; imageIndex <= Options.ImageCount; imageIndex++)
            {
                // Take image
                var imageTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var imageFile = Path.Combine(Common.GetTemporaryFolder(), String.Format("Snapshot {0} ({1}).jpg", imageTimestamp, Guid.NewGuid()));
                imageFiles.Add(imageFile);

                Logger.UserInterface.Info("Taking picture {0}/{1}", imageIndex, Options.ImageCount);
                _camera.TakeImage(imageFile);

                // Rotate camera (not after the last image was taken)
                if (imageIndex < Options.ImageCount)
                {
                    _camera.Rotate(CameraDirection.Right);
                }
            }

            if (Options.NoNetwork)
            {
                // Use example images
                imageFiles.Clear();

                for (var imageFileIndex = 1; imageFileIndex <= Options.ImageCount; imageFileIndex++)
                {
                    imageFiles.Add(String.Format(@"C:\temp\img{0}.jpg", imageFileIndex));
                }
            }

            return imageFiles;
        }

        private static void GeneratePanoramicImage(IEnumerable<string> imageFiles)
        {
            Console.WriteLine();

            Common.CheckDirectory(Options.OutputParsed.Directory.FullName);
            PanoramicImageGenerator.GeneratePanoramicImage(imageFiles, Options.OutputParsed.FullName);
        }


        private static void Main(string[] args)
        {
            try
            {
                Logger.Default.Info("Application started");
                var stopwatch = Stopwatch.StartNew();

                // Print header
                PrintHeading();

                // Parse parameters
                if (!ParseOptions(args))
                {
                    Environment.Exit(1);
                }

                // Print info
                Console.WriteLine("Camera IP address: {0}", Options.IpAddressParsed);

                // Capture images
                var imageFiles = TakeImages();

                // Generate panoramic image
                GeneratePanoramicImage(imageFiles);

                // Delete temporary files
                if (!Options.NoNetwork){
                    Logger.UserInterface.Debug("Deleting temporary files");

                    foreach (var imageFile in imageFiles)
                    {
                        Common.TryDeleteFile(imageFile);
                    }
                }

                // Print done
                stopwatch.Stop();
                var executionTime = Math.Round((decimal)stopwatch.ElapsedMilliseconds / 1000, 2);
                Logger.UserInterface.Info("Done ({0} seconds)", executionTime);
#if DEBUG
                Console.ReadLine();
#endif
            }
            catch (Exception exception)
            {
                Console.WriteLine();
                Logger.UserInterface.Fatal("A fatal error occurred: {0}" + Environment.NewLine +
                                           "See the log file for more information.", exception.Message);
                Logger.Default.FatalException("", exception);

#if DEBUG
                Console.ReadLine();
#endif
                Environment.Exit(1);
            }
        }
    }
}
