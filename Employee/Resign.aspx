<%@ Page Title="Add Resign" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="Resign.aspx.cs" Inherits="Employee_Resign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Content/ckeditor/ckeditor.js"></script>

    <script type="text/javascript">
        window.onload = function () {
            var textarea = document.getElementById('<%=descriptionTextBox.ClientID %>');
            descriptionTextBox.scrollTop = descriptionTextBox.scrollHeight;
        }
    </script>

    <style type="text/css">
        .cke_top {
            display: none !important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="content-header">
        <h1>Resign
            <small>Master</small>
            <br />

        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Forms</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-default">
            <div class="box-header with-border">


                <div class="col-lg-1">
                    <h4><strong>
                        <lable class="lable">Reason </lable>
                    </strong></h4>
                </div>
                <div class="col-lg-3">

                    <asp:DropDownList ID="ddlReason" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Select a Reason" Value=""></asp:ListItem>
                        <asp:ListItem Text="Absconding" Value="Absconding"></asp:ListItem>
                        <asp:ListItem Text="Benefits" Value="Benefits"></asp:ListItem>
                        <asp:ListItem Text="Dissatisfied/Management" Value="Dissatisfied/Management"></asp:ListItem>
                        <asp:ListItem Text="Family Responsibilities" Value="Family Responsibilities"></asp:ListItem>
                        <asp:ListItem Text="For Further Studies" Value="For Further Studies"></asp:ListItem>
                        <asp:ListItem Text="Health Retirement" Value="Health Retirement"></asp:ListItem>
                        <asp:ListItem Text="Job Advancement" Value="Job Advancement"></asp:ListItem>
                        <asp:ListItem Text="Personal" Value="Personal"></asp:ListItem>
                        <asp:ListItem Text="Relocation" Value="Relocation"></asp:ListItem>
                        <asp:ListItem Text="Retain By Last Company" Value="Retain By Last Company"></asp:ListItem>
                        <asp:ListItem Text="Salary" Value="Salary"></asp:ListItem>
                        <asp:ListItem Text="Started Own Business" Value="Started Own Business"></asp:ListItem>
                        <asp:ListItem Text="Traveling Issue" Value="Traveling Issue"></asp:ListItem>
                        <asp:ListItem Text="Backedout" Value="Backedout"></asp:ListItem>
                        <asp:ListItem Text="Market Issue like Product Availability is not there" Value="Market Issue like Product Availability is not there"></asp:ListItem>
                        <asp:ListItem Text="Market Issue like No Credit Facility is there" Value="Market Issue like No Credit Facility is there"></asp:ListItem>
                        <asp:ListItem Text="Lack of Support & guidance from RM" Value="Lack of Support & guidance from RM"></asp:ListItem>
                        <asp:ListItem Text="Dealer Cum Employee" Value="Dealer Cum Employee"></asp:ListItem>
                        <asp:ListItem Text="Dismissal" Value="Dismissal"></asp:ListItem>
                        <asp:ListItem Text="Job Eliminated" Value="Job Eliminated"></asp:ListItem>
                        <asp:ListItem Text="Termination" Value="Termination"></asp:ListItem>
                        <asp:ListItem Text="Asked to Go" Value="Asked to Go"></asp:ListItem>
                        <asp:ListItem Text="Death Case" Value="Death Case"></asp:ListItem>
                    </asp:DropDownList>
                    <!-- <asp:TextBox ID="txtreason" runat="server"
                        CssClass="form-control" PlaceHolder="Reason">
                    </asp:TextBox>-->
                </div>

                <div class="btn-group pull-right">
                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" Text="Cancel" OnClick="btnCancel_Click" />
                </div>

            </div>
            <br />

            <div class="box-body">
                <div class="container-fluid" style="margin: 0px 50px 10px 50px;">
                    <div class="row">

                        <div class="col-md-12">
                            <div class="form-group" id="querydesc">
                                <asp:TextBox ID="descriptionTextBox" runat="server" TextMode="MultiLine"></asp:TextBox>
                                <script type="text/javascript" lang="javascript">
                                    CKEDITOR.replace('<%=descriptionTextBox.ClientID%>');
                                </script>
                                <span id="descriptionSpan"></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

</asp:Content>

