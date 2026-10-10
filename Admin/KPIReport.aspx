<%@ Page Language="C#" AutoEventWireup="true" CodeFile="KPIReport.aspx.cs" Inherits="Admin_KPIReport"  %>

<asp:Content ID="form1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   
<div class="container mt-4">

    <h3 class="mb-3">Skills Report</h3>

    <!-- Filters -->
    <div class="mb-3">

        <asp:DropDownList ID="ddlStatus" runat="server"
            CssClass="form-select d-inline w-auto me-2"
            AutoPostBack="true"
            OnSelectedIndexChanged="FilterChanged">
            <asp:ListItem Text="All Status" Value=""></asp:ListItem>
            <asp:ListItem Text="Pending From Employee" Value="EMP"></asp:ListItem>
            <asp:ListItem Text="Pending From RM" Value="RM"></asp:ListItem>
        </asp:DropDownList>

        <asp:DropDownList ID="ddlYear" runat="server"
            CssClass="form-select d-inline w-auto me-2"
            AutoPostBack="true"
            OnSelectedIndexChanged="FilterChanged">
            <asp:ListItem Text="All Years" Value=""></asp:ListItem>
            <asp:ListItem Text="2022" Value="2022"></asp:ListItem>
            <asp:ListItem Text="2023" Value="2023"></asp:ListItem>
            <asp:ListItem Text="2024" Value="2024"></asp:ListItem>
            <asp:ListItem Text="2025" Value="2025"></asp:ListItem>
        </asp:DropDownList>

        <asp:Button ID="btnExport" runat="server"
            Text="Export Excel"
            CssClass="btn btn-success"
            OnClick="btnExport_Click" />

    </div>

    <!-- Grid -->
    <asp:GridView ID="GridView1" runat="server"
        CssClass="table table-bordered table-striped"
        AutoGenerateColumns="true">
    </asp:GridView>

</div>
</asp:Content>
