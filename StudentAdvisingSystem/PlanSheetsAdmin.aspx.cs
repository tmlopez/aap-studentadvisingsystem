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
using APToolkitNET;

namespace StudentAdvisingSystem
{
    public partial class PlanSheetsAdmin : System.Web.UI.Page
    {
        private SqlCollection objSqlCollection = new SqlCollection();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Office_Cd.Text = (string)Request.Params["dept"];
                PSId.Text = (string)Request.Params["psid"];

                if (Office_Cd.Text.ToString() == "")
                {
                    Office_Cd.Text = "AAP";
                }

                DataSet OfficeInfo_DS = objSqlCollection.Get_OfficeInfo(Office_Cd.Text.ToString());
                Office_Descr_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_Descr"].ToString();
                Office_Descr1_lbl.Text = Office_Descr_lbl.Text.ToString();
                Office_Location_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_Location"].ToString();
                Office_PhoneExt_lbl.Text = OfficeInfo_DS.Tables[0].Rows[0]["Office_PhoneExt"].ToString();
                CurrentDate_lbl.Text = DateTime.Now.ToLongDateString();

                if (PSId.Text.ToString() != "") //Update Planning Sheet
                {
                    BindPlanSheets();
                    PSGrps_DL.Visible = false;
                    PSItemsGrps_DL.Visible = false;
                }
            }
        }

        private void BindPlanSheets()
        {
            DataSet PlanSheets_DS = objSqlCollection.Get_PlanSheets(PSId.Text.ToString(), Office_Cd.Text.ToString());
            if (PlanSheets_DS.Tables[0].Rows.Count == 1)
            {
                DataRow PS_DR = PlanSheets_DS.Tables[0].Rows[0];
                PSName_txt.Text = PS_DR["PSName"].ToString();
                PSCatalog_yr_txt.Text = PS_DR["PSCatalog_yr"].ToString();
                for (int i = 0; i <= PSIsValid.Items.Count - 1; i++)
                {
                    if (PSIsValid.Items[i].Value.ToString() == PS_DR["IsValid"].ToString())
                    {
                        PSIsValid.Items[i].Selected = true;
                    }
                }
                PDF_FilePath_lbl.Text = PS_DR["PDF_FilePath"].ToString();
                PDF_FilePath_lbl.NavigateUrl = "~/App_Doc/" + PDF_FilePath_lbl.Text.ToString();                
            }
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            string savePath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_Doc\\";
            if (PDF_FilePath.HasFile)
            {
                if (PDF_FilePath.PostedFile.ContentLength <= 650000)
                {
                    string fileName = PDF_FilePath.FileName.Replace(" ", "").Replace(".pdf", "") + DateTime.Now.Ticks.ToString() + ".pdf";
                    savePath = savePath + fileName;
                    PDF_FilePath.SaveAs(savePath);
                    UpdateInsert_PlanSheets();                    
                    PDF_FilePath_lbl.Text = fileName;
                    PDF_FilePath_lbl.NavigateUrl = "~/App_Doc/" + PDF_FilePath_lbl.Text.ToString();                    
                }
                else
                {
                    PDF_FilePath_lbl.Text = "The File upload error: The PDF file must be less than 650 KB.  Please try to use the Adobe Acrobat File Optimize Tool (File > Reduce File Size or Document > Reduce File Size) to reduce the file size.";
                }
            }
            else
            {
                PDF_FilePath_lbl.Text = "The File upload error: File Not Found";
            }
        }

        protected void Save_PlanSheets_Click(object sender, EventArgs e)
        {
            UpdateInsert_PlanSheets();

            PlanSheets_tbl.Visible = false;
            PSGrps_DL.Visible = true;
            BindPSGrps(0);
        }

        private void UpdateInsert_PlanSheets()
        {
            if (PSId.Text.ToString() != "")
            {
                objSqlCollection.Update_PlanSheets(PSId.Text.ToString(), PSName_txt.Text.ToString(), Office_Cd.Text.ToString(), PSCatalog_yr_txt.Text.ToString(), PSIsValid.SelectedItem.Value.ToString(), PDF_FilePath_lbl.Text.ToString());
            }
            else
            {
                PSId.Text = objSqlCollection.Insert_PlanSheets(PSName_txt.Text.ToString(), Office_Cd.Text.ToString(), PSCatalog_yr_txt.Text.ToString(), PSIsValid.SelectedItem.Value.ToString(), PDF_FilePath_lbl.Text.ToString());
            }
        }

        private DataSet GetPDF_Fields()
        {
            APToolkitNET.APToolkit objPDF = new APToolkitNET.APToolkit();
            long objr;                                   
            objr = objPDF.OpenInputFile(AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_Doc\\" + PDF_FilePath_lbl.Text.ToString());            
            DataSet PDF_ds = new DataSet();
            DataTable PDF_dt = PDF_ds.Tables.Add("PDF");
            PDF_dt.Columns.Add(new DataColumn("PDF_FieldNames", typeof(String)));
            DataRow PDF_dr;            
            for (int i = 0; i <= objPDF.CountFormFields(); i++)
            {
                PDF_dr = PDF_dt.NewRow();
                PDF_dr[0] = objPDF.GetFormFieldName((short)i).ToString();
                PDF_dt.Rows.Add(PDF_dr);
            }
            
            return PDF_ds;
        }

        private void BindPSGrps(int NumNewRow)
        {
            DataSet PSGrps_DS = objSqlCollection.Get_PSGrps(PSId.Text.ToString());
            if (PSGrps_DS.Tables[0].Rows.Count == 0)            
            {
                DataRow PSGrps_DR = PSGrps_DS.Tables[0].NewRow();
                PSGrps_DR["PSId"] = PSId.Text.ToString();
                //PSGrps_DR["PSGroupId"] = "";
                PSGrps_DR["PSGroup_SubDescr"] = "";
                PSGrps_DR["PSGroup_SeqNum"] = "1";
                PSGrps_DR["PSInput1_descr"] = "";
                PSGrps_DR["PSInput2_descr"] = "";
                PSGrps_DR["PSInput3_descr"] = "";
                PSGrps_DS.Tables[0].Rows.InsertAt(PSGrps_DR, 0);
            }
            if (NumNewRow > 0)
            {
                int DSRowCnt = PSGrps_DS.Tables[0].Rows.Count;
                for (int i = 0; i <= NumNewRow - 1; i++)
                {
                    DataRow PSGrps_DR = PSGrps_DS.Tables[0].NewRow();
                    PSGrps_DR["PSId"] = PSId.Text.ToString();
                    //PSGrps_DR["PSGroupId"] = "";
                    PSGrps_DR["PSGroup_SubDescr"] = "";
                    PSGrps_DR["PSGroup_SeqNum"] = DSRowCnt + i + 1;
                    PSGrps_DR["PSInput1_descr"] = "";
                    PSGrps_DR["PSInput2_descr"] = "";
                    PSGrps_DR["PSInput3_descr"] = "";
                    PSGrps_DS.Tables[0].Rows.Add(PSGrps_DR);
                }
            }

            PSGrps_DL.DataSource = PSGrps_DS;
            PSGrps_DL.DataBind();
            DataSet PSGroup_Descr_DS = objSqlCollection.Get_PSGrps();
            
            for (int i = 0; i <= PSGrps_DL.Items.Count - 1; i++)
            {
                DropDownList objPSGroup_Descr = (DropDownList)PSGrps_DL.Items[i].FindControl("PSGroup_Descr");
                objPSGroup_Descr.DataSource = PSGroup_Descr_DS;
                objPSGroup_Descr.DataBind();
                
                for (int n = 0; n <= objPSGroup_Descr.Items.Count - 1; n++)
                {
                    if (objPSGroup_Descr.Items[n].Value.ToString() == PSGrps_DS.Tables[0].Rows[i]["PSGroupId"].ToString() && i <= PSGrps_DL.Items.Count - NumNewRow - 1)
                    {
                        objPSGroup_Descr.Items[n].Selected = true;
                    }                
                }
            }
        }

        protected void PSGrps_DL_Click(object sender, DataListCommandEventArgs e)
        { 
            switch(e.CommandName.ToString())
            {
                case "Add_PSGrps_tbl":
                    LinkButton objAdd_PSGrps_tbl_lbtn = (LinkButton)PSGrps_DL.Items[e.Item.ItemIndex].FindControl("Add_PSGrps_tbl_lbtn");
                    TextBox objPSGroup_Descr_txt = (TextBox)PSGrps_DL.Items[e.Item.ItemIndex].FindControl("PSGroup_Descr_txt");
                    DropDownList objPSGroup_Descr = (DropDownList)PSGrps_DL.Items[e.Item.ItemIndex].FindControl("PSGroup_Descr");
                    if (objAdd_PSGrps_tbl_lbtn.Text == "+")
                    {
                        objPSGroup_Descr_txt.Visible = true;
                        objPSGroup_Descr.Visible = false;
                        objPSGroup_Descr_txt.Text = "";
                        objAdd_PSGrps_tbl_lbtn.Text = "...";
                        objPSGroup_Descr_txt.Focus();
                    }
                    else if (objAdd_PSGrps_tbl_lbtn.Text == "..." && objPSGroup_Descr_txt.Text.ToString() != "")
                    {
                        objPSGroup_Descr_txt.Visible = false;
                        objPSGroup_Descr.Visible = true;
                        objSqlCollection.Insert_PSGrps(objPSGroup_Descr_txt.Text.ToString());
                        objPSGroup_Descr.DataSource = objSqlCollection.Get_PSGrps();
                        objPSGroup_Descr.DataBind();
                        for (int n = 0; n <= objPSGroup_Descr.Items.Count - 1; n++)
                        {
                            if (objPSGroup_Descr.Items[n].Text.ToString() == objPSGroup_Descr_txt.Text.ToString())
                            {
                                objPSGroup_Descr.Items[n].Selected = true;
                            }
                        }
                        objPSGroup_Descr_txt.Text = "";
                        objAdd_PSGrps_tbl_lbtn.Text = "+";
                        objPSGroup_Descr.Focus();
                    }
                    else
                    {
                        objPSGroup_Descr_txt.Visible = false;
                        objPSGroup_Descr.Visible = true;
                        objAdd_PSGrps_tbl_lbtn.Text = "+";
                        objPSGroup_Descr.Focus();
                    }
                    break;
                case "Add_PSGrps":
                    Save_PSGrps(-1);
                    //int intNewRowCnt = int.Parse(NewRowCnt.Text);
                    //intNewRowCnt += 1;
                    //NewRowCnt.Text = intNewRowCnt.ToString();
                    BindPSGrps(1);                    
                    objPSGroup_Descr = (DropDownList)PSGrps_DL.Items[PSGrps_DL.Items.Count-1].FindControl("PSGroup_Descr");
                    objPSGroup_Descr.Focus();
                    break;
                case "Delete_PSGrps":
                    Save_PSGrps(e.Item.ItemIndex);
                    BindPSGrps(0);
                    break;
                case "Save_PSGrps":
                    Save_PSGrps(-1);
                    NewRowCnt.Text = "0";
                    PSGrps_DL.Visible = false;
                    PSItemsGrps_DL.Visible = true;
                    BindPSItemsGrps(0);
                    break;
                case "PSAdmin_btn":
                    Save_PSGrps(-1);
                    NewRowCnt.Text = "0";
                    Response.Redirect("PlanSheetsAdmin.aspx?dept=" + Office_Cd.Text.ToString() + "&psid=" + PSId.Text.ToString());
                    break;
            }            
        }

        private void Save_PSGrps(int DeleteRow)
        {
            objSqlCollection.Delete_PSGrps(PSId.Text.ToString());
            for (int i = 0; i <= PSGrps_DL.Items.Count - 1; i++)
            {
                TextBox objPSGroup_SeqNum = (TextBox)PSGrps_DL.Items[i].FindControl("PSGroup_SeqNum");
                DropDownList objPSGroup_Descr = (DropDownList)PSGrps_DL.Items[i].FindControl("PSGroup_Descr");
                TextBox objPSGroup_Descr_txt = (TextBox)PSGrps_DL.Items[i].FindControl("PSGroup_Descr_txt");
                LinkButton objAdd_PSGrps_tbl_lbtn = (LinkButton)PSGrps_DL.Items[i].FindControl("Add_PSGrps_tbl_lbtn");
                TextBox objPSGroup_SubDescr = (TextBox)PSGrps_DL.Items[i].FindControl("PSGroup_SubDescr");
                TextBox objPSInput1_descr = (TextBox)PSGrps_DL.Items[i].FindControl("PSInput1_descr");
                TextBox objPSInput2_descr = (TextBox)PSGrps_DL.Items[i].FindControl("PSInput2_descr");
                TextBox objPSInput3_descr = (TextBox)PSGrps_DL.Items[i].FindControl("PSInput3_descr");
                string strPSGroupId = "";
                if (objAdd_PSGrps_tbl_lbtn.Text.ToString() == "..." && objPSGroup_Descr_txt.Text.ToString() != "")
                {
                    objSqlCollection.Insert_PSGrps(objPSGroup_Descr_txt.Text.ToString());
                    DataSet objPSGrps_DS = objSqlCollection.Get_PSGrps();
                    for (int n = 0; n <= objPSGrps_DS.Tables[0].Rows.Count - 1; n++)
                    {
                        if (objPSGrps_DS.Tables[0].Rows[n]["PSGroup_Descr"].ToString() == objPSGroup_Descr_txt.Text.ToString())
                        {
                            strPSGroupId = objPSGrps_DS.Tables[0].Rows[n]["PSGroupId"].ToString();
                            break;
                        }
                    }
                }
                else if (objPSGroup_Descr.SelectedIndex > 0)
                {
                    strPSGroupId = objPSGroup_Descr.SelectedItem.Value.ToString();
                }

                if (strPSGroupId != "" && i != DeleteRow)
                {
                    objSqlCollection.Insert_PSGrps(PSId.Text.ToString(), strPSGroupId, objPSGroup_SubDescr.Text.ToString(), objPSGroup_SeqNum.Text.ToString(), objPSInput1_descr.Text.ToString(), objPSInput2_descr.Text.ToString(), objPSInput3_descr.Text.ToString());
                    if (i == 0)
                    {
                        PSGroupId.Text = strPSGroupId;
                    }
                }
            }            
        }

        private void BindPSItemsGrps(int NumNewRow)
        {
            DataSet PDF_DS = GetPDF_Fields();
            DataSet PSItemsGrps_DS = objSqlCollection.Get_PSItemsGrps(PSId.Text.ToString(), PSGroupId.Text.ToString());
            if (PSItemsGrps_DS.Tables[0].Rows.Count == 0)
            {
                DataRow PSItemsGrps_DR = PSItemsGrps_DS.Tables[0].NewRow();                              
                PSItemsGrps_DR["PSId"] = PSId.Text.ToString();
                PSItemsGrps_DR["PSGroupId"] = PSGroupId.Text.ToString();
                //PSItemsGrps_DR["PSItemId"] = "";
                PSItemsGrps_DR["PSItem_SeqNum"] = "1";
                PSItemsGrps_DR["GetPSInfo"] = "0";
                PSItemsGrps_DR["Notes_PDFName"] = "";
                PSItemsGrps_DR["Input1_PDFName"] = "";
                PSItemsGrps_DR["Input2_PDFName"] = "";
                PSItemsGrps_DR["Input3_PDFName"] = "";
                DataSet PSGrps_DS = objSqlCollection.Get_PSGrps(PSId.Text.ToString());
                for (int i = 0; i <= PSGrps_DS.Tables[0].Rows.Count - 1; i++)
                {
                    if (PSGrps_DS.Tables[0].Rows[i]["PSGroupId"].ToString() == PSGroupId.Text.ToString())
                    {
                        PSItemsGrps_DR["PSGroup_Descr"] = PSGrps_DS.Tables[0].Rows[i]["PSGroup_Descr"].ToString();
                        PSItemsGrps_DR["PSGroup_SeqNum"] = PSGrps_DS.Tables[0].Rows[i]["PSGroup_SeqNum"].ToString();
                    }
                }
                PSItemsGrps_DS.Tables[0].Rows.InsertAt(PSItemsGrps_DR, 0);
            }
            if (NumNewRow > 0)
            {
                int DSRowCnt = PSItemsGrps_DS.Tables[0].Rows.Count;
                for (int i = 0; i <= NumNewRow - 1; i++)
                {
                    DataRow PSItemsGrps_DR = PSItemsGrps_DS.Tables[0].NewRow();
                    PSItemsGrps_DR["PSId"] = PSId.Text.ToString();
                    PSItemsGrps_DR["PSGroupId"] = PSGroupId.Text.ToString();
                    //PSItemsGrps_DR["PSItemId"] = "";
                    PSItemsGrps_DR["PSItem_SeqNum"] = DSRowCnt + i + 1;
                    PSItemsGrps_DR["GetPSInfo"] = "0";
                    PSItemsGrps_DR["Notes_PDFName"] = "";
                    PSItemsGrps_DR["Input1_PDFName"] = "";
                    PSItemsGrps_DR["Input2_PDFName"] = "";
                    PSItemsGrps_DR["Input3_PDFName"] = "";
                    DataSet PSGrps_DS = objSqlCollection.Get_PSGrps(PSId.Text.ToString());
                    for (int n = 0; n <= PSGrps_DS.Tables[0].Rows.Count - 1; n++)
                    {
                        if (PSGrps_DS.Tables[0].Rows[n]["PSGroupId"].ToString() == PSGroupId.Text.ToString())
                        {
                            PSItemsGrps_DR["PSGroup_Descr"] = PSGrps_DS.Tables[0].Rows[n]["PSGroup_Descr"].ToString();
                            PSItemsGrps_DR["PSGroup_SeqNum"] = PSGrps_DS.Tables[0].Rows[n]["PSGroup_SeqNum"].ToString();
                        }
                    }
                    PSItemsGrps_DS.Tables[0].Rows.Add(PSItemsGrps_DR);
                }
            }

            PSItemsGrps_DL.DataSource = PSItemsGrps_DS;
            PSItemsGrps_DL.DataBind();

            DataSet PSItems_DS = objSqlCollection.Get_PSItems();
            for (int i = 0; i <= PSItemsGrps_DL.Items.Count - 1; i++)
            {
                DropDownList objPSItem_Descr = (DropDownList)PSItemsGrps_DL.Items[i].FindControl("PSItem_Descr");
                DataList objPSItemsCrses_DL = (DataList)PSItemsGrps_DL.Items[i].FindControl("PSItemsCrses_DL");                
                DropDownList objNotes_PDFName = (DropDownList)PSItemsGrps_DL.Items[i].FindControl("Notes_PDFName");
                DropDownList objInput1_PDFName = (DropDownList)PSItemsGrps_DL.Items[i].FindControl("Input1_PDFName");
                DropDownList objInput2_PDFName = (DropDownList)PSItemsGrps_DL.Items[i].FindControl("Input2_PDFName");
                DropDownList objInput3_PDFName = (DropDownList)PSItemsGrps_DL.Items[i].FindControl("Input3_PDFName");
                RadioButtonList objGetPSInfo = (RadioButtonList)PSItemsGrps_DL.Items[i].FindControl("GetPSInfo");

                if (PSItemsGrps_DS.Tables[0].Rows[i]["PSItemGroupId"].ToString() != "")
                {
                    DataSet PSItemsCrses_DS = objSqlCollection.Get_PSItemsCrses(PSItemsGrps_DS.Tables[0].Rows[i]["PSItemGroupId"].ToString());
                    DataTable PSItemsCrses_dt = PSItemsCrses_DS.Tables[0];
                    objPSItemsCrses_DL.DataSource = PSItemsCrses_DS;
                    objPSItemsCrses_DL.DataBind();                   
                }
                //else
                //{
                //    PSItemsCrses_DS = new DataSet();
                //    PSItemsCrses_dt = PSItemsCrses_DS.Tables.Add("PSItemsCrses");
                //    PSItemsCrses_dt.Columns.Add(new DataColumn("PSItemGroupId", typeof(String)));
                //    PSItemsCrses_dt.Columns.Add(new DataColumn("Course_Id", typeof(String)));
                //    PSItemsCrses_dt.Columns.Add(new DataColumn("Class_Subject", typeof(String)));
                //    PSItemsCrses_dt.Columns.Add(new DataColumn("Class_Number", typeof(String)));
                //    PSItemsCrses_dr = PSItemsCrses_dt.NewRow();
                //    //PSItemsCrses_dr["PSItemGroupId"] = "";
                //    PSItemsCrses_dr["Course_Id"] = "";
                //    PSItemsCrses_dr["Class_Subject"] = "";
                //    PSItemsCrses_dr["Class_Number"] = "";
                //    PSItemsCrses_dt.Rows.Add(PSItemsCrses_dr);
                //}               
                

                if (PDF_DS.Tables[0].Rows.Count > 0)
                {
                    objNotes_PDFName.DataSource = PDF_DS;
                    objNotes_PDFName.DataBind();

                    objInput1_PDFName.DataSource = PDF_DS;
                    objInput1_PDFName.DataBind();

                    objInput2_PDFName.DataSource = PDF_DS;
                    objInput2_PDFName.DataBind();

                    objInput3_PDFName.DataSource = PDF_DS;
                    objInput3_PDFName.DataBind();
                
                    for (int n = 0; n <= objNotes_PDFName.Items.Count - 1; n++)
                    {
                        if (PSItemsGrps_DS.Tables[0].Rows[i]["Notes_PDFName"].ToString() == PDF_DS.Tables[0].Rows[n][0].ToString() && i <= PSItemsGrps_DS.Tables[0].Rows.Count - NumNewRow - 1)
                        {
                            objNotes_PDFName.SelectedIndex = n;
                        }
                        if (PSItemsGrps_DS.Tables[0].Rows[i]["Input1_PDFName"].ToString() == PDF_DS.Tables[0].Rows[n][0].ToString() && i <= PSItemsGrps_DS.Tables[0].Rows.Count - NumNewRow - 1)
                        {
                            objInput1_PDFName.SelectedIndex = n;
                        }
                        if (PSItemsGrps_DS.Tables[0].Rows[i]["Input2_PDFName"].ToString() == PDF_DS.Tables[0].Rows[n][0].ToString() && i <= PSItemsGrps_DS.Tables[0].Rows.Count - NumNewRow - 1)
                        {
                            objInput2_PDFName.SelectedIndex = n;
                        }
                        if (PSItemsGrps_DS.Tables[0].Rows[i]["Input3_PDFName"].ToString() == PDF_DS.Tables[0].Rows[n][0].ToString() && i <= PSItemsGrps_DS.Tables[0].Rows.Count - NumNewRow - 1)
                        {
                            objInput3_PDFName.SelectedIndex = n;
                        }
                    }
                
                }
                objPSItem_Descr.DataSource = PSItems_DS;
                objPSItem_Descr.DataBind();                
                for (int n = 0; n <= objPSItem_Descr.Items.Count - 1; n++)
                {
                    if (PSItems_DS.Tables[0].Rows[n]["PSItemId"].ToString() == PSItemsGrps_DS.Tables[0].Rows[i]["PSItemId"].ToString() && i <= PSItemsGrps_DS.Tables[0].Rows.Count - NumNewRow -1)
                    {
                        objPSItem_Descr.Items[n].Selected = true;
                    }
                }

                for (int n = 0; n <= objGetPSInfo.Items.Count - 1; n++)
                {
                    if (objGetPSInfo.Items[n].Value.ToString() == PSItemsGrps_DS.Tables[0].Rows[i]["GetPSInfo"].ToString() && i <= PSItemsGrps_DS.Tables[0].Rows.Count - NumNewRow - 1)
                    {
                        objGetPSInfo.Items[n].Selected = true;
                    }
                    else
                    {
                        objGetPSInfo.Items[n].Selected = false;
                    }
                }                
            }
            Literal1.Text = "";
        }

        protected void PSItems_DL_Click(object sender, DataListCommandEventArgs e)
        {
            switch (e.CommandName.ToString())
            { 
                case "Add_PSItems_tbl":
                    DropDownList objPSItem_Descr = (DropDownList)PSItemsGrps_DL.Items[e.Item.ItemIndex].FindControl("PSItem_Descr");
                    TextBox objPSItem_Descr_txt = (TextBox)PSItemsGrps_DL.Items[e.Item.ItemIndex].FindControl("PSItem_Descr_txt");
                    LinkButton objAdd_PSItems_tbl_lbtn = (LinkButton)PSItemsGrps_DL.Items[e.Item.ItemIndex].FindControl("Add_PSItems_tbl_lbtn");
                    if (objAdd_PSItems_tbl_lbtn.Text == "+")
                    {
                        objPSItem_Descr.Visible = false;
                        objPSItem_Descr_txt.Visible = true;
                        objPSItem_Descr_txt.Text = "";
                        objAdd_PSItems_tbl_lbtn.Text = "...";
                        objPSItem_Descr_txt.Focus();
                    }
                    else if (objAdd_PSItems_tbl_lbtn.Text.ToString() == "..." && objPSItem_Descr_txt.Text.ToString() != "")
                    {
                        objPSItem_Descr_txt.Visible = false;
                        objPSItem_Descr.Visible = true;
                        objSqlCollection.Insert_PSItems(objPSItem_Descr_txt.Text.ToString());
                        objPSItem_Descr.DataSource = objSqlCollection.Get_PSItems();
                        objPSItem_Descr.DataBind();
                        for (int n = 0; n <= objPSItem_Descr.Items.Count - 1; n++)
                        {
                            if (objPSItem_Descr.Items[n].Text.ToString() == objPSItem_Descr_txt.Text.ToString())
                            {
                                objPSItem_Descr.Items[n].Selected = true;
                            }
                        }
                        objPSItem_Descr_txt.Text = "";
                        objAdd_PSItems_tbl_lbtn.Text = "+";
                        objPSItem_Descr.Focus();
                    }
                    else
                    {
                        objPSItem_Descr_txt.Visible = false;
                        objPSItem_Descr.Visible = true;
                        objPSItem_Descr_txt.Text = "";
                        objAdd_PSItems_tbl_lbtn.Text = "+";
                        objPSItem_Descr.Focus();
                    }
                    Literal1.Text = "";
                    break;
                case "Add_PSItems":                    
                    Save_PSItemsGrps(-1);
                    //int intNewRowCnt = int.Parse(NewRowCnt.Text);
                    //intNewRowCnt += 1;
                    //NewRowCnt.Text = intNewRowCnt.ToString();
                    BindPSItemsGrps(1);
                    objPSItem_Descr = (DropDownList)PSItemsGrps_DL.Items[PSItemsGrps_DL.Items.Count - 1].FindControl("PSItem_Descr");
                    objPSItem_Descr.Focus();
                    break;
                case "Delete_PSItems":
                    Save_PSItemsGrps(e.Item.ItemIndex);                    
                    BindPSItemsGrps(0);
                    break;
                case "Save_PSItems":
                    Save_PSItemsGrps(-1);
                    NewRowCnt.Text = "0";
                    DataSet PSGrps_DS = objSqlCollection.Get_PSGrps(PSId.Text.ToString());
                    for (int i = 0; i <= PSGrps_DS.Tables[0].Rows.Count - 1; i++)
                    {
                        if (PSGrps_DS.Tables[0].Rows[i]["PSGroupId"].ToString() == PSGroupId.Text.ToString())
                        {
                            if (i == PSGrps_DS.Tables[0].Rows.Count - 1)
                            {
                                Response.Redirect("PlanSheetsAdmin.aspx?dept=" + Office_Cd.Text.ToString() + "&psid=" + PSId.Text.ToString());
                            }
                            else
                            {
                                PSGroupId.Text = PSGrps_DS.Tables[0].Rows[i + 1]["PSGroupId"].ToString();                                
                                break;
                            }
                        }
                    }
                    BindPSItemsGrps(0);
                    break;
                case "Add_PSItemsCrses":
                    DataList objPSItemsCrses_DL = (DataList)PSItemsGrps_DL.Items[e.Item.ItemIndex].FindControl("PSItemsCrses_DL");
                    if (PSItemsGrps_DL.DataKeys[e.Item.ItemIndex].ToString() != "")
                    {
                        objSqlCollection.Delete_PSItemCrses(PSItemsGrps_DL.DataKeys[e.Item.ItemIndex].ToString());
                        for (int i = 0; i <= objPSItemsCrses_DL.Items.Count - 1; i++)
                        {
                            TextBox objCourse_Id = (TextBox)objPSItemsCrses_DL.Items[i].FindControl("Course_Id");
                            TextBox objClass_Subject = (TextBox)objPSItemsCrses_DL.Items[i].FindControl("Class_Subject");
                            TextBox objClass_Number = (TextBox)objPSItemsCrses_DL.Items[i].FindControl("Class_Number");

                            if (objCourse_Id.Text.ToString() != "" && objClass_Subject.Text.ToString() != "" && objClass_Number.Text.ToString() != "")
                            {
                                objSqlCollection.Insert_PSItemsCrses(PSItemsGrps_DL.DataKeys[e.Item.ItemIndex].ToString(), objCourse_Id.Text.ToString(), objClass_Subject.Text.ToString(), objClass_Number.Text.ToString());
                            }
                        }
                    }
                    DataSet PSItemsCrses_DS;
                    DataTable PSItemsCrses_dt;
                    DataRow PSItemsCrses_dr;
                    if (PSItemsGrps_DL.DataKeys[e.Item.ItemIndex].ToString() != "")
                    {
                        PSItemsCrses_DS = objSqlCollection.Get_PSItemsCrses(PSItemsGrps_DL.DataKeys[e.Item.ItemIndex].ToString());
                        PSItemsCrses_dt = PSItemsCrses_DS.Tables[0];
                    }
                    else
                    {
                        PSItemsCrses_DS = new DataSet();
                        PSItemsCrses_dt = PSItemsCrses_DS.Tables.Add("PSItemsCrses");
                        PSItemsCrses_dt.Columns.Add(new DataColumn("PSItemGroupId", typeof(String)));
                        PSItemsCrses_dt.Columns.Add(new DataColumn("Course_Id", typeof(String)));
                        PSItemsCrses_dt.Columns.Add(new DataColumn("Class_Subject", typeof(String)));
                        PSItemsCrses_dt.Columns.Add(new DataColumn("Class_Number", typeof(String)));
                    }
                    PSItemsCrses_dr = PSItemsCrses_dt.NewRow();
                    //PSItemsCrses_dr["PSItemGroupId"] = "";
                    PSItemsCrses_dr["Course_Id"] = "";
                    PSItemsCrses_dr["Class_Subject"] = "";
                    PSItemsCrses_dr["Class_Number"] = "";
                    PSItemsCrses_dt.Rows.Add(PSItemsCrses_dr);
                    objPSItemsCrses_DL.DataSource = PSItemsCrses_DS;
                    objPSItemsCrses_DL.DataBind();

                    TextBox Class_Subject = (TextBox)objPSItemsCrses_DL.Items[objPSItemsCrses_DL.Items.Count - 1].FindControl("Class_Subject");
                    Class_Subject.Focus();
                    Literal1.Text = "";
                    break;
                case "PSAdmin_btn":
                    Save_PSItemsGrps(-1);
                    NewRowCnt.Text = "0";
                    Response.Redirect("PlanSheetsAdmin.aspx?dept=" + Office_Cd.Text.ToString() + "&psid=" + PSId.Text.ToString());
                    break;
            }
        }

        protected void PSItemsCrses_DL_Click(object sender, DataListCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "SearchCourse")
            {
                int PSItemsGrps_Index = int.Parse(e.Item.UniqueID.ToString().Substring(18, 2));
                DataList objPSItemsCrses_DL = (DataList)PSItemsGrps_DL.Items[PSItemsGrps_Index-1].FindControl("PSItemsCrses_DL");
                TextBox objCourse_Id = (TextBox)objPSItemsCrses_DL.Items[e.Item.ItemIndex].FindControl("Course_Id");
                TextBox objClass_Subject = (TextBox)objPSItemsCrses_DL.Items[e.Item.ItemIndex].FindControl("Class_Subject");
                TextBox objClass_Number = (TextBox)objPSItemsCrses_DL.Items[e.Item.ItemIndex].FindControl("Class_Number");
                DataSet Course_Id_DS = objSqlCollection.Get_Course_Id(objCourse_Id.Text.ToString(), objClass_Subject.Text.ToString(), objClass_Number.Text.ToString());
                if (Course_Id_DS.Tables[0].Rows.Count == 1)
                {
                    objCourse_Id.Text = Course_Id_DS.Tables[0].Rows[0]["Course_Id"].ToString();
                    objClass_Subject.Text = Course_Id_DS.Tables[0].Rows[0]["Class_Subject"].ToString();
                    objClass_Number.Text = Course_Id_DS.Tables[0].Rows[0]["Class_Number"].ToString();
                    //objCourse_Id.Focus();
                }
                else if (Course_Id_DS.Tables[0].Rows.Count > 1)
                {
                    string strjscript = "<script language=javascript>";
                    strjscript = strjscript + "SelectCourse_window=window.open('PlanSheetsAdmin_Crses.aspx?formfield=" + e.Item.UniqueID.ToString() + "&Course_Id=" + objCourse_Id.Text.ToString() + "&Class_Subject=" + objClass_Subject.Text.ToString() + "&Class_Number=" + objClass_Number.Text.ToString() + "','_blank','width=550,height=300,scrollbars=yes,resize=no');SelectCourse_window.focus();";
                    strjscript = strjscript + "</script" + ">";
                    Literal1.Text = strjscript;
                    
                }
                objClass_Subject.Focus();
            }
        }

        private void Save_PSItemsGrps(int DeleteRow)
        {            
            //objSqlCollection.Delete_PSItemsGrps(PSId.Text.ToString(), PSGroupId.Text.ToString());
            for (int i = 0; i <= PSItemsGrps_DL.Items.Count - 1; i++)
            { 
                TextBox objPSItem_SeqNum = (TextBox)PSItemsGrps_DL.Items[i].FindControl("PSItem_SeqNum");
                DropDownList objPSItem_Descr = (DropDownList)PSItemsGrps_DL.Items[i].FindControl("PSItem_Descr");
                TextBox objPSItem_Descr_txt = (TextBox)PSItemsGrps_DL.Items[i].FindControl("PSItem_Descr_txt");
                LinkButton objAdd_PSItems_tbl_lbtn = (LinkButton)PSItemsGrps_DL.Items[i].FindControl("Add_PSItems_tbl_lbtn");
                DataList objPSItemsCrses_DL = (DataList)PSItemsGrps_DL.Items[i].FindControl("PSItemsCrses_DL");
                DropDownList objNotes_PDFName = (DropDownList)PSItemsGrps_DL.Items[i].FindControl("Notes_PDFName");
                DropDownList objInput1_PDFName = (DropDownList)PSItemsGrps_DL.Items[i].FindControl("Input1_PDFName");
                DropDownList objInput2_PDFName = (DropDownList)PSItemsGrps_DL.Items[i].FindControl("Input2_PDFName");
                DropDownList objInput3_PDFName = (DropDownList)PSItemsGrps_DL.Items[i].FindControl("Input3_PDFName");
                RadioButtonList objGetPSInfo = (RadioButtonList)PSItemsGrps_DL.Items[i].FindControl("GetPSInfo");
                string strPSItemId = "";
                if (objAdd_PSItems_tbl_lbtn.Text.ToString() == "..." && objPSItem_Descr_txt.Text != "")
                {
                    objSqlCollection.Insert_PSItems(objPSItem_Descr_txt.Text.ToString());
                    DataSet ds = objSqlCollection.Get_PSItems();
                    for (int n = 0; n <= ds.Tables[0].Rows.Count - 1; n++)
                    {
                        if (ds.Tables[0].Rows[n]["PSItem_Descr"].ToString() == objPSItem_Descr_txt.Text.ToString())
                        {
                            strPSItemId = ds.Tables[0].Rows[n]["PSItemId"].ToString();
                            break;
                        }
                    }
                }
                else if (objPSItem_Descr.SelectedIndex > 0)
                {
                    strPSItemId = objPSItem_Descr.SelectedItem.Value.ToString();
                }

                if (strPSItemId != "" && i != DeleteRow)
                {
                    string strNotes_PDFName = "", strInput1_PDFName = "", strInput2_PDFName = "", strInput3_PDFName = "";
                    if (objNotes_PDFName != null && objNotes_PDFName.SelectedIndex > 0)
                    {
                        strNotes_PDFName = objNotes_PDFName.SelectedItem.Value.ToString();                             
                    }
                    if (objInput1_PDFName != null && objInput1_PDFName.SelectedIndex > 0)
                    {
                        strInput1_PDFName = objInput1_PDFName.SelectedItem.Value.ToString();
                    }
                    if (objInput2_PDFName != null && objInput2_PDFName.SelectedIndex > 0)
                    {
                        strInput2_PDFName = objInput2_PDFName.SelectedItem.Value.ToString();
                    }
                    if (objInput3_PDFName != null && objInput3_PDFName.SelectedIndex > 0)
                    {
                        strInput3_PDFName = objInput3_PDFName.SelectedItem.Value.ToString();
                    }

                    string strPSItemGroupId = PSItemsGrps_DL.DataKeys[i].ToString();
                    if (strPSItemGroupId == "")
                    {                        
                        strPSItemGroupId = objSqlCollection.Insert_PSItemsGrps(PSId.Text.ToString(), PSGroupId.Text.ToString(), strPSItemId, objPSItem_SeqNum.Text.ToString(), objGetPSInfo.SelectedItem.Value.ToString(), strNotes_PDFName, strInput1_PDFName, strInput2_PDFName, strInput3_PDFName);                        
                    }
                    else
                    {
                        objSqlCollection.Update_PSItemsGrps(strPSItemGroupId, strPSItemId, objPSItem_SeqNum.Text.ToString(), objGetPSInfo.SelectedItem.Value.ToString(), strNotes_PDFName, strInput1_PDFName, strInput2_PDFName, strInput3_PDFName);
                    }
                    
                    objSqlCollection.Delete_PSItemCrses(strPSItemGroupId);
                    for (int n = 0; n <= objPSItemsCrses_DL.Items.Count - 1; n++)
                    {
                        TextBox objCourse_Id = (TextBox)objPSItemsCrses_DL.Items[n].FindControl("Course_Id");
                        TextBox objClass_Subject = (TextBox)objPSItemsCrses_DL.Items[n].FindControl("Class_Subject");
                        TextBox objClass_Number = (TextBox)objPSItemsCrses_DL.Items[n].FindControl("Class_Number");

                        if (objCourse_Id.Text.ToString() != "" && objClass_Subject.Text.ToString() != "" && objClass_Number.Text.ToString() != "")
                        {
                            objSqlCollection.Insert_PSItemsCrses(strPSItemGroupId, objCourse_Id.Text.ToString(), objClass_Subject.Text.ToString(), objClass_Number.Text.ToString());
                        }
                    }
                }
                else if (PSItemsGrps_DL.DataKeys[i].ToString() != "" & i == DeleteRow)
                {
                    objSqlCollection.Delete_PSItemCrses(PSItemsGrps_DL.DataKeys[i].ToString());
                    objSqlCollection.Delete_PSItemsGrps(PSItemsGrps_DL.DataKeys[i].ToString());
                }
            }
        }

        protected void SASAdmin_Click(object sender, EventArgs e)
        {
            Response.Redirect("SASAdmin.aspx?dept=" + Office_Cd.Text.ToString());
        }
    }
}
