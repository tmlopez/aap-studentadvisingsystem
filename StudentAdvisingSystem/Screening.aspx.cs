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
    public partial class Screening : System.Web.UI.Page
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
                Office_Descr2_lbl.Text = Office_Descr_lbl.Text.ToString();
                Office_Location_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_Location"].ToString();
                Office_PhoneExt_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_PhoneExt"].ToString();
                CurrentDate_lbl.Text = DateTime.Now.ToLongDateString();

                BindStudents_Screening();
                BindStudents_AdvisorScreen();
            }
        }

        private void BindStudents_Screening()
        {
            SqlCollection objSqlCollection = new SqlCollection();
            DataSet Students_Screening_DS = objSqlCollection.Get_Students_Screening(Office_Cd.Text.ToString());
            Screening_DL.DataSource = Students_Screening_DS;
            Screening_DL.DataBind();

            for (int i = 0; i <= Screening_DL.Items.Count - 1; i++)
            {
                DropDownList objOffice_Status = (DropDownList)Screening_DL.Items[i].FindControl("Office_Status");
                objOffice_Status.DataSource = objSqlCollection.Get_Office_Status(Students_Screening_DS.Tables[0].Rows[i]["Office_Cd"].ToString());
                objOffice_Status.DataBind();
                for (int n = 0; n <= objOffice_Status.Items.Count - 1; n++)
                {
                    if (objOffice_Status.Items[n].Value.ToString() == "In Progress")
                    {
                        objOffice_Status.Items[n].Enabled = false;
                    }
                    if (objOffice_Status.Items[n].Value.ToString() == Students_Screening_DS.Tables[0].Rows[i]["Office_Status"].ToString())
                    {
                        objOffice_Status.Items[n].Selected = true;
                    }
                }

                CheckBoxList objQuestions_chkl = (CheckBoxList)Screening_DL.Items[i].FindControl("Questions_chkl");
                if (objQuestions_chkl != null)
                {
                    objQuestions_chkl.DataSource = objSqlCollection.Get_QuesFields_dtl(Students_Screening_DS.Tables[0].Rows[i]["Office_Cd"].ToString());
                    objQuestions_chkl.DataBind();
                    string strQues = Students_Screening_DS.Tables[0].Rows[i]["Ques"].ToString();                 
                    string[] br = new string[] { "<br />" };
                                                       
                    foreach (string subQues in strQues.Split(br, StringSplitOptions.RemoveEmptyEntries))
                    {
                        for (int n = 0; n <= objQuestions_chkl.Items.Count - 1; n++)
                        {       
                            if (objQuestions_chkl.Items[n].Value.ToString() == subQues)
                            {
                                objQuestions_chkl.Items[n].Selected = true;
                                break;
                            }
                            if (subQues.IndexOf("Other:") >= 0)
                            {
                                CheckBox objQuestions_Other_chk = (CheckBox)Screening_DL.Items[i].FindControl("Questions_Other_chk");
                                TextBox objQuestions_Other_txt = (TextBox)Screening_DL.Items[i].FindControl("Questions_Other_txt");
                                objQuestions_Other_chk.Checked = true;
                                objQuestions_Other_txt.Text = subQues.Substring(subQues.IndexOf("Other:") + 7);
                                break;
                            }
                        }
                    }
                }
            }
        }

        protected void Edit_Screening(object sender, DataListCommandEventArgs e)
        {
            Screening_DL.EditItemIndex = e.Item.ItemIndex;
            BindStudents_Screening();
            Screening_DL.Items[e.Item.ItemIndex].Focus();
        }

        protected void Cancel_Screening(object sender, DataListCommandEventArgs e)
        {
            Screening_DL.EditItemIndex = -1;
            BindStudents_Screening();
        }

        protected void Delete_Screening(object sender, DataListCommandEventArgs e)
        {
            SqlCollection objSqlCollection = new SqlCollection();
            objSqlCollection.Delete_Students(Screening_DL.DataKeys[e.Item.ItemIndex].ToString());
            Screening_DL.EditItemIndex = -1;
            BindStudents_Screening();
        }

        protected void Update_Screening(object sender, DataListCommandEventArgs e)
        {
            string strUpdateSql = "", strQuestions = "";            
            DropDownList objOffice_Status = (DropDownList)Screening_DL.Items[e.Item.ItemIndex].FindControl("Office_Status");
            if (objOffice_Status.SelectedItem.Text.IndexOf("Refer:") >= 0)
            {
                strUpdateSql = "Office_Cd = '" + objOffice_Status.SelectedItem.Value.ToString() + "'";
            }
            else if (objOffice_Status.SelectedItem.Text.IndexOf("See:") >= 0)
            {
                strUpdateSql = "Office_Status = 'Waiting', AdvisorEmplid = '" + objOffice_Status.SelectedItem.Value.ToString() + "'";
            }
            else
            {
                strUpdateSql = "Office_Status = '" + objOffice_Status.SelectedItem.Value.ToString() + "'";
            }

            TextBox objStudentId = (TextBox)Screening_DL.Items[e.Item.ItemIndex].FindControl("StudentId_txt");
            CheckBoxList objQuestions_chkl = (CheckBoxList)Screening_DL.Items[e.Item.ItemIndex].FindControl("Questions_chkl");
            if (objQuestions_chkl != null && objStudentId != null)
            {
                strUpdateSql = strUpdateSql + ", StudentId = '" + objStudentId.Text.ToString() + "'";

                for (int i = 0; i <= objQuestions_chkl.Items.Count - 1; i++)
                {
                    if (objQuestions_chkl.Items[i].Selected)
                    {
                        strQuestions = strQuestions + objQuestions_chkl.Items[i].Value.ToString() + "<br />";
                    }
                }
                CheckBox objQuestions_Other_chk = (CheckBox)Screening_DL.Items[e.Item.ItemIndex].FindControl("Questions_Other_chk");
                TextBox objQuestions_Other_txt = (TextBox)Screening_DL.Items[e.Item.ItemIndex].FindControl("Questions_Other_txt");
                if ((objQuestions_Other_chk.Checked) && (objQuestions_Other_txt.Text.ToString() != ""))
                {
                    strQuestions = strQuestions + objQuestions_Other_chk.Text.ToString() + ": " + objQuestions_Other_txt.Text.ToString();
                }
            }
            if (strQuestions != "")
            {
                strUpdateSql = strUpdateSql + ", Ques = '" + strQuestions + "'";
            }

            SqlCollection objSqlCollection = new SqlCollection();
            objSqlCollection.Update_Students(Screening_DL.DataKeys[e.Item.ItemIndex].ToString(), strUpdateSql);
            Screening_DL.EditItemIndex = -1;
            BindStudents_Screening();
            BindStudents_AdvisorScreen();
        }

        private void BindStudents_AdvisorScreen()
        {
            SqlCollection objSqlCollection = new SqlCollection();
            DataSet Students_AdvisorScreen_DS = objSqlCollection.Get_Students_AdvisorScreen_FrontDesk(Office_Cd.Text.ToString());
            AdvisorScreen_DL.DataSource = Students_AdvisorScreen_DS;
            AdvisorScreen_DL.DataBind();

            for (int i = 0; i <= AdvisorScreen_DL.Items.Count - 1; i++)
            {
                Label objOffice_Status_lbl = (Label)AdvisorScreen_DL.Items[i].FindControl("Office_Status_lbl");
                if (objOffice_Status_lbl != null)
                {
                    if (Students_AdvisorScreen_DS.Tables[0].Rows[i]["Office_Status"].ToString() == "In Progress")
                    {
                        objOffice_Status_lbl.ForeColor = System.Drawing.Color.MediumBlue;
                    }
                    else
                    {
                        objOffice_Status_lbl.ForeColor = System.Drawing.Color.Black;
                    }
                }

                //DropDownList objOffice_Status = (DropDownList)AdvisorScreen_DL.Items[i].FindControl("Office_Status");
                //if (objOffice_Status != null)
                //{
                //    objOffice_Status.DataSource = objSqlCollection.Get_Office_Status(Office_Cd.Text.ToString());
                //    objOffice_Status.DataBind();

                //    for (int n = 0; n <= objOffice_Status.Items.Count - 1; n++)
                //    {
                //        if (Students_AdvisorScreen_DS.Tables[0].Rows[i]["Office_Status"].ToString() == "Waiting" && objOffice_Status.Items[n].Value.ToString() == Students_AdvisorScreen_DS.Tables[0].Rows[i]["AdvisorEmplid"].ToString())
                //        {
                //            objOffice_Status.Items[n].Selected = true;
                //        }
                //        else if (objOffice_Status.Items[n].Value.ToString() == Students_AdvisorScreen_DS.Tables[0].Rows[i]["Office_Status"].ToString())
                //        {
                //            objOffice_Status.Items[n].Selected = true;
                //        }
                //    }

                //    CheckBoxList objQuestions_chkl = (CheckBoxList)AdvisorScreen_DL.Items[i].FindControl("Questions_chkl");

                //    objQuestions_chkl.DataSource = objSqlCollection.Get_QuesFields_dtl(Office_Cd.Text.ToString());
                //    objQuestions_chkl.DataBind();
                //    string strQues = Students_AdvisorScreen_DS.Tables[0].Rows[i]["Ques"].ToString();
                //    string[] br = new string[] { "<br />" };

                //    foreach (string subQues in strQues.Split(br, StringSplitOptions.RemoveEmptyEntries))
                //    {
                //        for (int n = 0; n <= objQuestions_chkl.Items.Count - 1; n++)
                //        {
                //            if (objQuestions_chkl.Items[n].Value.ToString() == subQues)
                //            {
                //                objQuestions_chkl.Items[n].Selected = true;
                //                break;
                //            }
                //            if (subQues.IndexOf("Other:") >= 0)
                //            {
                //                CheckBox objQuestions_Other_chk = (CheckBox)AdvisorScreen_DL.Items[i].FindControl("Questions_Other_chk");
                //                TextBox objQuestions_Other_txt = (TextBox)AdvisorScreen_DL.Items[i].FindControl("Questions_Other_txt");
                //                objQuestions_Other_chk.Checked = true;
                //                objQuestions_Other_txt.Text = subQues.Substring(subQues.IndexOf("Other:") + 7);
                //                break;
                //            }
                //        }
                //    }
                //}
            }         
        }

    }
}
