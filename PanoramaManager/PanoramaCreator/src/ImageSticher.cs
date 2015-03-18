using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace DimitriVranken.PanoramaCreator
{
    abstract class ImageStitcher
    {
        public abstract Bitmap StitchPanoramicImage(IList<FileInfo> imageFiles);

        public abstract Bitmap StitchPanoramicImage(IList<FileInfo> imageFiles,
            int maximumProcessingResolution, int maximumOutputResolution);


        public abstract Bitmap GenerateThumbnail(Bitmap image, int maximumResolution);
    }
}
