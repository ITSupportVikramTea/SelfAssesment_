<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Employee/MasterPage.master" CodeFile="UniApplications.aspx.cs" Inherits="Employee_UniApplications" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!-- Latest compiled and minified CSS -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">

<!-- Latest compiled JavaScript -->
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>"
            SelectCommand="UniApplication" SelectCommandType="StoredProcedure">
        </asp:SqlDataSource>

        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand">
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="Department" HeaderText="Department" />
                <asp:BoundField DataField="Position" HeaderText="Position" />
                <asp:BoundField DataField="Zone" HeaderText="Zone" />
                <asp:BoundField DataField="NoApp" HeaderText="No's Application" />
                <asp:BoundField DataField="HQ" HeaderText="HQ" />
                <asp:BoundField DataField="VaccancyId" HeaderText="VaccancyId" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
