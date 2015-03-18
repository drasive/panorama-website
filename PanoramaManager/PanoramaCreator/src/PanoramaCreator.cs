using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace DimitriVranken.PanoramaCreator
{
    static class PanoramaCreator
    {
        public static readonly Options Options = new Options();
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

            // Log raw options
            if (Options.Verbose)
            {
                Logger.UserInterface.UpdateLogLevel(NLog.LogLevel.Debug);
                Console.WriteLine("Raw options:");
            }

            Logger.UserInterface.Debug("ip-address: {0}", Options.IpAddress);
            Logger.UserInterface.Debug("image-count: {0}", Options.ImageCount);
            Logger.UserInterface.Debug("merge-mode: {0}", Options.MergeMode);
            Logger.UserInterface.Debug("output: {0}", Options.Output);
            Logger.UserInterface.Debug("archive: {0}", Options.Archive);
            Logger.UserInterface.Debug("thumbnail: {0}", Options.Thumbnail);

            Logger.UserInterface.Debug("proxy-address: {0}", Options.ProxyAddress);
            Logger.UserInterface.Debug("proxy-username: {0}", Options.ProxyUsername);
            Logger.UserInterface.Debug("proxy-password: {0}", Options.ProxyPassword);

            Logger.UserInterface.Debug("verbose: {0}", Options.Verbose);
            Logger.UserInterface.Debug("force: {0}", Options.Force);
            Logger.UserInterface.Debug("no-camera: {0}", Options.NoCamera);
            Logger.UserInterface.Debug("no-merge: {0}", Options.NoMerge);

            if (Options.Verbose)
            {
                Console.WriteLine();
            }

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

            // ---image-count
            int minimumImageCount = 2;
            // Enough to cover a 360° view when the angle changes at least 15° per picture
            var maximumImageCount = (int)Math.Ceiling(360d / 15); // 24
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

            // ---merge-mode doesn't need to be parsed

            // ---output
            try
            {
                if (!string.IsNullOrWhiteSpace(Path.GetExtension(Options.Output)))
                {
                    // Specified output is a file
                    optionInvalid = true;
                    Logger.UserInterface.Error("Error: The specified output is not a valid directory");
                }
                else
                {
                    Options.OutputParsed = new DirectoryInfo(Options.Output);
                }
            }
            catch
            {
                optionInvalid = true;
                Logger.UserInterface.Error("Error: The specified output is not a valid directory");
            }

            // ---archive doesn't need to be parsed

            // ---thumbnail doesn't need to be parsed

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
                Logger.UserInterface.Warn("Warning: The specified proxy-username is ignored because " +
                                          "no proxy-address is set");
            }
            else if (Options.ProxyUsername == null && Options.ProxyAddress != null)
            {
                optionInvalid = true;
                Logger.UserInterface.Error("Error: proxy-username needs to be set too " +
                                           "when using proxy-address");
            }

            // ---proxy-password
            if (Options.ProxyPassword != null && Options.ProxyAddress == null)
            {
                Logger.UserInterface.Warn("Warning: The specified proxy-password is ignored because " +
                                          "no proxy-address is set");
            }
            else if (Options.ProxyPassword == null && Options.ProxyAddress != null)
            {
                optionInvalid = true;
                Logger.UserInterface.Error("Error: proxy-password needs to be set too " +
                                           "when using proxy-address");
            }

            // ---verbose doesn't need to be parsed

            // ---force doesn't need to be parsed

            // ---no-camera doesn't need to be parsed

            // ---no-merge doesn't need to be parsed


            return !optionInvalid;
        }

        private static void SetupCamera(IPAddress ipAddress,
            Uri proxyAddress, string proxyUsername, string proxyPassword)
        {
            if (_camera != null)
            {
                return;
            }

            var proxy = new WebProxy(proxyAddress, true);
            proxy.Credentials = new NetworkCredential(proxyUsername, proxyPassword);

            _camera = new Camera(ipAddress, proxy);
        }

        private static List<FileInfo> TakeImages(int imageCount)
        {
            // TODO: _Adapt to border stitcher

            // Move into starting position
            _camera.Rotate(CameraDirection.Home);

            // TODO: 3@1 for feature stitcher, 3@0,-5,-5 for border stitcher
            const int panSpeed = 2;

            var imagesTakenToTheLeft = Math.Floor(imageCount / 2d); // Equal or one less than images taken to the right
            for (var stepsExecuted = 0; stepsExecuted < imagesTakenToTheLeft; stepsExecuted++)
            {
                _camera.Rotate(CameraDirection.Left, panSpeed);
            }

            // Take images
            var imageFiles = new List<FileInfo>();
            for (var imageIndex = 1; imageIndex <= imageCount; imageIndex++)
            {
                // Take image
                var imageTimestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                var imagePath = Path.Combine(
                    Common.GetTemporaryFolder(),
                    String.Format("Snapshot_{0}_({1}).jpg", imageTimestamp, Guid.NewGuid()));
                var imageFile = new FileInfo(imagePath);
                imageFiles.Add(imageFile);

                Logger.UserInterface.Info("Taking picture {0}/{1}", imageIndex, imageCount);
                _camera.TakeImage(imageFile);

                // Rotate camera (not after the last image was taken)
                if (imageIndex < imageCount)
                {
                    _camera.Rotate(CameraDirection.Right, panSpeed);
                }
            }

            return imageFiles;
        }

        private static void SavePanoramicImage(DirectoryInfo outputDirectory, Image image, Image thumbnail = null)
        {
            Common.CheckDirectory(outputDirectory);

            // TODO: Use format from config
            var imageTimestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            var imageName = imageTimestamp + ".png";
            var imageFile = Path.Combine(outputDirectory.FullName, imageName);

            // Check if image file already exists
            if (File.Exists(imageFile) && !Common.AskForUserConfirmation(
                    String.Format("The file '{0}' already exists. Do you want to override it?", imageFile),
                    true))
            {
                return;
            }

            // Save image
            Logger.UserInterface.Info("Saving the image to '{0}'", imageFile);
            image.Save(imageFile, ImageFormat.Png);

            // Save thumbnail
            if (thumbnail != null)
            {
                // TODO: Use format from config
                var thumbnailName = imageTimestamp + "_thumb.png";
                var thumbnailFile = Path.Combine(outputDirectory.FullName, thumbnailName);

                // Take the risk of overriding an existing file (the user already gave his OK to override the image)

                Logger.UserInterface.Info("Saving the image thumbnail to '{0}'", thumbnailFile);
                thumbnail.Save(thumbnailFile, ImageFormat.Png);
            }
        }

        private static void GenerateAndSavePanoramicImage(ImageStitcher imageStitcher, IList<FileInfo> imageFiles,
            DirectoryInfo outputDirectory, bool saveToArchive, bool saveThumbnail)
        {
            Bitmap image = null;
            Bitmap thumbnail = null;
            try
            {
                // Generate image
                // TODO: Use resolution from config
                image = imageStitcher.StitchPanoramicImage(imageFiles);
                if (saveThumbnail)
                {
                    // TODO: Use resolution from config
                    thumbnail = imageStitcher.GenerateThumbnail(image, 480);
                }

                // Save image
                Console.WriteLine();

                SavePanoramicImage(outputDirectory, image, thumbnail);
                if (saveToArchive)
                {
                    // TODO: Use format from config
                    var archiveSubdirectoryName = DateTime.Now.ToString("yyyy-MM-dd");
                    var archiveSubdirectory = new DirectoryInfo(Path.Combine(
                        outputDirectory.FullName,
                        archiveSubdirectoryName));
                    SavePanoramicImage(archiveSubdirectory, image, thumbnail);
                }
            }
            finally
            {
                if (thumbnail != null)
                {
                    thumbnail.Dispose();
                }

                if (image != null)
                {
                    image.Dispose();
                }
            }
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
#if DEBUG
                    Console.ReadLine();
#endif
                    Environment.Exit(1);
                }

                // Print info
                Console.WriteLine("Camera IP address: {0}", Options.IpAddressParsed);

                // Capture images
                Console.WriteLine();

                var imageFiles = new List<FileInfo>();
                if (!Options.NoCamera)
                {
                    SetupCamera(Options.IpAddressParsed,
                        Options.ProxyAddressParsed, Options.ProxyUsername, Options.ProxyPassword);

                    imageFiles = TakeImages(Options.ImageCount);
                }
                else
                {
                    // Use example images
                    Logger.UserInterface.Warn("Not taking any images with the network camera, " +
                                              "using example images instead (option --no-camera is set)");

                    for (var imageFileIndex = 1; imageFileIndex <= Options.ImageCount; imageFileIndex++)
                    {
                        var imageName = String.Format("Snapshot {0}.jpg", imageFileIndex);
                        var imageFile = new FileInfo(Path.Combine(Options.OutputParsed.FullName, imageName));

                        if (!imageFile.Exists)
                        {
                            // Assume that there are no more example images
                            break;
                        }

                        imageFiles.Add(imageFile);
                    }
                }

                // Generate and save panoramic image
                Console.WriteLine();

                if (!Options.NoMerge)
                {
                    ImageStitcher imageStitcher;
                    switch (Options.MergeMode)
                    {
                        case ImageStitcherType.Border:
                            imageStitcher = new BorderImageStitcher();
                            break;
                        case ImageStitcherType.Feature:
                            imageStitcher = new FeatureImageStitcher();
                            break;
                        default:
                            throw new Exception("Unknown enum value encountered.");
                    }

                    GenerateAndSavePanoramicImage(imageStitcher, imageFiles,
                        Options.OutputParsed, Options.Archive, Options.Thumbnail);
                }
                else
                {
                    Logger.UserInterface.Warn("Not merging the snapshots into a panoramic image " +
                                              "(option --no-merge is set)");

                }

#if DEBUG
#else
                // Delete temporary files
                if (!Options.NoCamera)
                {
                    Logger.UserInterface.Debug("Deleting temporary files");

                    foreach (var imageFile in imageFiles)
                    {
                        Common.TryDeleteFile(imageFile);
                    }
                }
#endif

                // Print done
                Console.WriteLine();

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
