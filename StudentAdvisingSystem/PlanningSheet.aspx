<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlanningSheet.aspx.cs" Inherits="StudentAdvisingSystem.PlanningSheet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Student Advising System</title>
    <link href="_styles/styles1.css" rel="stylesheet" title="default" type="text/css" media="screen" />
    <link href="_styles/print.css" rel="stylesheet" type="text/css" media="print" />    
</head>
<body>
<div id="wrapper">
<div id="skip"><a href="#content" title="Skip to Content"><img src="/images/dot.gif" alt="Skip to Content" width="1" height="1" /></a></div><!--skip-->
<div id="header" class="noprint">
		<div id="headerchicologo"><div id="headerbanner"><img src="images/headerLogo.gif" alt="Welcome to California State University, Chico - Today Decides Tomorrow" />
		</div><!--headerbanner-->
	</div><!--headerchicologo-->
</div><!--header-->
    <form id="form1" runat="server">
    <div id="main"><!--main-->
   
    <div id="mainhomecontent">  <!-- start mainhomecontent -->
        <h1><asp:Label ID="Office_Descr_lbl" runat="server" />- Planning Sheet</h1> 
        <p><STRONG><asp:Label ID="CurrentDate_lbl" runat="server"></asp:Label></STRONG></p>               
        <table id="dt_tbl" border="0" width="100%" cellpadding="4" cellspacing="0"><tr valign="top">
            <td align="left" width="50%">&nbsp;&nbsp;&nbsp;<asp:Label ID="FirstName" runat="server" Font-Size="Medium" />&nbsp;<asp:Label ID="LastName" runat="server"  Font-Size="Medium" /></td>
            <td align="left"><asp:Label ID="StudentId_lbl" runat="server" Font-Size="Medium" /></td></tr>                      
            <tr valign="bottom"><td align="left"><h2><asp:Label ID="PSName" runat="server" /></h2></td>
            <td align="right">Catalog: <asp:Label ID="PSCatalog_yr" runat="server"></asp:Label></td>
            </tr>
        </table>
        <asp:DataList ID="PSGrps_DL" Runat="server" BorderWidth="1" Width="100%" CellPadding="0" cellspacing="7" DataKeyField="PSGroupId">
            <ItemTemplate>
                <table id="PSGrps_tbl" runat="server" width="100%" border="0" CellPadding="0" cellspacing="0">
                    <tr valign="top"><td align="left" colspan="5"><h3><%# DataBinder.Eval(Container.DataItem, "PSGroup_Descr")%></h3> <%# DataBinder.Eval(Container.DataItem, "PSGroup_SubDescr")%></td></tr>
                    <tr valign="bottom">
                    <td align="left" width="500">&nbsp;</td>
                    
                    <td align="center" width="10%"><%# DataBinder.Eval(Container.DataItem, "PSInput1_descr")%></td>
                    <td align="center" width="10%"><%# DataBinder.Eval(Container.DataItem, "PSInput2_descr")%></td>
                    <td align="center" width="10%"><%# DataBinder.Eval(Container.DataItem, "PSInput3_descr")%></td>
                </tr></table>
                <asp:datalist ID="PSItemsGrps_DL" runat="server" Width="100%" CellPadding="0" cellspacing="7" BorderWidth="1" DataKeyField="PSItemGroupId" AlternatingItemStyle-BackColor="#C5D4E9">                    
                    <ItemTemplate>
                        <table id="PSItemsGrps_tbl" runat="server" width="100%" border="0" CellPadding="0" cellspacing="0"><tr valign="top">
                            <td align="left" width="500"><strong><%# DataBinder.Eval(Container.DataItem, "PSItem_SeqNum")%>. <%# DataBinder.Eval(Container.DataItem, "PSItem_Descr")%>:</strong>&nbsp;&nbsp;&nbsp;<asp:TextBox ID="PSNotes_txt" runat="server" Width="100px" MaxLength="100" /> <br /><asp:CheckBoxList ID="CourseList_chkl" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" /> <asp:Label ID="CourseList_lbl" runat="server" Visible="false" /></td>
                            
                            <td align="right" width="10%"><asp:TextBox ID="PSInput1_txt" runat="server" Width="60px" MaxLength="50"></asp:TextBox></td>
                            <td align="right" width="10%"><asp:TextBox ID="PSInput2_txt" runat="server" Width="60px" MaxLength="50"></asp:TextBox></td>
                            <td align="right" width="10%"><asp:TextBox ID="PSInput3_txt" runat="server" Width="60px" MaxLength="50"></asp:TextBox></td>
                        </tr></table>
                    </ItemTemplate>
                </asp:datalist>                
            </ItemTemplate>
        </asp:DataList>
        <asp:Table ID="save_tbl" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="0">                                          
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="50%"><asp:button ID="StudentInfo_btn" runat="server" Text="Student Info" OnClick="StudentInfo_clicked" /></asp:TableCell>
                <asp:TableCell Width="50%" HorizontalAlign="Right"><asp:button ID="Save_btn" runat="server" Text="Save" OnClick="PS_Save_clicked" />&nbsp;<asp:button ID="SaveNew_btn" runat="server" Text="Save As New" Visible="false" OnClick="PS_SaveNew_clicked" />&nbsp;<asp:Button ID="SaveAsPDF" runat="server" Text="View PDF" OnClick="PS_SavePDF_clicked" />&nbsp;<asp:Button ID="SaveToImageNow" runat="server" Text="Save To ImageNow" Enabled="false" /></asp:TableCell>
           </asp:TableRow>
        </asp:Table>        
        <asp:Label ID="OrgPkId" runat="server" Visible="false"></asp:Label><asp:Label ID="PkId" runat="server" Visible="false"></asp:Label><asp:Label ID="Office_Cd" runat="server" Visible="false"></asp:Label><asp:Label ID="PSId" runat="server" Visible="false"></asp:Label><asp:Label ID="PSStudentId" runat="server" Visible="false"></asp:Label>						
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