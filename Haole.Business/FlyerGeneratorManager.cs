using Haole.Business.Model;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;

namespace Haole.Business
{
    public class FlyerGeneratorManager : IFlyerGenerator
    {
        private readonly string _basePath;
        private readonly PrivateFontCollection _fontCollection;
        private readonly Brush _brush;
        private Font _fontEventName;
        private Font _fontEventDescription;
        private readonly Font _fontFooter;
        private readonly Font _fontAwesome;
        private readonly StringFormat _stringFormat;

        public FlyerGeneratorManager(string basePath)
        {
            _basePath = basePath;
            _brush = new SolidBrush(Color.Black);
            _fontCollection = new PrivateFontCollection();
            _fontCollection.AddFontFile(Path.Combine(_basePath, "Content", "Fonts", "Vnhltfap.ttf"));
            _fontCollection.AddFontFile(Path.Combine(_basePath, "Content", "Fonts", "fontawesome-webfont.ttf"));

            _fontFooter = new Font("Georgia", 14);
            _fontAwesome = new Font(_fontCollection.Families.First(), 14);

            _stringFormat = new StringFormat();
            _stringFormat.Alignment = StringAlignment.Center;
            _stringFormat.LineAlignment = StringAlignment.Center;
        }

        public void Dispose()
        {
            _brush.Dispose();
            _fontCollection.Dispose();
            _fontEventName.Dispose();
            _fontEventDescription.Dispose();
            _fontFooter.Dispose();
            _fontAwesome.Dispose();
            _stringFormat.Dispose();
        }

        private void InitializeFonts(FlyerModel model)
        {
            _fontEventName = new Font(_fontCollection.Families.Last(), model.EventNameFontSize);
            _fontEventDescription = new Font("Georgia", model.EventDescriptionFontSize);
        }

        public void Generate(FlyerModel model)
        {
            RectangleF eventNameRect = RectangleF.Empty;
            RectangleF eventDescriptionRect = RectangleF.Empty;
            RectangleF imgRect = RectangleF.Empty;
            RectangleF footerRect = RectangleF.Empty;

            InitializeFonts(model);

            using (var resizedImage = ScaleImage(model.Image, model.FlyerWidth))
            {
                #region Calculate placeholders for each section
                // calculate the height and position of each segment of the flyer: name, description, image, and footer.
                using (var graphic = Graphics.FromImage(new Bitmap(1, 1)))
                {
                    eventNameRect = new RectangleF(0, 10, model.FlyerWidth, graphic.MeasureString(model.EventName, _fontEventName).Height);
                    eventDescriptionRect = new RectangleF(0, eventNameRect.Bottom + model.EventDescriptionMarginTop, model.FlyerWidth, graphic.MeasureString(model.EventDescription, _fontEventDescription).Height);

                    if (resizedImage != null)
                    {
                        imgRect = new RectangleF(0, eventDescriptionRect.Bottom + 20, model.FlyerWidth, resizedImage.Height);
                        footerRect = new RectangleF(0, imgRect.Bottom + 13, model.FlyerWidth, 110);
                    }
                    else
                    {
                        footerRect = new RectangleF(0, eventDescriptionRect.Bottom + 100, model.FlyerWidth, 110);
                    }
                }
                #endregion

                // using the info calculated above, now we know how long the flyer should be
                // and we know where to position each segment
                using (var bitmap = new Bitmap(model.FlyerWidth, (int)footerRect.Bottom))
                {
                    using (var graphic = Graphics.FromImage(bitmap))
                    {
                        graphic.TextRenderingHint = TextRenderingHint.AntiAlias;

                        using (var background = Bitmap.FromFile(Path.Combine(_basePath, "Content", "Flyers", "Fondos", "paper_texture3873.jpg")))
                        {
                            // Paint background
                            graphic.DrawImage(background, 0, 0);

                            // Paint event name
                            graphic.DrawString(model.EventName, _fontEventName, _brush, eventNameRect, _stringFormat);

                            // Paint event description right after event name
                            graphic.DrawString(model.EventDescription, _fontEventDescription, _brush, eventDescriptionRect, _stringFormat);

                            // If there is an image, paint it after description and draw top and bottom lines
                            if (resizedImage != null)
                            {
                                graphic.DrawImage(resizedImage, imgRect);

                                using (var pen = new Pen(_brush, 3))
                                {
                                    graphic.DrawLine(pen, imgRect.Left, imgRect.Top, imgRect.Right, imgRect.Top);
                                    graphic.DrawLine(pen, imgRect.Left, imgRect.Bottom, imgRect.Right, imgRect.Bottom);
                                }
                            }

                            // Paint the footer
                            DrawFooter(bitmap, graphic, footerRect);

                            bitmap.Save(Path.Combine(_basePath, model.FileName), ImageFormat.Png);
                        }
                    }
                }
            }
        }

        private void DrawFooter(Bitmap bitmap, Graphics graphic, RectangleF footerRect)
        {
            using (var logo = Bitmap.FromFile(Path.Combine(_basePath, "Content", "Logos", "logo 02.png")))
            {
                graphic.DrawImage(logo, 10, footerRect.Top, 100, 100);
            }

            var sizePhone = graphic.MeasureString("(0351) 15-246-8058", _fontFooter);
            graphic.DrawString("(0351) 15-246-8058", _fontFooter, _brush, (bitmap.Width - sizePhone.Width - 30), footerRect.Top);
            graphic.DrawString("", _fontAwesome, _brush, (bitmap.Width - 30), footerRect.Top);

            var sizeFacebook = graphic.MeasureString("escuelahaole", _fontFooter);
            graphic.DrawString("escuelahaole", _fontFooter, _brush, (bitmap.Width - sizeFacebook.Width - 30), footerRect.Top + sizePhone.Height);
            graphic.DrawString("", _fontAwesome, _brush, (bitmap.Width - 30), footerRect.Top + sizePhone.Height);

            var sizeAddress = graphic.MeasureString("Pje. Bobone 515 esq. Montevideo 1150", _fontFooter);
            graphic.DrawString("Pje. Bobone 515 esq. Montevideo 1150", _fontFooter, _brush, (bitmap.Width - sizeAddress.Width - 30), footerRect.Top + sizePhone.Height + sizeAddress.Height);
            graphic.DrawString("", _fontAwesome, _brush, (bitmap.Width - 30), footerRect.Top + sizePhone.Height + sizeAddress.Height);
        }

        private static Image ScaleImage(Image image, int maxWidth)
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
