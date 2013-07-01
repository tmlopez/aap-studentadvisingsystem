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
    public partial class AdvisorComments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Office_Cd.Text = (string)Request.Params["dept"];
                PkId.Text = (string)Request.Params["pkid"];
                string strPrintVersion = (string)Request.Params["aid"];
                StudentId_lbl.Text = (string)Request.Params["sid"];               

                if (Office_Cd.Text.ToString() == "")
                {
                    Office_Cd.Text = "AAP";
                }
                if (strPrintVersion == "")
                {
                    strPrintVersion = "0";
                }
                SqlCollection objSqlCollection = new SqlCollection();
                DataSet OfficeInfo_DS = objSqlCollection.Get_OfficeInfo(Office_Cd.Text.ToString());
                Office_Descr_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_Descr"].ToString();
                Office_Descr1_lbl.Text = Office_Descr_lbl.Text.ToString();
                Office_Location_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_Location"].ToString();
                Office_PhoneExt_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_PhoneExt"].ToString();
                //CurrentDate_lbl.Text = DateTime.Now.ToLongDateString();
                if (PkId.Text.Length > 0)
                {
                    DataSet StudentInfo_DS = objSqlCollection.Get_Students(PkId.Text.ToString(), Office_Cd.Text.ToString());
                    if (StudentInfo_DS.Tables[0].Rows.Count == 1)
                    {
                        DataRow dr = StudentInfo_DS.Tables[0].Rows[0];
                        FirstName.Text = dr["FirstName"].ToString();
                        LastName.Text = dr["LastName"].ToString();
                        StudentId_lbl.Text = dr["StudentId"].ToString();
                        Visit_Date.Text = dr["Visit_DateTime"].ToString();
                        AdvisorName.Text = dr["AdvisorName"].ToString();
                        Advisors_ddl.Visible = false;
                        if (dr["Office_Status"].ToString() != "Completed")
                        {
                            Back.Visible = false;
                        }                       
                    }
                }
                else if (StudentId_lbl.Text.Length == 9)
                {
                    DataSet StudentInfo_DS = objSqlCollection.Get_Students_PS(StudentId_lbl.Text.ToString(), DateTime.Now);
                    if (StudentInfo_DS.Tables[0].Rows.Count == 1)
                    {
                        DataRow dr = StudentInfo_DS.Tables[0].Rows[0];
                        FirstName.Text = dr["first_name_preferred"].ToString();
                        LastName.Text = dr["last_name_preferred"].ToString();
                        StudentId_lbl.Text = dr["emplid"].ToString();
                        Visit_Date.Text = DateTime.Now.ToString();
                        AdvisorName.Visible = false;
                        Advisors_ddl.DataSource = objSqlCollection.Get_Advisors(Office_Cd.Text.ToString());
                        Advisors_ddl.DataBind();
                    }
                                        
                    StudentInfo_btn.Visible = false;
                    Back.Visible = false;
                }

                BindComments_DL();

                if (strPrintVersion == "1")
                {
                    Comments_Add_tbl.Visible = true;
                    PrintVersion.Checked = false;
                }
                else {
                    Comments_Add_tbl.Visible = false;
                    PrintVersion.Checked = true;
                }
            }
        }

        private void BindComments_DL()
        {
            SqlCollection objSqlCollection = new SqlCollection();
            Comments_DL.DataSource = objSqlCollection.Get_AdvisorComments(StudentId_lbl.Text.ToString(), Office_Cd.Text.ToString());
            Comments_DL.DataBind();
            for (int i = 0; i <= Comments_DL.Items.Count - 1; i++)
            {
                Label objComments_lbl = (Label)Comments_DL.Items[i].FindControl("Comments_lbl");
                objComments_lbl.Text = objComments_lbl.Text.Replace("\n", "<br />");
            }
        }

        protected void Comments_Save_Clicked(object sender, EventArgs e)
        {
            SqlCollection objSqlCollection = new SqlCollection();
            if (Advisors_ddl.Visible)
            {
                objSqlCollection.Insert_AdvisorComment(StudentId_lbl.Text.ToString(), Office_Cd.Text.ToString(), Advisors_ddl.SelectedItem.Value.ToString(), Comments.Text.ToString().Replace("'", "''"), ExtViewAllow.SelectedItem.Value.ToString());
            }
            else
            {
                objSqlCollection.Insert_AdvisorComment(PkId.Text.ToString(), Office_Cd.Text.ToString(), Comments.Text.ToString().Replace("'", "''"), ExtViewAllow.SelectedItem.Value.ToString());
            }
            BindComments_DL();
            Comments_Add_tbl.Visible = false;            
        }

        protected void PrintVersion_Checked(object sender, EventArgs e)
        {
            if (PrintVersion.Checked)
            {
                Comments_Add_tbl.Visible = false;
            }
            else
            {
                Comments_Add_tbl.Visible = true;  
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
    }
}
