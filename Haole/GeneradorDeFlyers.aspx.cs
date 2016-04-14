using Haole.Business;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Haole
{
    public partial class GeneradorDeFlyers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            using (var generator = new FlyerGeneratorManager(Server.MapPath("/")))
            {
                System.Drawing.Image img = null;

                if (fupImagen.PostedFile.ContentLength > 0 && !string.IsNullOrEmpty(fupImagen.PostedFile.FileName))
                {
                    img = Bitmap.FromStream(fupImagen.PostedFile.InputStream);
                }

                var filename = "flyer.png";
                try
                {
                    generator.Generate(new Business.Model.FlyerModel { EventName = txtNombre.Text, EventDescription = txtDescripcion.Text, FileName = filename, Image = img });
                    imgFlyer.ImageUrl = filename;
                }
                catch (Exception ex) { }

                if (img != null)
                    img.Dispose();

            }
        }
    }
}