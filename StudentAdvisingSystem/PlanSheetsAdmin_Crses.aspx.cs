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

namespace StudentAdvisingSystem
{
    public partial class PlanSheetsAdmin_Crses : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                formfield.Text = (string)Request.Params["formfield"];
                Course_Id.Text = (string)Request.Params["Course_Id"];
                Class_Subject.Text = (string)Request.Params["Class_Subject"];
                Class_Number.Text = (string)Request.Params["Class_Number"];
                Literal1.Text = "";

                SqlCollection objSqlCollection = new SqlCollection();
                Course_Id_DG.DataSource = objSqlCollection.Get_Course_Id(Course_Id.Text.ToString(), Class_Subject.Text.ToString(), Class_Number.Text.ToString());
                Course_Id_DG.DataBind();
            }
        }

        protected void Course_Id_Selected(object sender, DataGridCommandEventArgs e)
        {
            string strjscript = "<script language=javascript>";
            strjscript = strjscript + "window.opener.PlanSheetsAdmin." + formfield.Text.ToString() + "$Course_Id.value='" + Course_Id_DG.DataKeys[e.Item.ItemIndex].ToString() + "';";
            strjscript = strjscript + "window.opener.PlanSheetsAdmin." + formfield.Text.ToString() + "$Class_Subject.value='" + Course_Id_DG.Items[e.Item.ItemIndex].Cells[2].Text.ToString() + "';";
            strjscript = strjscript + "window.opener.PlanSheetsAdmin." + formfield.Text.ToString() + "$Class_Number.value='" + Course_Id_DG.Items[e.Item.ItemIndex].Cells[3].Text.ToString() + "';";

            strjscript = strjscript + "window.close();";
            strjscript = strjscript + "</script" + ">";
            Literal1.Text = strjscript;
        }
    }
}
