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
using System.Net.Mail;  //6/2013

namespace StudentAdvisingSystem
{
    public partial class StudentInfo : System.Web.UI.Page
    {
        private SqlCollection objSqlCollection = new SqlCollection();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Literal1.Text = "";
                Office_Cd.Text = (string)Request.Params["dept"];
                PkId.Text = (string)Request.Params["pkid"];

                if (Office_Cd.Text.ToString() == "")
                {
                    Office_Cd.Text = "AAP";
                }
                //SqlCollection objSqlCollection = new SqlCollection();
                DataSet OfficeInfo_DS = objSqlCollection.Get_OfficeInfo(Office_Cd.Text.ToString());
                Office_Descr_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_Descr"].ToString();
                Office_Descr1_lbl.Text = Office_Descr_lbl.Text.ToString();
                Office_Location_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_Location"].ToString();
                Office_PhoneExt_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_PhoneExt"].ToString();
                CurrentDate_lbl.Text = DateTime.Now.ToLongDateString();
                Advisor_Type.Text = OfficeInfo_DS.Tables[0].Rows[0]["Advisor_Type"].ToString();
                Office_Email_Address.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_Email_Address"].ToString();
                Visit_DateTime.Text = DateTime.Now.ToString();
                Email_Intro.Text = OfficeInfo_DS.Tables[0].Rows[0]["Email_Intro"].ToString();

                if (Session["Save"] != null)
                {
                    Save.Text = Session["Save"].ToString();
                }
                else
                { 
                    Save.Text = "0"; 
                }
                
                BindStudentInfo();
            }
        }

        private void BindStudentInfo()
        {            
            msg.Visible = false;
            Tools_Results_TC.Visible = false;
            //SqlCollection objSqlCollection = new SqlCollection();
            DataSet StudentInfo_DS = objSqlCollection.Get_Students(PkId.Text.ToString(), Office_Cd.Text.ToString());
            if (StudentInfo_DS.Tables[0].Rows.Count >= 1)
            { 
                DataRow dr = StudentInfo_DS.Tables[0].Rows[0];
                DateTime Vdt = (DateTime)dr["Visit_DateTime"];
                CurrentDate_lbl.Text = Vdt.ToLongDateString();            
                FirstName.Text = dr["FirstName"].ToString();
                LastName.Text = dr["LastName"].ToString();
                StudentId_lbl.Text = dr["StudentId"].ToString();
                StudentId_txt.Text = dr["StudentId"].ToString();
                if (dr["Phone"].ToString() != "")
                {
                    Phone.Text = dr["Phone"].ToString().Substring(0, 3) + "/" + dr["Phone"].ToString().Substring(3, 3) + "-" + dr["Phone"].ToString().Substring(6);
                }
                Email_Address.Text = dr["Email_Address"].ToString();
                Major.Text = dr["Major"].ToString();
                Minor.Text = "N/A";
                if (dr["Minor"].ToString() != "")
                {
                    Minor.Text = dr["Minor"].ToString();
                }
                Questions_chkl.DataSource = objSqlCollection.Get_QuesFields_dtl(Office_Cd.Text.ToString());
                Questions_chkl.DataBind();
                string strQues = dr["Ques"].ToString();
                string[] br = new string[] { "<br />" };
                foreach (string subQues in strQues.Split(br, StringSplitOptions.RemoveEmptyEntries))
                {
                    for (int n = 0; n <= Questions_chkl.Items.Count - 1; n++)
                    {
                        if (Questions_chkl.Items[n].Value.ToString() == subQues)
                        {
                            Questions_chkl.Items[n].Selected = true;
                            break;
                        }
                        if (subQues.IndexOf("Other:") >= 0)
                        {                            
                            Questions_Other_chk.Checked = true;
                            Questions_Other_txt.Text = subQues.Substring(subQues.IndexOf("Other:") + 7);
                            break;
                        }
                    }
                }

                if (dr["PastVisit"].ToString() == "Yes")
                {
                    PreviousAdvising_lbtn.Enabled = true;
                }
                else
                {
                    PreviousAdvising_lbtn.Enabled = false;
                }
                

                DataSet StudentInfo_PS_DS = objSqlCollection.Get_Students_PS(StudentId_lbl.Text.ToString(), Vdt);
                if (StudentInfo_PS_DS.Tables[0].Rows.Count >= 1)
                {
                    DataRow PS_dr = StudentInfo_PS_DS.Tables[0].Rows[0];
                    Phone_PS.Text = PS_dr["mail_phone_nbr"].ToString();
                    if ((Phone.Text.ToString() != "") && (Phone.Text.ToString().Replace("/", "").Replace("-", "").Replace("(", "").Replace(")", "").Trim() != Phone_PS.Text.ToString().Replace("/", "").Replace("-", "").Replace("(", "").Replace(")", "").Trim()))
                    {                        
                        Phone.Text = "<br/>(" + Phone.Text.ToString() + ")";
                    }
                    else
                    {
                        Phone.Text = "";
                    }
                    Email_Address_PS.Text = PS_dr["email_preferred"].ToString();
                    if ((Email_Address.Text.ToString() != "") && (Email_Address.Text.ToString() != Email_Address_PS.Text.ToString()))
                    {
                        Email_Address.Text = "<br />(" + Email_Address.Text.ToString() + ")";
                    }
                    else
                    {
                        Email_Address.Text = "";
                    }
                    Career.Text = PS_dr["acad_career"].ToString();
                    Acad_Program.Text = PS_dr["acad_prog_primary"].ToString();
                    Enr_Status.Text = PS_dr["enrl_status_sdesc"].ToString();
                    Academic_Level.Text = PS_dr["acad_level_bot_sdesc"].ToString();
                    Major_PS.Text = PS_dr["pri_acad_plan_ldesc"].ToString();
                    if ((Major.Text.ToString() != "") && (Major.Text.ToString() != PS_dr["pri_acad_plan_ldesc"].ToString()))
                    {
                        if (Major.Text.ToString() != PS_dr["sec_acad_plan_ldesc"].ToString())
                        {
                            Major.Text = "<br />(" + Major.Text.ToString() + ")";
                        }
                        else
                        {
                            Major.Text = "";
                        }
                    }
                    else
                    {
                        Major.Text = "";
                    }
                    
                    if (PS_dr["pri_acad_sub_plan1_ldesc"].ToString() != "")
                    {
                        Major_PS.Text = Major_PS.Text.ToString() + ": " + PS_dr["pri_acad_sub_plan1_ldesc"].ToString();
                    }
                    
                    DataTable stats_dt = new DataTable();
                    stats_dt.Columns.Add(new DataColumn("Statistic_Type", typeof(String)));
                    stats_dt.Columns.Add(new DataColumn("Units_Taken", typeof(decimal)));
                    stats_dt.Columns.Add(new DataColumn("Units_Passed", typeof(decimal)));
                    stats_dt.Columns.Add(new DataColumn("GPA", typeof(decimal)));
                    stats_dt.Columns.Add(new DataColumn("Grade_Points", typeof(decimal)));
                    stats_dt.Columns.Add(new DataColumn("GP_Variance", typeof(decimal)));
                    DataRow stats_dr;
                    stats_dr = stats_dt.NewRow();
                    stats_dr[0] = PS_dr["term_sdesc"].ToString() + "*";
                    stats_dr[1] = PS_dr["term_units_taken_progress"];
                    stats_dr[2] = PS_dr["term_units_passed"];
                    stats_dr[3] = PS_dr["term_gpa"];
                    stats_dr[4] = PS_dr["term_grade_points"];
                    stats_dr[5] = (decimal)PS_dr["term_grade_points"] - ((decimal)PS_dr["term_units_taken_gpa"] * 2);
                    stats_dt.Rows.Add(stats_dr);
                    DataSet StdntNxtTerm_ds = objSqlCollection.Get_StudentsNextTermInfo_PS(StudentId_lbl.Text.ToString(), PS_dr["term"].ToString());
                    if (StdntNxtTerm_ds.Tables[0].Rows.Count == 1)
                    {
                        stats_dr = stats_dt.NewRow();
                        stats_dr[0] = StdntNxtTerm_ds.Tables[0].Rows[0]["nxt_term_sdesc"].ToString();
                        stats_dr[1] = StdntNxtTerm_ds.Tables[0].Rows[0]["nxt_term_units_taken_progress"];
                        stats_dr[2] = StdntNxtTerm_ds.Tables[0].Rows[0]["nxt_term_units_passed"];
                        stats_dr[3] = StdntNxtTerm_ds.Tables[0].Rows[0]["nxt_term_gpa"];
                        stats_dr[4] = StdntNxtTerm_ds.Tables[0].Rows[0]["nxt_term_grade_points"];
                        stats_dr[5] = (decimal)StdntNxtTerm_ds.Tables[0].Rows[0]["nxt_term_grade_points"] - ((decimal)StdntNxtTerm_ds.Tables[0].Rows[0]["nxt_term_units_taken_gpa"] * 2);
                        stats_dt.Rows.Add(stats_dr);
                    }
                    stats_dr = stats_dt.NewRow();
                    stats_dr[0] = "Chico*";
                    stats_dr[1] = PS_dr["chico_taken_gpa"];
                    stats_dr[2] = PS_dr["chico_units_passed"];
                    stats_dr[3] = PS_dr["chico_gpa"];
                    stats_dr[4] = PS_dr["chico_grade_points"];
                    stats_dr[5] = (decimal)PS_dr["chico_grade_points"] - ((decimal)PS_dr["chico_taken_gpa"] * 2);
                    stats_dt.Rows.Add(stats_dr);
                    stats_dr = stats_dt.NewRow();
                    stats_dr[0] = "Cum*";
                    stats_dr[1] = PS_dr["cum_taken_gpa"];
                    stats_dr[2] = PS_dr["cum_units_passed"];
                    stats_dr[3] = PS_dr["cum_gpa"];
                    stats_dr[4] = PS_dr["cum_grade_points"];
                    stats_dr[5] = (decimal)PS_dr["cum_grade_points"] - ((decimal)PS_dr["cum_taken_gpa"] * 2);
                    stats_dt.Rows.Add(stats_dr);

                    Acad_Statistics_DG.DataSource = stats_dt;
                    Acad_Statistics_DG.DataBind();
                    for (int i = 0; i <= Acad_Statistics_DG.Items.Count - 1; i++)
                    {
                        if (Acad_Statistics_DG.Items[i].Cells[5].Text.StartsWith("-"))
                        {
                            Acad_Statistics_DG.Items[i].Cells[5].ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    Acad_Statistics_TR.Visible = true;

                    Acad_Standing.Text = PS_dr["acad_standing_ldesc"].ToString();
                    Neg_StdntService_Ind.Text = "";
                    DataSet SI_ds = objSqlCollection.Get_Neg_ServiceIndicator(StudentId_lbl.Text.ToString());
                    if (SI_ds.Tables.Count == 1)
                    {
                        for (int i = 0; i <= SI_ds.Tables[0].Rows.Count - 1; i++)
                        {
                            if (i == SI_ds.Tables[0].Rows.Count - 1)
                            {
                                Neg_StdntService_Ind.Text = Neg_StdntService_Ind.Text.ToString() + SI_ds.Tables[0].Rows[i][0].ToString();
                            }
                            else
                            {
                                Neg_StdntService_Ind.Text = Neg_StdntService_Ind.Text.ToString() + SI_ds.Tables[0].Rows[i][0].ToString() + "<br />";
                            }
                        }
                    }
                    
                    if (Acad_Standing.Text.ToString() == "N/A" && Neg_StdntService_Ind.Text.ToString() == "")
                    {
                        Acad_Standing_TR.Visible = false;
                    }
                    else
                    {
                        Acad_Standing_TR.Visible = true;
                    }

                    OLLE_Student_Group.Text = "N/A";
                    DataSet SG_ds = objSqlCollection.Get_OLLE_StudentGroup(StudentId_lbl.Text.ToString());
                    if (SG_ds.Tables.Count == 1)
                    {
                        for (int i = 0; i <= SG_ds.Tables[0].Rows.Count - 1; i++)
                        {
                            if (i == SG_ds.Tables[0].Rows.Count - 1)
                            {
                                OLLE_Student_Group.Text = OLLE_Student_Group.Text.ToString() + SG_ds.Tables[0].Rows[i][0].ToString();
                            }
                            else
                            {
                                OLLE_Student_Group.Text = OLLE_Student_Group.Text.ToString() + SG_ds.Tables[0].Rows[i][0].ToString() + "<br />";
                            }
                        }
                    }
                    OLLE_Status.Text = "";
                    DataSet OLLE_ds = objSqlCollection.Get_OLLE_Status(StudentId_lbl.Text.ToString());
                    if (OLLE_ds.Tables.Count == 1)
                    {
                        if (OLLE_ds.Tables[0].Rows.Count == 1)
                        {
                            switch (OLLE_ds.Tables[0].Rows[0]["Status"].ToString())
                            {
                                case "C":
                                    OLLE_Status.Text = "Completed";
                                    break;
                                case "I":
                                    OLLE_Status.Text = "Incomplete";
                                    break;
                                case "F":
                                    OLLE_Status.Text = "Failed";
                                    break;
                                default:
                                    break;
                            }
                            OLLE_Status.Text = OLLE_Status.Text.ToString() + " on " + OLLE_ds.Tables[0].Rows[0]["Date"].ToString() + "<br/ >Questions Correct: " + OLLE_ds.Tables[0].Rows[0]["QuestionsRight"].ToString() + "<br />Questions Incorrect: " + OLLE_ds.Tables[0].Rows[0]["QuestionsWrong"].ToString();
                        }
                    }
                    DataSet BackOnTrack_Wkshop_ds = objSqlCollection.Get_BackOnTrack_Wkshop(StudentId_lbl.Text.ToString());
                    if (BackOnTrack_Wkshop_ds.Tables.Count == 1)
                    {
                        BkOnTrack_Wkshop.Text = "";
                        for (int i = 0; i <= BackOnTrack_Wkshop_ds.Tables[0].Rows.Count -1; i++)
                        {
                            if (i == BackOnTrack_Wkshop_ds.Tables[0].Rows.Count - 1)
                            {
                                BkOnTrack_Wkshop.Text = BkOnTrack_Wkshop.Text.ToString() + BackOnTrack_Wkshop_ds.Tables[0].Rows[i][0].ToString();
                            }
                            else
                            {
                                BkOnTrack_Wkshop.Text = BkOnTrack_Wkshop.Text.ToString() + BackOnTrack_Wkshop_ds.Tables[0].Rows[i][0].ToString() + "<br />";
                            }
                        }                            
                    }
                    if (BkOnTrack_Wkshop.Text == "")
                    {
                        BkOnTrack_Wkshop.Text = "N/A";
                    }

                    //6/2011 tmlopez - added Whats Next Workshops
                    DataSet WhatsNext_Wkshop_ds = objSqlCollection.Get_WhatsNext_Wkshop(StudentId_lbl.Text.ToString());
                    if (WhatsNext_Wkshop_ds.Tables.Count == 1)
                    {
                        WhatsNext_Wkshop.Text = "";
                        for (int i = 0; i <= WhatsNext_Wkshop_ds.Tables[0].Rows.Count - 1; i++)
                        {
                            if (i == WhatsNext_Wkshop_ds.Tables[0].Rows.Count - 1)
                            {
                                WhatsNext_Wkshop.Text = WhatsNext_Wkshop.Text.ToString() + WhatsNext_Wkshop_ds.Tables[0].Rows[i][0].ToString();
                            }
                            else
                            {
                                WhatsNext_Wkshop.Text = WhatsNext_Wkshop.Text.ToString() + WhatsNext_Wkshop_ds.Tables[0].Rows[i][0].ToString() + "<br />";
                            }
                        }
                    }
                    if (WhatsNext_Wkshop.Text == "")
                    {
                        WhatsNext_Wkshop.Text = "N/A";
                    }
                    //end 6/2011


                    if (OLLE_Student_Group.Text.ToString() == "N/A" && OLLE_Status.Text.ToString() == "" && BkOnTrack_Wkshop.Text.ToString() == "N/A")
                    {
                        OLLE_TR.Visible = false;
                    }
                    else
                    {
                        OLLE_TR.Visible = true;
                    }

                }
                else
                {
                    if (Office_Cd.Text.Contains("GRAD"))
                    {
                        Career.Text = "PBAC";
                        Acad_Program.Text = "GRAD";
                    }
                    else {
                        Career.Text = "UGRD";
                        Acad_Program.Text = "UGRD";
                    }
                    Phone_PS.Text = "";
                    Email_Address_PS.Text = "";
                    Major_PS.Text = "";
                    Enr_Status.Text = "Prospective";
                    Acad_Standing_TR.Visible = false;
                    OLLE_TR.Visible = false;
                    Acad_Statistics_TR.Visible = false;
                }

                Advisors_ddl.DataSource = objSqlCollection.Get_Advisors(Office_Cd.Text.ToString());
                Advisors_ddl.DataBind();
                for (int i = 0; i <= Advisors_ddl.Items.Count - 1; i++)
                {
                    if (Advisors_ddl.Items[i].Value.ToString() == dr["AdvisorEmplid"].ToString())
                    {
                        Advisors_ddl.Items[i].Selected = true;
                    }
                    if (dr["AdvisorEmplid"].ToString() == "")
                    {
                        StudentAdvised_chk.Enabled = false;                        
                        StudentAdvised_btn.Enabled = false;
                    }
                    else if (dr["AdvisorEmplid"].ToString().Substring(0, 7) == "9999999")
                    {
                        StudentAdvised_chk.Enabled = false;                        
                        StudentAdvised_btn.Enabled = true;
                    }                    
                    else
                    {
                        if (dr["TimeSpent"].ToString() == "")
                        {
                            StudentAdvised_chk.Enabled = false;
                            StudentAdvised_btn.Enabled = true;
                        }
                        else
                        {
                            StudentAdvised_chk.Enabled = true;
                            StudentAdvised_btn.Enabled = true;
                        }
                    }
                }

                if (dr["Office_Status"].ToString() == "Completed")
                {                    
                    StudentAdvised_chk.Checked = true;
                    StudentAdvised_chk.Enabled = false;
                    Questions_chkl.Enabled = false;
                    Questions_Other_chk.Enabled = false;
                    Questions_Other_txt.Enabled = false;
                    Advisors_ddl.Enabled = false;
                    TimeSpent_ddl.Enabled = false;
                    Notes.Enabled = false;
                    Notes.Visible = false;
                    Notes_lbl.Visible = true;
                    Save.Text = "1";
                    StudentAdvised_btn.Visible = false;                   
                    PreText_ddl.Enabled = false;
                    PreText_lbtn.Enabled = false;
                    Tools_Results_lbl.Text = "Prev Notes to Student";
                    BindPreviousAdvising();
                    Tools_Results_ddl.Visible = false;
                    Tools_Results_add_lbtn.Visible = false;
                }
                else 
                {
                    StudentAdvised_chk.Checked = false;
                    Questions_chkl.Enabled = true;
                    Questions_Other_chk.Enabled = true;
                    Questions_Other_txt.Enabled = true;
                    Advisors_ddl.Enabled = true;
                    TimeSpent_ddl.Enabled = true;
                    Notes.Enabled = true;
                    Notes.Visible = true;
                    Notes_lbl.Visible = false;
                    StudentAdvised_btn.Visible = true;
                    PreText_ddl.Enabled = true;
                    PreText_lbtn.Enabled = true;
                    Tools_Results_lbl.Text = "Planning Sheets";
                    BindPlanningSheet();
                    Tools_Results_ddl.Visible = true;
                    Tools_Results_add_lbtn.Visible = true;
                }

                DataTable timer_dt = new DataTable();
                timer_dt.Columns.Add(new DataColumn("Timer", typeof(string)));
                DataRow timer_dr;
                for (int i = 0; i <= 120; i = i + 5)
                {                    
                    timer_dr = timer_dt.NewRow();
                    timer_dr[0] = i.ToString();
                    timer_dt.Rows.Add(timer_dr);
                }               
                timer_dr = timer_dt.NewRow();
                timer_dr[0] = "";
                timer_dt.Rows.InsertAt(timer_dr, 0);
                TimeSpent_ddl.DataSource = timer_dt;
                TimeSpent_ddl.DataBind();
                for (int i = 0; i <= TimeSpent_ddl.Items.Count - 1; i++)
                {
                    if (TimeSpent_ddl.Items[i].Value.ToString() == dr["TimeSpent"].ToString())
                    {
                        TimeSpent_ddl.Items[i].Selected = true;
                    }
                }

                Visit_Date.Text = Vdt.ToShortDateString();
                Visit_DateTime.Text = Vdt.ToString();
                Notes.Text = dr["Notes"].ToString();
                Notes_lbl.Text = dr["Notes"].ToString().Replace("\n", "<br />");
            }

            
            Tools_Results_DG.CurrentPageIndex = 0;
            Tools_Results_TC.Visible = true;
            EmailStudent_TC.Visible = false;     

            BindPreText();
        }

        protected void StudentId_Edit(object sender, EventArgs e)
        {
            Literal1.Text = "";
            if (StudentId_lbtn.Text.ToString() == "Edit")
            {
                StudentId_txt.Text = StudentId_lbl.Text.ToString();
                StudentId_lbl.Visible = false;                
                StudentId_txt.Visible = true;
                StudentId_lbtn.Text = "Update";                
            }
            else if (StudentId_lbtn.Text.ToString() == "Update")
            {
                StudentId_lbl.Text = StudentId_txt.Text.ToString();
                StudentId_lbl.Visible = true;                
                StudentId_txt.Visible = false;
                StudentId_lbtn.Text = "Edit";
                //SqlCollection objSqlCollection = new SqlCollection();
                objSqlCollection.Update_Students(PkId.Text.ToString(), "StudentId='" + StudentId_txt.Text.ToString() + "'");
                if (!StudentAdvised_chk.Checked)
                { 
                    BindStudentInfo();
                }
            }

            switch (Tools_Results_lbl.Text.ToString())
            {
                case "Prev Notes to Student":
                    BindPreviousAdvising();
                    break;
                case "Advisor Comments":
                    BindAdvisorComments();
                    break;
                case "Planning Sheets":
                    BindPlanningSheet();
                    break;
                default:
                    break;
            }
            msg.Visible = false;
            msg.Text = "Warning: You have unsaved data on this page.  Click Save, or click on your selection again to continue.";
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

        protected void PreviousAdvising_clicked(object sender, EventArgs e)
        {
            Literal1.Text = "";
            Tools_Results_lbl.Text = "Prev Notes to Student";
            BindPreviousAdvising();
            Tools_Results_DG.CurrentPageIndex = 0;
            Tools_Results_TC.Visible = true;
            EmailStudent_TC.Visible = false;
            Tools_Results_ddl.Visible = false;           
            Tools_Results_add_lbtn.Visible = false;
            msg.Visible = false;
            msg.Text = "Warning: You have unsaved data on this page.  Click Save, or click on your selection again to continue.";           
        }

        private void BindPreviousAdvising()
        {
            if (Tools_Results_DG.Columns.Count == 1)
            {
                Tools_Results_DG.Columns.Add(CreateBoundColumn("Office_Cd", "Office"));
                Tools_Results_DG.Columns.Add(CreateBoundColumn("Date", "Date"));
                Tools_Results_DG.Columns.Add(CreateBoundColumn("Advised", "Advised"));
                Tools_Results_DG.Columns.Add(CreateBoundColumn("Office_Status", "Office_Status"));
            }
            Tools_Results_DG.DataSource = objSqlCollection.Get_PrevStudents(PkId.Text.ToString(), Office_Cd.Text.ToString());
            Tools_Results_DG.DataBind();
            if (Tools_Results_DG.Items.Count == 0)
            {
                Tools_Results_DG.Visible = false;
                PreviousAdvising_lbtn.Focus();
            }
            else
            {
                Tools_Results_DG.Visible = true;
                Tools_Results_DG.Focus();
            }
        }

        protected void AdvisorComments_clicked(object sender, EventArgs e)
        {
            Literal1.Text = "";
            Tools_Results_lbl.Text = "Advisor Comments";
            BindAdvisorComments();
            Tools_Results_DG.CurrentPageIndex = 0;
            Tools_Results_TC.Visible = true;
            EmailStudent_TC.Visible = false;
            Tools_Results_ddl.Visible = false;
            Tools_Results_add_lbtn.Visible = true;
            msg.Visible = false;
            msg.Text = "Warning: You have unsaved data on this page.  Click Save, or click on your selection again to continue.";
        }

        private void BindAdvisorComments()
        {
            if (Tools_Results_DG.Columns.Count == 1)
            {
                Tools_Results_DG.Columns.Add(CreateBoundColumn("Office_Cd", "Office"));
                Tools_Results_DG.Columns.Add(CreateBoundColumn("Date", "Date"));
                Tools_Results_DG.Columns.Add(CreateBoundColumn("Commented", "Commented"));
            }
            DataView Tools_Results_DV = objSqlCollection.Get_AdvisorComments(StudentId_lbl.Text.ToString(), Office_Cd.Text.ToString());                        
            Tools_Results_DG.DataSource = Tools_Results_DV;
            Tools_Results_DG.DataBind();
            if (Tools_Results_DG.Items.Count == 0)
            {
                Tools_Results_DG.Visible = false;
                Tools_Results_add_lbtn.Focus();
            }
            else
            {
                Tools_Results_DG.Visible = true;
                Tools_Results_DG.Focus();
            }
        }

        protected void PlanningSheet_clicked(object sender, EventArgs e)
        {
            Literal1.Text = "";
            Tools_Results_lbl.Text = "Planning Sheets";
            BindPlanningSheet();
            Tools_Results_DG.CurrentPageIndex = 0;
            Tools_Results_TC.Visible = true;
            EmailStudent_TC.Visible = false;
            Tools_Results_ddl.Visible = true;
            Tools_Results_add_lbtn.Visible = true;
            if (Tools_Results_DG.Visible)
            {
                Tools_Results_DG.Focus();
            }
            else
            {
                Tools_Results_add_lbtn.Focus();
            }
            msg.Visible = false;
            msg.Text = "Warning: You have unsaved data on this page.  Click Save, or click on your selection again to continue.";
        }

        private void BindPlanningSheet()
        {
            DataSet Tools_Results_DS = objSqlCollection.Get_PlanSheets(Office_Cd.Text.ToString());
            DataTable Tools_Results_DT = Tools_Results_DS.Tables[0];
            DataRow Tools_Results_DR = Tools_Results_DT.NewRow();
            Tools_Results_DR["PSName_Ldesc"] = "";
            Tools_Results_DT.Rows.InsertAt(Tools_Results_DR, 0);

            Tools_Results_ddl.DataTextField = "PSName_Ldesc";
            Tools_Results_ddl.DataValueField = "PkId";
            Tools_Results_ddl.DataSource = Tools_Results_DS;
            Tools_Results_ddl.DataBind();
            if (Tools_Results_ddl.Items.Count == 2)
            {
                Tools_Results_ddl.SelectedIndex = 1;
            }
            
            Tools_Results_DS = objSqlCollection.Get_PSStudents(StudentId_lbl.Text.ToString(), Office_Cd.Text.ToString());
            if (Tools_Results_DG.Columns.Count == 1)
            { 
                Tools_Results_DG.Columns.Add(CreateBoundColumn("Office_Cd", "Office"));                            
                Tools_Results_DG.Columns.Add(CreateBoundColumn("PSDate", "PSDate"));
                Tools_Results_DG.Columns.Add(CreateBoundColumn("AdvisorName", "AdvisorName"));
                Tools_Results_DG.Columns.Add(CreateBoundColumn("PSName", "PSName"));
            }
            Tools_Results_DG.DataSource = Tools_Results_DS;
            Tools_Results_DG.DataBind();
            if (Tools_Results_DG.Items.Count == 0)
            {
                Tools_Results_DG.Visible = false;
            }
            else
            {
                Tools_Results_DG.Visible = true;
            }
        }

        protected void EmailStudent_clicked(object sender, EventArgs e)
        {
            Literal1.Text = "";
            Tools_Results_TC.Visible = false;
            EmailStudent_TC.Visible = true;
            Email_To_txt.Text = Email_Address_PS.Text.ToString();
            DataSet objPlanningSheets_ds = objSqlCollection.Get_PSStudents(StudentId_lbl.Text.ToString(), Office_Cd.Text.ToString(), PkId.Text.ToString());
            if (objPlanningSheets_ds.Tables[0].Rows.Count > 0)
            {
                Email_PlanSheets.Checked = true;
            }
            else
            {
                Email_PlanSheets.Checked = false;
            }
            if (Notes.Text.ToString() != "")
            {
                Email_StdntNotes.Checked = true;
            }
            else
            {
                Email_StdntNotes.Checked = false;
            }
            Email_Subj_txt.Text = "Advising session with " + Advisor_Type.Text.ToString() + " on " + Visit_DateTime.Text.ToString();
            if (Office_Email_Address.Text.ToString() != "")
            {
                Email_Send_btn.Enabled = true;                
            }
            else
            {
                Email_Send_btn.Enabled = false;                
            }
            Email_Send_btn.Focus();
            msg.Visible = false;
            msg.Text = "Warning: You have unsaved data on this page.  Click Save, or click on your selection again to continue.";
        }        

        protected void Email_Send_clicked(object sender, EventArgs e)
        {
            Literal1.Text = "";
            if (int.Parse(Save.Text.ToString()) == 0)
            {
                msg.Visible = true;
                Email_Send_btn.Focus();
                Save.Text = "1";
                Session["Save"] = Save.Text.ToString();
            }
            else
            {
                Session["Save"] = Save.Text.ToString();
                Save_StudentInfo();

                string strAttach = "";
                if (Email_PlanSheets.Checked)
                {
                    DataSet objPlanningSheets_ds = objSqlCollection.Get_PSStudents(StudentId_lbl.Text.ToString(), Office_Cd.Text.ToString(), PkId.Text.ToString());
                    if (objPlanningSheets_ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i <= objPlanningSheets_ds.Tables[0].Rows.Count - 1; i++)
                        {
                            string strPDFOutput = APToolkit_Merge(objPlanningSheets_ds.Tables[0].Rows[i]["PSId"].ToString(), objPlanningSheets_ds.Tables[0].Rows[i]["PSStudentId"].ToString());
                            if (i == objPlanningSheets_ds.Tables[0].Rows.Count - 1)
                            {
                                strAttach = strAttach + @"\\Enrmgmt-web\f$\ENRMGMT\aap\StudentAdvisingSystem\File_Download\" + strPDFOutput;
                            }
                            else
                            {
                                strAttach = strAttach + @"\\Enrmgmt-web\f$\ENRMGMT\aap\StudentAdvisingSystem\File_Download\" + strPDFOutput + ",";
                            }
                        }
                    }
                }
                string strBody = "";
                if (Email_StdntNotes.Checked)
                {
                    strBody = "<html><head><title>Student Advising System</title></head><body>";
                    strBody = strBody + "<p>Hi " + FirstName.Text.ToString() + ":</p>";
                    strBody = strBody + "<p>" + Email_Intro.Text.ToString() + "</p>";
                    strBody = strBody + "<p>Notes to Student: " + Notes.Text.ToString().Replace("\n", "<br />"); //.Replace("\r", "<br />");
                    strBody = strBody + "<p>&nbsp;</p>";
                    strBody = strBody + "<p>" + Advisors_ddl.SelectedItem.Text.ToString() + "<br />";
                    strBody = strBody + Office_Descr_lbl.Text.ToString() + "<br />";
                    strBody = strBody + "California State University, Chico<br />";
                    strBody = strBody + "Phone: 530-898-" + Office_PhoneExt_lbl.Text.ToString() + "<br />";
                    strBody = strBody + "Our office is located in " + Office_Location_lbl.Text.ToString() + "</p>";
                    strBody = strBody + "</body></html>";
                }

                if ((strBody != "" || strAttach != "") && (Email_To_txt.Text.ToString() != "") && (Office_Email_Address.Text.ToString() != ""))
                {
                    string strCc = "";
                    if (Email_Cc_txt.Text.ToString() != "")
                    {
                        strCc = Email_Cc_txt.Text.ToString();
                    }
                    //06/2013 send email using esys-bulkmail
                    //06/2013 objSqlCollection.Insert_BulkEmail(Email_To_txt.Text.ToString(), Office_Email_Address.Text.ToString(), strCc, Email_Subj_txt.Text.ToString(), strBody.Replace("'", "''"), strAttach);

                    //06/2013 - send email using esys-bulkmail
                    send_email(Email_To_txt.Text.ToString(), Office_Email_Address.Text.ToString(), strCc, Email_Subj_txt.Text.ToString(), strBody.Replace("'", "''"), strAttach);
                    // end 06/2013

                    Response.Redirect("StudentInfo.aspx?dept=" + Office_Cd.Text.ToString() + "&pkid=" + PkId.Text.ToString());
                }
                else
                {
                    msg.Visible = true;
                    msg.Text = "Error: Email Message was not successfully sent.  Please make sure you have valid email addresses.";
                    Email_Send_btn.Focus();                    
                }                
            }
        }

        //06/2013 send email using esys-bulkmail
        protected void send_email(string strTo, string strFrom, string strCC, string strEmailSubject, string strBody, string strAttach)
        {
            try
            {
                MailMessage mMailMessage = new MailMessage();
                Attachment notes = new Attachment(@strAttach);

                mMailMessage.From = new MailAddress(@strFrom);
                mMailMessage.To.Add(new MailAddress(@strTo));
                mMailMessage.CC.Add(new MailAddress(@strCC));

                mMailMessage.Subject = @strEmailSubject;
                mMailMessage.Body = @strBody;
                mMailMessage.isBodyHtml = true;
                mMailMessage.Attachments.Add(notes);

                mMailMessage.Priority = MailPriority.Normal;
                SmtpClient mSmtpClient = new SmtpClient();
                mSmtpClient.Send(mMailMessage);
            }
            catch (Exception ex)
            {

            }

        }


        protected void Tools_Results_View(object sender, DataGridCommandEventArgs e)
        {
            Literal1.Text = "";
            if (e.Item.ItemType.ToString() != "Pager")
            {
                if (int.Parse(Save.Text.ToString()) == 0)
                {
                    msg.Visible = true;                    
                    Save.Text = "1";
                    Session["Save"] = Save.Text.ToString();

                    switch (Tools_Results_lbl.Text.ToString())
                    {
                        case "Prev Notes to Student":
                            BindPreviousAdvising();
                            PreviousAdvising_lbtn.Focus();
                            break;
                        case "Advisor Comments":
                            BindAdvisorComments();
                            AdvisorComments_lbtn.Focus();
                            break;
                        case "Planning Sheets":
                            BindPlanningSheet();
                            PlanningSheet_lbtn.Focus();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Session["Save"] = Save.Text.ToString();
                    Save_StudentInfo();
                    switch (Tools_Results_lbl.Text.ToString())
                    {
                        case "Prev Notes to Student":
                            Response.Redirect("StdntNotes.aspx?dept=" + Office_Cd.Text.ToString() + "&pkid=" + PkId.Text.ToString());
                            break;
                        case "Advisor Comments":
                            Response.Redirect("AdvisorComments.aspx?dept=" + Office_Cd.Text.ToString() + "&pkid=" + PkId.Text.ToString() + "&aid=0");
                            break;
                        case "Planning Sheets":
                            //Response.Redirect("PlanningSheet.aspx?dept=" + Office_Cd.Text.ToString() + "&pkid=" + PkId.Text.ToString() + "&psstdntid=" + Tools_Results_DG.DataKeys[e.Item.ItemIndex].ToString() + "&aid=0");
                            string strjscript = "<script language=javascript>";
                            strjscript = strjscript + "PlanningSheet_window=window.open('" + "PlanningSheet.aspx?dept=" + Office_Cd.Text.ToString() + "&pkid=" + PkId.Text.ToString() + "&psstdntid=" + Tools_Results_DG.DataKeys[e.Item.ItemIndex].ToString() + "&aid=0'" + ",'_blank','');PlanningSheet_window.focus();";
                            strjscript = strjscript + "</script" + ">";
                            Literal1.Text = strjscript;                            
                            //LinkButton objTools_Results_DG_lbtn = (LinkButton)e.CommandSource;
                            //if (objTools_Results_DG_lbtn.Text.ToString() == "View")
                            //{
                            //    Response.Redirect("PlanningSheet.aspx?dept=" + Office_Cd.Text.ToString() + "&pkid=" + PkId.Text.ToString() + "&psstdntid=" + Tools_Results_DG.DataKeys[e.Item.ItemIndex].ToString() + "&aid=0");
                            //}
                            //else if (objTools_Results_DG_lbtn.Text.ToString() == "Add")
                            //{
                            //    Response.Redirect("PlanningSheet.aspx?dept=" + Office_Cd.Text.ToString() + "&pkid=" + PkId.Text.ToString() + "&psid=" + Tools_Results_DG.DataKeys[e.Item.ItemIndex].ToString() + "&aid=1");   
                            //}
                            BindPlanningSheet();
                            PlanningSheet_lbtn.Focus();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        protected void Tools_Results_add_clicked(object sender, EventArgs e)
        {
            Literal1.Text = "";
            if (int.Parse(Save.Text.ToString()) == 0)
            {
                msg.Visible = true;
                Tools_Results_add_lbtn.Focus();
                Save.Text = "1";
                Session["Save"] = Save.Text.ToString();

                switch (Tools_Results_lbl.Text.ToString())
                {
                    case "Advisor Comments":
                        BindAdvisorComments();
                        break;
                    case "Planning Sheets":
                        BindPlanningSheet();                        
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Session["Save"] = Save.Text.ToString();
                switch (Tools_Results_lbl.Text.ToString())
                {
                    case "Advisor Comments":
                        Response.Redirect("AdvisorComments.aspx?dept=" + Office_Cd.Text.ToString() + "&pkid=" + PkId.Text.ToString() + "&aid=1");
                        break;
                    case "Planning Sheets":
                        if (Tools_Results_ddl.SelectedIndex > 0)
                        {
                            //Response.Redirect("PlanningSheet.aspx?dept=" + Office_Cd.Text.ToString() + "&pkid=" + PkId.Text.ToString() + "&psid=" + Tools_Results_ddl.SelectedItem.Value.ToString() + "&aid=1");
                            string strjscript = "<script language=javascript>";
                            strjscript = strjscript + "PlanningSheet_window=window.open('" + "PlanningSheet.aspx?dept=" + Office_Cd.Text.ToString() + "&pkid=" + PkId.Text.ToString() + "&psid=" + Tools_Results_ddl.SelectedItem.Value.ToString() + "&aid=1'" + ",'_blank','');PlanningSheet_window.focus();";
                            strjscript = strjscript + "</script" + ">";
                            Literal1.Text = strjscript;                            
                        }
                        BindPlanningSheet();
                        Tools_Results_ddl.Focus();
                        break;
                    default:
                        break;
                }
            }            
        }

        protected void StudentAdvised_Save(object sender, EventArgs e)
        {
            Literal1.Text = "";
            Save.Text = "1";
            Session["Save"] = Save.Text.ToString();

            Save_StudentInfo();

            Back.Focus();
            if (StudentAdvised_chk.Checked)
            {
                Session["Save"] = "0";
                Response.Redirect("AdvisorScreen.aspx?dept=" + Office_Cd.Text.ToString());
            }
            msg.Visible = false;
            msg.Text = "Warning: You have unsaved data on this page.  Click Save, or click on your selection again to continue.";
        }

        private void Save_StudentInfo()
        {
            //Save Student;
            string strQuestions = "";
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
            string strAdvisor = "";
            if (Advisors_ddl.SelectedItem.Value.ToString() != "Not Seen" && Advisors_ddl.SelectedIndex > 0)
            {
                strAdvisor = Advisors_ddl.SelectedItem.Value.ToString();
            }
            string strUpdateStdnt = "Ques = '" + strQuestions + "', AdvisorEmplid = '" + strAdvisor + "', TimeSpent = '" + TimeSpent_ddl.SelectedItem.Value.ToString() + "'";
            if (Advisors_ddl.SelectedItem.Value.ToString() == "Not Seen")
            {
                strUpdateStdnt = strUpdateStdnt + ", Office_Status = 'Not Seen'";
            }
            else if (StudentAdvised_chk.Checked)
            {
                strUpdateStdnt = strUpdateStdnt + ", Office_Status = 'Completed'";
            }
            objSqlCollection.Update_Students(PkId.Text.ToString(), strUpdateStdnt);

            DataSet objStdntNotes = objSqlCollection.Get_StdntNotes(PkId.Text.ToString());
            if (objStdntNotes.Tables[0].Rows.Count == 1)
            {
                if (Notes.Text.ToString() != "")
                {
                    objSqlCollection.Update_StdntNotes(PkId.Text.ToString(), Notes.Text.Replace("'", "''").ToString());
                }
                else
                {
                    objSqlCollection.Delete_StdntNotes(PkId.Text.ToString());
                }
            }
            else if (Notes.Text.ToString() != "")
            {
                objSqlCollection.Insert_StdntNotes(PkId.Text.ToString(), Notes.Text.Replace("'", "''").ToString());
            }

            switch (Tools_Results_lbl.Text.ToString())
            {
                case "Prev Notes to Student":
                    BindPreviousAdvising();
                    break;
                case "Advisor Comments":
                    BindAdvisorComments();
                    break;
                case "Planning Sheets":
                    BindPlanningSheet();
                    break;
                default:
                    break;
            }
        }

        protected void Back_AdvisorScreen(object sender, EventArgs e)
        {
            Literal1.Text = "";
            if (int.Parse(Save.Text.ToString()) == 0)
            {
                msg.Visible = true;
                Back.Focus();
                Save.Text = "1";
                Session["Save"] = Save.Text.ToString();

                switch (Tools_Results_lbl.Text.ToString())
                {
                    case "Prev Notes to Student":
                        BindPreviousAdvising();
                        break;
                    case "Advisor Comments":
                        BindAdvisorComments();
                        break;
                    case "Planning Sheets":
                        BindPlanningSheet();
                        break;
                    default:                        
                        break;
                }
            }
            else
            {
                Session["Save"] = "0";
                //objSqlCollection.Update_Students(PkId.Text.ToString(), "Office_Status='Completed'"); 
                Response.Redirect("AdvisorScreen.aspx?dept=" + Office_Cd.Text.ToString());
            }
        }

        protected void Tools_Results_Paging(object sender, DataGridPageChangedEventArgs e)
        {
            Literal1.Text = "";
            Tools_Results_DG.CurrentPageIndex = e.NewPageIndex;
            switch (Tools_Results_lbl.Text.ToString())
            {
                case "Prev Notes to Student":
                    BindPreviousAdvising();
                    break;
                case "Advisor Comments":
                    BindAdvisorComments();
                    break;
                case "Planning Sheets":
                    BindPlanningSheet();
                    break;
                default:
                    break;
            }
            Tools_Results_DG.Focus();
        }

        private string APToolkit_Merge(string PSId, string PSStudentId)
        { 
            APToolkitNET.APToolkit objPDF = new APToolkitNET.APToolkit();
            long objr;
            DataSet PlanSheets_ds = objSqlCollection.Get_PlanSheets(PSId, Office_Cd.Text.ToString());
            if (PlanSheets_ds.Tables[0].Rows.Count == 1)
            {
                string strFileName_Input = PlanSheets_ds.Tables[0].Rows[0]["PDF_FilePath"].ToString();
                string strFileName_Output = FirstName.Text.ToString().Substring(0, 1) + LastName.Text.ToString().Replace("'", "") + "_" + PlanSheets_ds.Tables[0].Rows[0]["PSName"].ToString().Replace(" ", "") + "_" + DateTime.Now.Ticks.ToString() + ".pdf";
                objr = (long)objPDF.OpenOutputFile(AppDomain.CurrentDomain.BaseDirectory.ToString() + "File_Download\\" + strFileName_Output);
                objr = objPDF.OpenInputFile(AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_Doc\\" + strFileName_Input);
                DataSet PSStdntsItems_ds = objSqlCollection.Get_PSStdntsItems(PSStudentId);
                for (int i = 0; i <= PSStdntsItems_ds.Tables[0].Rows.Count - 1; i++)
                {
                    DataRow PSStdntsItems_dr = PSStdntsItems_ds.Tables[0].Rows[i];
                    objPDF.SetFormFieldData(PSStdntsItems_dr["Notes_PDFName"].ToString().Trim(), PSStdntsItems_dr["Notes"].ToString().Trim(), 0);
                    objPDF.SetFormFieldData(PSStdntsItems_dr["Input1_PDFName"].ToString().Trim(), PSStdntsItems_dr["Input1"].ToString().Trim(), 0);
                    objPDF.SetFormFieldData(PSStdntsItems_dr["Input2_PDFName"].ToString().Trim(), PSStdntsItems_dr["Input2"].ToString().Trim(), 0);
                    objPDF.SetFormFieldData(PSStdntsItems_dr["Input3_PDFName"].ToString().Trim(), PSStdntsItems_dr["Input3"].ToString().Trim(), 0);
                }

                objPDF.SetFormFieldData("Name", FirstName.Text.ToString() + " " + LastName.Text.ToString(), 0);
                if (objPDF.GetFormFieldDataByName("Emplid").ToString() != null)
                {
                    objPDF.SetFormFieldData("Emplid", StudentId_lbl.Text.ToString(), 0);
                }
                if (objPDF.GetFormFieldDataByName("ID").ToString() != null)
                {
                    objPDF.SetFormFieldData("ID", StudentId_lbl.Text.ToString(), 0);
                }
                if (objPDF.GetFormFieldDataByName("Date").ToString() != null)
                {
                    DataSet PSStudents_ds = objSqlCollection.Get_PSStudents(PSStudentId);
                    if (PSStudents_ds.Tables[0].Rows.Count == 1)
                    {
                        DateTime PSdt = (DateTime)PSStudents_ds.Tables[0].Rows[0]["PSDate"];
                        objPDF.SetFormFieldData("Date", PSdt.ToShortDateString(), 0);
                    }
                }
                if (objPDF.GetFormFieldDataByName("Student_Notes").ToString() != null)
                {
                    DataSet Students_ds = objSqlCollection.Get_Students(PkId.Text.ToString(), Office_Cd.Text.ToString());
                    if (Students_ds.Tables[0].Rows.Count == 1)
                    {
                        objPDF.SetFormFieldData("Student_Notes", Students_ds.Tables[0].Rows[0]["AdvisorName"].ToString() + ": " + Students_ds.Tables[0].Rows[0]["Notes"].ToString().Trim(), 0);
                    }
                }

                objr = (long)objPDF.CopyForm(0, 0);
                objPDF.CloseOutputFile();

                return strFileName_Output;
            }
            else
            {
                return "";
            }
        }

        protected void PreText_clicked(object sender, EventArgs e)
        {
            Literal1.Text = "";
            if (PreText_ddl.SelectedIndex > 0)
            {
                Notes.Text = Notes.Text.ToString() + PreText_ddl.SelectedItem.Value.ToString();
            }
            BindPreText();
            Notes.Focus();

            switch (Tools_Results_lbl.Text.ToString())
            {
                case "Prev Notes to Student":
                    BindPreviousAdvising();
                    break;
                case "Advisor Comments":
                    BindAdvisorComments();
                    break;
                case "Planning Sheets":
                    BindPlanningSheet();
                    break;
                default:
                    break;
            }
            msg.Visible = false;
            msg.Text = "Warning: You have unsaved data on this page.  Click Save, or click on your selection again to continue.";
        }

        private void BindPreText()
        {
            DataSet PreText_ds = objSqlCollection.Get_PreText(Office_Cd.Text.ToString());
            if (PreText_ds.Tables[0].Rows.Count > 0)
            {
                DataTable PreText_dt = PreText_ds.Tables[0];
                DataRow dr = PreText_dt.NewRow();
                dr["Text_Cd"] = "";
                dr["Text_Descr"] = "";
                PreText_dt.Rows.InsertAt(dr, 0);
                PreText_ddl.DataSource = PreText_ds;
                PreText_ddl.DataBind();
                PreText_ddl.Visible = true;
                PreText_lbtn.Visible = true;
                PreText_lbl.Visible = true;
            }
            else
            {
                PreText_ddl.Visible = false;
                PreText_lbtn.Visible = false;
                PreText_lbl.Visible = false;
            }
        }

        protected void Advisors_changed(object sender, EventArgs e)
        {
            Literal1.Text = "";
            if (Advisors_ddl.SelectedItem.Value.ToString() == "")
            {
                StudentAdvised_chk.Enabled = false;
                StudentAdvised_btn.Enabled = false;
            }
            else if (Advisors_ddl.SelectedItem.Value.ToString().Substring(0, 7) == "9999999")
            {
                StudentAdvised_chk.Enabled = false;
                StudentAdvised_btn.Enabled = true;
            }
            else
            {
                if (TimeSpent_ddl.SelectedItem.Value.ToString() == "")
                {
                    StudentAdvised_chk.Enabled = false;
                    StudentAdvised_btn.Enabled = true;
                }
                else
                {
                    StudentAdvised_chk.Enabled = true;
                    StudentAdvised_btn.Enabled = true;
                    objSqlCollection.Update_Students(PkId.Text.ToString(), "AdvisorEmplid = '" + Advisors_ddl.SelectedItem.Value.ToString() + "'");
                }
            }
            Advisors_ddl.Focus();

            switch (Tools_Results_lbl.Text.ToString())
            {
                case "Prev Notes to Student":
                    BindPreviousAdvising();
                    break;
                case "Advisor Comments":
                    BindAdvisorComments();
                    break;
                case "Planning Sheets":
                    BindPlanningSheet();
                    break;
                default:
                    break;
            }
            msg.Visible = false;
            msg.Text = "Warning: You have unsaved data on this page.  Click Save, or click on your selection again to continue.";
        }

        protected void TimeSpent_changed(object sender, EventArgs e)
        {
             Literal1.Text = "";
             if (TimeSpent_ddl.SelectedItem.Value.ToString() == "")
             {
                 StudentAdvised_chk.Enabled = false;
             }
             else
             {
                 if (Advisors_ddl.SelectedItem.Value.ToString() == "" || Advisors_ddl.SelectedItem.Value.ToString().Substring(0, 7) == "9999999")
                 {
                     StudentAdvised_chk.Enabled = false;
                 }
                 else
                 {
                     StudentAdvised_chk.Enabled = true;
                 }
             }
             TimeSpent_ddl.Focus();

             switch (Tools_Results_lbl.Text.ToString())
             {
                 case "Prev Notes to Student":
                     BindPreviousAdvising();
                     break;
                 case "Advisor Comments":
                     BindAdvisorComments();
                     break;
                 case "Planning Sheets":
                     BindPlanningSheet();
                     break;
                 default:
                     break;
             }
             msg.Visible = false;
             msg.Text = "Warning: You have unsaved data on this page.  Click Save, or click on your selection again to continue.";
        }
    }
}
