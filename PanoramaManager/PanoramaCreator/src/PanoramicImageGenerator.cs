using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Accord.Imaging;
using Accord.Imaging.Filters;

namespace DimitriVranken.PanoramaCreator
{
    static class PanoramicImageGenerator
    {
        // TODO: Use custom logger

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

        private static Bitmap MergeImages(Bitmap image1, Bitmap image2)
        {
            // TODO: Move resolution change to GeneratePanoramicImage
            const int maximumResolution = 15000; //1920
            var image1LowResolution = ReduceImageResolution(image1, maximumResolution);
            var image2LowResolution = ReduceImageResolution(image2, maximumResolution);

            // Detect feature points using Surf Corners Detector
            var featureDetector = new SpeededUpRobustFeaturesDetector();
            var featurePoints1 = featureDetector.ProcessImage(image1LowResolution).ToArray();
            var featurePoints2 = featureDetector.ProcessImage(image2LowResolution).ToArray();

            // Match feature points using a k-NN
            var featureMatcher = new KNearestNeighborMatching(5);
            var featureMatches = featureMatcher.Match(featurePoints1, featurePoints2);

            var correlationPoints1 = featureMatches[0];
            var correlationPoints2 = featureMatches[1];

            // Create the homography matrix using a RANSAC estimator
            var homographyEstimator = new RansacHomographyEstimator(0.001, 0.99);
            var homography = homographyEstimator.Estimate(correlationPoints1, correlationPoints2);

            // Blend the second image using the homography
            var blend = new Blend(homography, image1LowResolution);
            var mergedImage = blend.Apply(image2LowResolution);

            // Dispose bitmaps
            image1LowResolution.Dispose();
            image2LowResolution.Dispose();

            // Return
            return mergedImage;
        }


        public static void GeneratePanoramicImage(IEnumerable<string> imageFiles, string outputFile)
        {
            var imageFilesList = imageFiles as IList<string> ?? imageFiles.ToList();

            // Load bitmaps
            Logger.Default.Debug("PanoramicGenerator: Loading bitmaps");
            // TODO: Add existence check
            var images = imageFilesList.Select(imageFile => new Bitmap(imageFile)).ToList();

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
            panoramicImage.Save(outputFile, ImageFormat.Png);

            // Dispose bitmaps
            foreach (var image in images)
            {
                image.Dispose();
            }
            panoramicImage.Dispose();
        }
    }
}
