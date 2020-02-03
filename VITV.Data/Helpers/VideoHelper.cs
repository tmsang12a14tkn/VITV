using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace VITV.Data.Helpers
{
    public class VideoHelper
    {
        public static bool IsYouTubeUrl(string testUrl)
        {
            return TestUrl(@"^(http://youtu\.be/([a-zA-Z0-9]|_)+($|\?.*)|https?://www\.youtube\.com/watch\?v=([a-zA-Z0-9]|_)+($|&).*)", testUrl);
        }

        private static bool TestUrl(string pattern, string testUrl)
        {
            var l_expression = new Regex(pattern, RegexOptions.IgnoreCase);

            return l_expression.Matches(testUrl).Count > 0;
        }

        public static string GetYouTubeVideo(string testUrl)
        {
            return GetVideo(@"(/[^watch]|=)([a-zA-z0-9]|_)+($|(\?|&))", @"([a-zA-z0-9]|_)+", testUrl);
        }

        private static string GetVideo(string overallPattern, string videoPattern, string testUrl)
        {
            if (!string.IsNullOrEmpty(testUrl))
            {
                Regex l_overallExpression = new Regex(overallPattern, RegexOptions.IgnoreCase);
                MatchCollection l_overallMatches = l_overallExpression.Matches(testUrl);

                if (l_overallMatches.Count > 0)
                {
                    Regex l_videoExpression = new Regex(videoPattern, RegexOptions.IgnoreCase);

                    return l_videoExpression.Matches(l_overallMatches[0].Value)[0].Value;
                }
            }
            return "";
        }
    }
}