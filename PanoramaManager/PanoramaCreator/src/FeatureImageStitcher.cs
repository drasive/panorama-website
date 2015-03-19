using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Accord.Imaging;
using Accord.Imaging.Filters;

namespace DimitriVranken.PanoramaCreator
{
    class FeatureImageStitcher : ImageStitcher
    {
        // TODO: Rename?
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


        public override Bitmap StitchPanoramicImage(IList<FileInfo> imageFiles)
        {
            if (imageFiles.Count() < 2 || imageFiles.Count() > 100)
            {
                throw new ArgumentException("Invalid element count.", "imageFiles");
            }

            return StitchPanoramicImage(imageFiles, 1920, 7680);
        }

        public override Bitmap StitchPanoramicImage(IList<FileInfo> imageFiles,
            int maximumProcessingResolution, int maximumOutputResolution)
        {
            if (imageFiles.Count() < 2 || imageFiles.Count() > 100)
            {
                throw new ArgumentException("Invalid element count.", "imageFiles");
            }
            if (maximumProcessingResolution < 144 || maximumProcessingResolution > 7680)
            {
                throw new ArgumentOutOfRangeException("maximumProcessingResolution");
            }
            if (maximumOutputResolution < 144 || maximumOutputResolution > 7680)
            {
                throw new ArgumentOutOfRangeException("maximumOutputResolution");
            }


            var imagesRaw = new List<Bitmap>();
            var images = new List<Bitmap>();
            try
            {
                // Load raw bitmaps
                Logger.Default.Debug("FeatureImageStitcher: Loading bitmaps");

                foreach (var imageFile in imageFiles)
                {
                    if (!imageFile.Exists)
                    {
                        throw new FileNotFoundException(String.Format(
                                "The snapshot '{0}' doesn't exist.",
                                imageFile));
                    }

                    imagesRaw.Add(new Bitmap(imageFile.FullName));
                }

                // Process raw bitmaps
                Logger.Default.Debug("FeatureImageStitcher: Processing bitmaps");

                images = imagesRaw.Select(imageRaw => ReduceImageResolution(imageRaw, maximumProcessingResolution)).ToList();
                images = images.Select(image => ConvertImageFormat(image, PixelFormat.Format24bppRgb)).ToList();

                // Merge images
                Logger.UserInterface.Info("Merging images 1/{0}", images.Count() - 1);
                var panoramicImage = MergeImages(images[0], images[1]);

                for (var imageIndex = 2; imageIndex < images.Count(); imageIndex++)
                {
                    Logger.UserInterface.Info("Merging images {0}/{1}", imageIndex, images.Count() - 1);
                    panoramicImage = MergeImages(panoramicImage, images[imageIndex]);
                }

                // Process panoramic image
                Logger.Default.Debug("FeatureImageStitcher: Processing the panoramic image");

                panoramicImage = ReduceImageResolution(panoramicImage, maximumOutputResolution);

                // Return
                return panoramicImage;
            }
            finally
            {
                // Dispose bitmaps
                foreach (var image in images)
                {
                    if (image != null)
                    {
                        image.Dispose();
                    }
                }

                foreach (var imageRaw in imagesRaw)
                {
                    if (imageRaw != null)
                    {
                        imageRaw.Dispose();
                    }
                }
            }
        }
    }
}
