<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdvisorComments.aspx.cs" Inherits="StudentAdvisingSystem.AdvisorComments" %>

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
		<div id="headerchicologo"><div id="headerbanner1"><img src="images/headerLogo.gif" alt="Welcome to California State University, Chico - Today Decides Tomorrow" />
		</div><!--headerbanner-->
	</div><!--headerchicologo-->
</div><!--header-->
    <form id="form1" runat="server">
    <div id="main"><!--main-->
   
    <div id="mainhomecontent">  <!-- start mainhomecontent -->
        <h1><asp:Label ID="Office_Descr_lbl" runat="server" />- Advising Comments</h1>                
        <table id="dt_tbl" border="0" width="100%" cellpadding="4" cellspacing="0"><tr valign="top">
            <td align="left" width="50%">&nbsp;&nbsp;&nbsp;<asp:Label ID="FirstName" runat="server" Font-Size="Medium" />&nbsp;<asp:Label ID="LastName" runat="server"  Font-Size="Medium" /></td>
            <td align="left"><asp:Label ID="StudentId_lbl" runat="server" Font-Size="Medium" /></td></tr></table>
        <asp:Table ID="Comments_Add_tbl" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="2">            
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell ColumnSpan="2"><strong>Advisor Comments on <asp:Label ID="Visit_Date" Runat="server" /> <<asp:label ID="AdvisorName" runat="server" /><asp:DropDownList ID="Advisors_ddl" runat="server" DataTextField="AdvisorName" DataValueField="AdvisorEmplid"></asp:DropDownList>>:</strong><br />
                    <asp:textbox ID="Comments" runat="server" Rows="8" Width="100%" TextMode="MultiLine"></asp:textbox>
                </asp:TableCell>                
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell HorizontalAlign="Left" Width="50%"><strong>Office Privacy:</strong> <asp:RadioButtonList id="ExtViewAllow" Runat="server" RepeatDirection="Horizontal">
										<asp:ListItem Text="Yes" Value="0" Selected="True"></asp:ListItem>
										<asp:ListItem Text="No" Value="1"></asp:ListItem>
									</asp:RadioButtonList></asp:TableCell>
                <asp:TableCell HorizontalAlign="right" Width="50%"><asp:Button Runat="server" Text="Save" ID="Comments_Save_btn" OnClick="Comments_Save_Clicked"></asp:Button></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br />            
        <asp:DataList ID="Comments_DL" Runat="server" GridLines="Horizontal" BorderColor="#999999" BorderWidth="1" Width="100%" CellPadding="0" cellspacing="7">
            <ItemTemplate>
                <p><%# DataBinder.Eval(Container.DataItem, "Comments_Dt") %>&nbsp;<<strong><%# DataBinder.Eval(Container.DataItem, "AdvisorName")%></strong> (<%# DataBinder.Eval(Container.DataItem, "Office_Cd")%>)>:<br />
				    <asp:Label ID="Comments_lbl" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Comments")%>'></asp:Label>
				</p>
            </ItemTemplate>
        </asp:DataList>
        <asp:Table ID="save_tbl" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="0">                                          
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="50%"><asp:checkbox id="PrintVersion" Runat="server" Text="Print Version" OnCheckedChanged="PrintVersion_Checked" AutoPostBack="true"></asp:checkbox></asp:TableCell>
                <asp:TableCell Width="50%" HorizontalAlign="Right"><asp:button ID="StudentInfo_btn" runat="server" Text="Student Info" OnClick="StudentInfo_Clicked" />&nbsp;<asp:Button ID="Back" runat="server" Text="Advising Screen" OnClick="Back_AdvisorScreen" /></asp:TableCell>
           </asp:TableRow>
        </asp:Table>        
        <asp:Label ID="PkId" runat="server" Visible="false"></asp:Label><asp:Label ID="Office_Cd" runat="server" Visible="false"></asp:Label>						
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