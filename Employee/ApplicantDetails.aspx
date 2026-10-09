<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeFile="ApplicantDetails.aspx.cs" Inherits="Employee_ApplicantDetails" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="container">
        <div class="row">
            <div class="col-md-6">
                <asp:TextBox ID="LabelFullName" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-6">
                <asp:TextBox ID="LabelDepartment" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <asp:TextBox ID="Created" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-6">
                <asp:TextBox ID="Mobile" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <asp:TextBox ID="Email" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-6">
                <asp:TextBox ID="Status" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>

            </div>
        </div>

        <div class="row">
            <div class="col-md-6">
                <asp:TextBox ID="Interviewer" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-6">
                <asp:TextBox ID="Refferd" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>

    </div>
</asp:Content>





