<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Employee/MasterPage.master" CodeFile="TotalApplicant.aspx.cs" Inherits="Employee_ApplicantDetails" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!-- Latest compiled and minified CSS -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">

<!-- Latest compiled JavaScript -->
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
       <asp:GridView ID="GridViewDetails" runat="server" AutoGenerateColumns="False" OnRowCommand="GridViewDetails_RowCommand">
    <Columns>
        <asp:BoundField DataField="FName" HeaderText="First Name" />
        <asp:BoundField DataField="LName" HeaderText="Last Name" />
        <asp:BoundField DataField="MobNo" HeaderText="Mobile Number" />
        <asp:BoundField DataField="VaccancyId" HeaderText="VaccancyId" />
        <asp:BoundField DataField="Position" HeaderText="Position" />
         <asp:CommandField ShowSelectButton="True" />
    
        
    </Columns>
</asp:GridView>


    </div>
</asp:Content>