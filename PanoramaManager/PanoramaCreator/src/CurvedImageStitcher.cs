﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace DimitriVranken.PanoramaCreator
{
    /// <summary>
    /// Stitches images together to a curved panoramic image.
    /// </summary>
    class CurvedImageStitcher : ImageStitcher
    {
        private static Bitmap MergeImages(Image image1, Image image2)
        {
            var mergedWidth = image1.Width + image2.Width;
            var mergedHeight = image1.Height > image2.Height
                ? image1.Height
                : image2.Height;

            var mergedImage = new Bitmap(mergedWidth, mergedHeight);
            using (var graphics = Graphics.FromImage(mergedImage))
            {
                graphics.DrawImage(image1, new Rectangle(0, 0, image1.Width, image1.Height));

                // Draw image 2 at the right border of image 1
                graphics.DrawImage(image2, new Rectangle(image1.Width, 0, image2.Width, image2.Height));
            }

            return mergedImage;
        }


        /// <summary>
        /// Stitch together a curved panoramic image.
        /// </summary>
        /// <param name="imageFiles">The images to stitch together.</param>
        /// <returns>The stitched together panoramic image.</returns>
        public override Bitmap StitchPanoramicImage(IList<FileInfo> imageFiles)
        {
            if (imageFiles.Count() < 2 || imageFiles.Count() > 100)
            {
                throw new ArgumentException("Invalid element count.", "imageFiles");
            }

            return StitchPanoramicImage(imageFiles, 1920, 7680);
        }

        /// <summary>
        /// Stitch together a curved panoramic image.
        /// </summary>
        /// <param name="imageFiles">The images to stitch together.</param>
        /// <param name="maximumProcessingResolution">The maximum resolution for processing the images (shortest side).</param>
        /// <param name="maximumOutputResolution">The maximum resolution of the panoramic image (shortest side).</param>
        /// <returns>The stitched together panoramic image.</returns>
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
                Logger.Default.Debug("BorderImageStitcher: Loading bitmaps");

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
                Logger.Default.Debug("BorderImageStitcher: Processing bitmaps");

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
                Logger.Default.Debug("BorderImageStitcher: Processing the panoramic image");

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

            return null;
        }
    }
}
