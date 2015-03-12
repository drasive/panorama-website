using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Accord.Imaging;
using Accord.Imaging.Filters;

namespace DimitriVranken.PanoramaCreator
{
    static class PanoramaGenerator
    {
        private static Bitmap ChangeImageResolution(Image image, decimal scalingFactor)
        {
            return new Bitmap(image, (int)(image.Width * scalingFactor), (int)(image.Height * scalingFactor));
        }

        private static Bitmap ReduceImageResolution(Bitmap image, int maximumResolution)
        {
            if (image.Width < maximumResolution && image.Height < maximumResolution)
            {
                return image;
            }

            decimal scalingFactor;
            if (image.Height > image.Width)
            {
                scalingFactor = (decimal)maximumResolution / image.Height;
            }
            else
            {
                scalingFactor = (decimal)maximumResolution / image.Width;
            }

            return ChangeImageResolution(image, scalingFactor);
        }

        private static Bitmap ConvertImageFormat(Image image, PixelFormat format)
        {
            var newImage = new Bitmap(image.Width, image.Height, format);
            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, new Rectangle(0, 0, newImage.Width, newImage.Height));
            }

            return newImage;
        }

        private static Bitmap MergeImages(Bitmap image1, Bitmap image2)
        {
            // Detect feature points using Surf Corners Detector
            var featureDetector = new SpeededUpRobustFeaturesDetector();
            var featurePoints1 = featureDetector.ProcessImage(image1).ToArray();
            var featurePoints2 = featureDetector.ProcessImage(image2).ToArray();

            // Match feature points using a k-NN
            var featureMatcher = new KNearestNeighborMatching(5);
            var featureMatches = featureMatcher.Match(featurePoints1, featurePoints2);

            var correlationPoints1 = featureMatches[0];
            var correlationPoints2 = featureMatches[1];

            // Create the homography matrix using a RANSAC estimator
            var homographyEstimator = new RansacHomographyEstimator(0.001, 0.99);
            var homography = homographyEstimator.Estimate(correlationPoints1, correlationPoints2);

            // Blend the second image using the homography
            var blend = new Blend(homography, image1);
            return blend.Apply(image2);
        }


        public static void GeneratePanoramicImage(IEnumerable<string> imageFiles, string outputFile)
        {
            var imageFilesList = imageFiles as IList<string> ?? imageFiles.ToList();

            // Load raw bitmaps
            Logger.Default.Debug("PanoramicGenerator: Loading bitmaps");
            var imagesRaw = imageFilesList.Select(imageFile => new Bitmap(imageFile)).ToList();

            // Process raw bitmaps
            var images = imagesRaw.Select(image => ReduceImageResolution(image, 1920)).ToList();
            images = images.Select(image => ConvertImageFormat(image, PixelFormat.Format24bppRgb)).ToList();

            // Merge first two images
            Logger.UserInterface.Info("Merging images 1/{0}", imageFilesList.Count() - 1);
            var panoramicImage = MergeImages(images[0], images[1]);

            // Merge remaining images
            for (var imageIndex = 2; imageIndex < imageFilesList.Count(); imageIndex++)
            {
                Logger.UserInterface.Info("Merging images {0}/{1}", imageIndex, imageFilesList.Count() - 1);
                panoramicImage = MergeImages(panoramicImage, images[imageIndex]);
            }

            // Save panoramic image
            Logger.Default.Info("PanoramicGenerator: Saving the panoramic image to '{0}'", outputFile);
            panoramicImage = ReduceImageResolution(panoramicImage, 10000);
            panoramicImage.Save(outputFile, ImageFormat.Png);

            // Dispose bitmaps
            foreach (var imageRaw in images)
            {
                imageRaw.Dispose();
            }

            foreach (var image in images)
            {
                image.Dispose();
            }

            panoramicImage.Dispose();
        }
    }
}
