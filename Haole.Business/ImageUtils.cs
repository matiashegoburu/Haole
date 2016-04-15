using System.Drawing;

namespace Haole.Business
{
    public class ImageUtils : IImageUtils
    {
        public Image ScaleImage(Image image, int maxWidth)
        {
            if (image == null) return null;

            var ratio = (double)maxWidth / image.Width;

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(newImage))
            {
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }
    }
}
