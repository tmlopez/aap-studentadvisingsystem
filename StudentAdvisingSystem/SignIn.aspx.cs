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
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Pragma", "no-cache");
            Response.Expires = -1;

            if (!IsPostBack)
            {
                Office_Cd.Text = (string)Request.Params["dept"];
                adv.Text = (string)Request.Params["adv"];

                if (Office_Cd.Text.ToString() == "")
                {
                    Office_Cd.Text = "AAP";
                }
                SqlCollection objSqlCollection = new SqlCollection();
                DataSet OfficeInfo_DS = objSqlCollection.Get_OfficeInfo(Office_Cd.Text.ToString());
                Office_Descr_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_Descr"].ToString();
                Office_Descr1_lbl.Text = Office_Descr_lbl.Text.ToString();
                Advisor_Type_lbl.Text = "To see " + OfficeInfo_DS.Tables[0].Rows[0]["Advisor_Type"].ToString() + ", please sign in using your Portal ID";
                Advisor_Type1_lbl.Text = "To see " + OfficeInfo_DS.Tables[0].Rows[0]["Advisor_Type"].ToString() + ", please sign in with your first name, last name, and the last four digits of your social security number";
                Advisor_Type2_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Advisor_Type"].ToString();
                Office_Location_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_Location"].ToString();
                Office_PhoneExt_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_PhoneExt"].ToString();
                ThankYou_Msg_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["ThankYou_Msg"].ToString();

                CELLPHONE.Visible = Convert.ToBoolean((int)OfficeInfo_DS.Tables[0].Rows[0]["CellPhoneAllow"]);
                pkID.Text = "";
                Phone1.Disabled = false;
                Phone2.Disabled = false;
                Phone3.Disabled = false;
                Email_Address_txt.Enabled = true;
                Majors_ddl.Enabled = true;
                Minor_txt.Enabled = true;
                Questions_chkl.Enabled = true;
                Questions_Other_chk.Enabled = true;
                Questions_Other_txt.Enabled = true;

                if (adv.Text.ToString() == "1")
                {
                    SignIn_tbl.Visible = false;
                    OtherSignIn_tbl.Visible = true;
                    Advisor_Type1_lbl.Text = "To see a student, please sign in with the student's first name and last name, or their student id (emplid or SSN)";
                    FirstName_txt.Focus();                    
                    Last4SSN_lbl.Text = "Student Id (Emplid Or SSN)";
                    Requiredfieldvalidator4.Text = "(Please Enter a Student Id)";
                    Last4SSN_txt.MaxLength = 9;
                    Last4SSN_txt.TextMode = TextBoxMode.SingleLine;
                    OtherSignIn_btn.Text = "Search/Sign In";
                    SignInMethod_Portal_lbtn.Enabled = false;
                    Requiredfieldvalidator2.Enabled = false;
                    Requiredfieldvalidator3.Enabled = false;
                    Requiredfieldvalidator4.Enabled = false;
                }
                else
                {
                    SignIn_tbl.Visible = true;
                    OtherSignIn_tbl.Visible = false;
                    userName_txt.Focus();
                }

                SASBack_btn.Visible = Office_Cd.Text.Contains("AAP");
                SASBack1_btn.Visible = Office_Cd.Text.Contains("AAP");
            }            

            //SignInMethod_Other_lbtn.Click += new EventHandler(this.SignInMethod);
            //SignInMethod_Portal_lbtn.Click += new EventHandler(this.SignInMethod);
            //SignIn_btn.Click += new EventHandler(this.SignIn);
            //OtherSignIn_btn.Click += new EventHandler(this.SignIn);
            //Ok_btn.Click += new EventHandler(this.Ok_Cliked);
            //FinalOK_btn.Click += new EventHandler(this.FinalOk_Cliked);                        
        }

        protected void SignInMethod(object sender, EventArgs e)
        {
            LinkButton objSignInMethod_lbtn = (LinkButton)sender;
            if (objSignInMethod_lbtn.ID.ToString() == "SignInMethod_Other_lbtn")
            {
                SignIn_tbl.Visible = false;
                OtherSignIn_tbl.Visible = true;
                Advisor_Type1_lbl.Text = "To see " + Advisor_Type2_lbl.Text.ToString() + ", please sign in with your first name, last name, and the last four digits of your social security number";
                FirstName_txt.Focus();
                Last4SSN_lbl.Text = "Social Security Number (only last 4 digits)";
                Requiredfieldvalidator4.Text = "(Please Enter The Last 4 Digits of Your SSN)";
                Last4SSN_txt.MaxLength = 4;
                Last4SSN_txt.TextMode = TextBoxMode.Password;
                OtherSignIn_btn.Text = "Sign In";
                SignInMethod_Portal_lbtn.Enabled = true;
                Requiredfieldvalidator2.Enabled = true;
                Requiredfieldvalidator3.Enabled = true;
                Requiredfieldvalidator4.Enabled = true;
            }
            else
            {
                SignIn_tbl.Visible = true;
                OtherSignIn_tbl.Visible = false;
                userName_txt.Focus();
            }
        }

        protected void ResetForm()
        {
            userName_txt.Text = "";
            Pwd_txt.Text = "";
            FirstName_txt.Text = "";
            LastName_txt.Text = "";
            Last4SSN_txt.Text = "";
            Emplid.Text = "";
        }

        protected void SignIn(object sender, EventArgs e)
        {
            SqlCollection objSqlCollection = new SqlCollection();
            Button objSignIn_btn = (Button)sender;
                      
            if (objSignIn_btn.ID.ToString() == "SignIn_btn")
            {
                Boolean isAuthenticated = false;
                string LDAP_Host = ConfigurationManager.AppSettings.Get("LDAP_HOST").ToString();
                int LDAP_PORT = Convert.ToInt32(ConfigurationManager.AppSettings.Get("LDAP_PORT").ToString());   
                string LDAP_BindAsBase = ConfigurationManager.AppSettings.Get("LDAP_BINDASSERVICE_BASE").ToString();
                string LDAP_SearchBase = ConfigurationManager.AppSettings.Get("LDAP_SEARCH_BASE").ToString();
                string LDAP_BindAsUserBase = ConfigurationManager.AppSettings.Get("LDAP_BINDASUSER_BASE").ToString();

                try
                {
                    LDAP ldapclientAuth = new LDAP(LDAP_Host, LDAP_BindAsUserBase, LDAP_PORT);
                    LDAP ldapclientSearch = new LDAP(LDAP_Host, LDAP_BindAsBase, LDAP_SearchBase, LDAP_PORT);

                    if (ldapclientAuth.IsAuthenticatedLDAP(userName_txt.Text.ToString(), Pwd_txt.Text.ToString()))
                    {
                        isAuthenticated = true;
                    }
                    else
                    {
                        isAuthenticated = false;
                    }

                    if (isAuthenticated)
                    {
                        DataClass objDataClass = new DataClass();
                        ldapclientSearch.PopulateUIDAttributes(userName_txt.Text.ToString(), ConfigurationManager.AppSettings.Get("LDAP_BINDASSERVICE_USERNAME").ToString(), objDataClass.DecryptConnectionString(ConfigurationManager.AppSettings.Get("LDAP_BINDASSERVICE_PASSWORD").ToString()));
                        Emplid.Text = ldapclientSearch.Emplid.ToString();
                        First_Name_Preferred_lbl.Text = ldapclientSearch.GivenName.ToString();
                        LastName_txt.Text = ldapclientSearch.SN.ToString();
                    }
                    else
                    {
                        ResetForm();
                        Requiredfieldvalidator.ErrorMessage = "(Oops, unknown username or password. Please try again.)";
                        Requiredfieldvalidator.IsValid = false;
                    }
                }
                catch
                {
                    ResetForm();
                    Requiredfieldvalidator.ErrorMessage = "(Oops, unknown username or password. Please try again.)";
                    Requiredfieldvalidator.IsValid = false;
                }
            }
            else
            {                
                DataSet Emplid_DS = objSqlCollection.Get_Emplid(FirstName_txt.Text.ToString(), LastName_txt.Text.ToString(), Last4SSN_txt.Text.ToString());
                if (Emplid_DS.Tables[0].Rows.Count == 1)
                {                    
                    Emplid.Text = Emplid_DS.Tables[0].Rows[0]["Emplid"].ToString();
                    First_Name_Preferred_lbl.Text = Emplid_DS.Tables[0].Rows[0]["First_Name_Preferred"].ToString();                    
                    objSqlCollection.Update_PrevStudents(Emplid.Text.ToString(), FirstName_txt.Text.ToString(), LastName_txt.Text.ToString(), Last4SSN_txt.Text.ToString());
                    LastName_txt.Text = Emplid_DS.Tables[0].Rows[0]["Last_Name_Preferred"].ToString();
                    StudentSearch_DG.Visible = false;
                }
                else if (Emplid_DS.Tables[0].Rows.Count > 1 && adv.Text.ToString() == "1")                
                {
                    Emplid.Text = "";
                    StudentSearch_DG.Visible = true;
                    StudentSearch_DG.DataSource = Emplid_DS;
                    StudentSearch_DG.DataBind();
                }
                else
                {
                    Emplid.Text = Last4SSN_txt.Text.ToString();
                    First_Name_Preferred_lbl.Text = FirstName_txt.Text.ToString();
                    StudentSearch_DG.Visible = false;
                }
            }

            if (Emplid.Text.Length >= 4)
            {
                if (adv.Text.ToString() == "1")
                {
                    objSqlCollection.Insert_Students(Emplid.Text.ToString(), First_Name_Preferred_lbl.Text.Replace("'", "''"), LastName_txt.Text.Replace("'", "''"), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Office_Cd.Text.ToString());
                    DataSet Student_ds = objSqlCollection.Get_Students(Emplid.Text.ToString(), First_Name_Preferred_lbl.Text.Replace("'", "''"), LastName_txt.Text.Replace("'", "''"), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Office_Cd.Text.ToString());
                    if (Student_ds.Tables[0].Rows.Count == 1)
                    {
                        objSqlCollection.Update_Students(Student_ds.Tables[0].Rows[0]["pkID"].ToString(), "Office_Status='In Progress'");
                        Response.Redirect("StudentInfo.aspx?dept=" + Office_Cd.Text.ToString() + "&pkid=" + Student_ds.Tables[0].Rows[0]["pkID"].ToString());
                    }
                }
                else
                {
                    SignIn_div.Visible = false;
                    StudentQuestions_div.Visible = true;
                    Majors_ddl.DataSource = objSqlCollection.Get_Majors(Office_Cd.Text.ToString());
                    Majors_ddl.DataBind();
                    for (int i = 0; i <= Majors_ddl.Items.Count - 1; i++)
                    {
                        if (Majors_ddl.Items[i].Value.ToString() == "")
                        {
                            Majors_ddl.Items[i].Selected = true;
                        }
                    }
                    Questions_chkl.DataSource = objSqlCollection.Get_QuesFields_dtl(Office_Cd.Text.ToString());
                    Questions_chkl.DataBind();
                    CellPhone_Provider_ddl.DataSource = objSqlCollection.Get_TextMsgCarriers();
                    CellPhone_Provider_ddl.DataBind();

                    First_Name_Preferred1_lbl.Text = First_Name_Preferred_lbl.Text.ToString();
                }
            }
            else
            {
                SignIn_div.Visible = true;
                StudentQuestions_div.Visible = false;
            }
        }

        protected void Ok_Cliked(object sender, EventArgs e)
        {
            string strQuestions = "", strPhone = "", strCellPhone = "";

            if (CELLPHONE.Visible)
            {
                strCellPhone = CellPhone1.Value.ToString() + CellPhone2.Value.ToString() + CellPhone3.Value.ToString();
                if (strCellPhone.Length == 10)
                {
                    CellPhone_Provider_ddl.Enabled = true;
                    Requiredfieldvalidator7.Enabled = true;
                    if (CellPhone_Provider_ddl.SelectedIndex == 0)
                    {
                        Requiredfieldvalidator7.IsValid = false;
                    }
                    else
                    {
                        Requiredfieldvalidator7.IsValid = true; 
                    }
                }                
                else 
                {
                    Requiredfieldvalidator7.Enabled = false;
                }                
            }

            if (((Requiredfieldvalidator7.Enabled) && (Requiredfieldvalidator7.IsValid)) || (Requiredfieldvalidator7.Enabled == false) || (CELLPHONE.Visible == false))
            {
                for (int i = 0; i <= Questions_chkl.Items.Count - 1; i++)
                {
                    if (Questions_chkl.Items[i].Selected)
                    {
                        strQuestions = strQuestions + Questions_chkl.Items[i].Value.ToString() + "<br />";
                    }
                }
                if ((Questions_Other_chk.Checked) && (Questions_Other_txt.Text.ToString() != ""))
                {
                    strQuestions = strQuestions + Questions_Other_chk.Text.ToString() + ": " + Questions_Other_txt.Text.ToString();
                }
                strPhone = Phone1.Value.ToString() + Phone2.Value.ToString() + Phone3.Value.ToString();
                if (strCellPhone.Length == 10)
                {
                    strCellPhone = strCellPhone + "@" + CellPhone_Provider_ddl.SelectedItem.Value.ToString();
                }
                else
                {
                    strCellPhone = "";
                }

                SqlCollection objSqlCollection = new SqlCollection();
                if (pkID.Text.ToString() != "")
                {
                    objSqlCollection.Update_Students(pkID.Text.ToString(), "CellPhone='" + strCellPhone + "', Office_Status='Signed In'");
                }
                else
                {
                    pkID.Text = objSqlCollection.Insert_Students(Emplid.Text.ToString(), First_Name_Preferred_lbl.Text.Replace("'", "''").Substring(0, 1).ToUpper() + First_Name_Preferred_lbl.Text.Replace("'", "''").Substring(1).ToLower(), LastName_txt.Text.Replace("'", "''").Substring(0, 1).ToUpper() + LastName_txt.Text.Replace("'", "''").Substring(1).ToLower(), strPhone, Email_Address_txt.Text.ToString(), Majors_ddl.SelectedItem.Text.ToString(), Minor_txt.Text.ToString(), strQuestions, strCellPhone, Office_Cd.Text.ToString());
                }
                
                StudentQuestions_div.Visible = false;
                FinalSignIn_div.Visible = true;

                if (strCellPhone != "" && pkID.Text.Length > 0)
                {                    
                    CellPhone_Msg_lbl.Text = "please verify your Cell Phone number and service provider: " + strCellPhone + ". If this information is incorrect, please click on the Back button, otherwise,";
                    CellPhone_Msg_lbl.Visible = true;
                    Back_btn.Visible = true;                    
                }
                else
                {
                    CellPhone_Msg_lbl.Text = "";
                    CellPhone_Msg_lbl.Visible = false;
                    Back_btn.Visible = false;                   
                }
            }
            else
            {               
                StudentQuestions_div.Visible = true;
                FinalSignIn_div.Visible = false;
                CellPhone_Provider_ddl.Focus();
            }
        }

        protected void FinalOk_Clicked(object sender, EventArgs e)
        {
            pkID.Text = "";
            ResetForm();
            Phone1.Value = "";
            Phone2.Value = "";
            Phone3.Value = "";
            Email_Address_txt.Text = "";
            Majors_ddl.SelectedIndex = 0;
            Minor_txt.Text = "";
            for (int i = 0; i <= Questions_chkl.Items.Count - 1; i++)
            {
                Questions_chkl.Items[i].Selected = false;                
            }
            Questions_Other_chk.Checked = false;
            Questions_Other_txt.Text = "";
            CellPhone1.Value = "";
            CellPhone2.Value = "";
            CellPhone3.Value = "";
            CellPhone_Provider_ddl.SelectedIndex = 0;

            //redirect EVAL students to the EVAL page
            if (Office_Cd.Text.ToString() == "AAPEVAL")
            {
                Response.Redirect("EVAL.aspx");
            }
            //1/15/2011 
            //else if (Office_Cd.Text.ToString() == "AAPIADV")
            else if (Office_Cd.Text.ToString() == "AAPIADV" || Office_Cd.Text.ToString() == "GIISSA" || Office_Cd.Text.ToString() == "GIISINST")
            {
                Response.Redirect("International.aspx");
            }
            else if (Office_Cd.Text.Contains("AAP"))
            {
                Response.Redirect("AAP.aspx");
            }
            else
            {
                Response.Redirect("SignIn.aspx?dept=" + Office_Cd.Text.ToString());
            }
        }

        protected void Back_Clicked(object sender, EventArgs e)
        {
            SqlCollection objSqlCollection = new SqlCollection();
            objSqlCollection.Update_Students(pkID.Text.ToString(), "Office_Status=''");
            FinalSignIn_div.Visible = false;
            StudentQuestions_div.Visible = true;

            Phone1.Disabled = true;
            Phone2.Disabled = true;
            Phone3.Disabled = true;
            Email_Address_txt.Enabled = false;
            Majors_ddl.Enabled = false;
            Minor_txt.Enabled = false;
            Questions_chkl.Enabled = false;
            Questions_Other_chk.Enabled = false;
            Questions_Other_txt.Enabled = false;
        }

        protected void SplashScreen_clicked(object sender, EventArgs e)
        {
            if (adv.Text.ToString() == "1")
            {
                Response.Redirect("SAS.aspx?dept=" + Office_Cd.Text.ToString());
            }
            else
            {
                //09/03/2010
                if (Office_Cd.Text.ToString() == "AAPEVAL")
                {
                    Response.Redirect("EVAL.aspx");
                }
               //1/15/2011 else if (Office_Cd.Text.ToString() == "AAPIADV")
                else if (Office_Cd.Text.ToString() == "AAPIADV" || Office_Cd.Text.ToString() == "GIISSA" || Office_Cd.Text.ToString() == "GIISINST")
                {
                    Response.Redirect("International.aspx");
                }
                else 
                {
                    Response.Redirect("AAP.aspx");
                }           
            }
        }

        protected void StudentSearch_Selected(object sender, DataGridCommandEventArgs e)
        {
            SqlCollection objSqlCollection = new SqlCollection();
            if (e.CommandName.ToString() == "Edit")
            {
                string strEmplid, strName, strFirst_Name, strLast_Name;
                strEmplid = StudentSearch_DG.DataKeys[e.Item.ItemIndex].ToString();
                strName = StudentSearch_DG.Items[e.Item.ItemIndex].Cells[1].Text.ToString();
                strLast_Name = strName.Substring(0, strName.IndexOf(","));
                strFirst_Name = strName.Substring(strName.IndexOf(",") + 1, strName.IndexOf(" ") - strName.IndexOf(",") - 1);

                objSqlCollection.Insert_Students(strEmplid, strFirst_Name.Replace("'", "''"), strLast_Name.Replace("'", "''"), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Office_Cd.Text.ToString());
                DataSet Student_ds = objSqlCollection.Get_Students(strEmplid, strFirst_Name.Replace("'", "''"), strLast_Name.Replace("'", "''"), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Office_Cd.Text.ToString());
                if (Student_ds.Tables[0].Rows.Count == 1)
                {
                    objSqlCollection.Update_Students(Student_ds.Tables[0].Rows[0]["pkID"].ToString(), "Office_Status='In Progress'");
                    Response.Redirect("StudentInfo.aspx?dept=" + Office_Cd.Text.ToString() + "&pkid=" + Student_ds.Tables[0].Rows[0]["pkID"].ToString());
                }                
            }
        }

    }
}
