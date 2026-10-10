<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="SurveyReport.aspx.cs" Inherits="Admin_SurveyReport" %>

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
        <h1>Survey Report
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
                <div class="col-lg-12 col-md-12">
                    <div class="row">
                        <div class="col-lg-3">
                            <asp:TextBox ID="txtsearch" runat="server"
                                CssClass="form-control" PlaceHolder="Search">
                            </asp:TextBox>
                        </div>
                        <div class="col-lg-3">
                            <asp:Button CssClass="btn btn-bitbucket" Text="Search" runat="server" OnClick="Search" />
                        </div>


                        <div class="col-md-6">
                            <div class="form-group">

                                <h4>Select Survey</h4>

                                <asp:DropDownList runat="server" ID="dlSurvey" AppendDataBoundItems="true" AutoPostBack="true" OnTextChanged="dlSurvey_TextChanged" CssClass="form-control ">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                                <div class="btn-group pull-right">
                                    <asp:Button Text="Download" CssClass="btn btn-success" ID="btndownload" runat="server" OnClick="ExportToExcel" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <br />



            <div class="box-body">
                <br />

                <div class="col-md-12" style="overflow-x: auto">
                    <asp:GridView ID="surveyReport" runat="server" class="table table-striped table-bordered table-hover"
                        OnRowDataBound="surveyReport_RowDataBound"
                        AllowPaging="true" OnPageIndexChanging="OnPageIndexChanging" PageSize="10" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="EmpName" HeaderText="EmployeeName" ItemStyle-Width="100" />
                            <asp:BoundField DataField="Question" HeaderText="Question" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Ans" HeaderText="Ans" ItemStyle-Width="100" />
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" CssClass="pagination-ys" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </section>


</asp:Content>

