using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VITV.Data.DAL;
using System.Configuration;
using System.IO;
using VITV.Data.Helpers;
using System.Diagnostics;

namespace DeleteUnusedFiles
{
    class Program
    {
        private static string GetPhysicalPath(string url)
        {
            Uri videoUri;
            bool isAbsoluteUrl = Uri.TryCreate(url, UriKind.Absolute, out videoUri);
            if (isAbsoluteUrl)
            {
                string currentDomain = videoUri.GetLeftPart(UriPartial.Authority);
                Console.WriteLine(currentDomain);
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + System.Net.WebUtility.UrlDecode(videoUri.AbsolutePath);
                filePath = filePath.Replace("/", "\\");
                return filePath;
            }
            else
                return "";
        }
        private static HashSet<string> getThumbnailVideos()
        {
            var context = new VITVContext();
            HashSet<string> videoThumbnails = new HashSet<string>();
            foreach (var video in context.Videos)
            {
                var physicalPath = GetPhysicalPath(video.Thumbnail);
                var mPhysicalPath = GetPhysicalPath(video.MobileThumbnail);
                if (physicalPath != "")
                {
                    videoThumbnails.Add(physicalPath.ToLower());
                    videoThumbnails.Add(mPhysicalPath.ToLower());
                }
            }
            return videoThumbnails;
        }
        private static void checkThumbnailFiles()
        {
            HashSet<string> videoThumbnails = getThumbnailVideos();
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            var count = 0;
            try
            {
                using (new Impersonator("uploaduser", "", "Upload@@123"))
                {
                    foreach (string domain in appSettings.Keys)
                    {
                        var folder = appSettings[domain] + @"\uploadedimages\videothumbnail";
                        var tempFolder = folder + @"\temp\";
                        if (!Directory.Exists(tempFolder))
                            Directory.CreateDirectory(tempFolder);
                        var files = Directory.GetFiles(folder, "*.*", SearchOption.TopDirectoryOnly).Where(f=>!f.EndsWith("Thumbs.db")); 
                        foreach (var file in files)
                        {
                            if (!videoThumbnails.Contains(file.ToLower()))
                            {
                                File.Move(file, tempFolder + Path.GetFileName(file));
                                count++;
                            }
                        }
                    }
                }
                Console.WriteLine("Video Thumbnail: Move unused files to temp folder. Total - "+ count);
            }
            
            catch(Exception ex)
            {
                Console.WriteLine("Error-checkThumbnailFiles: " + ex.Message);
            }
        }

        private static HashSet<string> getReporterPhotos()
        {
            var context = new VITVContext();
            HashSet<string> reporterPhotos = new HashSet<string>();
            foreach (var reporter in context.Employees)
            {
                var physicalPath = GetPhysicalPath(reporter.ProfilePicture);
                if (physicalPath != "")
                {
                    reporterPhotos.Add(physicalPath.ToLower());
                }
            }
            return reporterPhotos;
        }
        private static void checkReporterPhotos()
        {
            HashSet<string> reporterPhotos = getReporterPhotos();
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            var count = 0;
            using (new Impersonator("uploaduser", "", "Upload@@123"))
            {
                try
                {
                    foreach (string domain in appSettings.Keys)
                    {
                        var folder = appSettings[domain] + @"\uploadedimages\profilepicture";
                        var tempFolder = folder + @"\temp\";
                        if (!Directory.Exists(tempFolder))
                            Directory.CreateDirectory(tempFolder);
                        var files = Directory.GetFiles(folder, "*.*", SearchOption.TopDirectoryOnly).Where(f => !f.EndsWith("Thumbs.db")); ;
                        foreach (var file in files)
                        {
                            if (!reporterPhotos.Contains(file.ToLower()))
                            {
                                File.Move(file, tempFolder + Path.GetFileName(file));
                                count++;
                            }
                        }
                    }
                    Console.WriteLine("Reporter Profile Photo: Move unused files to temp folder. Total - " + count);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error-checkReporterPhotos: " + ex.Message);
                }
            }
        }


        private static HashSet<string> getVideoCategoryPhotos()
        {
            var context = new VITVContext();
            HashSet<string> videoCategoryPhotos = new HashSet<string>();
            foreach (var cat in context.VideoCategories)
            {
                var physicalPath = GetPhysicalPath(cat.ProfilePhoto);
                var mPhysicalPath = GetPhysicalPath(cat.MobileProfilePhoto);
                if (physicalPath != "")
                {
                    videoCategoryPhotos.Add(physicalPath.ToLower());
                    videoCategoryPhotos.Add(mPhysicalPath.ToLower());
                }
            }
            return videoCategoryPhotos;
        }
        private static void checkVideoCategoryPhotos()
        {
            HashSet<string> videoCategoryPhotos = getVideoCategoryPhotos();
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            var count = 0;
            using (new Impersonator("uploaduser", "", "Upload@@123"))
            {
                try
                {
                    foreach (string domain in appSettings.Keys)
                    {
                        var folder = appSettings[domain] + @"\uploadedimages\videocategory";
                        var tempFolder = folder + @"\temp\";
                        if (!Directory.Exists(tempFolder))
                            Directory.CreateDirectory(tempFolder);
                        var files = Directory.GetFiles(folder, "*.*", SearchOption.TopDirectoryOnly).Where(f => !f.EndsWith("Thumbs.db")); ;
                        foreach (var file in files)
                        {
                            if (!videoCategoryPhotos.Contains(file.ToLower()))
                            {
                                File.Move(file, tempFolder + Path.GetFileName(file));
                                count++;
                            }
                        }
                    }
                    Console.WriteLine("Video Category's Photo: Move unused files to temp folder. Total - " + count);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error-checkVideoCategoryPhotos: " + ex.Message);
                }
            }
        }

        private static HashSet<string> getVideos()
        {
            var context = new VITVContext();
            HashSet<string> reporterPhotos = new HashSet<string>();
            foreach (var video in context.Videos)
            {
                var physicalPath = GetPhysicalPath(video.Url);
                if (physicalPath != "")
                {
                    reporterPhotos.Add(physicalPath.ToLower());
                }
            }
            return reporterPhotos;
        }
        private static void checkVideoFiles()
        {
            HashSet<string> videoFiles = getVideos();
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            var count = 0;
            using (new Impersonator("uploaduser", "", "Upload@@123"))
            {
                try
                {
                    foreach (string domain in appSettings.Keys)
                    {
                        var folder = appSettings[domain] + @"\uploadedvideos";
                        var tempFolder = folder + @"\temp\";
                        if (!Directory.Exists(tempFolder))
                            Directory.CreateDirectory(tempFolder);
                        var files = Directory.GetFiles(folder, "*.*", SearchOption.TopDirectoryOnly).Where(f => !f.EndsWith("Thumbs.db")); ;
                        foreach (var file in files)
                        {
                            if (!videoFiles.Contains(file.ToLower()))
                            {
                                File.Move(file, tempFolder + Path.GetFileName(file));
                                count++;
                            }
                        }
                    }
                    Console.WriteLine("Video File: Move unused files to temp folder. Total - " + count);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error-checkVideoFiles: " + ex.Message);
                }
            }
            Console.WriteLine(count);
        }
        static void Main(string[] args)
        {
            //checkThumbnailFiles();
            //checkReporterPhotos();
            //checkVideoCategoryPhotos();
            //checkVideoFiles();
            Console.WriteLine("Start");
            try
            {
                Console.WriteLine("Start 1");
                var context = new VITVContext();
                Console.WriteLine(context.Videos.Count());

                foreach (var video in context.Videos)
                {
                    if (video.Duration.TotalSeconds != 0)
                        break;
                    Console.WriteLine(video.Url);
                    var videoPath = GetPhysicalPath(video.Url);
                    Console.WriteLine(videoPath);
                    if (videoPath != "")
                    {
                        string ffMPEG = @"\\10.0.0.10\web\FFmpeg\ffmpeg.exe";
                        string username = "uploaduser";
                        string password = "Upload@@123";
                        System.Security.SecureString ssPwd = new System.Security.SecureString();
                        for (int x = 0; x < password.Length; x++)
                        {
                            ssPwd.AppendChar(password[x]);
                        }
                        string vArgs = string.Format("-i \"{0}\"", videoPath);

                        var thumbProc = new Process
                        {
                            StartInfo =
                            {
                                FileName = ffMPEG,
                                Arguments = vArgs,
                                UseShellExecute = false,
                                CreateNoWindow = true,
                                UserName = username,
                                Password = ssPwd,
                                LoadUserProfile = false,
                                RedirectStandardOutput = false,
                                WorkingDirectory = Path.GetDirectoryName(videoPath), //important: http://stackoverflow.com/questions/990562/win32exception-the-directory-name-is-invalid
                                RedirectStandardError = true
                            }
                        };

                        try
                        {
                            thumbProc.Start();
                            var SROutput = thumbProc.StandardError;
                            var outPut = SROutput.ReadToEnd();

                            var re = new System.Text.RegularExpressions.Regex("[D|d]uration:.((\\d|:|\\.)*)");
                            var m = re.Match(outPut);

                            thumbProc.WaitForExit();
                            thumbProc.Close();

                            if (m.Success)
                            {
                                //Means the output has cantained the string "Duration"
                                string temp = m.Groups[1].Value;
                                string[] timepieces = temp.Split(new char[] { ':', '.' });
                                if (timepieces.Length == 4)
                                {
                                    // Store duration
                                    var videoDuration = new TimeSpan(0, Convert.ToInt16(timepieces[0]), Convert.ToInt16(timepieces[1]), Convert.ToInt16(timepieces[2]), Convert.ToInt16(timepieces[3]));
                                    video.Duration = videoDuration;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }

                    }
                    Console.WriteLine(video.Id);
                   
                } 
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            
            Console.WriteLine("Completed");
            Console.ReadLine();
        }
    }
}
