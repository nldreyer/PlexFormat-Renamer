using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlexFormat_Renamer
{
    public static class NameHandler
    {
        /// <summary>
        /// Gets the current year.
        /// </summary>
        static DateTime dt = DateTime.Now;

        /// <summary>
        /// List of supported video extensions.
        /// </summary>
        private static string[] _videoExtensions = 
        {
            "webm", "mkv", "flv",  "vob", "ogv", "ogg",  "drc", "gif", "gifv", "mng", "avi",
            "mov",  "qt",  "wmv",  "yuv", "rm",  "rmvb", "asf", "amv", "mp4",  "m4p", "m4v",
            "mpg",  "mp2", "mpeg", "mpe", "mpv", "m2v",  "m4v", "svi", "3gp",  "3g2", "mxf",
            "roq",  "nsv", "f4v",  "f4p", "f4a", "fvb"
        };

        /// <summary>
        /// Common unwanted file name strings.
        /// </summary>
        private static string[] _commonUnwanted =
        {
            "BluRay",    "1080p", "720p",  "x264",      "WEBRip", "YTS",    "YIFY", "EVO", "BRrip", "XviD", "AAC",
            "720pBrRip", "BrRip", "HDRip", "anoXmous_", "www",    "UsaBit", "com",  ".."
        };

        private static char[] _dotsDashesBrackets =
        {
            '.', '-', '(', ')', '[', ']'
        };

        public static void Process(DirectoryInfo directory)
        {
            if (directory.Exists)
            {
                FileInfo[] files = directory.GetFiles();
                string[] fileNames = new string[files.Count()];
                string[] fileExtensions = new string[files.Count()];
                string[] fileRenamed = new string[files.Count()];
                for (int i = 0; i < files.Count(); i++)
                {
                    if (IsVideo(files[i].Extension))
                    {
                        fileNames[i] = files[i].Name;
                        fileExtensions[i] = files[i].Extension;
                    }
                }
                for (int j = 0; j < fileNames.Count(); j++)
                {
                    if (fileNames[j] != null)
                    {
                        fileRenamed[j] = RemoveCommon(fileNames[j]);
                    }
                }
                TestWriter(fileRenamed);
            }
        }

        /// <summary>
        /// Checks if the extension is a video.
        /// </summary>
        /// <param name="extension">Extension of the file to check.</param>
        /// <returns>True if video, false if not.</returns>
        private static bool IsVideo(string extension)
        {
            string lower = extension.ToLower();
            lower = lower.Substring(1);
            foreach (string s in _videoExtensions)
            {
                if (s.Equals(lower))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove common unwanted file name strings.
        /// </summary>
        /// <param name="fileName">The file name to remove from.</param>
        /// <returns>The file name without the unwanted strings.</returns>
        private static string RemoveCommon(string fileName)
        {
            fileName = RemoveDotsDashesBrackets(fileName);
            string[] splitArray = fileName.Split(' ');
            List<string> splitList = new List<string>(splitArray);

            splitList = RemoveAfterYear(splitList);

            for (int i = 0; i < splitList.Count(); i++)
            {
                foreach (string s in _commonUnwanted)
                {
                    if (splitList[i].Equals(s))
                    {
                        splitList.RemoveAt(i);
                        i--;
                    }
                }
            }
            return string.Join(" ", splitList.ToArray());
        }

        /// <summary>
        /// Remove dots from the file name.
        /// </summary>
        /// <param name="fileName">The file name to remove from.</param>
        /// <returns>The file name with spaces instead of dots.</returns>
        private static string RemoveDotsDashesBrackets(string fileName)
        {
            foreach (char c in _dotsDashesBrackets)
            {
                fileName = fileName.Replace(c, '!');
            }
            fileName = fileName.Replace("'", string.Empty);
            fileName = fileName.Replace('!', ' ');
            return fileName;
        }

        private static List<string> RemoveAfterYear(List<string> splitList)
        {
            bool after = false;
            for (int i = 1; i < splitList.Count; i++)
            {
                if (!after)
                {
                    if (Int32.TryParse(splitList[i], out int result) && result <= dt.Year && result >= 1950)
                    {
                        splitList[i] = splitList[i].Insert(0, "(");
                        splitList[i] = splitList[i].Insert(5, ")");
                        after = true;
                    }
                }
                else
                {
                    splitList.RemoveAt(i);
                    i--;
                }
            }
            return splitList;
        }

        public static void TestWriter(string[] fileList)
        {
            File.WriteAllLines("C:\\Users\\dubdr\\Desktop\\Test.txt", fileList);
        }
    }
}
