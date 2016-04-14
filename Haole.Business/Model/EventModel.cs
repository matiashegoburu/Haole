using System.Drawing;

namespace Haole.Business.Model
{
    public class FlyerModel
    {
        public FlyerModel()
        {
            BackgroundImageFilename = "paper_texture3873.jpg";
        }

        public string BaseUrl { get; set; }
        public int FlyerWidth { get; set; }
        public string EventName { get; set; }
        public int EventNameFontSize { get; set; }
        public string EventDescription { get; set; }
        public int EventDescriptionFontSize { get; set; }
        public string FileName { get; set; }
        public string BackgroundImageFilename { get; set; }
        public Image Image { get; set; }
    }
}
