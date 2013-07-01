using System;
using System.Data;
using System.Configuration;

namespace StudentAdvisingSystem
{
    public class SqlCollection
    {
        private DataClass objDataClass = new DataClass();
        private string SqlCmd;

        public DataSet Get_OfficeInfo(string strOffice_Cd)
        {
            SqlCmd = "SELECT Office_Descr, CASE WHEN substring(Advisor_Type, 1, 1) IN ('A', 'E', 'I', 'O', 'U') THEN 'an ' ELSE 'a ' END + Advisor_Type AS Advisor_Type, Office_Location, CellPhoneAllow, Office_PhoneExt, ThankYou_Msg, CellPhone_Msg, Office_Email_Address, CellPhone_WaitNum, Email_Intro from tblStudentAdvisingSystem_OfficeInfo_dtl where Office_Cd = '" + strOffice_Cd + "'";
            return objDataClass.ExecuteSQLStatement("ENRDBR", "OfficeInfo", SqlCmd);
        }

        public void Update_OfficeInfo(string Office_Cd, string Office_Descr, string Advisor_Type, string Office_Location, string CellPhoneAllow, string Office_PhoneExt, string ThankYou_Msg, string Office_Email_Address, string CellPhone_Msg, string CellPhone_WaitNum, string Email_Intro)
        {
            SqlCmd = "Update tblStudentAdvisingSystem_OfficeInfo_dtl set Office_Descr = '" + Office_Descr + "', Advisor_Type = '" + Advisor_Type + "', Office_Location = '" + Office_Location + "', CellPhoneAllow = " + CellPhoneAllow + ", Office_PhoneExt = '" + Office_PhoneExt + "', ThankYou_Msg = '" + ThankYou_Msg + "', Office_Email_Address = '" + Office_Email_Address + "', CellPhone_Msg = '" + CellPhone_Msg + "', CellPhone_WaitNum = " + CellPhone_WaitNum + ", Email_Intro = '" + Email_Intro + "' where Office_Cd = '" + Office_Cd + "'";
            objDataClass.ExecuteSQLStatement("ENRDBW", "Update_OfficeInfo", SqlCmd);
        }

        public DataSet Get_Emplid(string FirstName, string LastName, string Last4SSN)
        {
            if (Last4SSN.Length == 9)
            {
                SqlCmd = "select distinct t.emplid, t.first_name_preferred, t.last_name_preferred from cc_personal_data_dtl t where (t.emplid = '" + Last4SSN + "') or (t.national_id = '" + Last4SSN + "')";
            }
            else if (FirstName.Length > 0 && LastName.Length > 0 && Last4SSN.Length == 0)
            {
                SqlCmd = "select distinct t.emplid, t.first_name_preferred, t.last_name_preferred, t.name_preferred, t.gender_desc, to_char(t.birthdate, 'MM/DD') birthdate, substr(t.national_id, 6, 4) national_id from cc_personal_data_dtl t where (upper(t.first_name_preferred) like '" + FirstName.Replace("'", "''").ToUpper() + "%') and (upper(t.last_name_preferred) like '" + LastName.Replace("'", "''").ToUpper() + "%') order by t.name_preferred";
            }
            else
            {
                SqlCmd = "select distinct t.emplid, t.first_name_preferred, t.last_name_preferred from cc_personal_data_dtl t where (upper(t.first_name_preferred) = '" + FirstName.Replace("'", "''").ToUpper() + "') and  (upper(t.last_name_preferred) = '" + LastName.Replace("'", "''").ToUpper() + "') and (substr(t.national_id, 6, 4) = '" + Last4SSN + "')";
            }

            return objDataClass.ExecuteSQLStatement("RDSDBR", "Emplid", SqlCmd);
        }
        /* 10/29/09 - check first 4 characters of college code  */
        public DataView Get_Majors(string strOffice_Cd)
        {
            SqlCmd = "select distinct t.acad_plan Major, p.acad_plan_ldesc Major_Description, t.degree, p.college " +
                        "from enrmgmt.adv_major_acad_plan_dtl t, as_acad_plan_tbl p " +
                        "where t.acad_plan = p.acad_plan and extract(year from sysdate)-1 between substr(t.catalog_yr, 1, 4) and '20' || substr(t.catalog_yr, 6, 2) and (t.incatalog = 1 or t.degree like '%(PRE)') " +
                     "union all (select distinct p.acad_plan Major, p.acad_plan_ldesc Major_Description, t.degree, p.college " +
                        "from enrmgmt.adv_major_acad_plan_dtl t, as_acad_plan_tbl p, as_acad_subplan_tbl s " +
                        "where t.acad_plan = s.acad_sub_plan and p.acad_plan = s.acad_plan and substr(t.degree, 1, 2) = substr(p.acad_plan, 9, 2) and extract(year from sysdate)-1 between substr(t.catalog_yr, 1, 4) and '20' || substr(t.catalog_yr, 6, 2) and t.incatalog = 1)";
            
            DataSet Majors_DS = objDataClass.ExecuteSQLStatement("RDSDBR", "Majors", SqlCmd);            
            DataView Majors_DV = new DataView(Majors_DS.Tables[0]);
            string strRowFilter = "";

            //July 2010 Pulls all majors for the Office of International Education
            //Study Abroad,International Advising, and International Admissions will fall under OIE - supporting UGRD and Grad students.
            if ((strOffice_Cd.Contains("GIISSA")) || (strOffice_Cd.Contains("GIISINST")) || (strOffice_Cd.Contains("AAPIADV")))
            {
                strRowFilter = "substring(Degree, 1, 1) = 'B'";
                strRowFilter = strRowFilter + " OR substring(Major, 9, 1) = 'M'";
                strRowFilter = strRowFilter + " OR Major = 'UNDCNONEUN'";
            }
            else if (strOffice_Cd.Contains("GRAD"))
            {
                
               strRowFilter = "substring(Major, 9, 1) = 'M'";
                if (strOffice_Cd.Contains("AAP") == false)
                {
                    // 1/7/09 GIIS has a four digit code
                    if(strOffice_Cd.Substring(0,4) == "GIIS")
                    {
                       strRowFilter = strRowFilter + " and College = '" + strOffice_Cd.Substring(0, 4) + "'";  
                    }
                    else{
                        strRowFilter = strRowFilter + " and College = '" + strOffice_Cd.Substring(0, 3) + "'";
                    }
                }
            }
                
            else
            {
                strRowFilter = "substring(Degree, 1, 1) = 'B'";
                if (strOffice_Cd.Contains("AAP") == false)
                {
                    // 1/7/09 GIIS has a four digit code
                    if (strOffice_Cd.Substring(0, 4) == "GIIS")
                    {
                        strRowFilter = strRowFilter + " and College = '" + strOffice_Cd.Substring(0, 4) + "'";
                    }
                    else
                    {
                        strRowFilter = strRowFilter + " and College = '" + strOffice_Cd.Substring(0, 3) + "'";
                    }
                }
                strRowFilter = strRowFilter + " OR Major = 'UNDCNONEUN'";
            }
            Majors_DV.RowFilter = strRowFilter;
            Majors_DV.AllowNew = true;  
            DataRowView Majors_DRV;
            Majors_DRV = Majors_DV.AddNew();
            Majors_DRV["Major"] = "";
            Majors_DRV["Major_Description"] = "";
            //Majors_DV.RowStateFilter = DataViewRowState.Modifiedcurrent | DataViewRowState.Added;            
            Majors_DV.Sort = "Major_Description, Degree";

            return Majors_DV;
        }

        public DataSet Get_QuesFields_dtl(string strOffice_Cd)
        {
            SqlCmd = "Select distinct tblStudentAdvisingSystem_QuesFields_tbl.QuesField_Cd, tblStudentAdvisingSystem_QuesFields_tbl.QuesField_Descr " +
                     "from tblStudentAdvisingSystem_QuesFields_tbl inner join tblStudentAdvisingSystem_QuesFields_dtl on tblStudentAdvisingSystem_QuesFields_dtl.QuesField_cd = tblStudentAdvisingSystem_QuesFields_tbl.QuesField_Cd " +
                     "where tblStudentAdvisingSystem_QuesFields_dtl.Office_Cd = '" + strOffice_Cd + "' " +
                     "order by tblStudentAdvisingSystem_QuesFields_tbl.QuesField_Descr";
            return objDataClass.ExecuteSQLStatement("ENRDBR", "QuesFields_dtl", SqlCmd);
        }

        public void Insert_QuesFields_dtl(string Office_Cd, string QuesField_Cd)
        {            
            SqlCmd = "Insert into tblStudentAdvisingSystem_QuesFields_dtl(Office_Cd, QuesField_Cd) Values('" + Office_Cd + "', '" + QuesField_Cd + "')";
            objDataClass.ExecuteSQLStatement("ENRDBW", "Insert_QuesFields_dtl", SqlCmd);
        }

        public void Delete_QuesFields_dtl(string Office_Cd)
        {
            SqlCmd = "delete tblStudentAdvisingSystem_QuesFields_dtl where Office_Cd = '" + Office_Cd + "'";
            objDataClass.ExecuteSQLStatement("ENRDBW", "Delete_QuesFields_dtl", SqlCmd);
        }

        public DataSet Get_QuesFields_tbl()
        {
            SqlCmd = "Select distinct tblStudentAdvisingSystem_QuesFields_tbl.QuesField_Cd, tblStudentAdvisingSystem_QuesFields_tbl.QuesField_Descr, tblStudentAdvisingSystem_QuesFields_tbl.QuesField_Descr + ' (' + tblStudentAdvisingSystem_QuesFields_tbl.QuesField_Cd + ') ' as QuesField_LDescr " +
                     "from tblStudentAdvisingSystem_QuesFields_tbl order by tblStudentAdvisingSystem_QuesFields_tbl.QuesField_Descr";
            return objDataClass.ExecuteSQLStatement("ENRDBR", "QuesFields_tbl", SqlCmd);
        }

        public void Insert_QuesFields_tbl(string QuesField_Cd, string QuesField_Descr)
        {
            SqlCmd = "Insert into tblStudentAdvisingSystem_QuesFields_tbl(QuesField_Cd, QuesField_Descr) Values('" + QuesField_Cd.ToUpper() + "', '" + QuesField_Descr + "')";
            objDataClass.ExecuteSQLStatement("ENRDBW", "Insert_QuesFields_tbl", SqlCmd);
        }

        public DataSet Get_TextMsgCarriers()
        {
            SqlCmd = "Select distinct Name, EmailDomain from tblTextMsgCarriers order by Name";
            DataSet TextMsgCarriers_DS = objDataClass.ExecuteSQLStatement("ENRDBR", "TextMsgCarriers", SqlCmd);
            DataRow TextMsgCarriers_DR = TextMsgCarriers_DS.Tables[0].NewRow();
            TextMsgCarriers_DR["Name"] = "";
            TextMsgCarriers_DR["EmailDomain"] = "";
            TextMsgCarriers_DS.Tables[0].Rows.InsertAt(TextMsgCarriers_DR, 0);
            return TextMsgCarriers_DS;
        }

        public string Insert_Students(string StudentId, string FirstName, string LastName, string Phone, string Email_Address, string Major, string Minor, string Ques, string CellPhone, string Office_Cd)
        {
            SqlCmd = "Insert into tblStudentAdvisingSystem_Students_dtl(StudentId, FirstName, LastName, Phone, Email_Address, Major, Minor, Ques, CellPhone, SMS_Sent, PastVisit, Visit_DateTime, Office_Cd, Office_Status) " +
                        "Select '" + StudentId + "', '" + FirstName + "', '" + LastName + "', '" + Phone + "', '" + Email_Address + "', '" + Major + "', '" + Minor + "', '" + Ques + "', '" + CellPhone + "', 0, " + 
                            "(select case when count(*) >= 1 then 'Yes' else 'No' end as PastVisit from tblStudentAdvisingSystem_Students_dtl where StudentId = '" + StudentId + "'), Getdate(), '" + Office_Cd + "', 'Signed In'";
            objDataClass.ExecuteSQLStatement("ENRDBW", "Insert_Students", SqlCmd);
            SqlCmd = "Select Max(pkID) from tblStudentAdvisingSystem_Students_dtl where StudentID='" + StudentId + "' and FirstName='" + FirstName + "' and LastName='" + LastName + "' and Phone='" + Phone + "' " +
                        "and Email_Address='" + Email_Address + "' and Major='" + Major + "' and Minor='" + Minor + "' and Ques='" + Ques + "' and CellPhone='" + CellPhone + "' and SMS_Sent=0 and convert(varchar(12), Getdate(), 101) = convert(varchar(12), Visit_DateTime, 101) " +
                        "and Office_Cd='" + Office_Cd + "' and Office_Status='Signed In'";
            DataSet Students_ds = objDataClass.ExecuteSQLStatement("ENRDBR", "Insert_Students", SqlCmd);
            if (Students_ds.Tables[0].Rows.Count == 1)
            {
                return Students_ds.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }

        public void Update_Students(string PkId, string strUpdateSql)
        {
            SqlCmd = "Update tblStudentAdvisingSystem_Students_dtl set " + strUpdateSql + " " +
                        "Where PkId = " + PkId;
            objDataClass.ExecuteSQLStatement("ENRDBW", "Update_Students", SqlCmd);
        }

        public void Update_PrevStudents(string StudentId, string FirstName, string LastName, string Last4SSN)
        {
            SqlCmd = "Update tblStudentAdvisingSystem_Students_dtl set StudentId = '" + StudentId + "' where FirstName = '" + FirstName + "' and LastName = '" + LastName + "' and StudentId = '" + Last4SSN + "'";
            objDataClass.ExecuteSQLStatement("ENRDBW", "Update_PrevStudents", SqlCmd);
        }

        public DataSet Get_Students_Screening(string Office_Cd)
        {
            SqlCmd = "Select distinct a.* from tblStudentAdvisingSystem_Students_dtl a where a.Office_Cd like '" + Office_Cd + "%' and a.Office_Status = 'Signed In' and convert(varchar(12), a.Visit_DateTime, 101) = convert(varchar(12), Getdate(), 101) order by a.Visit_DateTime";
            return objDataClass.ExecuteSQLStatement("ENRDBR", "Students_Screening", SqlCmd);
        }

        public DataView Get_Office_Status(string Office_Cd)
        {
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add("Office_Status");
            dt.Columns.Add(new DataColumn("Office_Status", typeof(String)));
            dt.Columns.Add(new DataColumn("Office_Status_Value", typeof(String)));
            DataRow dr;
            dr = dt.NewRow();
            dr[0] = "Status: Signed In";
            dr[1] = "Signed In";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "Status: Not Seen";
            dr[1] = "Not Seen";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "Status: In Progress";
            dr[1] = "In Progress";
            dt.Rows.Add(dr);

            SqlCmd = "Select distinct 'See: ' + a.AdvisorName as Office_Status, a.AdvisorEmplid as Office_Status_Value from tblStudentAdvisingSystem_Advisors_dtl a where a.IsActive = 1 and a.Office_Cd = '" + Office_Cd + "'";
            ds.Merge(objDataClass.ExecuteSQLStatement("ENRDBR", "Office_Status", SqlCmd));
            
            SqlCmd = "Select distinct 'Refer: ' + a.Office_Cd as Office_Status, a.Office_Cd as Office_Status_Value from tblStudentAdvisingSystem_OfficeInfo_dtl a where substring(Office_Cd, 1, 3) = '" + Office_Cd.Substring(0, 3) + "' and (not a.Office_Cd = '" + Office_Cd + "')";
            ds.Merge(objDataClass.ExecuteSQLStatement("ENRDBR", "Office_Status", SqlCmd));

            DataView dv = new DataView(ds.Tables["Office_Status"]);
            dv.Sort = "Office_Status";
            return dv;
        }

        public void Delete_Students(string PkId)
        {
            SqlCmd = "Delete from tblStudentAdvisingSystem_StdntNotes_dtl where PkId = " + PkId;
            objDataClass.ExecuteSQLStatement("ENRDBW", "Students", SqlCmd);

            SqlCmd = "Select * from tblStudentAdvisingSystem_PSStudents_dtl where Pkid = " + PkId;
            DataSet PSStudents_ds = objDataClass.ExecuteSQLStatement("ENRDBR", "Students", SqlCmd);
            string PSStudentId = "";
            if (PSStudents_ds.Tables[0].Rows.Count == 1)
            {
                PSStudentId = PSStudents_ds.Tables[0].Rows[0]["PSStudentId"].ToString();
                SqlCmd = "Delete from tblStudentAdvisingSystem_PSStudents_dtl where PkId = " + PkId;
                objDataClass.ExecuteSQLStatement("ENRDBW", "Students", SqlCmd);

                SqlCmd = "Delete from tblStudentAdvisingSystem_PSStdntsItems_dtl where PSStudentId = " + PSStudentId;
                objDataClass.ExecuteSQLStatement("ENRDBW", "Students", SqlCmd);
            }            
            
            SqlCmd = "Delete from tblStudentAdvisingSystem_Students_dtl where PkId = " + PkId;
            objDataClass.ExecuteSQLStatement("ENRDBW", "Students", SqlCmd);
        }

        public DataSet Get_Students_AdvisorScreen(string Office_Cd)
        {
            SqlCmd = "Select distinct a.*, b.AdvisorName, c.CellPhone_WaitNum, c.Office_Email_Address, c.CellPhone_Msg, c.Office_Descr from tblStudentAdvisingSystem_Students_dtl a left outer join tblStudentAdvisingSystem_Advisors_dtl b on a.Office_Cd = b.Office_Cd and a.AdvisorEmplid = b.AdvisorEmplid inner join tblStudentAdvisingSystem_OfficeInfo_dtl c on a.Office_Cd = c.Office_Cd where a.Office_Cd = '" + Office_Cd + "' and (a.Office_Status = 'Waiting' or a.Office_Status = 'In Progress') and convert(varchar(12), a.Visit_DateTime, 101) = convert(varchar(12), Getdate(), 101) order by a.Visit_DateTime";
            return objDataClass.ExecuteSQLStatement("ENRDBR", "Students_AdvisorScreen", SqlCmd);
        }

        public DataSet Get_Students_AdvisorScreen_FrontDesk(string Office_Cd)
        {
            if (Office_Cd.Contains("AAP"))
            {
                SqlCmd = "Select distinct a.*, b.AdvisorName, c.CellPhone_WaitNum, c.Office_Email_Address, c.CellPhone_Msg, c.Office_Descr from tblStudentAdvisingSystem_Students_dtl a left outer join tblStudentAdvisingSystem_Advisors_dtl b on a.Office_Cd = b.Office_Cd and a.AdvisorEmplid = b.AdvisorEmplid inner join tblStudentAdvisingSystem_OfficeInfo_dtl c on a.Office_Cd = c.Office_Cd where a.Office_Cd like '" + Office_Cd + "%' and (a.Office_Status = 'Waiting' or a.Office_Status = 'In Progress') and convert(varchar(12), a.Visit_DateTime, 101) = convert(varchar(12), Getdate(), 101) order by a.Visit_DateTime";
            }
            else
            {
                SqlCmd = "Select distinct a.*, b.AdvisorName, c.CellPhone_WaitNum, c.Office_Email_Address, c.CellPhone_Msg, c.Office_Descr from tblStudentAdvisingSystem_Students_dtl a left outer join tblStudentAdvisingSystem_Advisors_dtl b on a.Office_Cd = b.Office_Cd and a.AdvisorEmplid = b.AdvisorEmplid inner join tblStudentAdvisingSystem_OfficeInfo_dtl c on a.Office_Cd = c.Office_Cd where a.Office_Cd = '" + Office_Cd + "' and (a.Office_Status = 'Waiting' or a.Office_Status = 'In Progress') and convert(varchar(12), a.Visit_DateTime, 101) = convert(varchar(12), Getdate(), 101) order by a.Visit_DateTime";
            }
            return objDataClass.ExecuteSQLStatement("ENRDBR", "Students_AdvisorScreen", SqlCmd);
        }

        public DataSet Get_Students(string PkId, string Office_Cd)
        {
            SqlCmd = "Select distinct a.*, b.AdvisorName, c.Notes from tblStudentAdvisingSystem_Students_dtl a left outer join tblStudentAdvisingSystem_Advisors_dtl b on a.Office_Cd = b.Office_Cd and a.AdvisorEmplid = b.AdvisorEmplid left outer join tblStudentAdvisingSystem_StdntNotes_dtl c on a.pkID = c.pkID where a.Office_Cd = '" + Office_Cd + "' and a.pkID = " + PkId;
            return objDataClass.ExecuteSQLStatement("ENRDBR", "Students", SqlCmd);
        }

        public DataSet Get_Students(string StudentId, string FirstName, string LastName, string Phone, string Email_Address, string Major, string Minor, string Ques, string CellPhone, string Office_Cd)
        {
            SqlCmd = "Select distinct a.* from tblStudentAdvisingSystem_Students_dtl a where a.StudentId = '" + StudentId + "' and a.FirstName = '" + FirstName + "' and a.LastName = '" + LastName + "' and a.Phone = '" + Phone + "' and a.Email_Address = '" + Email_Address + "' and a.Major = '" + Major + "' and a.Minor = '" + Minor + "' and a.Ques = '" + Ques + "' and a.CellPhone = '" + CellPhone + "' and a.Office_Cd = '" + Office_Cd + "' " +
                        "and a.Visit_DateTime = (select max(a1.Visit_DateTime) from tblStudentAdvisingSystem_Students_dtl a1 where a.StudentId = a1.StudentId and a.FirstName = a1.FirstName and a.LastName = a1.LastName and a.Phone = a1.Phone and a.Email_Address = a1.Email_Address and a.Major = a1.Major and a.Minor = a1.Minor and a.Ques = a1.Ques and a.CellPhone = a1.CellPhone and a.Office_Cd = a1.Office_Cd)";
            return objDataClass.ExecuteSQLStatement("ENRDBR", "Students", SqlCmd);
        }

        public DataSet Get_Students_PS(string emplid, DateTime vdt)
        {
            SqlCmd = "select a.emplid, a.first_name_preferred, a.last_name_preferred, a.mail_phone_nbr, a.email_preferred, b.term, b.term_sdesc, b.acad_career, b.acad_prog_primary, b.enrl_status_sdesc, b.acad_level_bot_sdesc, nvl(b.acad_standing_ldesc, 'N/A') as acad_standing_ldesc, " +
                        "c.pri_acad_plan_ldesc, c.pri_acad_sub_plan1_ldesc, c.sec_acad_plan_ldesc, c.sec_acad_sub_plan1_ldesc, " +
                        "b.term_units_taken_progress, b.term_units_taken_gpa, b.term_units_total as term_units_passed, b.term_gpa, b.term_grade_points, " +
                        "b.chico_taken_gpa, b.chico_units_passed, b.chico_gpa, b.chico_grade_points, " +
                        "b.cum_taken_gpa, b.cum_total_passed as cum_units_passed, b.cum_gpa, b.cum_grade_points " +
                     "from cc_personal_data_dtl a, rec_student_car_term_dtl b, rec_stdnt_pln_subpln_term_dtl c " +
                     "where a.emplid = b.emplid and b.term = (select max(b1.term) from rec_student_car_term_dtl b1, as_session_tbl b2 where b1.emplid = b.emplid and b1.acad_career = b2.acad_career and b1.term <= b2.term and to_date('" + vdt.ToShortDateString() + "', 'MM/DD/YYYY') <= b2.term_end_date) " + //between b2.term_begin_date and b2.term_end_date) " +
                     "and b.emplid = c.emplid and b.cterm_sid = c.cterm_sid(+) and b.acad_career = c.acad_career(+) and (c.pri_acad_plan_type = 'MAJ' or c.sec_acad_plan_type = 'MAJ') and a.emplid = '" + emplid + "'";
            DataSet Student_PS_ds = objDataClass.ExecuteSQLStatement("RDSDBR", "Students_PS", SqlCmd);
            if (Student_PS_ds.Tables[0].Rows.Count == 1)
            {
                if (Student_PS_ds.Tables[0].Rows[0]["acad_standing_ldesc"].ToString() == "N/A")
                {
                    SqlCmd = "select nvl(d.acad_standing_ldesc, 'N/A') as acad_standing_ldesc " +                           
                         "from rec_student_car_term_dtl b, rec_student_car_term_dtl d " +
                         "where b.term = (select max(b1.term) from rec_student_car_term_dtl b1, as_session_tbl b2 where b1.emplid = b.emplid and b1.acad_career = b2.acad_career and b1.term <= b2.term and to_date('" + vdt.ToShortDateString() + "', 'MM/DD/YYYY')  between b2.term_begin_date and b2.term_end_date) " +
                         "and b.emplid = d.emplid and b.acad_career = d.acad_career and d.term = (select max(d1.term) from rec_student_car_term_dtl d1 where d1.emplid = d.emplid and d1.acad_career = d.acad_career and d1.term < b.term) and b.emplid = '" + emplid + "'";
                    DataSet Student_PS_Prev_ds = objDataClass.ExecuteSQLStatement("RDSDBR", "Students_PS_Prev", SqlCmd);

                    if (Student_PS_Prev_ds.Tables[0].Rows.Count == 1)
                    {
                        Student_PS_ds.Tables[0].Rows[0]["acad_standing_ldesc"] = Student_PS_Prev_ds.Tables[0].Rows[0]["acad_standing_ldesc"].ToString();
                    }
                }
            }
            return Student_PS_ds;
        }

        public DataSet Get_StudentsNextTermInfo_PS(string emplid, string cur_term)
        {
            SqlCmd = "select e.emplid, e.term_sdesc as nxt_term_sdesc, e.term_units_taken_progress as nxt_term_units_taken_progress, e.term_units_taken_gpa as nxt_term_units_taken_gpa, e.term_units_passed_gpa + e.term_units_passed_nogpa as nxt_term_units_passed, e.term_gpa as nxt_term_gpa, e.term_grade_points as nxt_term_grade_points from rec_student_car_term_dtl e where e.emplid = '" + emplid + "' and e.term = (select min(e1.term) from rec_student_car_term_dtl e1 where e1.emplid = e.emplid and e1.acad_career = e.acad_career and e1.term > '" + cur_term + "')";
            return objDataClass.ExecuteSQLStatement("RDSDBR", "StudentsNextTermInfo_PS", SqlCmd);
        }

        public DataSet Get_Neg_ServiceIndicator(string emplid)
        {
            SqlCmd = "select t.indicator_sdesc || ' (' || t.indicator || '): ' || t.indicator_reason_sdesc || ' (' || t.indicator_reason || ')' from rdsprd.cc_service_indicator_dtl t where t.emplid = '" + emplid + "' and t.positive_srvc_indicator = 'N' and t.indicator_active_dt <= sysdate";
            return objDataClass.ExecuteSQLStatement("RDSDBR", "Neg_ServiceIndicator", SqlCmd);
        }

        public DataSet Get_OLLE_StudentGroup(string emplid)
        {
            SqlCmd = "select distinct g.descrshort || ' (' || h.stdnt_group || ')' from RDSSTG_HRSA.PS_STDNT_GROUP_TBL g, RDSSTG_HRSA.PS_STDNT_GRPS_HIST h where g.stdnt_group = h.stdnt_group and h.emplid = '" + emplid + "' and h.effdt <= sysdate  and h.eff_status = 'A'  and h.stdnt_group IN ('APOL', 'APCG', 'DQOL', 'DQCG') order by h.effdt desc";
            return objDataClass.ExecuteSQLStatement("RDSDBR", "OLLEStudentGroup", SqlCmd);
        }

        public DataSet Get_OLLE_Status(string emplid)
        {
            SqlCmd = "select distinct a.* from tblOLLE_Students a where a.StudentID = '" + emplid + "'";
            return objDataClass.ExecuteSQLStatement("ENRDBR", "OLLE_Status", SqlCmd);
        }

        public DataSet Get_Advisors(string Office_Cd)
        {
            SqlCmd = "Select distinct a.AdvisorName, a.AdvisorEmplid, a.AdvisorName + ' (' + a.AdvisorEmplid + ')' as AdvisorName_LDesc, a.IsActive from tblStudentAdvisingSystem_Advisors_dtl a where a.IsActive = 1 and a.Office_Cd = '" + Office_Cd + "' order by a.AdvisorName";
            DataSet ds = objDataClass.ExecuteSQLStatement("ENRDBR", "Advisors", SqlCmd);
            DataTable dt = ds.Tables[0];
            DataRow dr;
            dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dt.Rows.InsertAt(dr, 0);
            dr = dt.NewRow();
            dr[0] = "Status: Not Seen";
            dr[1] = "Not Seen";
            dt.Rows.InsertAt(dr, dt.Rows.Count);
            return ds;
        }

        public DataSet Get_Advisors_NotActive(string Office_Cd)
        {
            SqlCmd = "Select distinct a.AdvisorName, a.AdvisorEmplid, a.AdvisorName + ' (' + a.AdvisorEmplid + ')' as AdvisorName_LDesc, a.IsActive from tblStudentAdvisingSystem_Advisors_dtl a where a.IsActive = 0 and a.Office_Cd = '" + Office_Cd + "' order by a.AdvisorName";
            DataSet ds = objDataClass.ExecuteSQLStatement("ENRDBR", "Advisors", SqlCmd);
            return ds;
        }

        public void Insert_Advisors(string Office_Cd, string AdvisorEmplid, string AdvisorName)
        {
            SqlCmd = "Insert into tblStudentAdvisingSystem_Advisors_dtl(Office_Cd, AdvisorEmplid, AdvisorName, IsActive) Values('" + Office_Cd + "','" + AdvisorEmplid + "','" + AdvisorName + "',1)";
            objDataClass.ExecuteSQLStatement("ENRDBW", "Insert_Advisors", SqlCmd);
        }

        //10/23/2009
        //If the advisor is currently active, then update their name and active/inactive status
        public void Update_Advisors(string Office_Cd, string AdvisorEmplid, string AdvisorName, string IsActive)
        {
            SqlCmd = "Update tblStudentAdvisingSystem_Advisors_dtl set AdvisorName = '" + AdvisorName + "', IsActive = " + IsActive + " where Office_Cd = '" + Office_Cd + "' and AdvisorEmplid = '" + AdvisorEmplid + "' and IsActive = 1";
            objDataClass.ExecuteSQLStatement("ENRDBW", "Update_Advisors", SqlCmd);
        }

        public DataSet Get_PrevStudents(string pkid, string Office_Cd)
        {
            string strOffice_Cd = "";
            if (Office_Cd.Substring(0, 3) == "AAP")
            {
                strOffice_Cd = " and a.Office_Cd like '" + Office_Cd.Substring(0, 3) + "%' ";
            }
            else
            {
                strOffice_Cd = " and a.Office_Cd = '" + Office_Cd + "' ";
            }
            SqlCmd = "Select distinct a.PkID, a.Office_Cd, cast(a.Visit_DateTime as varchar) as [Date], b.AdvisorName as Advised, a.Visit_DateTime, a.Office_Status from tblStudentAdvisingSystem_Students_dtl a inner join tblStudentAdvisingSystem_Advisors_dtl b on a.AdvisorEmplid = b.AdvisorEmplid and a.Office_Cd = b.Office_Cd where a.StudentId = (select a1.StudentId from tblStudentAdvisingSystem_Students_dtl a1 where a1.pkid = " + pkid + ")" + strOffice_Cd + " and (not a.pkid = " + pkid + ") order by a.Visit_DateTime desc";
            return objDataClass.ExecuteSQLStatement("ENRDBR", "PrevStudents", SqlCmd);
        }        

        public DataView Get_AdvisorComments(string StudentId, string Office_Cd)
        {
            string strOffice_Cd = "";
            if (Office_Cd.Substring(0, 3) == "AAP")
            {
                strOffice_Cd = " and a.Office_Cd like '" + Office_Cd.Substring(0, 3) + "%' ";
            }
            else
            {
                strOffice_Cd = " and a.Office_Cd = '" + Office_Cd + "' ";
            }
            SqlCmd = "Select distinct a.AdvCmts_PkId as PkId, cast(a.Comments_Dt as varchar) as [Date], b.AdvisorName as Commented, b.AdvisorName, a.* from tblStudentAdvisingSystem_AdvCmts_dtl a inner join tblStudentAdvisingSystem_Advisors_dtl b on a.AdvisorEmplid = b.AdvisorEmplid and a.Office_Cd = b.Office_Cd where a.StudentId = '" + StudentId + "' and a.ExtViewAllow = '0' " + strOffice_Cd + 
                        "Union All " +
                        "(Select distinct a.AdvCmts_PkId as PkId, cast(a.Comments_Dt as varchar) as [Date], b.AdvisorName as Commented, b.AdvisorName, a.* from tblStudentAdvisingSystem_AdvCmts_dtl a inner join tblStudentAdvisingSystem_Advisors_dtl b on a.AdvisorEmplid = b.AdvisorEmplid and a.Office_Cd = b.Office_Cd where a.StudentId = '" + StudentId + "' and a.ExtViewAllow = '1')";
            DataView dv = objDataClass.ExecuteSQLStatement("ENRDBR", "AdvisorComments", SqlCmd).Tables[0].DefaultView;
            dv.Sort = "Comments_Dt desc";
            return dv;
        }

        public void Insert_AdvisorComment(string pkid, string Office_Cd, string Comments, string ExtViewAllow)
        {
            SqlCmd = "Insert into tblStudentAdvisingSystem_AdvCmts_dtl(StudentId, Office_Cd, Comments_Dt, AdvisorEmplid, Comments, ExtViewAllow) " +
                        "select a1.StudentId, a1.Office_Cd, Getdate(), a1.AdvisorEmplid, '" + Comments + "', '" + ExtViewAllow + "' from tblStudentAdvisingSystem_Students_dtl a1 where a1.pkid = " + pkid + " and a1.office_Cd = '" + Office_Cd + "'";
            objDataClass.ExecuteSQLStatement("ENRDBW", "AdvisorComments", SqlCmd);
        }

        public void Insert_AdvisorComment(string StudentId, string Office_Cd, string AdvisorEmplid, string Comments, string ExtViewAllow)
        {
            SqlCmd = "Insert into tblStudentAdvisingSystem_AdvCmts_dtl(StudentId, Office_Cd, Comments_Dt, AdvisorEmplid, Comments, ExtViewAllow) " +
                        "select '" + StudentId + "', '" + Office_Cd + "', Getdate(), '" + AdvisorEmplid + "', '" + Comments + "', '" + ExtViewAllow + "'";
            objDataClass.ExecuteSQLStatement("ENRDBW", "AdvisorComments", SqlCmd);
        }

        public DataSet Get_PlanSheets(string Office_Cd)
        {
            string strOffice_Cd = "";
            if (Office_Cd.Substring(0, 3) == "AAP")
            {
                strOffice_Cd = " and p.PSOffice_Cd like '" + Office_Cd.Substring(0, 3) + "%' ";
            }
            else
            {
                strOffice_Cd = " and p.PSOffice_Cd = '" + Office_Cd + "' ";
            }
            SqlCmd = "Select distinct 'Add' as action, p.PSId as PkId, p.PSOffice_Cd as Office_Cd, p.PSName, p.PSCatalog_yr as PSDate, p.PSName + ' (' + p.PSCatalog_yr + ')' as PSName_Ldesc from tblStudentAdvisingSystem_PlanSheets_dtl p where p.IsValid = '1' " + strOffice_Cd + " order by p.PSName, p.PSCatalog_yr desc";
            return objDataClass.ExecuteSQLStatement("ENRDBR", "PlanSheets", SqlCmd);
        }

        public DataSet Get_PlanSheets(string psid, string Office_Cd)
        {
            string strOffice_Cd = "";
            if (Office_Cd.Substring(0, 3) == "AAP")
            {
                strOffice_Cd = " and p.PSOffice_Cd like '" + Office_Cd.Substring(0, 3) + "%' ";
            }
            else
            {
                strOffice_Cd = " and p.PSOffice_Cd = '" + Office_Cd + "' ";
            }
            SqlCmd = "Select distinct p.* from tblStudentAdvisingSystem_PlanSheets_dtl p where p.psid = " + psid + strOffice_Cd;
            return objDataClass.ExecuteSQLStatement("ENRDBR", "PlanSheets", SqlCmd);
        }

        public DataSet Get_PlanSheets_All(string Office_Cd)
        {
            string strOffice_Cd = "";
            if (Office_Cd.Substring(0, 3) == "AAP")
            {
                strOffice_Cd = " p.PSOffice_Cd like '" + Office_Cd.Substring(0, 3) + "%' ";
            }
            else
            {
                strOffice_Cd = " p.PSOffice_Cd = '" + Office_Cd + "' ";
            }
            SqlCmd = "Select distinct p.* from tblStudentAdvisingSystem_PlanSheets_dtl p where " + strOffice_Cd + " order by p.IsValid desc, p.PSName, p.PSCatalog_yr desc";
            return objDataClass.ExecuteSQLStatement("ENRDBR", "PlanSheets", SqlCmd);
        }

        public void Update_PlanSheets(string PSId, string PSName, string PSOffice_Cd, string PSCatalog_yr, string PSIsValid, string PDF_FilePath)
        {
            SqlCmd = "Update tblStudentAdvisingSystem_PlanSheets_dtl set PSName = '" + PSName + "', PSCatalog_yr = '" + PSCatalog_yr + "', IsValid = " + PSIsValid + ", PDF_FilePath = '" + PDF_FilePath + "' where PSId = " + PSId + " and PSOffice_Cd = '" + PSOffice_Cd + "'";
            objDataClass.ExecuteSQLStatement("ENRDBW", "PlanSheets", SqlCmd);
        }

        public string Insert_PlanSheets(string PSName, string PSOffice_Cd, string PSCatalog_yr, string PSIsValid, string PDF_FilePath)
        {
            SqlCmd = "Insert into tblStudentAdvisingSystem_PlanSheets_dtl(PSName, PSOffice_Cd, PSCatalog_yr, IsValid, PDF_FilePath) VALUES('" + PSName + "','" + PSOffice_Cd + "','" + PSCatalog_yr + "','" + PSIsValid + "','" + PDF_FilePath + "')";
            objDataClass.ExecuteSQLStatement("ENRDBW", "PlanSheets", SqlCmd);
            SqlCmd = "Select distinct a.PSId from tblStudentAdvisingSystem_PlanSheets_dtl a where a.PSName = '" + PSName + "' and a.PSCatalog_yr = '" + PSCatalog_yr + "' and a.IsValid = " + PSIsValid + " and a.PDF_FilePath = '" + PDF_FilePath + "' and a.PSOffice_Cd = '" + PSOffice_Cd + "' and a.PSId = (select max(a1.PSId) from tblStudentAdvisingSystem_PlanSheets_dtl a1 where a1.PSName = a.PSName and a1.PSOffice_Cd = a.PSOffice_Cd and a1.PSCatalog_yr = a.PSCatalog_yr and a1.IsValid = a.IsValid and a1.PDF_FilePath = a.PDF_FilePath)";
            DataSet ds = objDataClass.ExecuteSQLStatement("ENRDBR", "PlanSheets", SqlCmd);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return ds.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }

        public DataSet Get_PSGrps()
        {
            SqlCmd = "SELECT tblStudentAdvisingSystem_PSGrps_tbl.* FROM tblStudentAdvisingSystem_PSGrps_tbl order by tblStudentAdvisingSystem_PSGrps_tbl.PSGroup_Descr";
            DataSet ds = objDataClass.ExecuteSQLStatement("ENRDBR", "PSGrps", SqlCmd);
            DataTable dt = ds.Tables[0];
            DataRow dr;
            dr = dt.NewRow();
            //dr[0] = "";
            dr[1] = "";
            dt.Rows.InsertAt(dr, 0);
            return ds;
        }

        public DataSet Get_PSGrps(string psid)
        {
            SqlCmd = "SELECT tblStudentAdvisingSystem_PSGrps_dtl.*, tblStudentAdvisingSystem_PSGrps_tbl.PSGroup_Descr AS PSGroup_Descr " +
                        "FROM tblStudentAdvisingSystem_PSGrps_dtl INNER JOIN tblStudentAdvisingSystem_PSGrps_tbl ON tblStudentAdvisingSystem_PSGrps_dtl.PSGroupId = tblStudentAdvisingSystem_PSGrps_tbl.PSGroupId " +
                        "WHERE tblStudentAdvisingSystem_PSGrps_dtl.PSId = " + psid + " order by tblStudentAdvisingSystem_PSGrps_dtl.PSGroup_SeqNum";
            return objDataClass.ExecuteSQLStatement("ENRDBR", "PSGrps", SqlCmd);
        }

        public void Insert_PSGrps(string PSGroup_Descr)
        {
            SqlCmd = "Select a.* from tblStudentAdvisingSystem_PSGrps_tbl a where a.PSGroup_Descr = '" + PSGroup_Descr + "'";
            DataSet ds = objDataClass.ExecuteSQLStatement("ENRDBR", "PSGrps", SqlCmd);
            if (ds.Tables[0].Rows.Count == 0)
            {
                SqlCmd = "Insert into tblStudentAdvisingSystem_PSGrps_tbl(PSGroup_Descr) VALUES('" + PSGroup_Descr + "')";
                objDataClass.ExecuteSQLStatement("ENRDBW", "PSGrps", SqlCmd);
            }
        }

        public void Insert_PSGrps(string PSId, string PSGroupId, string PSGroup_SubDescr, string PSGroup_SeqNum, string PSInput1_descr, string PSInput2_descr, string PSInput3_descr)
        {
            SqlCmd = "Insert into tblStudentAdvisingSystem_PSGrps_dtl(PSId, PSGroupId, PSGroup_SubDescr, PSGroup_SeqNum, PSInput1_descr, PSInput2_descr, PSInput3_descr) VALUES(" + PSId + "," + PSGroupId + ",'" + PSGroup_SubDescr + "'," + PSGroup_SeqNum + ",'" + PSInput1_descr + "','" + PSInput2_descr + "','" + PSInput3_descr + "')";
            objDataClass.ExecuteSQLStatement("ENRDBW", "PSGrps", SqlCmd);
        }

        public void Delete_PSGrps(string psid)
        {
            SqlCmd = "Delete tblStudentAdvisingSystem_PSGrps_dtl where PSId = " + psid;
            objDataClass.ExecuteSQLStatement("ENRDBW", "PSGrps", SqlCmd);
        }

        public DataSet Get_PSItemsGrps_All(string psid)
        {
            SqlCmd = "SELECT tblStudentAdvisingSystem_PSItemsGrps_dtl.*, tblStudentAdvisingSystem_PSItems_tbl.PSItem_Descr AS PSItem_Descr, tblStudentAdvisingSystem_PSGrps_dtl.PSGroup_SeqNum, tblStudentAdvisingSystem_PSGrps_tbl.PSGroup_Descr  " +
                        "FROM tblStudentAdvisingSystem_PSItemsGrps_dtl INNER JOIN tblStudentAdvisingSystem_PSItems_tbl ON tblStudentAdvisingSystem_PSItemsGrps_dtl.PSItemId = tblStudentAdvisingSystem_PSItems_tbl.PSItemId " +
                        "INNER JOIN tblStudentAdvisingSystem_PSGrps_dtl ON tblStudentAdvisingSystem_PSGrps_dtl.PSId = tblStudentAdvisingSystem_PSItemsGrps_dtl.PSId and tblStudentAdvisingSystem_PSGrps_dtl.PSGroupId = tblStudentAdvisingSystem_PSItemsGrps_dtl.PSGroupId " +
                        "INNER JOIN tblStudentAdvisingSystem_PSGrps_tbl ON tblStudentAdvisingSystem_PSGrps_tbl.PSGroupId = tblStudentAdvisingSystem_PSItemsGrps_dtl.PSGroupId " +
                        "WHERE tblStudentAdvisingSystem_PSItemsGrps_dtl.PSId = " + psid + " order by tblStudentAdvisingSystem_PSGrps_dtl.PSGroup_SeqNum, tblStudentAdvisingSystem_PSItemsGrps_dtl.PSItem_SeqNum, tblStudentAdvisingSystem_PSItems_tbl.PSItem_Descr";
            return objDataClass.ExecuteSQLStatement("ENRDBR", "PSItemsGrps", SqlCmd);
        }

        public DataSet Get_PSItemsGrps(string psid, string psgroupid)
        {
            SqlCmd = "SELECT tblStudentAdvisingSystem_PSItemsGrps_dtl.*, tblStudentAdvisingSystem_PSItems_tbl.PSItem_Descr AS PSItem_Descr, tblStudentAdvisingSystem_PSGrps_dtl.PSGroup_SeqNum, tblStudentAdvisingSystem_PSGrps_tbl.PSGroup_Descr  " +
                        "FROM tblStudentAdvisingSystem_PSItemsGrps_dtl INNER JOIN tblStudentAdvisingSystem_PSItems_tbl ON tblStudentAdvisingSystem_PSItemsGrps_dtl.PSItemId = tblStudentAdvisingSystem_PSItems_tbl.PSItemId " +
                        "INNER JOIN tblStudentAdvisingSystem_PSGrps_dtl ON tblStudentAdvisingSystem_PSGrps_dtl.PSId = tblStudentAdvisingSystem_PSItemsGrps_dtl.PSId and tblStudentAdvisingSystem_PSGrps_dtl.PSGroupId = tblStudentAdvisingSystem_PSItemsGrps_dtl.PSGroupId " +
                        "INNER JOIN tblStudentAdvisingSystem_PSGrps_tbl ON tblStudentAdvisingSystem_PSGrps_tbl.PSGroupId = tblStudentAdvisingSystem_PSItemsGrps_dtl.PSGroupId " +
                        "WHERE tblStudentAdvisingSystem_PSItemsGrps_dtl.PSId = " + psid + " and tblStudentAdvisingSystem_PSItemsGrps_dtl.PSGroupId = " + psgroupid + " order by tblStudentAdvisingSystem_PSItemsGrps_dtl.PSItem_SeqNum, tblStudentAdvisingSystem_PSItems_tbl.PSItem_Descr";
            return objDataClass.ExecuteSQLStatement("ENRDBR", "PSItemsGrps", SqlCmd);
        }

        public DataSet Get_PSItemsGrps(string psitemgroupid)
        {
            SqlCmd = "SELECT tblStudentAdvisingSystem_PSItemsGrps_dtl.*, tblStudentAdvisingSystem_PSItems_tbl.PSItem_Descr AS PSItem_Descr " +
                        "FROM tblStudentAdvisingSystem_PSItemsGrps_dtl INNER JOIN tblStudentAdvisingSystem_PSItems_tbl ON tblStudentAdvisingSystem_PSItemsGrps_dtl.PSItemId = tblStudentAdvisingSystem_PSItems_tbl.PSItemId " +                        
                        "WHERE tblStudentAdvisingSystem_PSItemsGrps_dtl.PSItemGroupId = " + psitemgroupid;
            return objDataClass.ExecuteSQLStatement("ENRDBR", "PSItemsGrps", SqlCmd);
        }

        public string Insert_PSItemsGrps(string PSId, string PSGroupId, string PSItemId, string PSItem_SeqNum, string GetPSInfo, string Notes_PDFName, string Input1_PDFName, string Input2_PDFName, string Input3_PDFName)
        {
            SqlCmd = "Insert into tblStudentAdvisingSystem_PSItemsGrps_dtl(PSId, PSGroupId, PSItemId, PSItem_SeqNum, GetPSInfo, Notes_PDFName, Input1_PDFName, Input2_PDFName, Input3_PDFName) VALUES (" + PSId + "," + PSGroupId + "," + PSItemId + "," + PSItem_SeqNum + "," + GetPSInfo + ",'" + Notes_PDFName + "','" + Input1_PDFName + "','" + Input2_PDFName + "','" + Input3_PDFName + "')";
            objDataClass.ExecuteSQLStatement("ENRDBW", "PSItemsGrps", SqlCmd);
            SqlCmd = "Select PSItemGroupId from tblStudentAdvisingSystem_PSItemsGrps_dtl where PSId = " + PSId + " and PSGroupId = " + PSGroupId + " and PSItemId = " + PSItemId + " and PSItem_SeqNum = " + PSItem_SeqNum + " and GetPSInfo = " + GetPSInfo + " and Notes_PDFName = '" + Notes_PDFName + "' and Input1_PDFName = '" + Input1_PDFName + "' and Input2_PDFName = '" + Input2_PDFName + "' and Input3_PDFName = '" + Input3_PDFName + "'";
            DataSet ds = objDataClass.ExecuteSQLStatement("ENRDBR", "PSItemsGrps", SqlCmd);
            return ds.Tables[0].Rows[0][0].ToString();
        }

        public void Update_PSItemsGrps(string PSItemGroupId, string PSItemId, string PSItem_SeqNum, string GetPSInfo, string Notes_PDFName, string Input1_PDFName, string Input2_PDFName, string Input3_PDFName)
        {
            SqlCmd = "Update tblStudentAdvisingSystem_PSItemsGrps_dtl set PSItemId = " + PSItemId + ", PSItem_SeqNum = " + PSItem_SeqNum + ", GetPSInfo = " + GetPSInfo + ", Notes_PDFName = '" + Notes_PDFName + "', Input1_PDFName = '" + Input1_PDFName + "', Input2_PDFName = '" + Input2_PDFName + "', Input3_PDFName = '" + Input3_PDFName + "' where PSItemGroupId = " + PSItemGroupId;
            objDataClass.ExecuteSQLStatement("ENRDBW", "PSItemsGrps", SqlCmd);
        }

        public void Delete_PSItemsGrps(string PSItemGroupId)
        {
            SqlCmd = "Delete tblStudentAdvisingSystem_PSItemsGrps_dtl where PSItemGroupId = " + PSItemGroupId;
            objDataClass.ExecuteSQLStatement("ENRDBW", "PSItemsGrps", SqlCmd);
        }

        public DataSet Get_PSItemsCrses(string PSItemGroupId)
        {
            SqlCmd = "SELECT DISTINCT tblStudentAdvisingSystem_PSItemsCrses_dtl.* FROM tblStudentAdvisingSystem_PSItemsCrses_dtl " +
                        "WHERE tblStudentAdvisingSystem_PSItemsCrses_dtl.PSItemGroupId = " + PSItemGroupId + " order by tblStudentAdvisingSystem_PSItemsCrses_dtl.Class_Subject, tblStudentAdvisingSystem_PSItemsCrses_dtl.Class_Number";
            return objDataClass.ExecuteSQLStatement("ENRDBR", "PSItemsCrses", SqlCmd);
        }

        public void Insert_PSItemsCrses(string PSItemGroupId, string Course_Id, string Class_Subject, string Class_Number)
        {
            SqlCmd = "Insert into tblStudentAdvisingSystem_PSItemsCrses_dtl(PSItemGroupId, Course_Id, Class_Subject, Class_Number) VALUES (" + PSItemGroupId + ",'" + Course_Id + "','" + Class_Subject + "','" + Class_Number + "')";
            objDataClass.ExecuteSQLStatement("ENRDBW", "PSItemsCrses", SqlCmd);
        }

        public void Delete_PSItemCrses(string PSItemGroupId)
        {
            SqlCmd = "Delete tblStudentAdvisingSystem_PSItemsCrses_dtl where PSItemGroupId = " + PSItemGroupId;
            objDataClass.ExecuteSQLStatement("ENRDBW", "PSItemsCrses", SqlCmd);
        }

        public DataSet Get_PSItems()
        {
            SqlCmd = "SELECT tblStudentAdvisingSystem_PSItems_tbl.* FROM tblStudentAdvisingSystem_PSItems_tbl order by tblStudentAdvisingSystem_PSItems_tbl.PSItem_Descr";
            DataSet ds = objDataClass.ExecuteSQLStatement("ENRDBR", "PSItems", SqlCmd);
            DataTable dt = ds.Tables[0];
            DataRow dr;
            dr = dt.NewRow();
            //dr[0] = "";
            dr[1] = "";
            dt.Rows.InsertAt(dr, 0);
            return ds;
        }

        public void Insert_PSItems(string PSItem_Descr)
        {
            SqlCmd = "Select a.* from tblStudentAdvisingSystem_PSItems_tbl a where a.PSItem_Descr = '" + PSItem_Descr + "'";
            DataSet ds = objDataClass.ExecuteSQLStatement("ENRDBR", "PSItems", SqlCmd);
            if (ds.Tables[0].Rows.Count == 0)
            {
                SqlCmd = "Insert into tblStudentAdvisingSystem_PSItems_tbl(PSItem_Descr) VALUES('" + PSItem_Descr + "')";
                objDataClass.ExecuteSQLStatement("ENRDBW", "PSGrps", SqlCmd);
            }
        }

        public DataSet Get_PSStudents(string StudentId, string Office_Cd)
        {
            string strOffice_Cd = "";
            if (Office_Cd.Substring(0, 3) == "AAP")
            {
                strOffice_Cd = " and a.Office_Cd like '" + Office_Cd.Substring(0, 3) + "%' ";
            }
            else
            {
                strOffice_Cd = " and a.Office_Cd = '" + Office_Cd + "' ";
            }
            SqlCmd = "Select distinct 'View' as action, a.PSStudentId as PkId, a.Office_Cd, b.PSName, cast(a.PSDate as varchar) as PSDate, dbo.tblStudentAdvisingSystem_Advisors_dtl.AdvisorName as AdvisorName, a.PSDate from tblStudentAdvisingSystem_PSStudents_dtl a INNER JOIN  tblStudentAdvisingSystem_PlanSheets_dtl b ON a.PSId = b.PSId INNER JOIN dbo.tblStudentAdvisingSystem_Students_dtl ON a.pkID = dbo.tblStudentAdvisingSystem_Students_dtl.pkID LEFT OUTER JOIN dbo.tblStudentAdvisingSystem_Advisors_dtl ON dbo.tblStudentAdvisingSystem_Students_dtl.Office_Cd = dbo.tblStudentAdvisingSystem_Advisors_dtl.Office_Cd AND dbo.tblStudentAdvisingSystem_Students_dtl.AdvisorEmplid = dbo.tblStudentAdvisingSystem_Advisors_dtl.AdvisorEmplid WHERE a.StudentId = '" + StudentId + "'" + strOffice_Cd + " order by a.PSDate desc";
            return objDataClass.ExecuteSQLStatement("ENRDBR", "PlanSheets", SqlCmd);
        }

        public DataSet Get_PSStudents(string PSStudentId)
        {
            SqlCmd = "Select distinct a.*, b.PSName, cast(a.PSDate as varchar) as PSDate from tblStudentAdvisingSystem_PSStudents_dtl a INNER JOIN tblStudentAdvisingSystem_PlanSheets_dtl b ON a.PSId = b.PSId WHERE a.PSStudentId = " + PSStudentId + " order by a.PSDate desc";
            return objDataClass.ExecuteSQLStatement("ENRDBR", "PlanSheets", SqlCmd);
        }

        public DataSet Get_PSStudents(string StudentId, string Office_Cd, string pkID)
        {
            SqlCmd = "Select distinct a.* from tblStudentAdvisingSystem_PSStudents_dtl a WHERE a.StudentId = '" + StudentId + "' and a.Office_Cd = '" + Office_Cd + "' and a.pkID = " + pkID + " order by a.PSDate desc";
            return objDataClass.ExecuteSQLStatement("ENRDBR", "PlanSheets", SqlCmd);
        }

        public string Insert_PSStudents(string StudentId, string Office_Cd, string PSId, string pkID)
        {
            SqlCmd = "Insert into tblStudentAdvisingSystem_PSStudents_dtl(pkID, StudentId, Office_Cd, PSId, PSDate) Select '" + pkID + "', '" + StudentId + "', '" + Office_Cd + "', " + PSId + ", GetDate()";
            objDataClass.ExecuteSQLStatement("ENRDBW", "PSStudent", SqlCmd);
            SqlCmd = "Select a.PSStudentId from tblStudentAdvisingSystem_PSStudents_dtl a where a.pkID = '" + pkID + "' and a.StudentId = '" + StudentId + "' and a.Office_Cd = '" + Office_Cd + "' and a.PSId = " + PSId + " and a.PSDate = (select max(a1.PSDate) from tblStudentAdvisingSystem_PSStudents_dtl a1 where a1.StudentId = a.StudentId and a1.Office_Cd = a.Office_Cd and a1.PSId = a.PSId)";
            DataSet ds = objDataClass.ExecuteSQLStatement("ENRDBR", "PSStudent", SqlCmd);
            if (ds.Tables[0].Rows.Count == 1)
            {
                return ds.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }

        public void Update_PSStudents(string PSStudentId)
        {
            SqlCmd = "Update tblStudentAdvisingSystem_PSStudents_dtl set PSDate = GetDate() where PSStudentId = " + PSStudentId;
            objDataClass.ExecuteSQLStatement("ENRDBW", "PSStudent", SqlCmd);
        }
        
        public void Insert_PSStdntsItems(string PSStudentId, string PSItemGroupId, string Notes, string Input1, string Input2, string Input3)
        {
            SqlCmd = "Insert into tblStudentAdvisingSystem_PSStdntsItems_dtl(PSStudentId, PSItemGroupId, Notes, Input1, Input2, Input3) " +
                        "Values(" + PSStudentId + ", " + PSItemGroupId + ", '" + Notes + "', '" + Input1 + "', '" + Input2 + "','" + Input3 + "')";
            objDataClass.ExecuteSQLStatement("ENRDBW", "PSStdntsItems", SqlCmd);
        }

        public void Update_PSStdntsItems(string PSStudentId, string PSItemGroupId, string Notes, string Input1, string Input2, string Input3)
        {
            SqlCmd = "Update tblStudentAdvisingSystem_PSStdntsItems_dtl set Notes = '" + Notes + "', Input1 = '" + Input1 + "', Input2 = '" + Input2 + "', Input3 = '" + Input3 + "' where PSStudentId = " + PSStudentId + " and PSItemGroupId = " + PSItemGroupId;
            objDataClass.ExecuteSQLStatement("ENRDBW", "PSStdntsItems", SqlCmd);
        }

        public DataSet Get_PSStdntsItems(string PSStudentId, string PSId)
        {
            SqlCmd = "SELECT tblStudentAdvisingSystem_PSStdntsItems_dtl.*, tblStudentAdvisingSystem_PSItemsGrps_dtl.PSId AS PSId, tblStudentAdvisingSystem_PSItemsGrps_dtl.PSGroupId AS PSGroupId, tblStudentAdvisingSystem_PSItemsGrps_dtl.PSItemId AS PSItemId, tblStudentAdvisingSystem_PSItemsGrps_dtl.PSItem_SeqNum AS PSItem_SeqNum " +
                        "FROM tblStudentAdvisingSystem_PSStdntsItems_dtl INNER JOIN tblStudentAdvisingSystem_PSItemsGrps_dtl ON tblStudentAdvisingSystem_PSStdntsItems_dtl.PSItemGroupId = tblStudentAdvisingSystem_PSItemsGrps_dtl.PSItemGroupId " +
                        "WHERE tblStudentAdvisingSystem_PSStdntsItems_dtl.PSStudentId = " + PSStudentId + " and tblStudentAdvisingSystem_PSItemsGrps_dtl.PSId = " + PSId;
            return objDataClass.ExecuteSQLStatement("ENRDBR", "PSStdntsItems", SqlCmd);
        }

        public DataSet Get_PSStdntsItems(string PSStudentId)
        { 
            SqlCmd = "SELECT tblStudentAdvisingSystem_PSStdntsItems_dtl.PSStudentId, tblStudentAdvisingSystem_PSStdntsItems_dtl.PSItemGroupId, " +
                      "tblStudentAdvisingSystem_PSStdntsItems_dtl.Notes, tblStudentAdvisingSystem_PSItemsGrps_dtl.Notes_PDFName, " +
                      "tblStudentAdvisingSystem_PSStdntsItems_dtl.Input1, tblStudentAdvisingSystem_PSItemsGrps_dtl.Input1_PDFName, " + 
                      "tblStudentAdvisingSystem_PSStdntsItems_dtl.Input2, tblStudentAdvisingSystem_PSItemsGrps_dtl.Input2_PDFName, " + 
                      "tblStudentAdvisingSystem_PSStdntsItems_dtl.Input3, tblStudentAdvisingSystem_PSItemsGrps_dtl.Input3_PDFName " +
                     "FROM tblStudentAdvisingSystem_PSStdntsItems_dtl INNER JOIN " +
                      "tblStudentAdvisingSystem_PSItemsGrps_dtl ON " +
                      "tblStudentAdvisingSystem_PSStdntsItems_dtl.PSItemGroupId = tblStudentAdvisingSystem_PSItemsGrps_dtl.PSItemGroupId " +
                     "WHERE tblStudentAdvisingSystem_PSStdntsItems_dtl.PSStudentId = " + PSStudentId;
            return objDataClass.ExecuteSQLStatement("ENRDBR", "PSStdntsItems", SqlCmd);
        }

        public DataSet Get_StudentEnrolled(string emplid, string crse_id)
        {
            SqlCmd = "select distinct c.crse_id, c.strm, case when e.units_attempted = 'Y' then to_char(e.unt_earned) else '' end as unt_earned, e.crse_grade_off, c.subject, trim(c.catalog_nbr) as class_number, e.grading_basis_enrl as grading_basis, e.grade_category " +
                        "from rdsstg_hrsa.ps_stdnt_enrl e, rdsstg_hrsa.ps_class_tbl c " +
                        "where c.class_nbr = e.class_nbr and c.strm = e.strm and e.stdnt_enrl_status = 'E' " + // and e.earn_credit = 'Y' " +
                            "and not (e.grading_basis_enrl = 'NON' and c.ssr_component in ('LAB','ACT')) and not (e.crse_grade_off = 'W') " +
                            "and e.emplid = '" + emplid + "' and c.crse_id in ('" + crse_id + "') "; // order by c.strm";
            /*DataSet StudentEnrolled_ds = objDataClass.ExecuteSQLStatement("RDSDBR", "StudentEnrolled", SqlCmd);*/
            SqlCmd = SqlCmd + "union all (" +
                        "select distinct t.crse_id, substr(t.term_year, 1, 2) || substr(t.term_year, 4, 1) || case when t.ext_term = 'FALL' then '8' else case when t.ext_term = 'SPR' then '2' else case when t.ext_term = 'SUMR' then '6' end end end as strm, case when t.units_attempted = 'Y' then to_char(t.unt_trnsfr) else '' end as unt_earned, t.crse_grade_off, trim(t.school_subject) as subject, trim(t.school_crse_nbr) || ' @ ' || e.descrshort  as class_number, t.grading_basis, t.grade_category " +
                        "from rdsstg_hrsa.ps_trns_crse_dtl t, rdsstg_hrsa.ps_ext_org_tbl e " +
                        "where t.trnsfr_src_id = e.ext_org_id and e.effdt = (select max(e1.effdt) from rdsstg_hrsa.ps_ext_org_tbl e1 where e1.ext_org_id = e.ext_org_id) " +
                            "and length(t.term_year) = 4 and (not nvl(t.school_subject, ' ') = ' ') and t.emplid = '" + emplid + "' and t.crse_id in ('" + crse_id + "')) ";
            SqlCmd = SqlCmd + "union all (" +
                        "select distinct t.crse_id, substr(x.term_year, 1, 2) || substr(x.term_year, 4, 1) || case when x.ext_term = 'FALL' then '8' else case when x.ext_term = 'SPR' then '2' else case when x.ext_term = 'SUMR' then '6' end end end as strm, case when t.units_attempted = 'Y' then to_char(t.unt_trnsfr) else '' end as unt_earned, x.crse_grade_off, trim(x.school_subject) as subject, trim(x.school_crse_nbr) || ' @ ' || e.descrshort  as class_number, t.grading_basis, t.grade_category " +
                        "from rdsstg_hrsa.ps_trns_crse_dtl t, rdsstg_hrsa.ps_ext_org_tbl e, rdsstg_hrsa.ps_ext_course x " +
                        "where t.trnsfr_src_id = e.ext_org_id and e.effdt = (select max(e1.effdt) from rdsstg_hrsa.ps_ext_org_tbl e1 where e1.ext_org_id = e.ext_org_id) " +
                            "and t.emplid = x.emplid and t.trnsfr_src_id = x.ext_org_id and t.ext_course_nbr = x.ext_course_nbr and nvl(t.school_subject, ' ') = ' ' " +
                            "and t.emplid = '" + emplid + "' and t.crse_id in ('" + crse_id + "')) ";
            SqlCmd = SqlCmd + "union all (" +
                        "select distinct t.crse_id, substr(x.term_year, 1, 2) || substr(x.term_year, 4, 1) || case when x.ext_term = 'FALL' then '8' else case when x.ext_term = 'SPR' then '2' else case when x.ext_term = 'SUMR' then '6' end end end as strm, case when t.units_attempted = 'Y' then to_char(t.unt_trnsfr) else '' end as unt_earned, x.crse_grade_off, trim(f.school_subject) as subject, trim(f.school_crse_nbr) || ' @ ' || e.descrshort  as class_number, t.grading_basis, t.grade_category " +
                        "from rdsstg_hrsa.ps_trns_crse_dtl t, rdsstg_hrsa.ps_ext_org_tbl e, rdsstg_hrsa.ps_trnsfr_from f, rdsstg_hrsa.ps_ext_course x " +
                        "where t.trnsfr_src_id = e.ext_org_id and e.effdt = (select max(e1.effdt) from rdsstg_hrsa.ps_ext_org_tbl e1 where e1.ext_org_id = e.ext_org_id) " +
                            "and t.trnsfr_src_id = f.trnsfr_src_id and t.comp_subject_area = f.comp_subject_area and t.trnsfr_eqvlncy_cmp = f.trnsfr_eqvlncy_cmp " + // and t.trnsfr_eqvlncy_seq = f.trnsfr_cmp_seq " +
                            "and f.effdt = (select max(f1.effdt) from rdsstg_hrsa.ps_trnsfr_from f1 where f1.institution = f.institution and f1.trnsfr_src_id = f.trnsfr_src_id and f1.comp_subject_area = f.comp_subject_area and f1.trnsfr_eqvlncy_cmp = f.trnsfr_eqvlncy_cmp) " +
                            "and t.emplid = x.emplid and t.trnsfr_src_id = x.ext_org_id and trim(f.school_subject) = trim(x.school_subject) and trim(f.school_crse_nbr) = trim(x.school_crse_nbr) " +
                            "and nvl(t.school_subject, ' ') = ' ' and t.emplid = '" + emplid + "' and t.crse_id in ('" + crse_id + "') " +
                            "and (t.crse_id, substr(x.term_year, 1, 2) || substr(x.term_year, 4, 1) || case when x.ext_term = 'FALL' then '8' else case when x.ext_term = 'SPR' then '2' else case when x.ext_term = 'SUMR' then '6' end end end, case when t.units_attempted = 'Y' then to_char(t.unt_trnsfr) else '' end, x.crse_grade_off, trim(f.school_subject), trim(f.school_crse_nbr) || ' @ ' || e.descrshort, t.grading_basis, t.grade_category) NOT IN " +
                             "(select distinct t1.crse_id, substr(x1.term_year, 1, 2) || substr(x1.term_year, 4, 1) || case when x1.ext_term = 'FALL' then '8' else case when x1.ext_term = 'SPR' then '2' else case when x1.ext_term = 'SUMR' then '6' end end end as strm, case when t1.units_attempted = 'Y' then to_char(t1.unt_trnsfr) else '' end as unt_earned, x1.crse_grade_off, trim(x1.school_subject) as subject, trim(x1.school_crse_nbr) || ' @ ' || e1.descrshort  as class_number, t1.grading_basis, t1.grade_category " +
                                "from rdsstg_hrsa.ps_trns_crse_dtl t1, rdsstg_hrsa.ps_ext_org_tbl e1, rdsstg_hrsa.ps_ext_course x1 " +
                                "where t1.trnsfr_src_id = e1.ext_org_id and e1.effdt = (select max(e1a.effdt) from rdsstg_hrsa.ps_ext_org_tbl e1a where e1a.ext_org_id = e1.ext_org_id) " +
                                "and t1.emplid = x1.emplid and t1.trnsfr_src_id = x1.ext_org_id and t1.ext_course_nbr = x1.ext_course_nbr " +
                                "and nvl(t1.school_subject, ' ') = ' ' and t1.emplid = '" + emplid + "' and t1.crse_id in ('" + crse_id + "')) " +
                            ") order by strm";
            /*StudentEnrolled_ds.Merge(objDataClass.ExecuteSQLStatement("RDSDBR", "StudentEnrolled", SqlCmd));*/
            return objDataClass.ExecuteSQLStatement("RDSDBR", "StudentEnrolled", SqlCmd);
        }

        public DataSet Get_Course_Id(string Course_Id, string Class_Subject, string Class_Number)
        {
            if (Course_Id != "")
            {
                SqlCmd = "select distinct t.course_Id, t.class_subject, trim(t.class_number) as class_number, t.class_title, t.units_course_maximum  from rdsprd.rec_class_dtl t where t.term = (select max(t1.term) from rec_class_dtl t1 where t1.course_id = t.course_id and t1.academic_year_ps <= extract(year from sysdate)) and t.course_id like '%" + Course_Id + "%' order by t.class_subject, trim(t.class_number)";
            }
            else
            {
                SqlCmd = "select distinct t.course_Id, t.class_subject, trim(t.class_number) as class_number, t.class_title, t.units_course_maximum  from rdsprd.rec_class_dtl t where t.term = (select max(t1.term) from rec_class_dtl t1 where t1.course_id = t.course_id and t1.academic_year_ps <= extract(year from sysdate)) and t.class_subject like '%" + Class_Subject.ToUpper() + "%' and trim(t.class_number) like '%" + Class_Number + "%' order by t.class_subject, trim(t.class_number)";
            }
            return objDataClass.ExecuteSQLStatement("RDSDBR", "Course_Id", SqlCmd);
        }

        public DataSet Get_StdntNotes(string pkID)
        {
            SqlCmd = "Select distinct t.* from tblStudentAdvisingSystem_StdntNotes_dtl t where t.pkID = " + pkID;
            return objDataClass.ExecuteSQLStatement("ENRDBR", "StdntNotes", SqlCmd);
        }

        public DataSet Get_StdntNotes(string pkid, string Office_Cd)
        {
            string strOffice_Cd = "";
            if (Office_Cd.Substring(0, 3) == "AAP")
            {
                strOffice_Cd = " and a.Office_Cd like '" + Office_Cd.Substring(0, 3) + "%' ";
            }
            else
            {
                strOffice_Cd = " and a.Office_Cd = '" + Office_Cd + "' ";
            }
            SqlCmd = "Select distinct a.PkID, a.Office_Cd, cast(a.Visit_DateTime as varchar) as [Date], b.AdvisorName as Advised, a.Visit_DateTime, isnull(c.Notes, 'Notes to Student Not Available') as Notes from tblStudentAdvisingSystem_Students_dtl a inner join tblStudentAdvisingSystem_Advisors_dtl b on a.AdvisorEmplid = b.AdvisorEmplid and a.Office_Cd = b.Office_Cd left outer join tblStudentAdvisingSystem_StdntNotes_dtl c on a.pkID = c.pkID where a.StudentId = (select a1.StudentId from tblStudentAdvisingSystem_Students_dtl a1 where a1.pkid = " + pkid + ") " + strOffice_Cd + " and (not a.pkid = " + pkid + ") order by a.Visit_DateTime desc";
            return objDataClass.ExecuteSQLStatement("ENRDBR", "PrevStudents", SqlCmd);
        }

        public void Delete_StdntNotes(string pkID)
        {
            SqlCmd = "Delete tblStudentAdvisingSystem_StdntNotes_dtl where pkID = " + pkID;
            objDataClass.ExecuteSQLStatement("ENRDBW", "StdntNotes", SqlCmd);
        }

        public void Insert_StdntNotes(string pkID, string Notes)
        { 
            SqlCmd = "Insert into tblStudentAdvisingSystem_StdntNotes_dtl(pkID, Notes) VALUES(" + pkID + ",'" + Notes + "')";
            objDataClass.ExecuteSQLStatement("ENRDBW", "StdntNotes", SqlCmd);
        }

        public void Update_StdntNotes(string pkID, string Notes)
        { 
            SqlCmd = "Update tblStudentAdvisingSystem_StdntNotes_dtl set Notes = '" + Notes + "' where pkID = " + pkID;
            objDataClass.ExecuteSQLStatement("ENRDBW", "StdntNotes", SqlCmd);
        }

        public DataSet Get_StudentSearch(string Office_Cd, string strSearch)
        {
            string strOffice_Cd = "";
            if (Office_Cd.Substring(0, 3) == "AAP")
            {
                strOffice_Cd = " t.Office_Cd like '" + Office_Cd.Substring(0, 3) + "%' ";
            }
            else
            {
                strOffice_Cd = " t.Office_Cd = '" + Office_Cd + "' ";
            }
            SqlCmd = "Select distinct t.*, '?dept=' + t.Office_Cd + '&pkid=' + cast(t.pkID as varchar) as ViewKey from tblStudentAdvisingSystem_Students_dtl t where " + strOffice_Cd + " and (t.StudentId like '%" + strSearch + "%' or t.FirstName like '%" + strSearch + "%' or t.LastName like '%" + strSearch + "%' or t.FirstName + ' ' + t.LastName like '%" + strSearch + "%' or t.Phone like '%" + strSearch + "%' or t.Email_Address like '%" + strSearch + "%' or t.Major like '%" + strSearch + "%' or t.Minor like '%" + strSearch + "%' or t.Ques like '%" + strSearch + "%' or convert(varchar(12), t.Visit_DateTime, 101) like '%" + strSearch + "%' or convert(varchar(12), t.Visit_DateTime, 1) like '%" + strSearch + "%') order by t.Visit_DateTime desc, t.LastName, t.FirstName";
            return objDataClass.ExecuteSQLStatement("ENRDBR", "StudentSearch", SqlCmd);
        }

        public void Insert_BulkEmail(string Email_Address, string Office_EmailAddress, string strCc, string strSubject, string strBody, string strAttach)
        {
            SqlCmd = "Insert into ENR_PublicODS.dbo.tblBulkEmail_Outbox ([TO], [FROM], CC, Subject, BodyText, Attachment, [TimeStamp], Requestor, RecordCount, BodyFormat) VALUES ('" + Email_Address + "','" + Office_EmailAddress + "','" + strCc + "','" + strSubject + "','" + strBody + "','" + strAttach + "','" + DateTime.Now.ToString() + "','" + Office_EmailAddress.Substring(0, Office_EmailAddress.IndexOf("@")) + "', 1,'Html')";
            objDataClass.ExecuteSQLStatement("ENRDBW", "BulkEmail", SqlCmd);
        }

        public DataSet Get_PreText(string Office_Cd)
        {
            SqlCmd = "Select distinct t.* from tblStudentAdvisingSystem_CommText_dtl t where t.Office_Cd = '" + Office_Cd + "' order by t.Text_Cd";
            return objDataClass.ExecuteSQLStatement("ENRDBR", "CommonText", SqlCmd);
        }

        public void Delete_PreText(string Office_Cd, string Text_Cd)
        {
            SqlCmd = "Delete tblStudentAdvisingSystem_CommText_dtl where Office_Cd = '" + Office_Cd + "' and Text_Cd = '" + Text_Cd + "'";
            objDataClass.ExecuteSQLStatement("ENRDBW", "CommonText", SqlCmd);
        }

        public void Insert_PreText(string Office_Cd, string Text_Cd, string Text_Descr)
        {
            SqlCmd = "Insert into tblStudentAdvisingSystem_CommText_dtl(Text_Cd, Office_Cd, Text_Descr) VALUES('" + Text_Cd + "','" + Office_Cd + "','" + Text_Descr + "')";
            objDataClass.ExecuteSQLStatement("ENRDBW", "CommonText", SqlCmd);
        }

        public DataSet Get_BackOnTrack_Wkshop(string StudentId)
        { 
            SqlCmd = "select 'Attended on ' || to_char(b.begin_date, 'mm/dd/yyyy') || ' ' || to_char(b.session_time) as Session_dt from enrmgmt.adv_event_student a, enrmgmt.adv_event_sessiondates b, enrmgmt.adv_event_info c where a.session_dates_pkid = b.session_dates_pkid and a.event_type = b.event_type and a.event_type = c.event_type and a.event_type = 'dq' and nvl(a.noshow_total, 0) = 0 and a.attended = 'Y' and a.student_id = '" + StudentId + "'";
            return objDataClass.ExecuteSQLStatement("RDSDBR", "BackOnTrack_Wkshop", SqlCmd);
        }

        public DataSet Get_WhatsNext_Wkshop(string StudentId)
        {
            SqlCmd = "select 'Attended on ' || to_char(b.begin_date, 'mm/dd/yyyy') || ' ' || to_char(b.session_time) as Session_dt from enrmgmt.adv_event_student a, enrmgmt.adv_event_sessiondates b, enrmgmt.adv_event_info c where a.session_dates_pkid = b.session_dates_pkid and a.event_type = b.event_type and a.event_type = c.event_type and a.event_type = 'whatnext' and nvl(a.noshow_total, 0) = 0 and a.attended = 'Y' and a.student_id = '" + StudentId + "'";
            return objDataClass.ExecuteSQLStatement("RDSDBR", "WhatsNext_Wkshop", SqlCmd);
        }
    }
}
