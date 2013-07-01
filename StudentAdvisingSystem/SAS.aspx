<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SAS.aspx.cs" Inherits="StudentAdvisingSystem.SAS" %>

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
		<div id="headerchicologo"><div id="headerbanner2"><img src="images/headerLogo.gif" alt="Welcome to California State University, Chico - Today Decides Tomorrow" />
		</div><!--headerbanner-->
	</div><!--headerchicologo-->
</div><!--header-->
    <form id="form1" runat="server">
    <div id="main"><!--main-->
   
    <div id="mainhomecontent">  <!-- start mainhomecontent -->
        <h1><asp:Label ID="Office_Descr_lbl" runat="server" />- Student Advising System</h1> 
        <p><STRONG><asp:Label ID="CurrentDate_lbl" runat="server"></asp:Label></STRONG></p> 
        <asp:Table ID="Student_tbl" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="2">                                          
            <asp:TableRow VerticalAlign="Top"><asp:TableCell><h3>Student System Links</h3></asp:TableCell></asp:TableRow>
            <asp:TableRow VerticalAlign="Top"><asp:TableCell>
                <ul><li><asp:HyperLink ID="SignIn" runat="server" Text="Student Sign In Screen" NavigateUrl="~/SignIn.aspx" /></li>
                <li><asp:HyperLink ID="Screening" runat="server" Text="Front Desk Screening" NavigateUrl="~/Screening.aspx" /></li>
                <li><asp:HyperLink ID="AdvisorScreen" runat="server" Text="Advising Screen" NavigateUrl="~/AdvisorScreen.aspx" /></li>
                <li><asp:HyperLink ID="SignIn_NoScreening" runat="server" Text="Advisor Sign In" NavigateUrl="~/SignIn.aspx?adv=1" /></li>
                </ul>                
            </asp:TableCell></asp:TableRow>
        </asp:Table>
        <br />
        <asp:Table ID="Admin_tbl" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="2">                                          
            <asp:TableRow VerticalAlign="Top"><asp:TableCell ColumnSpan="2"><h3>Admin System Links</h3></asp:TableCell></asp:TableRow>
            <asp:TableRow VerticalAlign="Top"><asp:TableCell Width="30%">
                <ul><li><asp:HyperLink ID="SASAdmin" runat="server" Text="SAS Admin Tool" NavigateUrl="~/SASAdmin.aspx" /></li>
                <li><asp:LinkButton ID="StudentSearch" runat="server" Text="Student Search" OnClick="StudentSearch_click"  /></li>
                <li><asp:HyperLink ID="Reporting" runat="server" Text="SAS Database" Target="_blank" NavigateUrl="https://em2.csuchico.edu:449/" /></li>                
                </ul>                
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top" Width="70%"><asp:Label ID="Student_Search_Title_lbl" runat="server" Text="Student Search:<br />" /><asp:TextBox ID="Student_Search_txt" runat="server" />&nbsp;<asp:Button ID="Student_Search_btn" runat="server" Text="Search" OnClick="Student_Search_btn_click" /><asp:label id="Student_Search_lbl" runat="server" Visible="false" />
                <asp:DataGrid ID="Student_Search_DG" runat="server" CellPadding="4" CellSpacing="0" BorderWidth="1" Width="100%" 
                        DataKeyField="ViewKey" GridLines="Horizontal" AutoGenerateColumns="false" OnItemCommand="Student_Search_View" 
                        ShowHeader="false" AllowPaging="true" PagerStyle-Mode="NextPrev" PagerStyle-HorizontalAlign="Right" PageSize="10" OnPageIndexChanged="Student_Search_Paging">                        
                        <Columns>
                            <asp:ButtonColumn ButtonType="LinkButton" text="View"/>                            
                        </Columns>
                    </asp:DataGrid> 
            </asp:TableCell>               
            </asp:TableRow>
        </asp:Table>
        
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