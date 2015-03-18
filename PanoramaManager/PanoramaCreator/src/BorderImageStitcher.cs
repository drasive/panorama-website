using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace DimitriVranken.PanoramaCreator
{
    class BorderImageStitcher : ImageStitcher
    {
        // TODO: _Implement border image stitcher

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


            //var imagesRaw = new List<Bitmap>();
            //var images = new List<Bitmap>();
            //try
            //{
            //    // Load raw bitmaps
            //    Logger.Default.Debug("PanoramicGenerator: Loading bitmaps");
            //
            //    foreach (var imageFile in imageFiles)
            //    {
            //        if (!imageFile.Exists)
            //        {
            //            throw new FileNotFoundException(String.Format(
            //                    "The snapshot '{0}' doesn't exist.",
            //                    imageFile));
            //        }
            //
            //        imagesRaw.Add(new Bitmap(imageFile.FullName));
            //    }
            //
            //    // Process raw bitmaps
            //    Logger.Default.Debug("PanoramicGenerator: Processing bitmaps");
            //
            //    images = imagesRaw.Select(imageRaw => ReduceImageResolution(imageRaw, maximumProcessingResolution)).ToList();
            //    images = images.Select(image => ConvertImageFormat(image, PixelFormat.Format24bppRgb)).ToList();
            //
            //    // Merge first two images
            //    Logger.UserInterface.Info("Merging images 1/{0}", images.Count() - 1);
            //    var panoramicImage = MergeImages(images[0], images[1]);
            //
            //    // Merge remaining images
            //    for (var imageIndex = 2; imageIndex < images.Count(); imageIndex++)
            //    {
            //        Logger.UserInterface.Info("Merging images {0}/{1}", imageIndex, images.Count() - 1);
            //        panoramicImage = MergeImages(panoramicImage, images[imageIndex]);
            //    }
            //
            //    // Process panoramic image
            //    Logger.Default.Debug("PanoramicGenerator: Processing the panoramic image");
            //
            //    panoramicImage = ReduceImageResolution(panoramicImage, maximumOutputResolution);
            //
            //    // Return
            //    return panoramicImage;
            //}
            //finally
            //{
            //    // Dispose bitmaps
            //    foreach (var image in images)
            //    {
            //        if (image != null)
            //        {
            //            image.Dispose();
            //        }
            //    }
            //
            //    foreach (var imageRaw in imagesRaw)
            //    {
            //        if (imageRaw != null)
            //        {
            //            imageRaw.Dispose();
            //        }
            //    }
            //}

            return null;
        }

        public override Bitmap GenerateThumbnail(Bitmap image, int maximumResolution)
        {
            //if (maximumResolution < 10 || maximumResolution > 7680)
            //{
            //    throw new ArgumentOutOfRangeException("maximumResolution");
            //}
            //
            //return ReduceImageResolution(image, maximumResolution);

            return null;
        }
    }
}
