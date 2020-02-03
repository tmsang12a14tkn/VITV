using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using System.Security.Principal;
using VITV.Web.Helpers;
using VITV.Data.Helpers;
using System.Diagnostics;

namespace VITV.Web
{
    /// <summary>
    /// Summary description for UploadHandler
    /// </summary>
    public class VideoUploadHandler : IHttpHandler
    {
        private readonly JavaScriptSerializer js;
        public VideoUploadHandler()
        {
            js = new JavaScriptSerializer();
        }
        public void ProcessRequest(HttpContext context)
        {
            
            context.Response.ContentType = "application/json";
            //context.Response.Expires = -1;
            try
            {
                HttpPostedFile postedFile = context.Request.Files["file_upload"];
                
                string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                string physicalStoragePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain];
                string videoFolder = System.Configuration.ConfigurationManager.AppSettings["UploadedVideoFolder"];
                string fullPath = physicalStoragePath + @"\" + videoFolder;
                
                string extension = Path.GetExtension(postedFile.FileName);
                string timeUpload = DateTime.Now.ToString("yyyyMMddHHmmss");

                string fileName = string.Empty;
                if (context.Request.Form["DisplayTime"] != null)
                {
                    string date = context.Request.Form["DisplayTime"].ToString().Split(',')[0];
                    string categoryName = context.Request.Form["categoryname"].ToString();
                    DateTime displayTime = DateTime.Parse(date);
                    fileName = fileName = categoryName + "_" + displayTime.Year.ToString() + "-" + displayTime.Month.ToString() + "-" + displayTime.Day.ToString() + "-" + displayTime.Hour + "h" + displayTime.Minute + "m_" + timeUpload + extension;
                }
                else 
                {
                    fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + postedFile.FileName;   
                }
                 
                string filePath = fullPath + @"\" + fileName;
                string fileUrl = currentDomain + "/" + videoFolder + "/" + fileName;

                if (File.Exists(filePath))
                {
                    string tempFileUrl = currentDomain + "/" + videoFolder + "/temp/" + fileName;
                    string tempFilePath = fullPath + @"\temp\" + fileName;
                    using (new Impersonator("uploaduser", "", "Upload@@123"))
                    {
                        postedFile.SaveAs(tempFilePath);
                    }
                    var duration = GetVideoDuration(tempFilePath, physicalStoragePath);
                    context.Response.Write(js.Serialize(new { success = false, error = "tập tin đã tồn tại", errorcode = 1, videourl = fileUrl, tempvideourl = tempFileUrl, duration = duration.ToString(@"hh\:mm\:ss") }));
                }
                else
                {
                    using (new Impersonator("uploaduser", "", "Upload@@123"))
                    {
                        postedFile.SaveAs(filePath);
                    }
                    var duration = GetVideoDuration(filePath, physicalStoragePath);
                    context.Response.Write(js.Serialize(new { success = true, videourl = fileUrl, duration = duration.ToString(@"hh\:mm\:ss") }));
                    context.Response.StatusCode = 200;
                }
            }

            catch (Exception ex)
            {
                context.Response.Write(js.Serialize(new { success = false, error = ex.Message, errorcode = 2 }));
            }
        }

        private TimeSpan GetVideoDuration(string videoPath, string physicalPath)
        {
            string ffMPEG = physicalPath + @"\FFmpeg\ffmpeg.exe";
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
                        return videoDuration;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return new TimeSpan();
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}