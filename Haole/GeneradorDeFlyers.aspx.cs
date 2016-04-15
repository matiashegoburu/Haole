using Haole.Business.Templates;
using System;
using System.Drawing;

namespace Haole
{
    public partial class GeneradorDeFlyers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            using (var generator = new TemplateGenericoAriel01(Server.MapPath("/")))
            {
                System.Drawing.Image img = null;

                if (fupImagen.PostedFile.ContentLength > 0 && !string.IsNullOrEmpty(fupImagen.PostedFile.FileName))
                {
                    img = Bitmap.FromStream(fupImagen.PostedFile.InputStream);
                }

                var filename = "flyer.png";
                try
                {
                    generator.Generate(new Business.Model.TemplateGenericoAriel01Model
                    {
                        FlyerWidth = int.Parse(txtFlyerAncho.Text),
                        EventName = txtNombre.Text,
                        EventNameFontSize = int.Parse(txtNombreTamanoFuente.Text),
                        EventDescription = txtDescripcion.Text,
                        EventDescriptionFontSize = int.Parse(txtDescriptionTamanoFuente.Text),
                        EventDescriptionMarginTop = int.Parse(txtDescritionMargenSuperior.Text),
                        FileName = filename,
                        Image = img
                    });
                    imgFlyer.ImageUrl = filename;
                }
                catch (Exception ex) { }

                if (img != null)
                    img.Dispose();

            }
        }
    }
}