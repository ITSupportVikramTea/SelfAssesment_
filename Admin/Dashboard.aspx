<%@ Page Title="Admin | Dashboard" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true"
    CodeFile="Dashboard.aspx.cs" Inherits="Admin_Dashboard" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



        <section class="content-header">
            <h1>Hi,Users Welcome Back!
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
                    <div class="pull-right">
                        <div class="btn-group">

                            <asp:Button ID="btnChangepass" CssClass="btn btn-success form-control"
                                Text="Change Password" runat="server" OnClick="btnChangepass_click" />
                        </div>
                    </div>
                </div>
                <br />

                <div class="box-body">
                    <br />

                    <div class="col-md-12" style="overflow-x: auto">
                    </div>
                </div>
            </div>
        </section>
    </asp:Content>