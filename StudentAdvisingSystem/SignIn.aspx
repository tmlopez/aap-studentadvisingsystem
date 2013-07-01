<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="StudentAdvisingSystem._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Student Advising System</title>
    <link href="_styles/styles1.css" rel="stylesheet" title="default" type="text/css" media="screen" />
    <link href="_styles/print.css" rel="stylesheet" type="text/css" media="print" />

    <script language="javascript">
		function Disable_Right_Mouse(){
	    	if (event.button == 2){
		    	oncontextmenu= 'retrun false';
			    }
			}
			document.onmousedown = Disable_Right_Mouse;
	</script>   
</head>
<body id="homepage" oncontextmenu="return false" onselectstart="return false" ondragstart="return false">
<div id="wrapper">
<div id="skip"><a href="#content" title="Skip to Content"><img src="/images/dot.gif" alt="Skip to Content" width="1" height="1" /></a></div><!--skip-->
<div id="header" class="noprint">
		<div id="headerchicologo"><div id="headerbanner"><img src="images/headerLogo.gif" alt="Welcome to California State University, Chico - Today Decides Tomorrow" />
		</div><!--headerbanner-->
	</div><!--headerchicologo-->
</div><!--header-->

    <form id="SignInSheet" runat="server">
    <div id="main"><!--main-->
   
      <div id="mainhomecontent">  <!-- start mainhomecontent -->
    <div id="SignIn_div" runat="server">
        <h1>Welcome to the <asp:Label ID="Office_Descr_lbl" runat="server" /></h1>        
        <table id="SignIn_tbl" runat="server" cellspacing="7" cellpadding="0" width="100%" border="0">
            <tr valign="top">
                <td><h3><asp:Label ID="Advisor_Type_lbl" runat="server" /></h3></td>
            </tr>
            <tr valign="top">
                <td><strong>Username:</strong><br />
                    <asp:TextBox id="userName_txt" Runat="server"></asp:TextBox>
					<asp:RequiredFieldValidator id="Requiredfieldvalidator" Runat="server" ControlToValidate="userName_txt"
						Display="Dynamic" ForeColor="#990000" ErrorMessage="(Please Enter Your Portal Username)" SetFocusOnError="true"></asp:RequiredFieldValidator>
				</TD>
			</TR>
			<TR vAlign="top">
				<TD><strong>Password:</strong><BR />
					<asp:TextBox id="Pwd_txt" Runat="server" TextMode="Password"></asp:TextBox>
					<asp:RequiredFieldValidator id="Requiredfieldvalidator1" Runat="server" ControlToValidate="Pwd_txt"
						Display="Dynamic" ForeColor="#990000" ErrorMessage="(Please Enter Your Portal Password)"></asp:RequiredFieldValidator>
				</TD>
		    </TR>
			<TR vAlign="top">
				<TD>
					<asp:Button id="SignIn_btn" Runat="server" Text="Sign In" OnClick="SignIn"></asp:Button>&nbsp;<asp:Button ID="SASBack_btn" runat="server" Text="Back" OnClick="SplashScreen_clicked" CausesValidation="false" /></TD>
			</TR>
			<TR vAlign="top">
				<TD align="center">If you are a prospective student or having trouble with your Portal sign in,
						<asp:LinkButton id="SignInMethod_Other_lbtn" Runat="server" text="Click here" CausesValidation="False" OnClick="SignInMethod"></asp:LinkButton>
				</TD>
			</TR>            
        </table>
        <table id="OtherSignIn_tbl" runat="server" cellspacing="7" cellpadding="0" width="100%" border="0" visible="false">
            <tr valign="top">
                <td><h3><asp:Label ID="Advisor_Type1_lbl" runat="server" /></h3></td>
            </tr>
            <tr valign="top">
                <td><strong>First Name:</strong><br />
                    <asp:TextBox id="FirstName_txt" Runat="server"></asp:TextBox>
					<asp:RequiredFieldValidator id="Requiredfieldvalidator2" Runat="server" ControlToValidate="FirstName_txt"
						Display="Dynamic" ForeColor="#990000" ErrorMessage="(Please Enter Your First Name)" SetFocusOnError="true"></asp:RequiredFieldValidator>
				</TD>
			</TR>
			<TR vAlign="top">
				<TD><strong>Last Name:</strong><BR />
					<asp:TextBox id="LastName_txt" Runat="server"></asp:TextBox>
					<asp:RequiredFieldValidator id="Requiredfieldvalidator3" Runat="server" ControlToValidate="LastName_txt"
						Display="Dynamic" ForeColor="#990000" ErrorMessage="(Please Enter Your Last Name)"></asp:RequiredFieldValidator>
				</TD>
		    </TR>
		    <TR vAlign="top">
				<TD><strong><asp:Label ID="Last4SSN_lbl" runat="server" Text="Social Security Number (only last 4 digits)" />:</strong><BR />
					<asp:TextBox id="Last4SSN_txt" Runat="server" TextMode="Password" MaxLength="4"></asp:TextBox>
					<asp:RequiredFieldValidator id="Requiredfieldvalidator4" Runat="server" ControlToValidate="Last4SSN_txt"
						Display="Dynamic" ForeColor="#990000" ErrorMessage="(Please Enter The Last 4 Digits of Your SSN)"></asp:RequiredFieldValidator>
				</TD>
		    </TR>
		    <tr valign="top">
		        <td><asp:DataGrid ID="StudentSearch_DG" runat="server" CellPadding="4" CellSpacing="0" BorderWidth="1" Width="100%" 
                        DataKeyField="Emplid" GridLines="Horizontal" AutoGenerateColumns="false" OnItemCommand="StudentSearch_Selected">
		            <Columns>
		                <asp:ButtonColumn ButtonType="LinkButton" CommandName="Edit" DataTextField="Emplid" HeaderText="ID" />
		                <asp:BoundColumn DataField="name_preferred" HeaderText="Name"  />
		                <asp:BoundColumn DataField="gender_desc" HeaderText="Gender" />
		                <asp:BoundColumn DataField="birthdate" HeaderText="Date of Birth" />
		                <asp:BoundColumn DataField="national_id" HeaderText="National ID" />
		            </Columns>		            
		        </asp:DataGrid> </td>
		    </tr>
			<TR vAlign="top">
				<TD>
					<asp:Button id="OtherSignIn_btn" Runat="server" Text="Sign In" OnClick="SignIn"></asp:Button>&nbsp;<asp:Button ID="SASBack1_btn" runat="server" Text="Back" OnClick="SplashScreen_clicked" CausesValidation="false" /></TD>
			</TR>
			<TR vAlign="top">
				<TD align="center">If you are a current student,
						<asp:LinkButton id="SignInMethod_Portal_lbtn" Runat="server" text="Click here" CausesValidation="False" OnClick="SignInMethod"></asp:LinkButton>
				</TD>
			</TR>            
        </table>
        <asp:Label ID="Emplid" runat="server" Visible="false"></asp:Label>        
    </div>
    <div id="StudentQuestions_div" runat="server" visible="false">
        <asp:Table ID="StudentQuestions_tbl" runat="server" cellSpacing="15" cellPadding="0" width="100%" border="0">
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell ColumnSpan="2"><h3>Hi <asp:Label ID="First_Name_Preferred_lbl" runat="server" Font-Bold="true" />, please 
						let us know your current contact information, and select your questions 
						concern below; then click on the <STRONG>OK</STRONG> button to continue.</h3></asp:TableCell>						
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="50%"><strong>Phone Number:</strong>
                <asp:RequiredFieldValidator id="Requiredfieldvalidator5" Runat="server" ControlToValidate="Phone3"
						Display="Dynamic" ForeColor="#990000" ErrorMessage="<br/>(Please Enter Your Phone Number)"></asp:RequiredFieldValidator>
                </asp:TableCell>
                <asp:TableCell Width="50%">
                    <INPUT id="Phone1" onkeyup="var n=this.value.length;if (n == 3){SignInSheet.Phone2.focus();}"
					    type="text" maxLength="3" size="3" name="Phone1" runat="server"> - <INPUT id="Phone2" onkeyup="var n=this.value.length;if (n == 3){SignInSheet.Phone3.focus();}"
						type="text" maxLength="3" size="3" name="Phone2" runat="server"> - <INPUT id="Phone3" type="text" maxLength="4" size="4" name="Phone3" runat="server">					
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="50%"><strong>E-mail Address:</strong>                    
                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator11" Runat="server" ForeColor="#990000" ControlToValidate="Email_Address_txt" Display="Dynamic" ErrorMessage="<br />(Please Enter Your E-mail Address)"></asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator ID="Requiredfieldvalidator12" Runat="server" ForeColor="#990000" ControlToValidate="Email_Address_txt" Display="Dynamic" ErrorMessage="<br />(Please Enter Your E-mail Address)" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                </asp:TableCell>
                <asp:TableCell Width="50%">
                    <asp:TextBox ID="Email_Address_txt" Runat="server"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="50%"><strong>Current Academic Plan/Major:</strong>                  
					<asp:RequiredFieldValidator id="Requiredfieldvalidator6" Runat="server" ControlToValidate="Majors_ddl"
						Display="Dynamic" ForeColor="#990000" ErrorMessage="<br />(Please Select Your Academic Plan)"></asp:RequiredFieldValidator>
                </asp:TableCell>
                <asp:TableCell Width="50%"><asp:DropDownList id="Majors_ddl" Runat="server" DataTextField="Major_Description" DataValueField="Major"></asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="50%"><strong>Minor(s) or Second Major:</strong></asp:TableCell>
                <asp:TableCell Width="50%"><asp:TextBox id="Minor_txt" Runat="server" AutoPostBack="true"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell ColumnSpan="2"><strong>Questions Concern:</strong><br />         
                <asp:CheckBoxList id="Questions_chkl" Runat="server" RepeatDirection="Horizontal" RepeatColumns="3" DataTextField="QuesField_Descr" DataValueField="QuesField_Descr"></asp:CheckBoxList>
					<asp:CheckBox id="Questions_Other_chk" runat="server" Text="Other"></asp:CheckBox>&nbsp;
					<asp:TextBox id="Questions_Other_txt" runat="server"></asp:TextBox>
			    </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top" ID="CELLPHONE" runat="server">
                <asp:TableCell Width="50%"><strong>Cell Phone Number and Service Provider (optional):</strong><br />                
                <font Color="#375288">We can send you a text message on your cell phone to notify your session with <asp:Label ID="Advisor_Type2_lbl" runat="server" /> is coming up shortly,
                    but if you are not present in the office when your name is called, we have the rights to cancel your request. Text message charges
                    may be applied.</font>
                </asp:TableCell>
                <asp:TableCell Width="50%"><INPUT id="CellPhone1" onkeyup="var n=this.value.length;if (n == 3){SignInSheet.CellPhone2.focus();}"
	                    type="text" maxLength="3" size="3" name="CellPhone1" runat="server"> - <INPUT id="CellPhone2" onkeyup="var n=this.value.length;if (n == 3){SignInSheet.CellPhone3.focus();}"	                    
	                    type="text" maxLength="3" size="3" name="CellPhone2" runat="server"> - <INPUT id="CellPhone3" onkeyup="var n=this.value.length;if (n == 4){SignInSheet.CellPhone_Provider_ddl.disabled=false;SignInSheet.CellPhone_Provider_ddl.focus();} else {SignInSheet.CellPhone_Provider_ddl.disabled=true;}" type="text" maxLength="4" size="4" name="CellPhone3" runat="server">&nbsp;@<br /><asp:DropDownList ID="CellPhone_Provider_ddl" runat="server" DataTextField="Name" DataValueField="EmailDomain" Enabled="false"></asp:DropDownList>	                    
	                    <asp:RequiredFieldValidator id="Requiredfieldvalidator7" Runat="server" ControlToValidate="CellPhone_Provider_ddl" Enabled=false Display="Dynamic" ForeColor="#990000" ErrorMessage="<br />(Please Select Your Cell Phone Service Provider)" SetFocusOnError="true"></asp:RequiredFieldValidator>
	            </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell ColumnSpan="2">
                    <asp:Button ID="Ok_btn" runat="server" Text="OK" OnClick="Ok_Cliked" />                    
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    <div id="FinalSignIn_div" runat="server" visible="false">
        <h3>Thank You <asp:Label ID="First_Name_Preferred1_lbl" runat="server" Font-Bold="true" />, </h3>
        <p><asp:Label ID="CellPhone_Msg_lbl" runat="server" /> <asp:label ID="ThankYou_Msg_lbl" runat="server" /></p>
        
        <asp:Table ID="save_tbl" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="0">                                          
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="100%" HorizontalAlign="Left"><asp:Button ID="FinalOK_btn" runat="server" Text="OK" OnClick="FinalOk_Clicked" />&nbsp;<asp:Button ID="Back_btn" runat="server" Text="Back" OnClick="Back_Clicked" /></asp:TableCell>                
            </asp:TableRow>
        </asp:Table>                   
    </div>
    
     <asp:Label ID="Office_Cd" runat="server" Visible="false"></asp:Label>
     <asp:Label ID="adv" runat="server" Visible="false" Text="0"></asp:Label>
      <asp:Label ID="pkID" runat="server" Visible="false"></asp:Label>
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
