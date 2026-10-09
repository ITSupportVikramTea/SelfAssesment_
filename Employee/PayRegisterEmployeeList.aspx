<%@ Page Title="" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="PayRegisterEmployeeList.aspx.cs" Inherits="Employee_PayRegisterEmployeeList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style type="text/css">
        .pagination-ys {
            /*display: inline-block;*/
            padding-left: 0;
            margin: 20px 0;
            border-radius: 4px;
        }

            .pagination-ys table > tbody > tr > td {
                display: inline;
            }

                .pagination-ys table > tbody > tr > td > a,
                .pagination-ys table > tbody > tr > td > span {
                    position: relative;
                    float: left;
                    padding: 8px 12px;
                    line-height: 1.42857143;
                    text-decoration: none;
                    color: #dd4814;
                    background-color: #ffffff;
                    border: 1px solid #dddddd;
                    margin-left: -1px;
                }

                .pagination-ys table > tbody > tr > td > span {
                    position: relative;
                    float: left;
                    padding: 8px 12px;
                    line-height: 1.42857143;
                    text-decoration: none;
                    margin-left: -1px;
                    z-index: 2;
                    color: #aea79f;
                    background-color: #f5f5f5;
                    border-color: #dddddd;
                    cursor: default;
                }

                .pagination-ys table > tbody > tr > td:first-child > a,
                .pagination-ys table > tbody > tr > td:first-child > span {
                    margin-left: 0;
                    border-bottom-left-radius: 4px;
                    border-top-left-radius: 4px;
                }

                .pagination-ys table > tbody > tr > td:last-child > a,
                .pagination-ys table > tbody > tr > td:last-child > span {
                    border-bottom-right-radius: 4px;
                    border-top-right-radius: 4px;
                }

                .pagination-ys table > tbody > tr > td > a:hover,
                .pagination-ys table > tbody > tr > td > span:hover,
                .pagination-ys table > tbody > tr > td > a:focus,
                .pagination-ys table > tbody > tr > td > span:focus {
                    color: #97310e;
                    background-color: #eeeeee;
                    border-color: #dddddd;
                }
    </style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="content-header">
        <h1>PayRegister Employee List
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
            <div class="box-body">
                <br />

                <div class="col-lg-3">
                    <asp:TextBox ID="txtsearchuser" runat="server"
                        CssClass="form-control" PlaceHolder="Search User">
                    </asp:TextBox>
                </div>
                <div class="col-lg-3">
                    <asp:Button CssClass="btn btn-bitbucket" Text="Search" runat="server" OnClick="Search" />
                </div>

                <div class="col-lg-2">
                    <div class="form-group">

                        <asp:DropDownList runat="server" ID="dlIsMailSent" CssClass="form-control " OnSelectedIndexChanged="dlIsMailSent_SelectedIndexChanged" AppendDataBoundItems="true" AutoPostBack="true">
                            <asp:ListItem Text="ALL" Value=""></asp:ListItem>
                            <asp:ListItem Text="Mail Sent" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Mail Not Sent" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>


                <div class="pull-right col-md-2" style="padding-bottom: 10px;">
                    <asp:Button ID="btnSendEmails" CssClass="btn btn-success" runat="server" Text="Send Increment Letter" OnClick="btnSendEmails_Click" />
                </div>

                <div class="col-md-12" style="overflow-x: auto">
                    <asp:GridView ID="PayRegisterEmployeeList" runat="server" class="table table-bordered table-striped table-responsive"
                        AllowPaging="true" PageSize="10" DataKeyNames="Id"
                        AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="250">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All" OnCheckedChanged="chkSelectAll_CheckedChanged" AutoPostBack="true" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-Width="150" />
                            <asp:BoundField DataField="emp_abbrivation" HeaderText="Abbrivation" ItemStyle-Width="150" />
                            <asp:BoundField DataField="employee_name" HeaderText="Employee Name" ItemStyle-Width="250" />
                            <asp:BoundField DataField="emp_email_id" HeaderText="EmailId" ItemStyle-Width="300" />
                            <asp:BoundField DataField="department" HeaderText="Department" ItemStyle-Width="250" />
                            <asp:BoundField DataField="hod_name" HeaderText="HOD" ItemStyle-Width="250" />
                            <asp:TemplateField HeaderText="Increment Letter" ItemStyle-Width="250">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlIncrementLetter" runat="server" Target="_blank" Text="View Letter"
                                        NavigateUrl='<%# ResolveUrl(Eval("increment_letter_path").ToString()) %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <PagerStyle HorizontalAlign="Right" CssClass="pagination-ys" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </section>

</asp:Content>

