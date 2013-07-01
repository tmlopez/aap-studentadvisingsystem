<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlanSheetsAdmin_Crses.aspx.cs" Inherits="StudentAdvisingSystem.PlanSheetsAdmin_Crses" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Student Advising System- Admin</title>
    <link href="_styles/styles1.css" rel="stylesheet" title="default" type="text/css" media="screen" />
    <link href="_styles/print.css" rel="stylesheet" type="text/css" media="print" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>Planning Sheet Course Search</h2>
        <br />
        <asp:DataGrid ID="Course_Id_DG" runat="server" BorderWidth="2" Width="95%" CellPadding="0" cellspacing="0" DataKeyField="Course_Id" BackColor="White" GridLines="Horizontal" AutoGenerateColumns="false" OnItemCommand="Course_Id_Selected">
        <HeaderStyle Font-Bold="True" ForeColor="#FFFFFF" BackColor="Maroon"></HeaderStyle>
        <Columns>
		    <asp:ButtonColumn ButtonType="LinkButton" CommandName="Select" Text="Select"></asp:ButtonColumn>
		    <asp:BoundColumn DataField="Course_Id" HeaderText="Course Id"></asp:BoundColumn>
		    <asp:BoundColumn DataField="Class_Subject" HeaderText="Subject"></asp:BoundColumn>
		    <asp:BoundColumn DataField="Class_Number" HeaderText="Number"></asp:BoundColumn>
		    <asp:BoundColumn DataField="class_title" HeaderText="Title"></asp:BoundColumn>
		    <asp:BoundColumn DataField="units_course_maximum" HeaderText="Units"></asp:BoundColumn>
		</Columns>            
        </asp:DataGrid>
        <asp:Label ID="formfield" runat="server" Visible="false" /><asp:Label ID="Course_Id" runat="server" Visible="false" /><asp:Label ID="Class_Subject" runat="server" Visible="false" /><asp:Label ID="Class_Number" runat="server" Visible="false" />
        <asp:Literal ID="Literal1" Runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
