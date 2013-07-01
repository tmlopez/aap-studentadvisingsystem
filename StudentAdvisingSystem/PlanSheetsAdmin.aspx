<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlanSheetsAdmin.aspx.cs" Inherits="StudentAdvisingSystem.PlanSheetsAdmin" %>

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
    <form id="PlanSheetsAdmin" runat="server">
    <div id="main"><!--main-->
   
    <div id="mainhomecontent">  <!-- start mainhomecontent -->
        <h1><asp:Label ID="Office_Descr_lbl" runat="server" />- Planning Sheet Admin Tool</h1>
        <p><STRONG><asp:Label ID="CurrentDate_lbl" runat="server"></asp:Label></STRONG></p>
        <asp:Table ID="PlanSheets_tbl" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="2">
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell ColumnSpan="2"><h3>Planning Sheet Info</h3></asp:TableCell>
            </asp:TableRow>            
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="30%"><strong>Name:</strong></asp:TableCell><asp:TableCell Width="70%"><asp:TextBox ID="PSName_txt" runat="server" MaxLength="100" Width="450"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="30%"><strong>Catalog Yr:</strong></asp:TableCell><asp:TableCell Width="70%"><asp:TextBox ID="PSCatalog_yr_txt" runat="server" MaxLength="50" Width="450"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="30%"><strong>Is Active:</strong></asp:TableCell><asp:TableCell Width="70%"><asp:RadioButtonList ID="PSIsValid" runat="server" RepeatDirection="Horizontal"><asp:ListItem Text="Yes" Value="1" Selected="True" /><asp:ListItem Text="No" Value="0" /></asp:RadioButtonList></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="30%"><strong>PDF File Name:</strong></asp:TableCell>
                <asp:TableCell Width="70%"><asp:HyperLink ID="PDF_FilePath_lbl" runat="server" Target="_blank" /><br />
                    <asp:FileUpload ID="PDF_FilePath" runat="server" />&nbsp;<asp:Button id="UploadButton" Text="Upload file" OnClick="UploadButton_Click" runat="server" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="30%"><asp:button ID="SASAdmin_btn" runat="server" Text="SAS Admin" OnClick="SASAdmin_Click" /></asp:TableCell>
                <asp:TableCell HorizontalAlign="Right"><asp:Button ID="Save_PlanSheets" runat="server" Text="Save/Next" OnClick="Save_PlanSheets_Click" /></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:DataList ID="PSGrps_DL" Runat="server" BorderWidth="1" Width="100%" CellPadding="0" cellspacing="0" DataKeyField="PSGroupId" GridLines="Horizontal" Visible="false" OnItemCommand="PSGrps_DL_Click">
        <HeaderTemplate>
            <h3>Planning Sheet Categories</h3>
        </HeaderTemplate>
        <ItemTemplate>
            <table id="PSGrps_tbl" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="2">
            <tr valign="top">
                <td align="left" width="25%"><strong>Seq/Name and<br />Description:</strong></td>
                <td align="left" colspan="2" width="50%"><asp:TextBox ID="PSGroup_SeqNum" runat="server" MaxLength="2" Width="20" Text='<%# DataBinder.Eval(Container.DataItem, "PSGroup_SeqNum")%>' />&nbsp;<asp:DropDownList ID="PSGroup_Descr" runat="server" DataTextField="PSGroup_Descr" DataValueField="PSGroupId" Width="300"  /><asp:TextBox ID="PSGroup_Descr_txt" runat="server" MaxLength="100" Width="290" Visible="false" />&nbsp;[<asp:LinkButton ID="Add_PSGrps_tbl_lbtn" runat="server" Text="+" CommandName="Add_PSGrps_tbl" />]<br /><asp:TextBox ID="PSGroup_SubDescr" runat="server" MaxLength="500"  Width="300" TextMode="MultiLine" Rows="2" Text='<%# DataBinder.Eval(Container.DataItem, "PSGroup_SubDescr")%>' /></td>
                <td valign="middle" align="right" width="25%">[<asp:LinkButton ID="Add_PSGrps" runat="server" Text="Add" CommandName="Add_PSGrps" />]&nbsp;[<asp:LinkButton ID="Delete_PSGrps" runat="server" Text="Delete" CommandName="Delete_PSGrps" />]</td>
            </tr>
            <tr valign="bottom">
                <td align="left" width="25%"><strong>Column Header:</strong>&nbsp;</td>                    
                <td align="left" width="25%"><asp:Textbox ID="PSInput1_descr" runat="server" MaxLength="50" Text='<%# DataBinder.Eval(Container.DataItem, "PSInput1_descr")%>' /></td>
                <td align="left" width="25%"><asp:Textbox ID="PSInput2_descr" runat="server" MaxLength="50" Text='<%# DataBinder.Eval(Container.DataItem, "PSInput2_descr")%>' /></td>
                <td align="left" width="25%"><asp:Textbox ID="PSInput3_descr" runat="server" MaxLength="50" Text='<%# DataBinder.Eval(Container.DataItem, "PSInput3_descr")%>' /></td>
            </tr></table>                          
        </ItemTemplate>
        <FooterTemplate>
            <table id="PSGrps_footer" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="0">
                <tr valign="top">
                    <td align="left" width="50%"><asp:Button ID="PSAdmin_btn" runat="server" Text="Back" CommandName="PSAdmin_btn" /></td>
                    <td align="right" width="50%"><asp:Button ID="Save_PSGrps" runat="server" Text="Save/Next" CommandName="Save_PSGrps" /></td>
                </tr>
            </table>            
        </FooterTemplate>
        </asp:DataList>
        <asp:DataList ID="PSItemsGrps_DL" Runat="server" BorderWidth="1" Width="100%" CellPadding="0" cellspacing="0" DataKeyField="PSItemGroupId" GridLines="Horizontal" Visible="false"  OnItemCommand="PSItems_DL_Click">
        <HeaderTemplate>
            <h3>Planning Sheet Category Items</h3>            
        </HeaderTemplate>
        <ItemTemplate>
            <table id="PSItemsGrps_tbl" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="2">
            <tr valign="top">
                <td align="left" colspan="4"><h4><%# DataBinder.Eval(Container.DataItem, "PSGroup_SeqNum")%>:&nbsp;<%# DataBinder.Eval(Container.DataItem, "PSGroup_Descr")%></h4></td>                
            </tr>
            <tr valign="top">
                <td align="left" width="25%"><strong>Seq/Item:</strong></td>
                <td align="left" colspan="2"><asp:TextBox ID="PSItem_SeqNum" runat="server" MaxLength="2" Width="20" Text='<%# DataBinder.Eval(Container.DataItem, "PSItem_SeqNum")%>' />&nbsp;<asp:DropDownList ID="PSItem_Descr" runat="server" DataTextField="PSItem_Descr" DataValueField="PSItemId" Width="300"  /><asp:TextBox ID="PSItem_Descr_txt" runat="server" MaxLength="500" Width="290" Visible="false" />&nbsp;[<asp:LinkButton ID="Add_PSItems_tbl_lbtn" runat="server" Text="+" CommandName="Add_PSItems_tbl" />]</td>
                <td valign="middle" align="right" width="25%">[<asp:LinkButton ID="Add_PSItems" runat="server" Text="Add" CommandName="Add_PSItems" />]&nbsp;[<asp:LinkButton ID="Delete_PSItems" runat="server" Text="Delete" CommandName="Delete_PSItems" />]</td>
            </tr>
            <tr valign="top">
                <td align="left" width="25%"><strong>Courses:</strong></td>
                <td align="left" colspan="3">
                    <asp:DataList ID="PSItemsCrses_DL" runat="server" BorderWidth="0" CellPadding="0" cellspacing="0" GridLines="Horizontal" OnItemCommand="PSItemsCrses_DL_Click">
                        <ItemTemplate>
                            <asp:Label ID="PSItemGroupId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "PSItemGroupId")%>' />
                            <asp:TextBox ID="Course_Id" runat="server" MaxLength="10" Width="50" Text='<%# DataBinder.Eval(Container.DataItem, "Course_Id")%>' />&nbsp;
                            <asp:TextBox ID="Class_Subject" runat="server" MaxLength="50" Width="50" Text='<%# DataBinder.Eval(Container.DataItem, "Class_Subject")%>' />&nbsp;
                            <asp:TextBox ID="Class_Number" runat="server" MaxLength="50" Width="50" Text='<%# DataBinder.Eval(Container.DataItem, "Class_Number")%>' />&nbsp;
                            [<asp:LinkButton ID="SearchCourse" runat="server" Text="..." CommandName="SearchCourse" />]                            
                        </ItemTemplate>
                    </asp:DataList>
                    [<asp:LinkButton ID="PSItemsCrses_lbtn" runat="server" Text="Add" CommandName="Add_PSItemsCrses"  />]
                </td>
            </tr>
            <tr valign="top">
                <td align="left" width="25%"><strong>Get PS Term/Units/Grade:</strong></td>
                <td align="left" colspan="3"><asp:RadioButtonList ID="GetPSInfo" runat="server" RepeatDirection="Horizontal"><asp:ListItem Text="Yes" Value="1" /><asp:ListItem Text="No" Value="0" Selected="True" /></asp:RadioButtonList></td>
            </tr>            
            <tr valign="top">
                <td align="left" width="25%"><strong>Notes PDF Field:</strong><br />
                    <asp:DropDownList ID="Notes_PDFName" runat="server" DataTextField="PDF_FieldNames" DataValueField="PDF_FieldNames" /></td>
                <td align="left" width="25%"><strong>Input1 PDF Field:</strong><br />
                    <asp:DropDownList ID="Input1_PDFName" runat="server" DataTextField="PDF_FieldNames" DataValueField="PDF_FieldNames" /></td>
                <td align="left" width="25%"><strong>Input2 PDF Field:</strong><br />
                    <asp:DropDownList ID="Input2_PDFName" runat="server" DataTextField="PDF_FieldNames" DataValueField="PDF_FieldNames" /></td>            
                <td align="left" width="25%"><strong>Input3 PDF Field:</strong><br />
                    <asp:DropDownList ID="Input3_PDFName" runat="server" DataTextField="PDF_FieldNames" DataValueField="PDF_FieldNames" /></td>
            </tr>             
            </table>                          
        </ItemTemplate>
        <FooterTemplate>
             <table id="PSItemsGrps_footer" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="0">
                <tr valign="top">
                    <td align="left" width="50%"><asp:Button ID="PSAdmin_btn" runat="server" Text="Back" CommandName="PSAdmin_btn" /></td>
                    <td align="right" width="50%"><asp:Button ID="Save_PSItems" runat="server" Text="Save/Next" CommandName="Save_PSItems" /></td>
                </tr>
             </table>
        </FooterTemplate>
        </asp:DataList>               
    <asp:Label ID="Office_Cd" runat="server" Visible="false"></asp:Label><asp:Label ID="PSId" runat="server" Visible="false"></asp:Label><asp:Label ID="PSGroupId" runat="server" Visible="false"></asp:Label><asp:Label ID="NewRowCnt" runat="server" Visible="false" Text="0"></asp:Label>
    <asp:Literal ID="Literal1" Runat="server"></asp:Literal>
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