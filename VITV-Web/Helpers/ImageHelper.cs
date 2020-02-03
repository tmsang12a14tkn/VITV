using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace VITV.Web.Helpers
{
    public class ImageHelper
    {
        public static string ScaleBySize(string filePath, string newFilePath, int size)
        {
            Image imgPhoto = Image.FromFile(filePath);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int destHeight = 0;
            int destWidth = 0;

            if (sourceWidth > sourceHeight)
            {
                destWidth = size;
                destHeight = sourceHeight * size / sourceWidth;
            }
            else
            {
                destHeight = size;
                destWidth = sourceWidth * size / sourceHeight;
            }

            return ScalePhoto(imgPhoto, newFilePath, destWidth, destHeight);
        }

        public static string ScaleHeight(string filePath, string newFilePath, int height)
        {
            Image imgPhoto = Image.FromFile(filePath);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;


            int destHeight = height;
            int destWidth = sourceWidth * height / sourceHeight;

            return ScalePhoto(imgPhoto, newFilePath, destWidth, destHeight);

        }

        private static string ScalePhoto(Image imgPhoto, string newFilePath, int destWidth, int destHeight)
        {

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight,
                                                   System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(0, 0, destWidth, destHeight),
                new Rectangle(0, 0, imgPhoto.Width, imgPhoto.Height),
                GraphicsUnit.Pixel);

            imgPhoto.Dispose();
            grPhoto.Dispose();

            bmPhoto.Save(newFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);

            return newFilePath;
        }



        public static string RandomFileString()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            GuidString = GuidString.Replace("/", "");
            return GuidString;
        }
    }
}