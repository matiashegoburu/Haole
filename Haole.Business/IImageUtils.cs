using System.Drawing;

namespace Haole.Business
{
    public interface IImageUtils
    {
        Image ScaleImage(Image image, int maxWidth);
    }
}