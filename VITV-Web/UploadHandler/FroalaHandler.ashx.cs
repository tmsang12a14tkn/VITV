using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using VITV.Data.Helpers;

namespace VITV_Web.UploadHandler
{
    /// <summary>
    /// Summary description for FroalaHandler
    /// </summary>
    /// 

    public class FroalaHandler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var files = context.Request.Files;
            if (files.Keys.Count > 0)
            {
                foreach (string fileKey in files)
                {
                    var file = context.Request.Files[fileKey];
                    if (file == null || file.ContentLength == 0)
                        continue;

                    string fileName = Convert.ToInt32((DateTime.Now - new DateTime(2010, 01, 01)).TotalSeconds) + "_" + file.FileName.Replace(' ', '-');
                    string currentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
                    string folder = "UploadedImages/Article";
                    string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + @"\" + folder + @"\" + fileName;

                    using (new Impersonator("uploaduser", "", "Upload@@123"))
                    {
                        file.SaveAs(filePath);
                    }

                    var json = new JavaScriptSerializer().Serialize(new { link = currentDomain + "/" + folder + "/" + fileName });

                    context.Response.ContentType = "text/plain";
                    context.Response.Write(json);
                    context.Response.End();
                }
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write("");
            context.Response.End();
        }

        #endregion
    }
}