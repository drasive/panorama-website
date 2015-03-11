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
                Logger.Default.Debug("Created directory '{0}'", directoryPath);
            }
            else
            {
                Logger.Default.Debug("Directory '{0}' already exists and is not created.", directoryPath);
            }
        }

        public static void TryDeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    Logger.Default.Debug("Deleted file {0}", filePath);
                }
                catch
                {
                    Logger.Default.Debug("Failed to delete file {0}", filePath);
                    // ignore
                }
            }
            else
            {
                Logger.Default.Debug("File {0} doesn't exists and is not deleted", filePath);
            }
        }
    }
}
