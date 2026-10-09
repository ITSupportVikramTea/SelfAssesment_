<%@ Page Title="Schedule Interview" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="Schedule.aspx.cs" Inherits="HR_Schedule" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

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
        <h1>Schedule
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
            <asp:ScriptManager ID="scriptman" runat="server"></asp:ScriptManager>
            <div class="box-body">
                <div class="row">

                    <div class="col-md-6">
                        <div class="form-group">
                            <lable>Interviewer Name </lable>

                            <telerik:RadComboBox RenderMode="Lightweight" ID="txtintrname" runat="server" Width="400" Height="400px"
                                EmptyMessage="Type an E-mail" DataTextField="EmpName">
                            </telerik:RadComboBox>

                            
                        </div>
                        <div class="form-group">
                            <lable>HR </lable>
                            <telerik:RadComboBox RenderMode="Lightweight" ID="txthr" runat="server" Width="400" Height="400px"
                                EmptyMessage="Type an E-mail" DataTextField="EmpName">
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">

                            <lable>Interview Status </lable>
                            <asp:DropDownList ID="dlintvwstatus" AppendDataBoundItems="true" class="form-control" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="Pending"></asp:ListItem>
                                <asp:ListItem Value="Not Attended"></asp:ListItem>
                                <asp:ListItem Value="Attended"></asp:ListItem>
                                <asp:ListItem Value="Postpone"></asp:ListItem>
                                <asp:ListItem Value="Scheduled"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-12">
                        <div class="form-group" id="filldetails">
                            <label>Fill Details</label>
                            <asp:TextBox ID="descriptionTextBox" runat="server" TextMode="MultiLine"></asp:TextBox>
                            <script type="text/javascript" lang="javascript">
                                CKEDITOR.replace('<%=descriptionTextBox.ClientID%>');
                            </script>
                            <span id="descriptionSpan"></span>
                        </div>
                    </div>
                </div>
                <div class="form-group input-group-append">
                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-info" Text="Submit & Mail" OnClick="btnSubmitMail_Click" />
                    <asp:Button ID="btnSubmitonly" runat="server" CssClass="btn btn-success" Text="Submit" OnClick="btnSubmit_Click" />
                    <asp:Button ID="cancel" runat="server" CssClass="btn btn-danger" Text="Cancel" OnClick="btncancel_click" />
                </div>
            </div>
        </div>
    </section>
</asp:Content>

