using CustomHttpHandler.Configuration;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;

namespace CustomHttpHandler.CustomHandlers
{
    public class ImageConverterHandler : System.Web.IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            using (Bitmap bmap = new Bitmap(context.Request.PhysicalPath))
            {
                var image = WriteTextOnImage(context, bmap);

                var isMobile = context.Request.Browser.IsMobileDevice;

                if (isMobile)
                {
                    context.Response.ContentType = "image/gif";
                    image.Save(context.Response.OutputStream,
                        ImageFormat.Gif);
                }
                else
                {
                    context.Response.ContentType = "image/jpeg";
                    image.Save(context.Response.OutputStream,
                        ImageFormat.Jpeg);
                }
            }
        }

        private Bitmap WriteTextOnImage(HttpContext context, Bitmap image)
        {
            ImageConverterSection config =
                (ImageConverterSection)
                    ConfigurationManager.GetSection("imageConverter");

            if (!config.UseWatermark)
                return image;

            Bitmap tempBitmap = new Bitmap(image.Width, image.Height);

            using (Graphics gphx = Graphics.FromImage(tempBitmap))
            {
                gphx.DrawImage(image, 0, 0);

                Font fontWatermark =
                    new Font("Verdana", 12, FontStyle.Italic);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                var watermarkText = config.WatermarkText;

                if (string.IsNullOrWhiteSpace(watermarkText))
                    watermarkText = " - Protected - ";

                gphx.DrawString(watermarkText, fontWatermark,
                    Brushes.Red,
                    new Rectangle(10, 10,
                    image.Width - 10,
                    image.Height - 10), stringFormat);

                return tempBitmap;
            }
        }

    }
}
