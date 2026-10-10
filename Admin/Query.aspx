<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="Query.aspx.cs" Inherits="Admin_Query" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="../Content/ckeditor/ckeditor.js"></script>
    <script type="text/javascript">
        window.onload = function () {
            var textarea = document.getElementById('<%=descriptionTextBox.ClientID %>');
            descriptionTextBox.scrollTop = descriptionTextBox.scrollHeight;
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="content-header">
        <h1>Support
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
                        <lable class="lable">Subject </lable>
                    </strong></h4>
                </div>
                <div class="col-lg-3">

                    <asp:TextBox ID="txtsubject" runat="server"
                        CssClass="form-control" PlaceHolder="subject">
                    </asp:TextBox>
                </div>

                <div class="btn-group pull-right">
                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-success" Text="Submit" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnSubmitClose" runat="server" CssClass="btn btn-primary" Text="Submit&Close" OnClick="btnSubmitClose_Click" Visible="false" />
                </div>

            </div>
            <br />
            <asp:ScriptManager ID="scriptman" runat="server"></asp:ScriptManager>

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
                <div class="form-group col-md-3" id="dvattachment" runat="server">

                    <lable>Select if any Attachment </lable>
                    <asp:FileUpload ID="fuattachment" runat="server" />

                </div>
                <asp:Panel ID="pnlhistory" runat="server" Visible="false">
                </asp:Panel>
            </div>
        </div>
        </div>
    </section>

</asp:Content>

