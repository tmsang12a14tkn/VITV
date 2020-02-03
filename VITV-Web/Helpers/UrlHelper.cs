using System;
using System.Text;

namespace VITV.Web.Helpers
{
    public class UrlHelper
    {
        public static string GetPhysicalPath(System.Web.Mvc.Controller controller, string url)
        {
            Uri videoUri;
            bool isAbsoluteUrl = Uri.TryCreate(url, UriKind.Absolute, out videoUri);
            if (isAbsoluteUrl)
            {
                string currentDomain = videoUri.GetLeftPart(UriPartial.Authority);
                string filePath = System.Configuration.ConfigurationManager.AppSettings[currentDomain] + System.Net.WebUtility.UrlDecode(videoUri.AbsolutePath);
                return filePath;
            }
            else
                return controller.Server.MapPath(url);
        }
        public static string GetPhysicalFolder(System.Web.Mvc.Controller controller, string url)
        {
            Uri videoUri;
            bool isAbsoluteUrl = Uri.TryCreate(url, UriKind.Absolute, out videoUri);
            if (isAbsoluteUrl)
            {
                string currentDomain = videoUri.GetLeftPart(UriPartial.Authority);
                return System.Configuration.ConfigurationManager.AppSettings[currentDomain];
            }
            else
                return controller.Server.MapPath("~/");
        }
        public static string GetHostName(System.Web.Mvc.Controller controller, string url)
        {
            Uri videoUri;
            bool isAbsoluteUrl = Uri.TryCreate(url, UriKind.Absolute, out videoUri);
            if (isAbsoluteUrl)
            {
                return videoUri.GetLeftPart(UriPartial.Authority);
            }
            else
            {
                return controller.Request.Url.GetLeftPart(UriPartial.Authority);
            }
        }

        public static string URLFriendly(string title)
        {
            if (title == null) return "";

            const int maxlen = 80;
            int len = title.Length;
            bool prevdash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                    c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if ((int)c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (i == maxlen) break;
            }

            if (prevdash)
                return sb.ToString().Substring(0, sb.Length - 1);
            else
                return sb.ToString();
        }

        private static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáäãåąảạăằắẳẳẵặâấầẩẫậ".Contains(s))
            {
                return "a";
            }
            else if ("èéëęẻẽẹêếềểễệ".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïıỉĩị".Contains(s))
            {
                return "i";
            }
            else if ("òóỏọôõöøőðốồổỗộơớờởỡợ".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭůủũụưứừửữự".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿỳỷỹ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if ("đ".Contains(s))
            {
                return "d";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'Þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else
            {
                return "";
            }
        }
    }
}