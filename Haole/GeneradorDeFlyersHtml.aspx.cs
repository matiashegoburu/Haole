using Haole.Business;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Haole
{
    public partial class GeneradorDeFlyersHtml : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            var model = new Business.Model.FlyerModel
            {
                BaseUrl = ConfigurationManager.AppSettings["baseUrl"], 
                EventName = txtNombre.Text,
                EventDescription = txtDescripcion.Text,
                FileName = "flyer2.png"
            };

            if (fupImagen.PostedFile != null && fupImagen.PostedFile.ContentLength > 0)
            {
                model.Image = System.Drawing.Image.FromStream(fupImagen.PostedFile.InputStream);
            }

            try
            {
                new HtmlFlyerGeneratorManager(Server.MapPath("/")).Generate(model);
                imgFlyer.ImageUrl = model.FileName;
            }
            catch (Exception ex) { }
        }
    }
}