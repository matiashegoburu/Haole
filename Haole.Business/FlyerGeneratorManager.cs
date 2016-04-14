using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haole.Business
{
    public class FlyerGeneratorManager : IDisposable
    {
        private const int WIDTH = 800;

        private readonly string _basePath;
        private readonly PrivateFontCollection _fontCollection;
        private readonly Brush _brush;
        private readonly Font _fontEventName;
        private readonly Font _fontEventDescription;
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

            _fontEventName = new Font(_fontCollection.Families.Last(), 60);
            _fontEventDescription = new Font("Georgia", 20);
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

        public void Generate(string eventName, string eventDescription, string filename, Image img)
        {
            RectangleF eventNameRect = RectangleF.Empty;
            RectangleF eventDescriptionRect = RectangleF.Empty;
            RectangleF imgRect = RectangleF.Empty;
            RectangleF footerRect = RectangleF.Empty;

            using (var resizedImage = ScaleImage(img, WIDTH))
            {
                using (var graphic = Graphics.FromImage(new Bitmap(1, 1)))
                {
                    eventNameRect = new RectangleF(0, 10, WIDTH, graphic.MeasureString(eventName, _fontEventName).Height);
                    eventDescriptionRect = new RectangleF(0, eventNameRect.Height, WIDTH, graphic.MeasureString(eventDescription, _fontEventDescription).Height);

                    if (resizedImage != null)
                    {
                        imgRect = new RectangleF(0, eventDescriptionRect.Bottom + 20, WIDTH, resizedImage.Height);
                        footerRect = new RectangleF(0, imgRect.Bottom + 13, WIDTH, 110);
                    }
                    else
                    {
                        footerRect = new RectangleF(0, eventDescriptionRect.Bottom + 100, WIDTH, 110);
                    }
                }

                using (var bitmap = new Bitmap(WIDTH, (int)footerRect.Bottom))
                {
                    using (var graphic = Graphics.FromImage(bitmap))
                    {
                        using (var background = Bitmap.FromFile(Path.Combine(_basePath, "Content", "Flyers", "Fondos", "paper_texture3873.jpg")))
                        {
                            var eventNameSize = graphic.MeasureString(eventName, _fontEventName);
                            var eventDescriptionSize = graphic.MeasureString(eventDescription, _fontEventDescription);

                            graphic.DrawImage(background, 0, 0);
                            graphic.DrawString(eventName, _fontEventName, _brush, eventNameRect, _stringFormat);
                            graphic.DrawString(eventDescription, _fontEventDescription, _brush, eventDescriptionRect, _stringFormat);

                            if (resizedImage != null)
                            {
                                graphic.DrawImage(resizedImage, imgRect);

                                using (var pen = new Pen(_brush, 3))
                                {
                                    graphic.DrawLine(pen, imgRect.Left, imgRect.Top, imgRect.Right, imgRect.Top);
                                    graphic.DrawLine(pen, imgRect.Left, imgRect.Bottom, imgRect.Right, imgRect.Bottom);
                                }
                            }

                            DrawFooter(bitmap, graphic, footerRect);

                            bitmap.Save(Path.Combine(_basePath, filename), ImageFormat.Png);
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
