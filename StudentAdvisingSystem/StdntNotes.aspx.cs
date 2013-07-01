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
    public partial class StdntNotes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Office_Cd.Text = (string)Request.Params["dept"];
                PkId.Text = (string)Request.Params["pkid"];

                if (Office_Cd.Text.ToString() == "")
                {
                    Office_Cd.Text = "AAP";
                }                
                SqlCollection objSqlCollection = new SqlCollection();
                DataSet OfficeInfo_DS = objSqlCollection.Get_OfficeInfo(Office_Cd.Text.ToString());
                Office_Descr_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_Descr"].ToString();
                Office_Descr1_lbl.Text = Office_Descr_lbl.Text.ToString();
                Office_Location_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_Location"].ToString();
                Office_PhoneExt_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_PhoneExt"].ToString();
                //CurrentDate_lbl.Text = DateTime.Now.ToLongDateString();
                
                DataSet StudentInfo_DS = objSqlCollection.Get_Students(PkId.Text.ToString(), Office_Cd.Text.ToString());
                if (StudentInfo_DS.Tables[0].Rows.Count == 1)
                {
                        DataRow dr = StudentInfo_DS.Tables[0].Rows[0];
                        FirstName.Text = dr["FirstName"].ToString();
                        LastName.Text = dr["LastName"].ToString();
                        StudentId_lbl.Text = dr["StudentId"].ToString();                       
                        if (dr["Office_Status"].ToString() != "Completed")
                        {
                            Back.Visible = false;
                        }
                 }                
                
                BindStdntNotes_DL();
            }
        }

        private void BindStdntNotes_DL()
        {
            SqlCollection objSqlCollection = new SqlCollection();
            StdntNotes_DL.DataSource = objSqlCollection.Get_StdntNotes(PkId.Text.ToString(), Office_Cd.Text.ToString());
            StdntNotes_DL.DataBind();
            for (int i = 0; i <= StdntNotes_DL.Items.Count - 1; i++)
            {
                Label objNotes_lbl = (Label)StdntNotes_DL.Items[i].FindControl("Notes_lbl");
                objNotes_lbl.Text = objNotes_lbl.Text.Replace("\n", "<br />");
            }
        }

        protected void StudentInfo_Clicked(object sender, EventArgs e)
        {
            Response.Redirect("StudentInfo.aspx?dept=" + Office_Cd.Text.ToString() + "&pkid=" + PkId.Text.ToString());
        }

        protected void Back_AdvisorScreen(object sender, EventArgs e)
        {
            Response.Redirect("AdvisorScreen.aspx?dept=" + Office_Cd.Text.ToString());
        }

        protected void ViewSession_Clicked(object sender, DataListCommandEventArgs e)
        {            
           Response.Redirect("StudentInfo.aspx?dept=" + Office_Cd.Text.ToString() + "&pkid=" + StdntNotes_DL.DataKeys[e.Item.ItemIndex].ToString());
         
        }
    }
}
