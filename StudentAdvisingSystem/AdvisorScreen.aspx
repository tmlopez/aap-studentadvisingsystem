<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdvisorScreen.aspx.cs" Inherits="StudentAdvisingSystem.AdvisorScreen" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
   <title>Student Advising System</title>
    <META HTTP-EQUIV="Refresh" CONTENT="45">
	<META HTTP-EQUIV="Expires" CONTENT="<% = DateTime.Now.ToString() %>">
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
        <h1><asp:Label ID="Office_Descr_lbl" runat="server" />- Advising Screen</h1>
        <table id="dt_tbl" border="0" width="100%" cellpadding="0" cellspacing="0"><tr valign="top"><td align="left"><STRONG><asp:Label ID="CurrentDate_lbl" runat="server"></asp:Label></STRONG></td><td align="right">Number of Student(s) Waiting: <strong><asp:Label ID="StudWaiting" Runat="server"></asp:Label></strong></td></tr></table>
        <p>&nbsp;</p>
        <asp:DataList ID="AdvisorScreen_DL" Runat="server" AlternatingItemStyle-BorderColor="#999999" AlternatingItemStyle-BorderWidth="1"
		    AlternatingItemStyle-BorderStyle="Solid" CellPadding="3" CellSpacing="0" DataKeyField="PkId" Width="100%"
		    OnItemCommand="SeeStudent_AdvisorScreen" OnEditCommand="Edit_AdvisorScreen" OnCancelCommand="Cancel_AdvisorScreen" OnUpdateCommand="Update_AdvisorScreen" OnDeleteCommand="Delete_AdvisorScreen">
        <HeaderTemplate>
            <table cellpadding="0" cellspacing="0" width="100%" border="0">
			    <tr valign="top">
				<td width="15%"><strong><u>Time</u></strong></td>
				<td width="20%"><strong><u>Student Name</u></strong></td>											
                <td width="25%"><strong><u>Questions Concern</u></strong></td>
				<td width="30%"><strong><u>Office Status</u></strong></td>
                <td width="10%">&nbsp;</td>											
                </tr>
			</table>
		</HeaderTemplate>
		<ItemTemplate>
			<table cellpadding="0" cellspacing="0" width="100%" border="0">
		    	<tr valign="top">
				<td width="15%"><%# DataBinder.Eval(Container.DataItem, "Visit_DateTime", "{0:T}") %></td>
                <td width="20%"><asp:linkbutton ID="StudentInfo" CommandName="Select" runat="server" Font-Bold="true"><%# DataBinder.Eval(Container.DataItem, "FirstName") %>&nbsp;<%# DataBinder.Eval(Container.DataItem, "LastName")%></asp:linkbutton><br /><%# DataBinder.Eval(Container.DataItem, "StudentId")%></td>
				<td width="25%"><%# DataBinder.Eval(Container.DataItem, "Ques")%></td>				
				<td width="30%"><asp:label ID="Office_Status_lbl" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Office_Status")%>' />: <%# DataBinder.Eval(Container.DataItem, "AdvisorName")%></td>				
				<td width="10%">[<asp:linkButton ID="Edit_btn" CommandName="Edit" Text="Edit" runat="server" />]</td>
				</tr>
			 </table>
		</ItemTemplate>		
		<EditItemTemplate>
		    <table cellpadding="0" cellspacing="0" width="100%" border="0">
		    	<tr valign="top">
				<td width="15%"><%# DataBinder.Eval(Container.DataItem, "Visit_DateTime", "{0:T}") %></td>
                <td width="20%"><strong><%# DataBinder.Eval(Container.DataItem, "FirstName") %>&nbsp;<%# DataBinder.Eval(Container.DataItem, "LastName")%></strong><br /><asp:textbox ID="StudentId_txt" runat="server" MaxLength="9" Width="70px" Text='<%# DataBinder.Eval(Container.DataItem, "StudentId")%>'></asp:textbox></td>
				<td width="25%"><asp:CheckBoxList id="Questions_chkl" Runat="server" RepeatDirection="Horizontal" RepeatColumns="1" DataTextField="QuesField_Descr" DataValueField="QuesField_Descr"></asp:CheckBoxList>
					&nbsp;<asp:CheckBox id="Questions_Other_chk" runat="server" Text="Other"></asp:CheckBox>&nbsp;
					<asp:TextBox id="Questions_Other_txt" runat="server" Width="130px"></asp:TextBox></td>				
				<td width="30%"><asp:DropDownList ID="Office_Status" Runat="server" DataTextField="Office_Status" DataValueField="Office_Status_Value"></asp:DropDownList></td>				
				<td width="10%">[<asp:linkButton ID="Update_btn" CommandName="Update" Text="Update" Runat="server" />]<br />[<asp:linkButton ID="Delete_btn" CommandName="Delete" Text="Delete" runat="server" />]<br />[<asp:linkButton ID="Cancel" CommandName="Cancel" Text="Cancel" runat="server" />]</td>
				</tr>
			 </table>
		</EditItemTemplate>
        </asp:DataList>
        <asp:Label ID="Office_Cd" runat="server" Visible="false"></asp:Label>
        
        <script type="text/javascript" language="javascript">
        var cX = 0;
        var cY = 0;
        var rX = 0;
        var rY = 0;
        
        function AssignPosition(d) {
            cX = event.x; 
            cY = event.y;
	        d.style.left = (cX+10) + "px";
	        d.style.top = (cY+10) + "px"; }
        
        function HideContent(d) {
	        if(d.length < 1) { return; }
	        document.getElementById(d).style.display = "none";}
        
        function ShowContent(d) {
	        if(d.length < 1) {return; }
	        var dd = document.getElementById(d);
	        AssignPosition(dd);
	        dd.style.display = "block";}
        
        function ReverseContentDisplay(d) {
	        if(d.length < 1) {return; }
	        var dd = document.getElementById(d);AssignPosition(dd);
	        if(dd.style.display == "none") { dd.style.display = "block"; }
	        else { dd.style.display = "none"; }}
    </script>
        <asp:Literal ID="Literal1" Runat="server"></asp:Literal>
        <p>&nbsp;</p>
        <p>&nbsp;</p>
        <p>&nbsp;</p>
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