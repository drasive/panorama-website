using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace DimitriVranken.PanoramaCreator
{
    abstract class ImageStitcher
    {
        protected static Bitmap ChangeImageResolution(Image image, decimal scalingFactor)
        {
            return new Bitmap(image, (int)(image.Width * scalingFactor), (int)(image.Height * scalingFactor));
        }

        protected static Bitmap ReduceImageResolution(Bitmap image, int maximumResolution)
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

        protected static Bitmap ConvertImageFormat(Image image, PixelFormat format)
        {
            var newImage = new Bitmap(image.Width, image.Height, format);
            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, new Rectangle(0, 0, newImage.Width, newImage.Height));
            }

            return newImage;
        }


        public abstract Bitmap StitchPanoramicImage(IList<FileInfo> imageFiles);

        public abstract Bitmap StitchPanoramicImage(IList<FileInfo> imageFiles,
            int maximumProcessingResolution, int maximumOutputResolution);

        public Bitmap GenerateThumbnail(Bitmap image, int maximumResolution)
        {
            if (maximumResolution < 10 || maximumResolution > 7680)
            {
                throw new ArgumentOutOfRangeException("maximumResolution");
            }

            return ReduceImageResolution(image, maximumResolution);
        }
    }
}
