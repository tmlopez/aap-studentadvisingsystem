using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

namespace StudentAdvisingSystem
{
    public class PhotoHandler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                WCCard.WildCatCardClient wccard = new WCCard.WildCatCardClient("wsHttp");
                // get the client credentials to contact the service.
                // normally get this from an encrypted config file
                // for this demo we are getting it from the input boxes on page one.
                DataClass objDataClass = new DataClass();

                wccard.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings.Get("CHICO_SRVACCT_USRNAME").ToString();
                wccard.ClientCredentials.UserName.Password = objDataClass.DecryptConnectionString(ConfigurationManager.AppSettings.Get("CHICO_SRVACCT_PASSWD").ToString());
                //wccard.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings.Get("CHICO_SRVACCT_PASSWD").ToString(); 

                WCCard.WildCatImage wcimage;

                wcimage = wccard.GetWildCatImageAny(context.Request.QueryString["id"], WCCard.ImageSize.Size100x100);
                //wcimage = wccard.GetWildCatImageAny("000052339", WCCard.ImageSize.Size100x100);
                byte[] _image;
                _image = wcimage.Image;
                context.Response.BinaryWrite(_image);
                wccard.Close();
            }

            catch (Exception ex)
            {
                context.Response.Write("Error: " + ex.Message.ToString());
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
