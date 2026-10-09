<%@ Page Title="HR | Report" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="JobApplicationsReport.aspx.cs" Inherits="Employee_JobApplicationsReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <section class="content-header">
        <h1>Job Application Report
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
                    <asp:TextBox ID="txtsearchposhq" runat="server"
                        CssClass="form-control" PlaceHolder="Search Position,HQ,ZONE,SDM">
                    </asp:TextBox>
                </div>
                <div class="col-lg-3">
                    <asp:Button CssClass="btn btn-bitbucket" Text="Search" runat="server" OnClick="Search" />
                </div>
                <div class="btn-group pull-right">
                    <asp:Button Text="Download" CssClass="btn btn-success" ID="btndownload" runat="server" OnClick="ExportToExcel" />
                </div>
            </div>

            <br />



            <div class="box-body">
                <br />

                <div class="col-md-12" style="overflow-x: auto">
                    <asp:GridView ID="JobAppReport" runat="server" class="table table-striped table-bordered table-hover"
                        OnRowCommand="JobAppReport_RowCommand" OnRowDataBound="JobAppReport_RowDataBound"
                        AllowPaging="true" OnPageIndexChanging="OnPageIndexChanging" PageSize="10"
                        AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="HQ" HeaderText="HQ" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Position" HeaderText="Position" ItemStyle-Width="250" />
                            <asp:BoundField DataField="ZONE" HeaderText="ZONE" ItemStyle-Width="250" />
                            <asp:BoundField DataField="SDM" HeaderText="SDM" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Shortlited" HeaderText="Shortlisted" ItemStyle-Width="50" />
                            <asp:BoundField DataField="Rejected" HeaderText="Rejected" ItemStyle-Width="50" />
                            <asp:BoundField DataField="ShortlitedByInterviewer" HeaderText="Shortlisted By Interviewer" ItemStyle-Width="50" />
                            <asp:BoundField DataField="RejectedByInterviwer" HeaderText="Rejected By Interviewer" ItemStyle-Width="50" />
                            <asp:BoundField DataField="RejectedByHR" HeaderText="Rejected By HR" ItemStyle-Width="50" />
                            <asp:BoundField DataField="Attended" HeaderText="Attended" ItemStyle-Width="50" />
                            <asp:BoundField DataField="NotDeclared" HeaderText="Result Not Declared" ItemStyle-Width="50" />
                            <asp:BoundField DataField="Total" HeaderText="Total" ItemStyle-Width="50" />
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" CssClass="pagination-ys" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </section>

</asp:Content>

