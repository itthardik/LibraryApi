﻿namespace LMS2.Utility
{
    /// <summary>
    /// Logger Class
    /// </summary>
    public static class Logger
    {
        
        private static readonly string logFilePath = "exception_log.txt";
        /// <summary>
        /// Log Exception into LogFile
        /// </summary>
        /// <param name="ex"></param>
        public static void LogException(Exception ex)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine("------------------------------------------------");
                    writer.WriteLine($"Date: {DateTime.Now}");
                    writer.WriteLine($"Message: {ex.Message}");
                    writer.WriteLine($"StackTrace: {ex.StackTrace}");
                    writer.WriteLine("------------------------------------------------");
                }
            }
            catch (Exception loggingEx)
            {
                Console.WriteLine($"Failed to log exception: {loggingEx.Message}");
            }
        }
    }
    
}
