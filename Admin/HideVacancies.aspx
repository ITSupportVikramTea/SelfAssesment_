<%@ Page Title="Admin | Hide Vacancies" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="HideVacancies.aspx.cs" Inherits="Admin_HideVacancies" %>

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
        <h1>Employees & Vacancies List
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

                <div class="col-lg-3">
                    <asp:TextBox ID="txtsearch" runat="server"
                        CssClass="form-control" PlaceHolder="Search">
                    </asp:TextBox>
                </div>
                <div class="btn-group">
                    <asp:Button CssClass="btn btn-bitbucket" Text="Search User" ID="btnusersearch"
                        runat="server" OnClick="btnusersearch_click" />
                    <asp:Button CssClass="btn btn-github" Text="Search Vacancy" ID="btnvacancysearrch"
                        runat="server" OnClick="btnvacancysearrch_click" />
                </div>

                <div class="btn-group pull-right">
                    <asp:Button Text="Submit" CssClass="btn btn-success" ID="btnSubmit" runat="server" OnClick="btnSubmit_click" />
                </div>
            </div>

            <br />



            <div class="box-body">
                <br />

                <div class="col-md-6" style="overflow-x: auto">
                    <asp:GridView ID="EmployeeList" runat="server" class="table table-striped table-bordered table-hover"
                        OnRowCommand="EmployeeList_RowCommand" OnRowDataBound="EmployeeList_RowDataBound"
                        DataKeyNames="Id"
                        AllowPaging="true" OnPageIndexChanging="EmployeeList_OnPageIndexChanging" PageSize="6"
                        AutoGenerateColumns="false">
                        <Columns>

                            <asp:BoundField DataField="EmpName" HeaderText="Employee Name" ItemStyle-Width="250" />
                            <asp:BoundField DataField="EmpDepartment" HeaderText="Department" ItemStyle-Width="250" />
                            <asp:BoundField DataField="EmpDesignation" HeaderText="Designation" ItemStyle-Width="250" />
                            <asp:BoundField DataField="HOD" HeaderText="HOD" ItemStyle-Width="250" />
                            <asp:BoundField DataField="RM" HeaderText="RM" ItemStyle-Width="250" />
                            <asp:TemplateField HeaderText="Select" ItemStyle-Width="50">
                                <ItemTemplate>
                                    <asp:CheckBox ID="emp_chkRow" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" CssClass="pagination-ys" />
                    </asp:GridView>
                </div>

                <div class="col-md-6" style="overflow-x: auto">
                    <asp:GridView ID="gdvacancies" runat="server" class="table table-striped table-bordered table-hover"
                        OnRowCommand="gdvacancies_RowCommand" OnRowDataBound="gdvacancies_RowDataBound"
                        DataKeyNames="Id" AllowPaging="true" OnPageIndexChanging="OnPageIndexChanging_gdvacancies" PageSize="6"
                        AutoGenerateColumns="false">
                        <Columns>


                            <asp:BoundField DataField="Department" HeaderText="Department" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Position" HeaderText="Position" ItemStyle-Width="250" />
                            <asp:BoundField DataField="NoOfVC" HeaderText="No. Of Vacancies" ItemStyle-Width="250" />
                            <asp:BoundField DataField="ZONE" HeaderText="ZONE" ItemStyle-Width="250" />
                            <asp:BoundField DataField="SDM" HeaderText="SDM" ItemStyle-Width="250" />
                            <asp:BoundField DataField="HQ" HeaderText="HQ" ItemStyle-Width="250" />
                            <asp:TemplateField HeaderText="Select" ItemStyle-Width="50">
                                <ItemTemplate>
                                    <asp:CheckBox ID="vac_chkRow" runat="server" />
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

