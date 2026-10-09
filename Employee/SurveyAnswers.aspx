<%@ Page Title="" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="SurveyAnswers.aspx.cs" Inherits="Employee_SurveyAnswers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="content-header">
        <h1>Survey
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
                <div class="col-lg-8">
                    <h4><strong>
                        <lable class="lable">Survey: </lable>
                        <u> <asp:Label CssClass="text-primary" ID="lbltitle" runat="server">
                        </asp:Label></u>
                    </strong></h4>
                </div>

                <div class="btn-group pull-right">
                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-success" Text="Submit" OnClick="btnSubmit_click" />
                    <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary" Text="Back" OnClick="btnBack_click" />
                </div>

            </div>
            <br />

            <div class="box-body">
                <div class="container text-center">
                    <h4><strong>
                        **<asp:Label ID="lblDesc" runat="server">
                        </asp:Label>**
                    </strong></h4>
     
                </div>
                <hr />

                <div class="container m-auto">
                    <div class="row">
                       
                        <asp:Panel ID="pnlQues" runat="server">
                        </asp:Panel>
                    </div>

                </div>
            </div>
        </div>
    </section>

</asp:Content>

