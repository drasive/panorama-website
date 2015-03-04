using System.IO;

namespace DimitriVranken.PanoramaCreator
{
    static class Common
    {
        public static void CheckDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}
