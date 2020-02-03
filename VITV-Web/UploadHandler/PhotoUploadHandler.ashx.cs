using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;

namespace VITV.Web
{
    /// <summary>
    /// Summary description for UploadHandler
    /// </summary>
    public class PhotoUploadHandler : IHttpHandler
    {
        private readonly JavaScriptSerializer js;
        public PhotoUploadHandler()
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

                string savepath = "";
                string tempPath = "";
                tempPath = System.Configuration.ConfigurationManager.AppSettings["UploadedPhotoPath"];
                savepath = HttpContext.Current.Server.MapPath("~" + tempPath);
                string filename = postedFile.FileName;
                if (!Directory.Exists(savepath))
                    Directory.CreateDirectory(savepath);

                string filePath = savepath + @"\" + filename;
                string fileUrl = tempPath + "/" + filename;
                if (File.Exists(filePath))
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    if (fileInfo.Length == postedFile.ContentLength)
                    {
                        context.Response.Write(js.Serialize(new { success = true, videourl = fileUrl }));
                    }
                    else
                    {
                        string guild = Guid.NewGuid().ToString();
                        filePath = savepath + @"\" + guild + filename;
                        fileUrl = tempPath + "/" + guild + filename;
                        postedFile.SaveAs(filePath);
                        context.Response.Write(js.Serialize(new { success = true, videourl = fileUrl }));
                    }
                }
                else
                {
                    postedFile.SaveAs(filePath);

                    context.Response.Write(js.Serialize(new { success = true, videourl = fileUrl }));
                    context.Response.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(js.Serialize(new { success = false, error = ex.Message, errorcode = 2}));
            }
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