<%@ Page Title="" Language="C#" MasterPageFile="~/Candidate/MasterPage.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Candidate_Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="content-header">
        <h1>Vacancies
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

                <br />

                <div class="box-body">
                    <br />
                    <h2><b>
                        <asp:Label runat="server" ID="lblShowMsg" Visible="false" CssClass="text-blue text-center" Text="Documents Uploaded" />
                    </b></h2>
                    <asp:Panel ID="pnlMain" runat="server">
                        <div class="row">

                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>10th Marksheet</label>
                                    <div class="input-group">
                                        <div class="custom-file">
                                            <asp:FileUpload ID="futenthmark" CssClass="custom-file-input" runat="server" />
                                            <label class="custom-file-label"></label>
                                        </div>
                                    </div>
                                    <label>12th Marksheet</label>
                                    <div class="input-group">
                                        <div class="custom-file">
                                            <asp:FileUpload ID="futwevlemarks" CssClass="custom-file-input" runat="server" />
                                            <label class="custom-file-label"></label>
                                        </div>
                                    </div>
                                    <label>Graduation Certificates</label>
                                    <div class="input-group">
                                        <div class="custom-file">
                                            <asp:FileUpload ID="fugraduationcert" CssClass="custom-file-input" runat="server" />
                                            <label class="custom-file-label"></label>
                                        </div>
                                    </div>
                                    <label>Driving Licence</label>
                                    <div class="input-group">
                                        <div class="custom-file">
                                            <asp:FileUpload ID="fudrivinglicense" CssClass="custom-file-input" runat="server" />
                                            <label class="custom-file-label"></label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>Aadhar Card</label>
                                    <div class="input-group">
                                        <div class="custom-file">
                                            <asp:FileUpload ID="fuaadhar" CssClass="custom-file-input" runat="server" />
                                            <label class="custom-file-label"></label>
                                        </div>
                                    </div>

                                    <label>Experience Certificates</label>
                                    <div class="input-group">
                                        <div class="custom-file">
                                            <asp:FileUpload ID="fuexpcert" CssClass="custom-file-input" runat="server" />
                                            <label class="custom-file-label"></label>
                                        </div>
                                    </div>
                                    <label>Salary Slip</label>
                                    <div class="input-group">
                                        <div class="custom-file">
                                            <asp:FileUpload ID="fuslslip" CssClass="custom-file-input" runat="server" />
                                            <label class="custom-file-label"></label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Button runat="server" ID="btnupload" OnClick="btnUpload_click" Text="Submit" class="btn btn-info" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </section>

</asp:Content>

