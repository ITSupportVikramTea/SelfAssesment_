<%@ Page Title="Admin | Employee List" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="EmployeeList.aspx.cs" Inherits="Admin_EmployeeList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css'
        media="screen" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>
    <script src="Scripts/jquery-1.3.2.min.js"></script>
    <script>

        //$(document).ready(function () {
        //    $('[id*=txtsearchemp]').keyup(function () {
        //        _this = this;
        //        $.each($('[id*=EmployeeList] tbody').find("tr:has(td)"), function () {
        //            if ($(this).text().toLowerCase().indexOf($(_this).val().toLowerCase()) != -1)
        //                $(this).show();
        //            else
        //                $(this).hide();

        //        });
        //    });
        //});

    </script>

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
        <h1>Employees
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
                    <asp:TextBox ID="txtsearchuser" runat="server"
                        CssClass="form-control" PlaceHolder="Search User">
                    </asp:TextBox>
                </div>
                <div class="col-lg-3">
                    <asp:Button CssClass="btn btn-bitbucket" Text="Search" runat="server" OnClick="Search" />
                </div>

                 <div class="col-lg-2">
                    <div class="form-group">
                        <asp:DropDownList runat="server" ID="dlRm" CssClass="form-control ml-3" AppendDataBoundItems="true" OnSelectedIndexChanged="dlRm_SelectedIndexChanged" AutoPostBack="true">
                             <asp:ListItem  Text="Select RM" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="btn-group pull-right">
                      <asp:Button Text="Change RM" CssClass="btn btn-success" ID="Button1" runat="server" />
                    <asp:Button Text="Add Employee" CssClass="btn btn-success" ID="btnaddemp" runat="server" OnClick="btnaddemp_click" />
                    <button type="button" class="btn btn-primary" data-toggle="modal" style="margin-left: 5px;" data-target="#myModal">
                        <i class="fa fa-plus-circle"></i>Import Excel  
                    </button>

                    <asp:Button Text="Export Data" CssClass="btn btn-info" ID="btnexport" style="margin-left: 5px;" runat="server" OnClick="btnexport_Click" />
                </div>
            </div>

            <div class="modal fade" id="myModal">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">Import Excel File</h4>
                            <button type="button" class="close" data-dismiss="modal">×</button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label>Choose excel file</label>
                                        <div class="input-group">
                                            <div class="custom-file">
                                                <asp:FileUpload ID="FileUpload1" CssClass="custom-file-input" runat="server" />
                                                <label class="custom-file-label"></label>
                                            </div>
                                            <label id="filename"></label>
                                            <div class="input-group-append">
                                                <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-info" Text="Upload" OnClick="btnUpload_Click" />
                                                <asp:Button ID="btnSample" runat="server" CssClass="btn btn-warning pull-right" Text="Sample Download" OnClick="btnSample_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>


            <br />

            <!--   <div class="row">

                 <div class="col-md-2">
                            <h4>Select Year</h4>
                            <div class="form-group">
                                <asp:DropDownList runat="server" ID="dlyear" AppendDataBoundItems="true"  CssClass="form-control ">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-2">
                            <h4>Select Status</h4>
                            <div class="form-group">
                                <asp:DropDownList runat="server" ID="dlstatus" CssClass="form-control"  AutoPostBack="true">
                                    <asp:ListItem></asp:ListItem>
                                    
                                    <asp:ListItem Value="1">Active</asp:ListItem>
                                    <asp:ListItem Value="2">InActive</asp:ListItem>
                                   
                                </asp:DropDownList>
                            </div>
                        </div>

                        
                    </div>-->

            <div class="box-body">
                <br />

                <div class="col-md-12" style="overflow-x: auto">
                    <asp:GridView ID="EmployeeList" runat="server" class="table table-striped table-bordered table-hover"
                        OnRowCommand="EmployeeList_RowCommand" OnRowDataBound="EmployeeList_RowDataBound" DataKeyNames="Id"
                        AllowPaging="true" OnPageIndexChanging="OnPageIndexChanging" PageSize="10"
                        AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-Width="250" />
                            <asp:BoundField DataField="ERPId" HeaderText="ERPId" ItemStyle-Width="250" />
                            <asp:BoundField DataField="EmpName" HeaderText="Employee Name" ItemStyle-Width="250" />
                            <asp:BoundField DataField="EmpDepartment" HeaderText="Department" ItemStyle-Width="250" />
                            <asp:BoundField DataField="EmpDesignation" HeaderText="Designation" ItemStyle-Width="250" />
                            <asp:BoundField DataField="HOD" HeaderText="HOD" ItemStyle-Width="250" />
                            <asp:BoundField DataField="RM" HeaderText="RM" ItemStyle-Width="250" />
                            <asp:BoundField DataField="SDM" HeaderText="SDM" ItemStyle-Width="250" />
                            <asp:BoundField DataField="ASM" HeaderText="ASM" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="250" />

                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <asp:Button ID="btnselect" Text="Select" runat="server" CssClass="btn btn-primary" CommandName='<%# Eval("Id") %>' CommandArgument="<%# Container.DataItemIndex %>"></asp:Button>
                                    </div>
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

