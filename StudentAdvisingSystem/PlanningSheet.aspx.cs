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
    public partial class PlanningSheet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Office_Cd.Text = (string)Request.Params["dept"];
                PkId.Text = (string)Request.Params["pkid"];
                OrgPkId.Text = (string)Request.Params["pkid"];
                PSId.Text = (string)Request.Params["psid"];
                PSStudentId.Text = (string)Request.Params["psstdntid"];
                string action = (string)Request.Params["aid"];

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
                if (PSId.Text.ToString() != "" && PSStudentId.Text.ToString() == "" && action == "1") //Add
                {
                    DataSet StudentInfo_DS = objSqlCollection.Get_Students(PkId.Text.ToString(), Office_Cd.Text.ToString());
                    if (StudentInfo_DS.Tables[0].Rows.Count == 1)
                    {
                        DataRow dr = StudentInfo_DS.Tables[0].Rows[0];
                        FirstName.Text = dr["FirstName"].ToString();
                        LastName.Text = dr["LastName"].ToString();
                        StudentId_lbl.Text = dr["StudentId"].ToString();
                        CurrentDate_lbl.Text = DateTime.Now.ToLongDateString();
                    }
                    BindPlanSheet();
                    BindPSStudent_PS();
                    Save_btn.Visible = true;
                    SaveNew_btn.Visible = false;
                }
                else if (PSId.Text.ToString() == "" && PSStudentId.Text.ToString() != "" && action == "0") //View
                {
                    DataSet PSStudent_DS = objSqlCollection.Get_PSStudents(PSStudentId.Text.ToString());
                    if (PSStudent_DS.Tables[0].Rows.Count == 1)
                    {
                        DataSet StudentInfo_DS = objSqlCollection.Get_Students(PSStudent_DS.Tables[0].Rows[0]["pkID"].ToString(), Office_Cd.Text.ToString());
                        if (StudentInfo_DS.Tables[0].Rows.Count == 1)
                        {
                            DataRow dr = StudentInfo_DS.Tables[0].Rows[0];
                            FirstName.Text = dr["FirstName"].ToString();
                            LastName.Text = dr["LastName"].ToString();
                            StudentId_lbl.Text = dr["StudentId"].ToString();
                            DateTime PSDate = (DateTime)PSStudent_DS.Tables[0].Rows[0]["PSDate"];
                            CurrentDate_lbl.Text = PSDate.ToLongDateString();
                        }
                        PSId.Text = PSStudent_DS.Tables[0].Rows[0]["PSId"].ToString();
                        PkId.Text = PSStudent_DS.Tables[0].Rows[0]["pkID"].ToString();                        
                    }
                    BindPlanSheet();
                    BindPSStudent();
                    Save_btn.Visible = false;
                    SaveNew_btn.Visible = true;
                }                
            }
        }

        private void BindPlanSheet()
        {
            SqlCollection objSqlCollection = new SqlCollection();
            DataSet PS_DS = objSqlCollection.Get_PlanSheets(PSId.Text.ToString(), Office_Cd.Text.ToString());
            if (PS_DS.Tables[0].Rows.Count == 1)
            {
                PSName.Text = PS_DS.Tables[0].Rows[0]["PSName"].ToString();
                PSCatalog_yr.Text = PS_DS.Tables[0].Rows[0]["PSCatalog_yr"].ToString();
                PSGrps_DL.DataSource = objSqlCollection.Get_PSGrps(PSId.Text.ToString());
                PSGrps_DL.DataBind();
                for (int i = 0; i<= PSGrps_DL.Items.Count - 1; i++)
                {
                    DataList objPSItemsGrps_DL = (DataList)PSGrps_DL.Items[i].FindControl("PSItemsGrps_DL");
                    if (objPSItemsGrps_DL != null)
                    {
                        DataSet PSItemsGrps_DS = objSqlCollection.Get_PSItemsGrps(PSId.Text.ToString(), PSGrps_DL.DataKeys[i].ToString());
                        objPSItemsGrps_DL.DataSource = PSItemsGrps_DS;
                        objPSItemsGrps_DL.DataBind();
                        for (int n = 0; n <= objPSItemsGrps_DL.Items.Count - 1; n++)
                        {
                            //Input Boxes
                            if (PSItemsGrps_DS.Tables[0].Rows[n]["Notes_PDFName"].ToString() == "")
                            {
                                TextBox objPSNotes_txt = (TextBox)objPSItemsGrps_DL.Items[n].FindControl("PSNotes_txt");
                                objPSNotes_txt.BackColor = System.Drawing.Color.FloralWhite;
                            }
                            if (PSItemsGrps_DS.Tables[0].Rows[n]["Input1_PDFName"].ToString() == "")
                            {
                                TextBox objPSInput1_txt = (TextBox)objPSItemsGrps_DL.Items[n].FindControl("PSInput1_txt");
                                objPSInput1_txt.BackColor = System.Drawing.Color.FloralWhite;
                            }
                            if (PSItemsGrps_DS.Tables[0].Rows[n]["Input2_PDFName"].ToString() == "")
                            {
                                TextBox objPSInput2_txt = (TextBox)objPSItemsGrps_DL.Items[n].FindControl("PSInput2_txt");
                                objPSInput2_txt.BackColor = System.Drawing.Color.FloralWhite;
                            }
                            if (PSItemsGrps_DS.Tables[0].Rows[n]["Input3_PDFName"].ToString() == "")
                            {
                                TextBox objPSInput3_txt = (TextBox)objPSItemsGrps_DL.Items[n].FindControl("PSInput3_txt");
                                objPSInput3_txt.BackColor = System.Drawing.Color.FloralWhite;
                            }
                            
                            //Course List Checkbox List
                            CheckBoxList objCourseList_chkl = (CheckBoxList)objPSItemsGrps_DL.Items[n].FindControl("CourseList_chkl");

                            string strCourseList = "";
                            DataSet PSItemsCrses_DS = objSqlCollection.Get_PSItemsCrses(objPSItemsGrps_DL.DataKeys[n].ToString());                            
                            if (PSItemsCrses_DS.Tables[0].Rows.Count > 0)
                            {
                                string strLastClass_Subject = "";
                                for (int k = 0; k <= PSItemsCrses_DS.Tables[0].Rows.Count - 1; k++)
                                {
                                    if (!strCourseList.Contains(PSItemsCrses_DS.Tables[0].Rows[k]["Class_Subject"].ToString() + " " + PSItemsCrses_DS.Tables[0].Rows[k]["Class_Number"].ToString()))
                                    {
                                        ListItem objCourseList_chkl_Items = new ListItem();
                                        if (PSItemsCrses_DS.Tables[0].Rows[k]["Class_Subject"].ToString() != strLastClass_Subject)
                                        {
                                            //strCourseList = strCourseList + "; " + PSItemsCrses_DS.Tables[0].Rows[k]["Class_Subject"].ToString() + " " + PSItemsCrses_DS.Tables[0].Rows[k]["Class_Number"].ToString();                                            
                                            objCourseList_chkl_Items.Text = "<strong>" + PSItemsCrses_DS.Tables[0].Rows[k]["Class_Subject"].ToString() + "</strong> " + PSItemsCrses_DS.Tables[0].Rows[k]["Class_Number"].ToString();                                            
                                        }
                                        else
                                        {
                                            //strCourseList = strCourseList + ", " + PSItemsCrses_DS.Tables[0].Rows[k]["Class_Number"].ToString();                                            
                                            objCourseList_chkl_Items.Text = PSItemsCrses_DS.Tables[0].Rows[k]["Class_Number"].ToString();                                                                                       
                                        }
                                        objCourseList_chkl_Items.Value = PSItemsCrses_DS.Tables[0].Rows[k]["Course_Id"].ToString() + " " + PSItemsCrses_DS.Tables[0].Rows[k]["Class_Subject"].ToString() + " " + PSItemsCrses_DS.Tables[0].Rows[k]["Class_Number"].ToString();
                                        objCourseList_chkl.Items.Add(objCourseList_chkl_Items); 
                                        strLastClass_Subject = PSItemsCrses_DS.Tables[0].Rows[k]["Class_Subject"].ToString();
                                    }                                    
                                }
                                //objCourseList_lbl.Text = strCourseList.Substring(2);
                            }
                        }
                    }
                }
            }
        }

        private void BindPSStudent()
        { 
            SqlCollection objSqlCollection = new SqlCollection();
            DataSet PSStdntsItems_DS = objSqlCollection.Get_PSStdntsItems(PSStudentId.Text.ToString(), PSId.Text.ToString());            
            for (int i = 0; i <= PSGrps_DL.Items.Count - 1; i++)
            {
                DataList objPSItemsGrps_DL = (DataList)PSGrps_DL.Items[i].FindControl("PSItemsGrps_DL");
                if (objPSItemsGrps_DL != null)
                {
                    for (int n = 0; n <= objPSItemsGrps_DL.Items.Count - 1; n++)
                    {
                        TextBox objPSNotes_txt = (TextBox)objPSItemsGrps_DL.Items[n].FindControl("PSNotes_txt");
                        TextBox objPSInput1_txt = (TextBox)objPSItemsGrps_DL.Items[n].FindControl("PSInput1_txt");
                        TextBox objPSInput2_txt = (TextBox)objPSItemsGrps_DL.Items[n].FindControl("PSInput2_txt");
                        TextBox objPSInput3_txt = (TextBox)objPSItemsGrps_DL.Items[n].FindControl("PSInput3_txt");
                        CheckBoxList objCourseList_chkl = (CheckBoxList)objPSItemsGrps_DL.Items[n].FindControl("CourseList_chkl");

                        for (int k = 0; k <= PSStdntsItems_DS.Tables[0].Rows.Count - 1; k++)
                        {
                            if (PSStdntsItems_DS.Tables[0].Rows[k]["PSItemGroupId"].ToString() == objPSItemsGrps_DL.DataKeys[n].ToString())
                            {
                                objPSNotes_txt.Text = PSStdntsItems_DS.Tables[0].Rows[k]["Notes"].ToString();
                                objPSInput1_txt.Text = PSStdntsItems_DS.Tables[0].Rows[k]["Input1"].ToString();
                                objPSInput2_txt.Text = PSStdntsItems_DS.Tables[0].Rows[k]["Input2"].ToString();
                                objPSInput3_txt.Text = PSStdntsItems_DS.Tables[0].Rows[k]["Input3"].ToString();

                                if (objCourseList_chkl.Items.Count > 0)
                                {
                                    for (int m = 0; m <= objCourseList_chkl.Items.Count - 1; m++)
                                    {
                                        if (objPSNotes_txt.Text.Contains(objCourseList_chkl.Items[m].Value.ToString().Substring(7)) || objPSNotes_txt.Text.Contains(objCourseList_chkl.Items[m].Value.ToString().Substring(0, 6)))
                                        {
                                            objCourseList_chkl.Items[m].Selected = true;
                                            //objPSNotes_txt.Text = objPSNotes_txt.Text.Replace(objCourseList_chkl.Items[m].Value.ToString() + ", ", "").Replace(objCourseList_chkl.Items[m].Value.ToString(), "/", "").Replace(objCourseList_chkl.Items[m].Value.ToString(), "").Trim();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } 
        }

        private void BindPSStudent_PS()
        {
            SqlCollection objSqlCollection = new SqlCollection();
            for (int i = 0; i <= PSGrps_DL.Items.Count - 1; i++)
            {
                DataList objPSItemsGrps_DL = (DataList)PSGrps_DL.Items[i].FindControl("PSItemsGrps_DL");
                if (objPSItemsGrps_DL != null)
                {
                    for (int n = 0; n <= objPSItemsGrps_DL.Items.Count - 1; n++)
                    {
                        TextBox objPSNotes_txt = (TextBox)objPSItemsGrps_DL.Items[n].FindControl("PSNotes_txt");
                        TextBox objPSInput1_txt = (TextBox)objPSItemsGrps_DL.Items[n].FindControl("PSInput1_txt");
                        TextBox objPSInput2_txt = (TextBox)objPSItemsGrps_DL.Items[n].FindControl("PSInput2_txt");
                        TextBox objPSInput3_txt = (TextBox)objPSItemsGrps_DL.Items[n].FindControl("PSInput3_txt");
                        CheckBoxList objCourseList_chkl = (CheckBoxList)objPSItemsGrps_DL.Items[n].FindControl("CourseList_chkl");

                        DataSet PSItemsGrps_DS = objSqlCollection.Get_PSItemsGrps(objPSItemsGrps_DL.DataKeys[n].ToString());
                        if (PSItemsGrps_DS.Tables[0].Rows[0]["GetPSInfo"].ToString() == "1")
                        {
                            DataSet PSItemsCrses_DS = objSqlCollection.Get_PSItemsCrses(objPSItemsGrps_DL.DataKeys[n].ToString());
                            string strCourse_Id = "";
                            if (PSItemsCrses_DS.Tables[0].Rows.Count > 0)
                            {                                    
                                for (int m = 0; m <= PSItemsCrses_DS.Tables[0].Rows.Count - 1; m++)
                                {
                                    if (m == PSItemsCrses_DS.Tables[0].Rows.Count - 1)
                                    {
                                        strCourse_Id = strCourse_Id + PSItemsCrses_DS.Tables[0].Rows[m]["Course_Id"].ToString();
                                    }
                                    else
                                    {
                                        strCourse_Id = strCourse_Id + PSItemsCrses_DS.Tables[0].Rows[m]["Course_Id"].ToString() + "','";
                                    }
                                }
                                DataSet StudentEnroll_DS = objSqlCollection.Get_StudentEnrolled(StudentId_lbl.Text.ToString(), strCourse_Id);
                                if (StudentEnroll_DS.Tables[0].Rows.Count > 0)
                                {
                                    for (int j = 0; j <= StudentEnroll_DS.Tables[0].Rows.Count - 1; j++)
                                    {
                                        if (j == StudentEnroll_DS.Tables[0].Rows.Count - 1)
                                        {
                                            objPSNotes_txt.Text = objPSNotes_txt.Text.ToString() + StudentEnroll_DS.Tables[0].Rows[j]["subject"].ToString() + " " + StudentEnroll_DS.Tables[0].Rows[j]["class_number"].ToString();
                                            objPSInput1_txt.Text = objPSInput1_txt.Text.ToString() + StudentEnroll_DS.Tables[0].Rows[j]["strm"].ToString().Substring(1, 2) + StudentEnroll_DS.Tables[0].Rows[j]["strm"].ToString().Substring(3).Replace("2", "S").Replace("8", "F").Replace("6", "Su").Replace("4", "MJ");
                                            objPSInput2_txt.Text = objPSInput2_txt.Text.ToString() + StudentEnroll_DS.Tables[0].Rows[j]["unt_earned"].ToString();
                                            if (StudentEnroll_DS.Tables[0].Rows[j]["grading_basis"].ToString() == "PET" && StudentEnroll_DS.Tables[0].Rows[j]["grade_category"].ToString() == "PET")
                                            {
                                                objPSInput3_txt.Text = objPSInput3_txt.Text.ToString() + "*" + StudentEnroll_DS.Tables[0].Rows[j]["crse_grade_off"].ToString();
                                            }
                                            else
                                            {
                                                objPSInput3_txt.Text = objPSInput3_txt.Text.ToString() + StudentEnroll_DS.Tables[0].Rows[j]["crse_grade_off"].ToString();
                                            }
                                        }
                                        else
                                        {
                                            objPSNotes_txt.Text = objPSNotes_txt.Text.ToString() + StudentEnroll_DS.Tables[0].Rows[j]["subject"].ToString() + " " + StudentEnroll_DS.Tables[0].Rows[j]["class_number"].ToString() + "/";
                                            objPSInput1_txt.Text = objPSInput1_txt.Text.ToString() + StudentEnroll_DS.Tables[0].Rows[j]["strm"].ToString().Substring(1, 2) + StudentEnroll_DS.Tables[0].Rows[j]["strm"].ToString().Substring(3).Replace("2", "S").Replace("8", "F").Replace("6", "Su").Replace("4", "MJ") + "/";
                                            objPSInput2_txt.Text = objPSInput2_txt.Text.ToString() + StudentEnroll_DS.Tables[0].Rows[j]["unt_earned"].ToString() + "/";
                                            if (StudentEnroll_DS.Tables[0].Rows[j]["grading_basis"].ToString() == "PET" && StudentEnroll_DS.Tables[0].Rows[j]["grade_category"].ToString() == "PET")
                                            {
                                                objPSInput3_txt.Text = objPSInput3_txt.Text.ToString() + "*" + StudentEnroll_DS.Tables[0].Rows[j]["crse_grade_off"].ToString() + "/";
                                            }
                                            else
                                            {
                                                objPSInput3_txt.Text = objPSInput3_txt.Text.ToString() + StudentEnroll_DS.Tables[0].Rows[j]["crse_grade_off"].ToString() + "/";
                                            }
                                        }

                                        int intCourseList_chkl = 0;
                                        if (objCourseList_chkl.Items.Count > 0)
                                        {
                                            for (int k = 0; k <= objCourseList_chkl.Items.Count - 1; k++)
                                            {
                                                if (objCourseList_chkl.Items[k].Value.ToString().Substring(0, 6) == StudentEnroll_DS.Tables[0].Rows[j]["crse_id"].ToString())
                                                {
                                                    if (!objPSNotes_txt.Text.Contains(objCourseList_chkl.Items[k].Value.ToString().Substring(7)) || (objPSNotes_txt.Text.Contains(objCourseList_chkl.Items[k].Value.ToString().Substring(7)) && objPSNotes_txt.Text.Contains("/")))
                                                    {
                                                        if (objPSNotes_txt.Text.ToString().Substring(objPSNotes_txt.Text.ToString().Length - 1) == "/")
                                                        {
                                                            objPSNotes_txt.Text = objPSNotes_txt.Text.ToString().Substring(0, objPSNotes_txt.Text.ToString().Length - 1) + " (" + objCourseList_chkl.Items[k].Value.ToString().Substring(7) + ")/";
                                                        }
                                                        else
                                                        {
                                                            objPSNotes_txt.Text = objPSNotes_txt.Text.ToString() + " (" + objCourseList_chkl.Items[k].Value.ToString().Substring(7) + ")";
                                                        }
                                                    }
                                                    objCourseList_chkl.Items[k].Selected = true;
                                                    intCourseList_chkl += 1;
                                                }
                                            }                                            
                                        }
                                        //if (intCourseList_chkl == 0)
                                        //{
                                        //    objPSNotes_txt.Text = StudentEnroll_DS.Tables[0].Rows[j]["subject"].ToString() + " " + StudentEnroll_DS.Tables[0].Rows[j]["class_number"].ToString();
                                        //}
                                    }                                    
                                }
                            }                                
                        }                         
                    }
                }
            }
        }

        protected void PS_Save_clicked(object sender, EventArgs e)
        {
            Literal1.Text = "";
            if (SaveNew_btn.Visible) //Update
            {
                //SqlCollection objSqlCollection = new SqlCollection();
                //objSqlCollection.Update_PSStudents(PSStudentId.Text.ToString());
                UpdatePSStudents();
                Response.Redirect("PlanningSheet.aspx?dept=" + Office_Cd.Text.ToString() + "&pkid=" + OrgPkId.Text.ToString() + "&psstdntid=" + PSStudentId.Text.ToString() + "&aid=0");
            }
            else
            {
                InsertPSStudents();
                Response.Redirect("PlanningSheet.aspx?dept=" + Office_Cd.Text.ToString() + "&pkid=" + PkId.Text.ToString() + "&psstdntid=" + PSStudentId.Text.ToString() + "&aid=0");
            }
        }

        private void UpdatePSStudents()
        {           
            SqlCollection objSqlCollection = new SqlCollection();
            //objSqlCollection.Update_PSStudents(PSStudentId.Text.ToString());
            for (int i = 0; i <= PSGrps_DL.Items.Count - 1; i++)
            {
                DataList objPSItemsGrps_DL = (DataList)PSGrps_DL.Items[i].FindControl("PSItemsGrps_DL");
                if (objPSItemsGrps_DL != null)
                {
                    for (int n = 0; n <= objPSItemsGrps_DL.Items.Count - 1; n++)
                    {
                        DataSet PSItemsGrps_DS = objSqlCollection.Get_PSItemsGrps(objPSItemsGrps_DL.DataKeys[n].ToString());
                        string GetPSInfo = PSItemsGrps_DS.Tables[0].Rows[0]["GetPSInfo"].ToString();

                        TextBox objPSNotes_txt = (TextBox)objPSItemsGrps_DL.Items[n].FindControl("PSNotes_txt");
                        TextBox objPSInput1_txt = (TextBox)objPSItemsGrps_DL.Items[n].FindControl("PSInput1_txt");
                        TextBox objPSInput2_txt = (TextBox)objPSItemsGrps_DL.Items[n].FindControl("PSInput2_txt");
                        TextBox objPSInput3_txt = (TextBox)objPSItemsGrps_DL.Items[n].FindControl("PSInput3_txt");
                        string strCourseList_chkl = "";
                        CheckBoxList objCourseList_chkl = (CheckBoxList)objPSItemsGrps_DL.Items[n].FindControl("CourseList_chkl");
                        if (objCourseList_chkl.Items.Count > 0)
                        {
                            for (int m = 0; m <= objCourseList_chkl.Items.Count - 1; m++)
                            {
                                if (objCourseList_chkl.Items[m].Selected && !objPSNotes_txt.Text.Contains(objCourseList_chkl.Items[m].Value.ToString().Substring(7)) && !objPSNotes_txt.Text.Contains(objCourseList_chkl.Items[m].Value.ToString().Substring(0,6)) && !objPSNotes_txt.Text.Contains("@"))
                                {
                                    if (strCourseList_chkl != "")
                                    {
                                        strCourseList_chkl = strCourseList_chkl + "/" + objCourseList_chkl.Items[m].Value.ToString().Substring(7);
                                    }
                                    else
                                    {
                                        strCourseList_chkl = objCourseList_chkl.Items[m].Value.ToString().Substring(7);
                                    }
                                }
                            }
                        }
                        //if (strCourseList_chkl != "" && objPSNotes_txt.Text.ToString() != "")
                        //{
                        if (strCourseList_chkl != "")
                        {
                            if (objPSNotes_txt.Text.ToString().Contains("/"))
                            {
                                strCourseList_chkl = strCourseList_chkl + "/" + objPSNotes_txt.Text.ToString().Replace("'", "''");
                            }
                            else
                            {
                                strCourseList_chkl = strCourseList_chkl + " " + objPSNotes_txt.Text.ToString().Replace("'", "''");
                            }

                        }
                        else
                        {
                            strCourseList_chkl = objPSNotes_txt.Text.ToString().Replace("'", "''");
                        }
                        //}
                        objSqlCollection.Update_PSStdntsItems(PSStudentId.Text.ToString(), objPSItemsGrps_DL.DataKeys[n].ToString(), strCourseList_chkl, objPSInput1_txt.Text.ToString(), objPSInput2_txt.Text.ToString(), objPSInput3_txt.Text.ToString());
                    }
                }
            }            
        }

        protected void PS_SaveNew_clicked(object sender, EventArgs e)
        {
            Literal1.Text = "";
            InsertPSStudents();
            Response.Redirect("PlanningSheet.aspx?dept=" + Office_Cd.Text.ToString() + "&pkid=" + OrgPkId.Text.ToString() + "&psstdntid=" + PSStudentId.Text.ToString() + "&aid=0");
        }

        private void InsertPSStudents()
        {
            SqlCollection objSqlCollection = new SqlCollection();
            PSStudentId.Text = objSqlCollection.Insert_PSStudents(StudentId_lbl.Text.ToString(), Office_Cd.Text.ToString(), PSId.Text.ToString(), OrgPkId.Text.ToString());
            for (int i = 0; i <= PSGrps_DL.Items.Count - 1; i++)
            {
                DataList objPSItemsGrps_DL = (DataList)PSGrps_DL.Items[i].FindControl("PSItemsGrps_DL");
                if (objPSItemsGrps_DL != null)
                {   
                    for (int n = 0; n <= objPSItemsGrps_DL.Items.Count - 1; n++)
                    {
                        DataSet PSItemsGrps_DS = objSqlCollection.Get_PSItemsGrps(objPSItemsGrps_DL.DataKeys[n].ToString());
                        string GetPSInfo = PSItemsGrps_DS.Tables[0].Rows[0]["GetPSInfo"].ToString();

                        TextBox objPSNotes_txt = (TextBox)objPSItemsGrps_DL.Items[n].FindControl("PSNotes_txt");
                        TextBox objPSInput1_txt = (TextBox)objPSItemsGrps_DL.Items[n].FindControl("PSInput1_txt");
                        TextBox objPSInput2_txt = (TextBox)objPSItemsGrps_DL.Items[n].FindControl("PSInput2_txt");
                        TextBox objPSInput3_txt = (TextBox)objPSItemsGrps_DL.Items[n].FindControl("PSInput3_txt");
                        string strCourseList_chkl = "";
                        CheckBoxList objCourseList_chkl = (CheckBoxList)objPSItemsGrps_DL.Items[n].FindControl("CourseList_chkl");
                        if (objCourseList_chkl.Items.Count > 0)
                        {
                            for (int m = 0; m <= objCourseList_chkl.Items.Count - 1; m++)
                            {
                                if (objCourseList_chkl.Items[m].Selected && !objPSNotes_txt.Text.Contains(objCourseList_chkl.Items[m].Value.ToString().Substring(7)) && !objPSNotes_txt.Text.Contains(objCourseList_chkl.Items[m].Value.ToString().Substring(0,6)) && !objPSNotes_txt.Text.Contains("@"))
                                {
                                    if (strCourseList_chkl != "")
                                    {
                                        strCourseList_chkl = strCourseList_chkl + "/" + objCourseList_chkl.Items[m].Value.ToString().Substring(7);
                                    }
                                    else
                                    {
                                        strCourseList_chkl = objCourseList_chkl.Items[m].Value.ToString().Substring(7);
                                    }
                                }
                            }
                        }
                        //if (strCourseList_chkl != "" && objPSNotes_txt.Text.ToString() != "")
                        //{
                        if (strCourseList_chkl != "")
                        {
                            if (objPSNotes_txt.Text.ToString().Contains("/"))
                            {
                                strCourseList_chkl = strCourseList_chkl + "/" + objPSNotes_txt.Text.ToString().Replace("'", "''");
                            }
                            else
                            {
                                strCourseList_chkl = strCourseList_chkl + " " + objPSNotes_txt.Text.ToString().Replace("'", "''");
                            }
                        }
                        else
                        {
                            strCourseList_chkl = objPSNotes_txt.Text.ToString().Replace("'", "''");
                        }
                        //}
                        objSqlCollection.Insert_PSStdntsItems(PSStudentId.Text.ToString(), objPSItemsGrps_DL.DataKeys[n].ToString(), strCourseList_chkl, objPSInput1_txt.Text.ToString(), objPSInput2_txt.Text.ToString(), objPSInput3_txt.Text.ToString());
                    }
                }
            }
        }

        protected void StudentInfo_clicked(object sender, EventArgs e)
        {
            Literal1.Text = "";
            Response.Redirect("StudentInfo.aspx?dept=" + Office_Cd.Text.ToString() + "&pkid=" + OrgPkId.Text.ToString());
        }

        protected void PS_SavePDF_clicked(object sender, EventArgs e)
        {
            SqlCollection objSqlCollection = new SqlCollection();
            if (SaveNew_btn.Visible) //Update
            {
                UpdatePSStudents();
            }
            else
            {
                InsertPSStudents();
                SaveNew_btn.Visible = true;
            }
            APToolkitNET.APToolkit objPDF = new APToolkitNET.APToolkit();
            long objr;
            DataSet PlanSheets_ds = objSqlCollection.Get_PlanSheets(PSId.Text.ToString(), Office_Cd.Text.ToString());
            if (PlanSheets_ds.Tables[0].Rows.Count == 1)
            {
                string strFileName_Input = PlanSheets_ds.Tables[0].Rows[0]["PDF_FilePath"].ToString();
                string strFileName_Output = FirstName.Text.ToString().Substring(0, 1) + LastName.Text.ToString().Replace("'", "") + "_" + PlanSheets_ds.Tables[0].Rows[0]["PSName"].ToString().Replace(" ", "") + "_" + DateTime.Now.Ticks.ToString() + ".pdf";
                objr = (long)objPDF.OpenOutputFile(AppDomain.CurrentDomain.BaseDirectory.ToString() + "File_Download\\" + strFileName_Output);
                objr = objPDF.OpenInputFile(AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_Doc\\" + strFileName_Input);
                DataSet PSStdntsItems_ds = objSqlCollection.Get_PSStdntsItems(PSStudentId.Text.ToString());
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
                    DataSet PSStudents_ds = objSqlCollection.Get_PSStudents(PSStudentId.Text.ToString());
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

                //Response.Redirect("~/File_Download/" + strFileName_Output);
                string strjscript = "<script language=javascript>";
                strjscript = strjscript + "PlanningSheetPDF_window=window.open('File_Download/" + strFileName_Output + "','_blank','');PlanningSheetPDF_window.focus();";
                strjscript = strjscript + "</script" + ">";
                Literal1.Text = strjscript;
            }            
        }
    }
}
