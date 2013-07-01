using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using edu.csu.chico.enr.authentication;

namespace StudentAdvisingSystem
{
    public partial class EVAL : System.Web.UI.Page
    {
        protected void SAS_Clicked(object sender, EventArgs e)
        {
            ImageButton SAS_btn = (ImageButton)sender;
            Response.Redirect("SignIn.aspx?dept=" + SAS_btn.CommandName.ToString());
        }
    }
}
