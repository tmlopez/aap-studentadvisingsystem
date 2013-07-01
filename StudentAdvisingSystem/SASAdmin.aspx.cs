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
    public partial class SASAdmin : System.Web.UI.Page
    {
        private SqlCollection objSqlCollection = new SqlCollection();

        protected void Page_Load(object sender, EventArgs e)
        {
            string SASAdmin_Allow_lst = ConfigurationManager.AppSettings.Get("SASAdmin_Allow").ToString().ToUpper();
            if (SASAdmin_Allow_lst.Contains(User.Identity.Name.Substring(7).ToUpper()))
            {
                Office_Descr_txt.Enabled = true;
                Advisor_Type_txt.Enabled = true;
                Office_Location_txt.Enabled = true;
                Office_PhoneExt_txt.Enabled = true;
                Office_Email_Address_txt.Enabled = true;
                QuesField_lbtn.Enabled = true;
                Questions_chkl.Enabled = true;
                CellPhoneAllow_chkl.Enabled = true;
                CellPhone_WaitNum_txt.Enabled = true;
                CellPhone_Msg_txt.Enabled = true;
                ThankYou_Msg_txt.Enabled = true;
                Advisor_lbtn.Enabled = true;
                Advisors_DL.Enabled = true;
                PlanSheets_Add_lnk.Enabled = true;
                PlanSheets_DG.Enabled = true;
                Email_Intro_txt.Enabled = true;
                PreText_lbtn.Enabled = true;
                PreText_DG.Enabled = true;
                Save_btn.Enabled = true;
            }
            else
            {
                Office_Descr_txt.Enabled = false;
                Advisor_Type_txt.Enabled = false;
                Office_Location_txt.Enabled = false;
                Office_PhoneExt_txt.Enabled = false;
                Office_Email_Address_txt.Enabled = false;
                QuesField_lbtn.Enabled = false;
                Questions_chkl.Enabled = false;
                CellPhoneAllow_chkl.Enabled = false;
                CellPhone_WaitNum_txt.Enabled = false;
                CellPhone_Msg_txt.Enabled = false;
                ThankYou_Msg_txt.Enabled = false;
                Advisor_lbtn.Enabled = false;
                Advisors_DL.Enabled = false;
                PlanSheets_Add_lnk.Enabled = false;
                PlanSheets_DG.Enabled = false;
                Email_Intro_txt.Enabled = false;
                PreText_lbtn.Enabled = false;
                PreText_DG.Enabled = false;
                Save_btn.Enabled = false;
            }
            
            if (!IsPostBack)
            {
                Office_Cd.Text = (string)Request.Params["dept"];

                if (Office_Cd.Text.ToString() == "")
                {
                    Office_Cd.Text = "AAP";
                }
                BindOfficeInfo();
                BindQuesFields();
                BindAdvisorList();
                BindPreText();

                PlanSheets_DG.DataSource = objSqlCollection.Get_PlanSheets_All(Office_Cd.Text.ToString());
                PlanSheets_DG.DataBind();                                
            }
        }

        private void BindOfficeInfo()
        {
            //SqlCollection objSqlCollection = new SqlCollection();
            DataSet OfficeInfo_DS = objSqlCollection.Get_OfficeInfo(Office_Cd.Text.ToString());
            Office_Descr_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_Descr"].ToString();
            Office_Descr1_lbl.Text = Office_Descr_lbl.Text.ToString();
            Office_Location_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_Location"].ToString();
            Office_PhoneExt_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_PhoneExt"].ToString();
            CurrentDate_lbl.Text = DateTime.Now.ToLongDateString();

            OfficeInfo_tbl.Visible = true;
            Office_Descr_txt.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_Descr"].ToString();
            Advisor_Type_txt.Text = OfficeInfo_DS.Tables[0].Rows[0]["Advisor_Type"].ToString().Replace("an ", "").Replace("a ", "");
            Office_Location_txt.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_Location"].ToString();
            Office_PhoneExt_txt.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_PhoneExt"].ToString();
            Office_Email_Address_txt.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_Email_Address"].ToString();
            CellPhone_Msg_txt.Text = OfficeInfo_DS.Tables[0].Rows[0]["CellPhone_Msg"].ToString();
            CellPhone_WaitNum_txt.Text = OfficeInfo_DS.Tables[0].Rows[0]["CellPhone_WaitNum"].ToString();
            ThankYou_Msg_txt.Text = OfficeInfo_DS.Tables[0].Rows[0]["ThankYou_Msg"].ToString();
            for (int i = 0; i <= CellPhoneAllow_chkl.Items.Count - 1; i++)
            {
                if (CellPhoneAllow_chkl.Items[i].Value.ToString() == OfficeInfo_DS.Tables[0].Rows[0]["CellPhoneAllow"].ToString())
                {
                    CellPhoneAllow_chkl.Items[i].Selected = true;
                }
            }
            Email_Intro_txt.Text = OfficeInfo_DS.Tables[0].Rows[0]["Email_Intro"].ToString();
        }

        private void BindQuesFields()
        {
            //SqlCollection objSqlCollection = new SqlCollection();
            DataSet QuesFields_Office_DS = objSqlCollection.Get_QuesFields_dtl(Office_Cd.Text.ToString());
            Questions_chkl.DataSource = objSqlCollection.Get_QuesFields_tbl();
            Questions_chkl.DataBind();
            if (QuesFields_Office_DS.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= Questions_chkl.Items.Count - 1; i++)
                {
                    for (int n = 0; n <= QuesFields_Office_DS.Tables[0].Rows.Count - 1; n++)
                    {
                        if (Questions_chkl.Items[i].Value.ToString() == QuesFields_Office_DS.Tables[0].Rows[n]["QuesField_Cd"].ToString())
                        {
                            Questions_chkl.Items[i].Selected = true;
                        }
                    }
                }
            }
        }

        protected void QuesField_clicked(object sender, EventArgs e)
        {
            Questions_Add_err_lbl.Visible = false;
            if (QuesField_lbtn.Text == "Add")
            {
                QuesField_Cd_txt.Text = "";
                QuesField_Descr_txt.Text = "";
                Questions_Add_TC.Visible = true;
                Questions_List_TC.Visible = false;
                QuesField_lbtn.Text = "Select";
                QuesField_Cd_txt.Focus();
            }
            else if (QuesField_lbtn.Text == "Select")
            {
                Questions_Add_TC.Visible = false;
                Questions_List_TC.Visible = true;
                QuesField_lbtn.Text = "Add";
                BindQuesFields();
                Questions_chkl.Focus();
            }
        }

        protected void QuesFields_Save_clicked(object sender, EventArgs e)
        {
            //SqlCollection objSqlCollection = new SqlCollection();            
            for (int i = 0; i <= Questions_chkl.Items.Count - 1; i++)
            {
                if (Questions_chkl.Items[i].Value.ToString() == QuesField_Cd_txt.Text.ToString().ToUpper())
                {
                    Questions_Add_err_lbl.Visible = true;
                    QuesField_Cd_txt.Focus();
                    break;
                }
            }
            if (!Questions_Add_err_lbl.Visible)
            {
                objSqlCollection.Insert_QuesFields_tbl(QuesField_Cd_txt.Text.ToString(), QuesField_Descr_txt.Text.ToString().Replace("'", "''"));
                QuesField_Cd_txt.Text = "";
                QuesField_Descr_txt.Text = "";
                Questions_Add_TC.Visible = false;
                Questions_List_TC.Visible = true;
                QuesField_lbtn.Text = "Add";
                Questions_Add_err_lbl.Visible = false;
                BindQuesFields();
                Questions_chkl.Focus();               
            }
        }

        private void BindAdvisorList()
        {
            DataSet Advisors_DS = objSqlCollection.Get_Advisors(Office_Cd.Text.ToString());            
            Advisors_DS.Tables[0].Rows[Advisors_DS.Tables[0].Rows.Count - 1].Delete();
            Advisors_DS.Tables[0].Rows[0].Delete();
            //Advisors_DS.Merge(objSqlCollection.Get_Advisors_NotActive(Office_Cd.Text.ToString()));
            Advisors_DL.DataSource = Advisors_DS;
            Advisors_DL.DataBind();
            //for (int i = 0; i <= Advisors_DL.Items.Count - 1; i++)
            //{
            //    CheckBox objIsActive_ckb = (CheckBox)Advisors_DL.Items[i].FindControl("IsActive_ckb");
            //    if (Advisors_DS.Tables[0].Rows[i]["IsActive"].ToString() == "1")
            //    {
            //        objIsActive_ckb.Checked = true;
            //    }
            //    else
            //    {
            //        objIsActive_ckb.Checked = false;
            //    }
            //}
        }

        protected void Advisor_clicked(object sender, EventArgs e)
        {
            Advisor_add_err_lbl.Visible = false;
            if (Advisor_lbtn.Text == "Add")
            {
                AdvisorEmplid_Add_txt.Text = "";
                AdvisorName_Add_txt.Text = "";
                Advisors_Add_TC.Visible = true;
                Advisors_List_TC.Visible = false;
                Advisor_lbtn.Text = "Select";
                AdvisorEmplid_Add_txt.Focus();
            }
            else if (Advisor_lbtn.Text == "Select")
            {
                Advisors_Add_TC.Visible = false;
                Advisors_List_TC.Visible = true;
                Advisor_lbtn.Text = "Add";
                BindAdvisorList();
                Advisors_DL.Focus();
            }
        }

        protected void Advisor_Save_clicked(object sender, EventArgs e)
        {
            for (int i = 0; i <= Advisors_DL.Items.Count - 1; i++)
            {
                Label objAdvisorEmplid_txt = (Label)Advisors_DL.Items[i].FindControl("AdvisorEmplid_txt");
                if (objAdvisorEmplid_txt != null)
                {
                    if (objAdvisorEmplid_txt.Text.ToString() == AdvisorEmplid_Add_txt.Text.ToString())
                    {
                        Advisor_add_err_lbl.Visible = true;
                        AdvisorEmplid_Add_txt.Focus();
                        break;
                    }
                }
            }
            if (!Advisor_add_err_lbl.Visible)
            {
                objSqlCollection.Insert_Advisors(Office_Cd.Text.ToString(), AdvisorEmplid_Add_txt.Text.ToString(), AdvisorName_Add_txt.Text.ToString());
                AdvisorEmplid_Add_txt.Text = "";
                AdvisorName_Add_txt.Text = "";
                Advisors_Add_TC.Visible = false;
                Advisors_List_TC.Visible = true;
                Advisor_lbtn.Text = "Add";
                Advisor_add_err_lbl.Visible = false;
                BindAdvisorList();
                Advisors_DL.Focus();
            }
        }

        protected void Save_clicked(object sender, EventArgs e)
        {
            objSqlCollection.Update_OfficeInfo(Office_Cd.Text.ToString(), Office_Descr_txt.Text.ToString().Replace("'", "''"), Advisor_Type_txt.Text.ToString().Replace("'", "''"), Office_Location_txt.Text.ToString().Replace("'", "''"), CellPhoneAllow_chkl.SelectedItem.Value.ToString(), Office_PhoneExt_txt.Text.ToString(), ThankYou_Msg_txt.Text.ToString().Replace("'", "''"), Office_Email_Address_txt.Text.ToString(), CellPhone_Msg_txt.Text.ToString().Replace("'", "''"), CellPhone_WaitNum_txt.Text.ToString(), Email_Intro_txt.Text.ToString().Replace("'", "''"));
            objSqlCollection.Delete_QuesFields_dtl(Office_Cd.Text.ToString());
            for (int i = 0; i <= Questions_chkl.Items.Count - 1; i++)
            {
                if (Questions_chkl.Items[i].Selected)
                {
                    objSqlCollection.Insert_QuesFields_dtl(Office_Cd.Text.ToString(), Questions_chkl.Items[i].Value.ToString());
                }
            }
            for (int i = 0; i <= Advisors_DL.Items.Count - 1; i++)
            {
                CheckBox objIsActive_ckb = (CheckBox)Advisors_DL.Items[i].FindControl("IsActive_ckb");
                Label objAdvisorEmplid_txt = (Label)Advisors_DL.Items[i].FindControl("AdvisorEmplid_txt");
                TextBox objAdvisorName_txt = (TextBox)Advisors_DL.Items[i].FindControl("AdvisorName_txt");
                string strIsActive = "1";
                if (!objIsActive_ckb.Checked)
                {
                    strIsActive = "0";
                }
                objSqlCollection.Update_Advisors(Office_Cd.Text.ToString(), objAdvisorEmplid_txt.Text.ToString(), objAdvisorName_txt.Text.ToString(), strIsActive);
            }
            Response.Redirect("SASAdmin.aspx?dept=" + Office_Cd.Text.ToString());
        }

        protected void PlanSheets_Add_clicked(object sender, EventArgs e)
        {
            Response.Redirect("PlanSheetsAdmin.aspx?dept=" + Office_Cd.Text.ToString());
        }

        protected void PlanSheets_Click(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Edit")
            {
                Response.Redirect("PlanSheetsAdmin.aspx?dept=" + Office_Cd.Text.ToString() + "&psid=" + PlanSheets_DG.DataKeys[e.Item.ItemIndex].ToString());
            }
            else if (e.CommandName.ToString() == "Copy")
            {
                DataSet PS_ds = objSqlCollection.Get_PlanSheets(PlanSheets_DG.DataKeys[e.Item.ItemIndex].ToString(), Office_Cd.Text.ToString());
                if (PS_ds.Tables[0].Rows.Count == 1)
                { 
                    DataRow PS_dr = PS_ds.Tables[0].Rows[0];
                    string PSId = objSqlCollection.Insert_PlanSheets(PS_dr["PSName"].ToString(), PS_dr["PSOffice_Cd"].ToString(), PS_dr["PSCatalog_yr"].ToString(), PS_dr["IsValid"].ToString(), PS_dr["PDF_FilePath"].ToString());
                    DataSet PSGrps_ds = objSqlCollection.Get_PSGrps(PlanSheets_DG.DataKeys[e.Item.ItemIndex].ToString());
                    for (int i = 0; i<= PSGrps_ds.Tables[0].Rows.Count - 1; i++)
                    {
                        DataRow PSGrps_dr = PSGrps_ds.Tables[0].Rows[i];
                        objSqlCollection.Insert_PSGrps(PSId, PSGrps_dr["PSGroupId"].ToString(), PSGrps_dr["PSGroup_SubDescr"].ToString(), PSGrps_dr["PSGroup_SeqNum"].ToString(), PSGrps_dr["PSInput1_descr"].ToString(), PSGrps_dr["PSInput2_descr"].ToString(), PSGrps_dr["PSInput3_descr"].ToString());
                    }
                    DataSet PSItemsGrps_ds = objSqlCollection.Get_PSItemsGrps_All(PlanSheets_DG.DataKeys[e.Item.ItemIndex].ToString());
                    for (int i = 0; i<= PSItemsGrps_ds.Tables[0].Rows.Count - 1; i++)
                    {
                        DataRow PSItemsGrps_dr = PSItemsGrps_ds.Tables[0].Rows[i];
                        string PSItemGroupId = objSqlCollection.Insert_PSItemsGrps(PSId, PSItemsGrps_dr["PSGroupId"].ToString(), PSItemsGrps_dr["PSItemId"].ToString(), PSItemsGrps_dr["PSItem_SeqNum"].ToString(), PSItemsGrps_dr["GetPSInfo"].ToString(), PSItemsGrps_dr["Notes_PDFName"].ToString(), PSItemsGrps_dr["Input1_PDFName"].ToString(), PSItemsGrps_dr["Input2_PDFName"].ToString(), PSItemsGrps_dr["Input3_PDFName"].ToString());
                        DataSet PSItemsCrses_ds = objSqlCollection.Get_PSItemsCrses(PSItemsGrps_dr["PSItemGroupId"].ToString());
                        for (int n = 0; n<= PSItemsCrses_ds.Tables[0].Rows.Count - 1; n++)
                        { 
                            DataRow PSItemCrses_dr = PSItemsCrses_ds.Tables[0].Rows[n];
                            objSqlCollection.Insert_PSItemsCrses(PSItemGroupId, PSItemCrses_dr["Course_Id"].ToString(), PSItemCrses_dr["Class_Subject"].ToString(), PSItemCrses_dr["Class_Number"].ToString());        
                        }
                    }
                }

                PlanSheets_DG.DataSource = objSqlCollection.Get_PlanSheets_All(Office_Cd.Text.ToString());
                PlanSheets_DG.DataBind();
                PlanSheets_DG.Items[PlanSheets_DG.Items.Count - 1].Cells[0].Focus();
            }
        }

        private void BindPreText()
        {
            DataSet PreText_ds = objSqlCollection.Get_PreText(Office_Cd.Text.ToString());            
            PreText_DG.DataSource = PreText_ds;
            PreText_DG.DataBind();
        }

        protected void PreText_clicked(object sender, EventArgs e)
        {
            PreText_Add_err_lbl.Visible = false;
            if (PreText_lbtn.Text == "Add")
            {
                Text_Cd_txt.Text = "";
                Text_Descr_txt.Text = "";
                PreText_Add_TC.Visible = true;
                PreText_List_TC.Visible = false;
                PreText_lbtn.Text = "Select";
                Text_Cd_txt.Focus();
            }
            else if (PreText_lbtn.Text == "Select")
            {
                PreText_Add_TC.Visible = false;
                PreText_List_TC.Visible = true;
                PreText_lbtn.Text = "Add";
                BindPreText();
                PreText_DG.Focus();
            }
        }

        protected void PreText_Save_clicked(object sender, EventArgs e)
        {
            for (int i = 0; i <= PreText_DG.Items.Count - 1; i++)
            {
                if (PreText_DG.Items[i].Cells[0].Text.ToString() == Text_Cd_txt.Text.ToString())
                {
                    PreText_Add_err_lbl.Visible = true;
                    Text_Cd_txt.Focus();
                    break;
                }
            }
            if (!PreText_Add_err_lbl.Visible)
            {
                objSqlCollection.Insert_PreText(Office_Cd.Text.ToString(), Text_Cd_txt.Text.ToString().Replace("'", "''"), Text_Descr_txt.Text.ToString().Replace("'", "''"));
                Text_Cd_txt.Text = "";
                Text_Descr_txt.Text = "";
                PreText_Add_TC.Visible = false;
                PreText_List_TC.Visible = true;
                PreText_lbtn.Text = "Add";
                PreText_Add_err_lbl.Visible = false;
                BindPreText();
                PreText_DG.Focus();
            }
        }

        protected void PreText_delete_clicked(object sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete")
            {
                objSqlCollection.Delete_PreText(Office_Cd.Text.ToString(), PreText_DG.DataKeys[e.Item.ItemIndex].ToString().Replace("'","''"));
                BindPreText();
                PreText_DG.Focus();
            }
        }

        protected void Back_btn_clicked(object sender, EventArgs e)
        {
            Response.Redirect("SAS.aspx?dept=" + Office_Cd.Text.ToString());
        }
    }
}
