using Haole.Business.Model;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace Haole.Business
{
    public class HtmlFlyerGeneratorManager : IFlyerGenerator
    {
        private const int WIDTH = 800;

        private readonly string _basePath;

        public HtmlFlyerGeneratorManager(string basePath)
        {
            _basePath = basePath;
        }

        public void Generate(FlyerModel model)
        {
            var templateManager = new ResolvePathTemplateManager(new[] { Path.Combine(_basePath, "Content", "Templates") });
            var config = new TemplateServiceConfiguration();
            config.TemplateManager = templateManager;
            config.CachingProvider = new DefaultCachingProvider();
            
            var templateService = RazorEngineService.Create(config);
            var result = templateService.RunCompile(Path.Combine(_basePath, "Content", "Templates", "FlyerGenericoAriel01"), model.GetType(), model);

            if (model.Image != null)
            {
                model.Image.Save(Path.Combine(_basePath, "Content", "Uploaded", "image.png"));
            }

            using (var background = Image.FromFile(Path.Combine(_basePath, "Content", "Flyers", "Fondos", model.BackgroundImageFilename)))
            {
                SizeF size = SizeF.Empty;
                using (var graphic = Graphics.FromImage(background))
                {
                    size = HtmlRender.Measure(graphic, result, WIDTH);
                }

                using (var flyer = new Bitmap(WIDTH, (int)size.Height))
                {
                    using (var graphic = Graphics.FromImage(flyer))
                    {
                        graphic.DrawImage(background, Point.Empty);
                        HtmlRender.RenderToImage(flyer, result, PointF.Empty, size);
                    }

                    flyer.Save(Path.Combine(_basePath, model.FileName), ImageFormat.Png);
                }
            }

        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~HtmlFlyerGeneratorManager() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
