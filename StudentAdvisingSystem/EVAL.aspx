<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EVAL.aspx.cs" Inherits="StudentAdvisingSystem.EVAL" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
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
           <h1>Welcome to the Academic Evaluations</h1>
           <h3>Please select&nbsp; Acadmic Evaluations and then login.</h3>
           <br />
           <p align="center">
               &nbsp;</p>
           <p align="center"><asp:ImageButton ID="AAPEVAL_btn" runat="server" BorderWidth="0" ImageUrl="images/evaluations.jpg" CommandName="AAPEVAL"  OnClick="SAS_Clicked" AlternateText="Academic Evaluations" /></p>
           <p align="center">&nbsp;</p>
            <br />
    </div><!-- end  mainhomecontent -->           
     </div><!--main-->
   
    <div id="footer" class ="noprint" style="left: 0px; top: 0px">  <!-- start footer -->
    <div id="footerbtmnav">&nbsp;&nbsp;&nbsp;</div>
    <div id="footeraddress"><!-- start footeraddress -->
            <p>Academic Evaluations<br />
			  California State University, Chico</p>
			  
			  <p>Phone: 530-898-5957</p>
	  		  
			  <p>Our office is located in SSC 110</p>
     </div>  <!-- end footeraddress -->  
     </div>  <!-- end footer  -->
   </form>
		</div><!--wrappter -->
</body>
</html>