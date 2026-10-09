<%@ Page Title="" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="HRAssessment.aspx.cs" Inherits="HR_HRAssessment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="content-header">
        <h1>HR Assessment
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
            </div>
            <br />

            <div class="box-body">
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <asp:RadioButtonList ID="rbchoice" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbchoice_SelectedIndexChanged">
                                <asp:ListItem Text="Reject CV" Value="RejectCV" />
                                <asp:ListItem Text="HR Assessment" Value="HRAssessment" />
                            </asp:RadioButtonList>
                        </div>

                    </div>
                </div>
                <div class="row form-group">

                    <asp:Panel ID="pnlMain" runat="server" Visible="false">
                        <div class="col-md-12">
                            <h3><b>Fill Your Assessment</b></h3>
                        </div>
                        <div class="col-md-2">
                            <lable>Appearance </lable>
                            <asp:DropDownList AppendDataBoundItems="true" ID="dlappearance" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Poor" Value="Poor"></asp:ListItem>
                                <asp:ListItem Text="Good" Value="Good"></asp:ListItem>
                                <asp:ListItem Text="Very Good" Value="Very Good"></asp:ListItem>
                                <asp:ListItem Text="Excellent" Value="Excellent"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <lable>Job Knowledge </lable>
                            <asp:DropDownList AppendDataBoundItems="true" ID="dljobknowledge" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Poor" Value="Poor"></asp:ListItem>
                                <asp:ListItem Text="Good" Value="Good"></asp:ListItem>
                                <asp:ListItem Text="Very Good" Value="Very Good"></asp:ListItem>
                                <asp:ListItem Text="Excellent" Value="Excellent"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <lable>Relavant Experience </lable>
                            <asp:DropDownList AppendDataBoundItems="true" ID="dlrelexp" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Poor" Value="Poor"></asp:ListItem>
                                <asp:ListItem Text="Good" Value="Good"></asp:ListItem>
                                <asp:ListItem Text="Very Good" Value="Very Good"></asp:ListItem>
                                <asp:ListItem Text="Excellent" Value="Excellent"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <lable>Matuarity </lable>
                            <asp:DropDownList AppendDataBoundItems="true" ID="dlmatuarity" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Poor" Value="Poor"></asp:ListItem>
                                <asp:ListItem Text="Good" Value="Good"></asp:ListItem>
                                <asp:ListItem Text="Very Good" Value="Very Good"></asp:ListItem>
                                <asp:ListItem Text="Excellent" Value="Excellent"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <lable>Communication Skills</lable>
                            <asp:DropDownList AppendDataBoundItems="true" ID="dlcomskills" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Poor" Value="Poor"></asp:ListItem>
                                <asp:ListItem Text="Good" Value="Good"></asp:ListItem>
                                <asp:ListItem Text="Very Good" Value="Very Good"></asp:ListItem>
                                <asp:ListItem Text="Excellent" Value="Excellent"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4">
                            <lable class="form-group">Positive Points</lable>
                            <asp:TextBox ID="txtpospoints" CssClass="form-control" runat="server" TextMode="MultiLine" />
                        </div>
                        <div class="col-lg-4">
                            <lable class="form-group">Negative Points</lable>
                            <asp:TextBox ID="txtnegpoints" CssClass="form-control" runat="server" TextMode="MultiLine" />
                        </div>
                        <div class="col-lg-4">
                            <lable>Final Feedback</lable>
                            <asp:DropDownList AppendDataBoundItems="true" ID="dlhrfeedback" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Shortlist" Value="Shortlisted"></asp:ListItem>
                                <asp:ListItem Text="Reject" Value="Rejected"></asp:ListItem>
                                <asp:ListItem Text="Interview With Interveiwer" Value="Interview With Interveiwer"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </asp:Panel>
                </div>
                <div class="form-group">
                    <asp:Button Text="Submit" CssClass="btn btn-bitbucket" runat="server" ID="btnsubmit" OnClick="btnsubmit_click" />
                                        <asp:Button Text="Cancel" CssClass="btn btn-github" runat="server" ID="btncancel" OnClick="btncancel_click" />
                </div>
            </div>
        </div>
    </section>

</asp:Content>

