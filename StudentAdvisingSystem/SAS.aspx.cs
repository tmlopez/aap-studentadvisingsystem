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
    public partial class SAS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Office_Cd.Text = (string)Request.Params["dept"];

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
                CurrentDate_lbl.Text = DateTime.Now.ToLongDateString();

                SignIn.NavigateUrl = SignIn.NavigateUrl.ToString() + "?dept=" + Office_Cd.Text.ToString();
                Screening.NavigateUrl = Screening.NavigateUrl.ToString() + "?dept=" + Office_Cd.Text.ToString();
                AdvisorScreen.NavigateUrl = AdvisorScreen.NavigateUrl.ToString() + "?dept=" + Office_Cd.Text.ToString();
                SignIn_NoScreening.NavigateUrl = SignIn_NoScreening.NavigateUrl.ToString() + "&dept=" + Office_Cd.Text.ToString();
                SASAdmin.NavigateUrl = SASAdmin.NavigateUrl.ToString() + "?dept=" + Office_Cd.Text.ToString();

                if (Office_Cd.Text.Contains("AAP"))
                {
                    Reporting.Text = "SAS Database (CRA Job# 1021 & 1025)";
                }
                if (Office_Cd.Text.Contains("BUS"))
                {
                    Reporting.Text = "SAS Database (CRA Job# 1022 & 1026)";
                }

                Student_Search_Title_lbl.Visible = false;
                Student_Search_txt.Visible = false;
                Student_Search_btn.Visible = false;
                Student_Search_DG.Visible = false;
            }
        }

        protected void StudentSearch_click(object sender, EventArgs e)
        {
            Student_Search_Title_lbl.Visible = true;
            Student_Search_txt.Visible = true;
            Student_Search_btn.Visible = true;
            Student_Search_DG.Visible = true;
            Student_Search_txt.Text = "";
            Student_Search_lbl.Text = "";
        }

        BoundColumn CreateBoundColumn(String DataFieldValue, String HeaderTextValue)
        {

            // This version of the CreateBoundColumn method sets only the
            // DataField and HeaderText properties.

            // Create a BoundColumn.
            BoundColumn column = new BoundColumn();

            // Set the properties of the BoundColumn.
            column.DataField = DataFieldValue;
            column.HeaderText = HeaderTextValue;

            return column;

        }

        protected void Student_Search_btn_click(object sender, EventArgs e)
        {
            Student_Search_lbl.Text = Student_Search_txt.Text.ToString();
            BindStudent_Search();
            Student_Search_txt.Text = "";            
        }

        private void BindStudent_Search()
        {
            SqlCollection objSqlCollection = new SqlCollection();
            DataSet objSS_ds = objSqlCollection.Get_StudentSearch(Office_Cd.Text.ToString(), Student_Search_lbl.Text.ToString());
            if (Student_Search_DG.Columns.Count == 1)
            {
                Student_Search_DG.Columns.Add(CreateBoundColumn("StudentId", "Student Id"));
                Student_Search_DG.Columns.Add(CreateBoundColumn("FirstName", "First Name"));
                Student_Search_DG.Columns.Add(CreateBoundColumn("LastName", "Last Name"));
                Student_Search_DG.Columns.Add(CreateBoundColumn("Office_Cd", "Office"));
                Student_Search_DG.Columns.Add(CreateBoundColumn("Visit_DateTime", "Time"));
                Student_Search_DG.Columns.Add(CreateBoundColumn("Office_Status", "Office Status"));

            }
            Student_Search_DG.DataSource = objSS_ds;
            Student_Search_DG.DataBind();
            Student_Search_DG.Focus();
        }

        protected void Student_Search_View(object sender, DataGridCommandEventArgs e)
        {
            if (e.Item.ItemType.ToString() != "Pager")
            {
                Response.Redirect("StudentInfo.aspx" + Student_Search_DG.DataKeys[e.Item.ItemIndex].ToString());
            }
        }

        protected void Student_Search_Paging(object sender, DataGridPageChangedEventArgs e)
        {
            Student_Search_DG.CurrentPageIndex = e.NewPageIndex;
            BindStudent_Search();            
        }
    }
}
