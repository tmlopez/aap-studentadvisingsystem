<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SASAdmin.aspx.cs" Inherits="StudentAdvisingSystem.SASAdmin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Student Advising System- Admin</title>
    <link href="_styles/styles1.css" rel="stylesheet" title="default" type="text/css" media="screen" />
    <link href="_styles/print.css" rel="stylesheet" type="text/css" media="print" />    
</head>
<body>
<div id="wrapper">
<div id="skip"><a href="#content" title="Skip to Content"><img src="/images/dot.gif" alt="Skip to Content" width="1" height="1" /></a></div><!--skip-->
<div id="header" class="noprint">
		<div id="headerchicologo"><div id="headerbanner2"><img src="images/headerLogo.gif" alt="Welcome to California State University, Chico - Today Decides Tomorrow" />
		</div><!--headerbanner-->
	</div><!--headerchicologo-->
</div><!--header-->
    <form id="form1" runat="server">
    <div id="main"><!--main-->
   
    <div id="mainhomecontent">  <!-- start mainhomecontent -->
        <h1><asp:Label ID="Office_Descr_lbl" runat="server" />- SAS Admin Tool</h1>
        <p><STRONG><asp:Label ID="CurrentDate_lbl" runat="server"></asp:Label></STRONG></p>            
        <asp:Table ID="OfficeInfo_tbl" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="2">
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell ColumnSpan="2"><h3>General Office Info</h3></asp:TableCell>
            </asp:TableRow>            
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="30%"><strong>Office Descr:</strong></asp:TableCell><asp:TableCell Width="70%"><asp:TextBox ID="Office_Descr_txt" runat="server" MaxLength="50" Width="450"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="30%"><strong>Advisor Type:</strong></asp:TableCell><asp:TableCell Width="70%"><asp:TextBox ID="Advisor_Type_txt" runat="server" MaxLength="50" Width="450"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="30%"><strong>Office Location:</strong></asp:TableCell><asp:TableCell Width="70%"><asp:TextBox ID="Office_Location_txt" runat="server" MaxLength="50" Width="450"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="30%"><strong>Office Phone Ext:</strong></asp:TableCell><asp:TableCell Width="70%"><asp:TextBox ID="Office_PhoneExt_txt" runat="server" MaxLength="4" Width="450"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="30%"><strong>Office Email Address:</strong></asp:TableCell><asp:TableCell Width="70%"><asp:TextBox ID="Office_Email_Address_txt" runat="server" MaxLength="100" Width="450"></asp:TextBox></asp:TableCell>
            </asp:TableRow>            
        </asp:Table>
        <br />
        <asp:Table ID="SignIn_tbl" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="2">
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell ColumnSpan="2"><h3>Student Sign In Options</h3></asp:TableCell>
            </asp:TableRow>            
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="30%"><strong>Questions Concern (*Ques Code):</strong> [<asp:LinkButton ID="QuesField_lbtn" runat="server" Text="Add" OnClick="QuesField_clicked" />]
                    
                </asp:TableCell>
                    <asp:TableCell ID="Questions_Add_TC" runat="server" Width="70%" Visible="false">Ques Code: <asp:Label ID="Questions_Add_err_lbl" runat="server" ForeColor="Maroon" Text="Ques Code must be unique because it is a key field" Visible="false" /><br /><asp:TextBox ID="QuesField_Cd_txt" runat="server" Width="300" MaxLength="10" /><br />Description:<br /><asp:TextBox ID="QuesField_Descr_txt" runat="server" Width="300" MaxLength="50" /><br /><asp:Button ID="QuesFields_Save_btn" runat="server" Text="Add Ques" OnClick="QuesFields_Save_clicked" /></asp:TableCell>
                    <asp:TableCell ID="Questions_List_TC" runat="server" Width="70%"><asp:CheckBoxList id="Questions_chkl" Runat="server" RepeatDirection="Horizontal" RepeatColumns="1" DataTextField="QuesField_LDescr" DataValueField="QuesField_Cd"></asp:CheckBoxList></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="30%"><strong>Cell Phone Paging:</strong></asp:TableCell><asp:TableCell Width="70%"><asp:RadioButtonList ID="CellPhoneAllow_chkl" runat="server" RepeatDirection="Horizontal"><asp:ListItem Text="Yes" Value="1" /><asp:ListItem Text="No" Value="0" /></asp:RadioButtonList></asp:TableCell>
            </asp:TableRow>
             <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="30%"><strong>Send SMS with Number of Waiting (<=):</strong></asp:TableCell><asp:TableCell Width="70%"><asp:TextBox ID="CellPhone_WaitNum_txt" runat="server" MaxLength="2" Width="450"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="30%"><strong>Cell Phone Msg:</strong></asp:TableCell><asp:TableCell Width="70%"><asp:TextBox ID="CellPhone_Msg_txt" runat="server" MaxLength="100" Width="450"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="30%"><strong>Thank You Msg:</strong></asp:TableCell><asp:TableCell Width="70%"><asp:TextBox ID="ThankYou_Msg_txt" runat="server" MaxLength="500" Width="450" Rows="3" TextMode="MultiLine"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        *Key Field must be unique        
        <br />        
        <asp:Table ID="Advisor_tbl" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="2">
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell ColumnSpan="2"><h3>Advisor List</h3></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="30%"><strong>Advisor (*Emplid):</strong> [<asp:LinkButton ID="Advisor_lbtn" runat="server" Text="Add" OnClick="Advisor_clicked" />]                  
                </asp:TableCell>
                    <asp:TableCell ID="Advisors_Add_TC" runat="server" Width="70%" Visible="false">Emplid: <asp:Label ID="Advisor_add_err_lbl" runat="server" ForeColor="Maroon" Text="Advsior Emplid must be unique because it is a key field" Visible="false" /><br /><asp:TextBox ID="AdvisorEmplid_Add_txt" runat="server" Width="300" MaxLength="10" /><br />Name:<br /><asp:TextBox ID="AdvisorName_Add_txt" runat="server" Width="300" MaxLength="50" /><br /><asp:Button ID="Advisor_Save_btn" runat="server" Text="Add Advisor" OnClick="Advisor_Save_clicked" /></asp:TableCell>
                    <asp:TableCell ID="Advisors_List_TC" runat="server" Width="70%">
                        <asp:DataList ID="Advisors_DL" runat="server" CellPadding="4" CellSpacing="0" Width="100%" >
                            <HeaderTemplate>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Emplid&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="IsActive_ckb" runat="server" Checked="true" /> <asp:Label ID="AdvisorEmplid_txt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AdvisorEmplid")%>' />&nbsp;&nbsp;&nbsp;<asp:TextBox ID="AdvisorName_txt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AdvisorName")%>' />
                            </ItemTemplate>                           
                        </asp:DataList>
                     </asp:TableCell>                                         
            </asp:TableRow>       
        </asp:Table>
        *Key Field must be unique
        <br />    
        <asp:Table ID="PlanSheets_tbl" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="2">
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell><h3>Planning Sheet</h3></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="30%"><strong>Planning Sheets:</strong> [<asp:LinkButton ID="PlanSheets_Add_lnk" runat="server" Text="Add" OnClick="PlanSheets_Add_clicked" />]</asp:TableCell>
                <asp:TableCell Width="70%">
                    <asp:DataGrid id="PlanSheets_DG" runat="server" CellPadding="4" CellSpacing="0" BorderWidth="1" Width="100%" 
                        DataKeyField="PSId" GridLines="Horizontal" AutoGenerateColumns="false" ShowHeader="false" OnItemCommand="PlanSheets_Click">
                        <Columns>                           
                            <asp:ButtonColumn ButtonType="LinkButton" CommandName="Edit" Text="Edit"></asp:ButtonColumn>
                            <asp:BoundColumn DataField="PSOffice_Cd" />
                            <asp:BoundColumn DataField="PSName" />
                            <asp:BoundColumn DataField="PSCatalog_yr" />
                            <asp:BoundColumn DataField="IsValid" />
                            <asp:ButtonColumn ButtonType="LinkButton" CommandName="Copy" Text="Copy New"></asp:ButtonColumn>
                        </Columns>
                    </asp:DataGrid>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br />
        <asp:Table ID="Email_tbl" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="2">
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell ColumnSpan="2"><h3>Email Options</h3></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="30%"><strong>Intro Paragraph:</strong></asp:TableCell><asp:TableCell Width="70%"><asp:TextBox ID="Email_Intro_txt" runat="server" MaxLength="500" Width="450" Rows="3" TextMode="MultiLine"></asp:TextBox></asp:TableCell>
            </asp:TableRow>                                                  
        </asp:Table>
        <br />
        <asp:Table ID="PreText_tbl" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="2">
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell ColumnSpan="2"><h3>Pre-Defined Text</h3></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">               
                <asp:TableCell Width="30%"><strong>Student Notes:</strong> [<asp:LinkButton ID="PreText_lbtn" runat="server" Text="Add" OnClick="PreText_clicked" />]                  
                </asp:TableCell>
                    <asp:TableCell ID="PreText_Add_TC" runat="server" Width="70%" Visible="false">Text Code: <asp:Label ID="PreText_Add_err_lbl" runat="server" ForeColor="Maroon" Text="Text Code must be unique because it is a key field" Visible="false" /><br /><asp:TextBox ID="Text_Cd_txt" runat="server" Width="300" MaxLength="10" /><br />Text:<br /><asp:TextBox ID="Text_Descr_txt" runat="server" Width="300" MaxLength="300" /><br /><asp:Button ID="PreText_save_btn" runat="server" Text="Add Common Text" OnClick="PreText_Save_clicked" /></asp:TableCell>
                    <asp:TableCell ID="PreText_List_TC" runat="server" Width="70%">
                       <asp:DataGrid id="PreText_DG" runat="server" CellPadding="4" CellSpacing="0" BorderWidth="1" Width="100%" 
                        DataKeyField="Text_Cd" GridLines="Horizontal" AutoGenerateColumns="false" ShowHeader="false" OnItemCommand="PreText_delete_clicked">
                        <Columns>                           
                            <asp:BoundColumn DataField="Text_Cd" />
                            <asp:BoundColumn DataField="Text_Descr" />
                            <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete"></asp:ButtonColumn>
                        </Columns>
                    </asp:DataGrid>
                     </asp:TableCell>
            </asp:TableRow>                                                  
        </asp:Table>
        *Key Field must be unique
        <p align="right"></p>
        <table id="PSItemsGrps_footer" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="0">
                <tr valign="top">
                    <td align="left" width="50%"><asp:button ID="Back_btn" runat="server" Text="Back" OnClick="Back_btn_clicked" /></td>
                    <td align="right" width="50%"><asp:Button ID="Save_btn" runat="server" Text="Save" OnClick="Save_clicked" /></td>
                </tr>
             </table>
        <asp:Label ID="Office_Cd" runat="server" Visible="false"></asp:Label>				
    </div><!-- end  mainhomecontent -->           
    </div><!--main-->
    
    <div id="footer" class ="noprint" style="left: 0px; top: 0px">  <!-- start footer -->
    <div id="footerbtmnav">&nbsp;&nbsp;&nbsp;</div>
    <div id="footeraddress"><!-- start footeraddress -->
            <p><asp:Label ID="Office_Descr1_lbl" runat="server" /> <br />
			  California State University, Chico</p>
			  
			  <p>Phone: 530-898-<asp:Label ID="Office_PhoneExt_lbl" runat="server" /></p>
	  		  
			  <p>Our office is located in <asp:Label ID="Office_Location_lbl" runat="server" /></p>
     </div>  <!-- end footeraddress -->  
     </div>  <!-- end footer  -->
   </form>
		</div><!--wrappter -->
</body>
</html>