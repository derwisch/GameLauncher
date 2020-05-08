using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GameLauncher.Misc
{
    internal static class Extensions
    {
        /// <summary>
        /// Combines two paths.
        /// </summary>
        /// <param name="pathBase">First part of the final path</param>
        /// <param name="pathExtension">Second part of the final path</param>
        /// <returns>Both paths combined with a check to sort out \\ midpath</returns>
        public static string CombinePath(this string pathBase, string pathExtension)
        {
            string str1 = pathBase;
            string str2 = pathExtension;
            if (!str1.EndsWith("\\"))
                str1 += "\\";
            if (str2.StartsWith("\\"))
                str2.Remove(0, 1);
            return str1 + str2;
        }

        /// <summary>
        /// Wraps text in the HTML for display in the web browser control
        /// </summary>
        /// <param name="text">Plain text</param>
        /// <returns>html encoded and formatted text</returns>
        public static string WrapAsHTML(this string text)
        {
            string result = text;

            result = HttpUtility.HtmlEncode(result);

            result = result.Replace("\r", String.Empty);
            result = result.Replace("\n", "<br />");

#pragma warning disable CS0162
            if (LauncherSetup.ADDITIONAL_HTML_REPLACEMENTS)
            {
                result = result.Replace("---", "<hr />");
                result = result.Replace("**(", "<b>");
                result = result.Replace(")**", "</b>");
                result = result.Replace("//(", "<i>");
                result = result.Replace(")//", "</i>");
                result = result.Replace("__(", "<u>");
                result = result.Replace(")__", "</u>");
            }
#pragma warning restore CS0162

            return $"<html><body style='{LauncherSetup.TEXT_CHANGELOG_STYLE}'>{result}</body></html>";
        }
    }
}
