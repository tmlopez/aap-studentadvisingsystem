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
using System.Net.Mail;

namespace StudentAdvisingSystem
{
    public partial class AdvisorScreen : System.Web.UI.Page
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

                StudWaiting.Text = BindStudents_AdvisorScreen().ToString();                               
            }
        }

        private int BindStudents_AdvisorScreen()
        {
            int intStudWaiting = 0;
            string strjscript = "";
            SqlCollection objSqlCollection = new SqlCollection();
            DataSet Students_AdvisorScreen_DS = objSqlCollection.Get_Students_AdvisorScreen(Office_Cd.Text.ToString());
            AdvisorScreen_DL.DataSource = Students_AdvisorScreen_DS;
            AdvisorScreen_DL.DataBind();

            for (int i = 0; i <= AdvisorScreen_DL.Items.Count - 1; i++)
            {
                if (Students_AdvisorScreen_DS.Tables[0].Rows[i]["Office_Status"].ToString() == "Waiting")
                {
                     intStudWaiting += 1;
                }

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

                DropDownList objOffice_Status = (DropDownList)AdvisorScreen_DL.Items[i].FindControl("Office_Status");                
                if (objOffice_Status != null)
                {
                    objOffice_Status.DataSource = objSqlCollection.Get_Office_Status(Office_Cd.Text.ToString());
                    objOffice_Status.DataBind();

                    for (int n = 0; n <= objOffice_Status.Items.Count - 1; n++)
                    {
                        if (Students_AdvisorScreen_DS.Tables[0].Rows[i]["Office_Status"].ToString() == "Waiting" && objOffice_Status.Items[n].Value.ToString() == Students_AdvisorScreen_DS.Tables[0].Rows[i]["AdvisorEmplid"].ToString())
                        {                           
                            objOffice_Status.Items[n].Selected = true;
                        }
                        else if (objOffice_Status.Items[n].Value.ToString() == Students_AdvisorScreen_DS.Tables[0].Rows[i]["Office_Status"].ToString())
                        {
                                objOffice_Status.Items[n].Selected = true;
                        }                                               
                    }

                    CheckBoxList objQuestions_chkl = (CheckBoxList)AdvisorScreen_DL.Items[i].FindControl("Questions_chkl");

                    objQuestions_chkl.DataSource = objSqlCollection.Get_QuesFields_dtl(Office_Cd.Text.ToString());
                    objQuestions_chkl.DataBind();
                    string strQues = Students_AdvisorScreen_DS.Tables[0].Rows[i]["Ques"].ToString();
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
                                CheckBox objQuestions_Other_chk = (CheckBox)AdvisorScreen_DL.Items[i].FindControl("Questions_Other_chk");
                                TextBox objQuestions_Other_txt = (TextBox)AdvisorScreen_DL.Items[i].FindControl("Questions_Other_txt");
                                objQuestions_Other_chk.Checked = true;
                                objQuestions_Other_txt.Text = subQues.Substring(subQues.IndexOf("Other:") + 7);
                                break;
                            }
                        }
                    }
                }

                LinkButton objStudentInfo = (LinkButton)AdvisorScreen_DL.Items[i].FindControl("StudentInfo");
                if (objStudentInfo != null)
                { 
                    objStudentInfo.Attributes.Add("onmouseover", "ShowContent('" + Students_AdvisorScreen_DS.Tables[0].Rows[i]["StudentId"].ToString() + "'); return true;");
                    objStudentInfo.Attributes.Add("onmouseout", "ReverseContentDisplay('" + Students_AdvisorScreen_DS.Tables[0].Rows[i]["StudentId"].ToString() + "'); return true;");
                }

                strjscript = strjscript + "<div ";
                strjscript = strjscript + "id='" + Students_AdvisorScreen_DS.Tables[0].Rows[i]["StudentId"].ToString() + "' style='display:none;position:absolute;border-style:solid;border-width:thin;background-color:white;padding:0px;'>";
                strjscript = strjscript + "<img src='PhotoHandler1.ashx?id=" + Students_AdvisorScreen_DS.Tables[0].Rows[i]["StudentId"].ToString() + "' alt='StudentPhoto'/>";
                strjscript = strjscript + "</div" + ">";                
            }
            Literal1.Text = strjscript;

            Send_CellPhoneSMS();
            return intStudWaiting;
        }

        protected void Edit_AdvisorScreen(object sender, DataListCommandEventArgs e)
        {
            AdvisorScreen_DL.EditItemIndex = e.Item.ItemIndex;
            StudWaiting.Text = BindStudents_AdvisorScreen().ToString();
            AdvisorScreen_DL.Items[e.Item.ItemIndex].Focus();
        }

        protected void Cancel_AdvisorScreen(object sender, DataListCommandEventArgs e)
        {
            AdvisorScreen_DL.EditItemIndex = -1;
            StudWaiting.Text = BindStudents_AdvisorScreen().ToString();
        }

        protected void Delete_AdvisorScreen(object sender, DataListCommandEventArgs e)
        {
            SqlCollection objSqlCollection = new SqlCollection();
            objSqlCollection.Delete_Students(AdvisorScreen_DL.DataKeys[e.Item.ItemIndex].ToString());
            AdvisorScreen_DL.EditItemIndex = -1;
            StudWaiting.Text = BindStudents_AdvisorScreen().ToString();
        }

        protected void Update_AdvisorScreen(object sender, DataListCommandEventArgs e)
        {
            string strUpdateSql = "", strQuestions = "";
            DropDownList objOffice_Status = (DropDownList)AdvisorScreen_DL.Items[e.Item.ItemIndex].FindControl("Office_Status");
            if (objOffice_Status.SelectedItem.Text.IndexOf("Refer:") >= 0)
            {
                strUpdateSql = "Office_Status = 'Waiting', Office_Cd = '" + objOffice_Status.SelectedItem.Value.ToString() + "', AdvisorEmplid = '999999901'";
            }
            else if (objOffice_Status.SelectedItem.Text.IndexOf("See:") >= 0)
            {
                strUpdateSql = "Office_Status = 'Waiting', AdvisorEmplid = '" + objOffice_Status.SelectedItem.Value.ToString() + "'";
            }
            else
            {
                strUpdateSql = "Office_Status = '" + objOffice_Status.SelectedItem.Value.ToString() + "'";
            }

            TextBox objStudentId = (TextBox)AdvisorScreen_DL.Items[e.Item.ItemIndex].FindControl("StudentId_txt");
            CheckBoxList objQuestions_chkl = (CheckBoxList)AdvisorScreen_DL.Items[e.Item.ItemIndex].FindControl("Questions_chkl");
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
                CheckBox objQuestions_Other_chk = (CheckBox)AdvisorScreen_DL.Items[e.Item.ItemIndex].FindControl("Questions_Other_chk");
                TextBox objQuestions_Other_txt = (TextBox)AdvisorScreen_DL.Items[e.Item.ItemIndex].FindControl("Questions_Other_txt");
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
            objSqlCollection.Update_Students(AdvisorScreen_DL.DataKeys[e.Item.ItemIndex].ToString(), strUpdateSql);
            AdvisorScreen_DL.EditItemIndex = -1;
            StudWaiting.Text = BindStudents_AdvisorScreen().ToString();
        }

        protected void SeeStudent_AdvisorScreen(object sender, DataListCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Select")
            {
                SqlCollection objSqlCollection = new SqlCollection();
                string strUpdateSql = "Office_Status = 'In Progress'";
                objSqlCollection.Update_Students(AdvisorScreen_DL.DataKeys[e.Item.ItemIndex].ToString(), strUpdateSql);
                Send_CellPhoneSMS();
                Response.Redirect("StudentInfo.aspx?dept=" + Office_Cd.Text.ToString() + "&pkid=" + AdvisorScreen_DL.DataKeys[e.Item.ItemIndex].ToString());
            }
        }

        private void Send_CellPhoneSMS()
        {
            int intwaiting = 0;
            SqlCollection objSqlCollection = new SqlCollection();
            DataSet Students_AdvisorScreen_DS = objSqlCollection.Get_Students_AdvisorScreen(Office_Cd.Text.ToString());
            if (Students_AdvisorScreen_DS.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= Students_AdvisorScreen_DS.Tables[0].Rows.Count - 1; i++)
                {
                    if (Students_AdvisorScreen_DS.Tables[0].Rows[i]["Office_Status"].ToString() == "Waiting")
                    {
                        intwaiting += 1;
                    }

                    if ((intwaiting <= int.Parse(Students_AdvisorScreen_DS.Tables[0].Rows[i]["CellPhone_WaitNum"].ToString())) && (int.Parse(Students_AdvisorScreen_DS.Tables[0].Rows[i]["CellPhone_WaitNum"].ToString()) > 0) && (Students_AdvisorScreen_DS.Tables[0].Rows[i]["CellPhone"].ToString() != "") && (Students_AdvisorScreen_DS.Tables[0].Rows[i]["Office_Email_Address"].ToString() != "") && (Students_AdvisorScreen_DS.Tables[0].Rows[i]["CellPhone_Msg"].ToString() != "") && (Students_AdvisorScreen_DS.Tables[0].Rows[i]["SMS_Sent"].ToString() == "0"))
                    {
                        SmtpClient objSMTP = new SmtpClient();
                        objSMTP.Host = ConfigurationManager.AppSettings.Get("SMTP_HOST").ToString();
                        MailMessage objMail = new MailMessage();
                        objMail.To.Add(Students_AdvisorScreen_DS.Tables[0].Rows[i]["CellPhone"].ToString());
                        objMail.From = new MailAddress(Students_AdvisorScreen_DS.Tables[0].Rows[i]["Office_Email_Address"].ToString());
                        //objMail.Subject = Students_AdvisorScreen_DS.Tables[0].Rows[i]["Office_Descr"].ToString();
                        objMail.Body = Students_AdvisorScreen_DS.Tables[0].Rows[i]["CellPhone_Msg"].ToString();
                        objMail.IsBodyHtml = false;

                        try
                        {
                            objSMTP.Send(objMail);
                        }
                        catch (Exception e)
                        {
                            //response.write(e);
                        }
                        finally
                        {
                            objSqlCollection.Update_Students(Students_AdvisorScreen_DS.Tables[0].Rows[i]["pkID"].ToString(), "SMS_Sent=1");
                        }
                    }
                }
            }
        }
    }
}
