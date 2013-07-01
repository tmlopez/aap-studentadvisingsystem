<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentInfo.aspx.cs" Inherits="StudentAdvisingSystem.StudentInfo" ValidateRequest="false" %>

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
        <h1><asp:Label ID="Office_Descr_lbl" runat="server" />- Student Information</h1>
        <p><STRONG><asp:Label ID="CurrentDate_lbl" runat="server"></asp:Label></STRONG></p>
        <table id="dt_tbl" border="0" width="100%" cellpadding="4" cellspacing="0"><tr valign="top">
            <td align="left" width="50%">&nbsp;&nbsp;&nbsp;<asp:Label ID="FirstName" runat="server" Font-Size="Medium" />&nbsp;<asp:Label ID="LastName" runat="server"  Font-Size="Medium" /></td>
            <td align="left"><asp:Label ID="StudentId_lbl" runat="server" Font-Size="Medium" /><asp:textbox ID="StudentId_txt" runat="server" MaxLength="9" Visible="false" />&nbsp;&nbsp;&nbsp;[<asp:LinkButton ID="StudentId_lbtn" runat="server" Text="Edit" OnClick="StudentId_Edit" />]</td></tr></table>       
        <asp:Table ID="StudentInfo_tbl" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="2">            
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="20%"><strong>Phone Number:</strong></asp:TableCell><asp:TableCell Width="30%"><asp:Label ID="Phone_PS" runat="server"/><asp:Label ID="Phone" runat="server" BackColor="#C5D4E9" /></asp:TableCell>
                <asp:TableCell Width="20%"><strong>E-mail:</strong></asp:TableCell><asp:TableCell Width="30%"><asp:Label ID="Email_Address_PS" runat="server"/><asp:Label ID="Email_Address" runat="server"  BackColor="#C5D4E9" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="20%"><strong>Career/Acad Prog:</strong></asp:TableCell><asp:TableCell Width="30%"><asp:Label ID="Career" runat="server" /> / <asp:Label ID="Acad_Program" runat="server" /></asp:TableCell>
                <asp:TableCell Width="20%"><strong>Enr Status/Acad Lvl:</strong></asp:TableCell><asp:TableCell Width="30%"><asp:Label ID="Enr_Status" runat="server" /> / <asp:Label ID="Academic_Level" runat="server" /></asp:TableCell>
            </asp:TableRow>
             <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="20%"><strong>Academic Plan:</strong></asp:TableCell><asp:TableCell Width="30%"><asp:Label ID="Major_PS" runat="server" /><asp:label ID="Major" runat="server" BackColor="#C5D4E9"/></asp:TableCell>
                <asp:TableCell Width="20%"><strong>Minor(s)/2nd Major:</strong></asp:TableCell><asp:TableCell Width="30%"><asp:Label ID="Minor" runat="server" Text="N/A" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top" ID="Acad_Standing_TR" runat="server">
                <asp:TableCell Width="20%"><strong>Acad Standing:</strong></asp:TableCell><asp:TableCell Width="30%"><asp:Label ID="Acad_Standing" runat="server"  Text="N/A" /></asp:TableCell>
                <asp:TableCell Width="20%"><strong>Service Indicator(-):</strong></asp:TableCell><asp:TableCell Width="30%"><asp:Label ID="Neg_StdntService_Ind" runat="server" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top" ID="OLLE_TR" runat="server">
                <asp:TableCell ColumnSpan="2" Width="50%"><strong>OLLE Student Group:</strong>&nbsp;<asp:Label ID="OLLE_Student_Group" runat="server" Text="N/A" /><br />
                <strong>OLLE Status:</strong>&nbsp;<asp:Label ID="OLLE_Status" runat="server" /></asp:TableCell>
                <asp:TableCell ColumnSpan="2" Width="50%"><strong>"Back on Track" Wrkshop:</strong>&nbsp;<asp:Label ID="BkOnTrack_Wkshop" runat="server"  Text="N/A" /></asp:TableCell>
                
            </asp:TableRow>     
            <asp:TableRow><asp:TableCell Width="30%"><strong>DQ What's Next Wkshp:</strong></asp:TableCell><asp:TableCell Width="30%"><asp:Label ID="WhatsNext_Wkshop" runat="server"  Text="N/A" /></asp:TableCell></asp:TableRow>                              
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell ColumnSpan="4"><strong>Questions Concern:</strong><br />
                    <table id="Ques_tbl" width="100%" CellPadding="4" CellSpacing="0" border="1"><tr valign="top"><td><asp:CheckBoxList id="Questions_chkl" Runat="server" RepeatDirection="Horizontal" RepeatColumns="3" DataTextField="QuesField_Descr" DataValueField="QuesField_Descr"></asp:CheckBoxList>
					&nbsp;<asp:CheckBox id="Questions_Other_chk" runat="server" Text="Other"></asp:CheckBox>&nbsp;
					<asp:TextBox id="Questions_Other_txt" runat="server" Width="130px"></asp:TextBox></td></tr></table></asp:TableCell>
            </asp:TableRow>            
            <asp:TableRow VerticalAlign="Top" id="Acad_Statistics_TR" runat="server">
                <asp:TableCell ColumnSpan="4"><strong>Academic Statistics:</strong><br />
                    <asp:DataGrid ID="Acad_Statistics_DG" runat="server" CellPadding="4" CellSpacing="0" BorderWidth="1" Width="100%" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundColumn DataField="Statistic_Type" ItemStyle-Font-Bold="true"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Units_Taken" HeaderText="Units Taken" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:F3}"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Units_Passed" HeaderText="Units Passed" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:F3}"></asp:BoundColumn>
                            <asp:BoundColumn DataField="GPA" HeaderText="GPA" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:F3}"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Grade_Points" HeaderText="Grade Points" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:F3}"></asp:BoundColumn>
                            <asp:BoundColumn DataField="GP_Variance" HeaderText="G.P. Variance" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:F3}"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                    *Current Term Status
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br />
        <asp:Table ID="SAS_Students_tbl" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="2">                        
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="20%"><strong>Advisor Name:</strong></asp:TableCell>
                <asp:TableCell Width="30%"><asp:DropDownList ID="Advisors_ddl" runat="server" DataTextField="AdvisorName" DataValueField="AdvisorEmplid" AutoPostBack="true" OnSelectedIndexChanged="Advisors_changed"></asp:DropDownList></asp:TableCell>
                <asp:TableCell Width="50%"><asp:CheckBox ID="StudentAdvised_chk" runat="server" Text="Office Status: Completed" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="20%"><strong>Time Spent (Min):</strong></asp:TableCell>
                <asp:TableCell ColumnSpan="2"><asp:dropdownlist ID="TimeSpent_ddl" runat="server" DataTextField="Timer" DataValueField="Timer" AutoPostBack="true" OnSelectedIndexChanged="TimeSpent_changed"></asp:dropdownlist></asp:TableCell>
                
            </asp:TableRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell ColumnSpan="3">
                    <asp:Table ID="Notes_tbl" runat="server" CellPadding="0" CellSpacing="0" Width="98%" BorderWidth="0">
                        <asp:TableRow VerticalAlign="Top">
                            <asp:TableCell Width="50%" HorizontalAlign="Left"><strong>Notes to Student on <asp:Label ID="Visit_Date" Runat="server" />:</strong><br /></asp:TableCell>
                            <asp:TableCell Width="50%" HorizontalAlign="Right"><asp:Label ID="PreText_lbl" Runat="server" Text="<strong>Pre-Defined Text:</strong> " /><asp:DropDownList ID="PreText_ddl" runat="server" DataTextField="Text_Cd" DataValueField="Text_Descr" />&nbsp;[<asp:linkbutton ID="PreText_lbtn" runat="server" Text="Add" OnClick="PreText_clicked" />]</asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow VerticalAlign="Top">
                            <asp:TableCell ColumnSpan="2"><asp:textbox ID="Notes" runat="server" Rows="8" Width="98%" TextMode="MultiLine"></asp:textbox><asp:Label ID="Notes_lbl" runat="server" /></asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>    
                </asp:TableCell>                
            </asp:TableRow>
        </asp:Table>
        <br />
        <asp:Table ID="Tools_tbl" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="2">                                          
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="25%"><strong>Tools:</strong><br />
                               [<asp:linkbutton ID="PreviousAdvising_lbtn" runat="server" Text="Prev Notes to Student" OnClick="PreviousAdvising_clicked" />]<br />
                               [<asp:LinkButton ID="AdvisorComments_lbtn" runat="server" Text="Advisor Comments" OnClick="AdvisorComments_clicked" />]<br />
                               [<asp:linkbutton ID="PlanningSheet_lbtn" runat="server" Text="Planning Sheets" OnClick="PlanningSheet_clicked" />]<br />                               
                               [<asp:LinkButton ID="EmailStudent_lbtn" runat="server" Text="Email Student" OnClick="EmailStudent_clicked" />]<br />                               
                               [<asp:HyperLink ID="AAPForm_lbtn" runat="server" Text="AAP Forms" NavigateUrl="http://em.csuchico.edu/aap/Undergrad/forms/" Target="_blank" />]
                </asp:TableCell>             
                <asp:TableCell Width="85%" ID="Tools_Results_TC" runat="server" Visible="false"><asp:Label ID="Tools_Results_lbl" runat="server" />:&nbsp;<asp:DropDownList ID="Tools_Results_ddl" runat="server" Visible="false" />&nbsp;[<asp:LinkButton ID="Tools_Results_add_lbtn" runat="server" Text="Add" OnClick="Tools_Results_add_clicked" />]<br /><br />
                    <asp:DataGrid ID="Tools_Results_DG" runat="server" CellPadding="4" CellSpacing="0" BorderWidth="1" Width="100%" 
                        DataKeyField="PkId" GridLines="Horizontal" AutoGenerateColumns="false" OnItemCommand="Tools_Results_View" 
                        ShowHeader="false" AllowPaging="true" PagerStyle-Mode="NextPrev" PagerStyle-HorizontalAlign="Right" PageSize="10" OnPageIndexChanged="Tools_Results_Paging">                        
                        <Columns>
                            <asp:ButtonColumn ButtonType="LinkButton" text="View"/>
                        </Columns>
                    </asp:DataGrid>                    
                </asp:TableCell>
                 <asp:TableCell Width="80%" ID="EmailStudent_TC" runat="server" Visible="false">Compose Email:<br /><br />                 
                    <asp:Table ID="EmailStudent_tbl" runat="server" CellPadding="4" CellSpacing="0" Width="100%" BorderWidth="0">                                          
                        <asp:TableRow VerticalAlign="Top">
                            <asp:TableCell Width="20%">To:</asp:TableCell><asp:TableCell Width="80%"><asp:TextBox id="Email_To_txt" runat="server" Width="300" /></asp:TableCell>
                         </asp:TableRow>
                         <asp:TableRow VerticalAlign="Top">
                            <asp:TableCell Width="20%">Cc:</asp:TableCell><asp:TableCell Width="80%"><asp:TextBox id="Email_Cc_txt" runat="server" Width="300" /></asp:TableCell>
                         </asp:TableRow>
                         <asp:TableRow VerticalAlign="Top">
                            <asp:TableCell Width="20%">Subj:</asp:TableCell><asp:TableCell Width="80%"><asp:TextBox id="Email_Subj_txt" runat="server" Width="300" /></asp:TableCell>
                         </asp:TableRow>
                         <asp:TableRow VerticalAlign="Top">
                           <asp:TableCell Width="20%">Attach:</asp:TableCell><asp:TableCell Width="80%"><asp:CheckBox ID="Email_PlanSheets" runat="server" Text="Session Planning Sheet(s)" Checked="true" /></asp:TableCell>
                          </asp:TableRow>
                          <asp:TableRow VerticalAlign="Top">
                            <asp:TableCell Width="20%">Body:</asp:TableCell><asp:TableCell Width="80%"><asp:CheckBox ID="Email_StdntNotes" runat="server" Text="Session Notes" Checked="true" /></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow VerticalAlign="Top">
                            <asp:TableCell ColumnSpan="2"><asp:Button ID="Email_Send_btn" runat="server" Text="Send Email" OnClick="Email_Send_clicked" /></asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>                   
                </asp:TableCell>
            </asp:TableRow>
          </asp:Table> 
             <br />
            <asp:Table ID="save_tbl" runat="server" CellPadding="7" CellSpacing="0" Width="100%" BorderWidth="2">                                          
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell Width="80%"><asp:Label ID="msg" runat="server" ForeColor="Red" Font-Bold="true" Text="Warning: You have unsaved data on this page.  Click Save, or click on your selection again to continue." Visible="false"></asp:Label></asp:TableCell>
                <asp:TableCell Width="20%" HorizontalAlign="Right"><asp:button ID="StudentAdvised_btn" runat="server" Text="Save" OnClick="StudentAdvised_Save" />&nbsp;<asp:Button ID="Back" runat="server" Text="Advising Screen" OnClick="Back_AdvisorScreen" /></asp:TableCell>
            </asp:TableRow>
            </asp:Table>
                 
        <asp:Label ID="Office_Cd" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="PkId" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="Save" runat="server" Visible="false"></asp:Label>							
        <asp:Label ID="Visit_DateTime" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="Advisor_Type" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="Office_Email_Address" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="Email_Intro" runat="server" Visible="false"></asp:Label>
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
