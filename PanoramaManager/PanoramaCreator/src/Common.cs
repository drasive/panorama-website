using System;
using System.Diagnostics;
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
                Logger.Default.Trace("Directory '{0}' already exists and is not created.", directoryPath);
            }
        }

        public static void CheckDirectory(DirectoryInfo directory)
        {
            CheckDirectory(directory.FullName);
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
                    Logger.Default.Warn("Failed to delete file {0}", filePath);
                    // ignore
                }
            }
            else
            {
                Logger.Default.Trace("File {0} doesn't exists and is not deleted", filePath);
            }
        }

        public static void TryDeleteFile(FileInfo file)
        {
            TryDeleteFile(file.FullName);
        }

        public static string GetTemporaryFolder()
        {
            var temporaryFolder = Path.GetTempPath();

            var assemblyInfo = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
            var programSubfolder = assemblyInfo.ProductName;


            return Path.Combine(temporaryFolder, programSubfolder);
        }


        public static bool AskForUserConfirmation(string message, bool? defaultOption)
        {
            // Build options string
            string options;
            if (!defaultOption.HasValue)
            {
                options = "[y/n]";
            }
            else if (defaultOption.Value)
            {
                options = "[Y/n]";
            }
            else
            {
                options = "[y/N]";
            }

            // Ask for confirmation
            var question = String.Format("{0} {1}: ", message, options);
            Logger.Default.Debug("Confirmation: " + question);
            while (true)
            {
                Console.Write(question);

                // Use automatic answer
                if (PanoramaCreator.Options.Force)
                {
                    Logger.Default.Debug("Confirmation: Answered with 'yes' (automatically)");
                    Console.WriteLine("y (automatically accepting)");

                    return true;
                }

                // Parse answer
                var input = Console.ReadLine();
                bool? answer = null;
                if (string.IsNullOrEmpty(input) && defaultOption.HasValue)
                {
                    answer = defaultOption.Value;
                }
                if (input != null && (input.ToLower() == "y" || input.ToLower() == "yes"))
                {
                    answer = true;
                }
                if (input != null && (input.ToLower() == "n" || input.ToLower() == "no"))
                {
                    answer = false;
                }

                // Return answer
                if (answer.HasValue)
                {
                    Logger.Default.Debug("Confirmation: Answered with '{0}'", answer.Value ? "yes" : "no");
                    return answer.Value;
                }
            }
        }
    }
}
