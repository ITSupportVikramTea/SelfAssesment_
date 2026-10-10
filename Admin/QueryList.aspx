<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="QueryList.aspx.cs" Inherits="Admin_QueryList" %>

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
        <h1>Job Applications
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
                    <asp:TextBox ID="txtsearchquery" runat="server"
                        CssClass="form-control" PlaceHolder="Search Query,User">
                    </asp:TextBox>
                </div>
                <div class="col-lg-3">
                    <asp:Button CssClass="btn btn-bitbucket" Text="Search" runat="server" OnClick="Search" />
                </div>
                <div class="pull-right">
                    <asp:Button CssClass="btn btn-success" ID="raisequery" Text="New Query" runat="server" OnClick="raisequery_Click" />
                </div>
                
            </div>

            <br />

            <div class="box-body">
                <br />

                <div class="container-fluid col-md-12" style="overflow-x: auto; ">
                    <asp:GridView ID="gdQueryList" runat="server" class="table table-striped table-bordered table-hover"
                        DataKeyNames="QueryId" OnRowCommand="gdQueryList_RowCommand" OnRowDataBound="gdQueryList_RowDataBound"
                        AllowPaging="true" OnPageIndexChanging="OnPageIndexChanging" PageSize="10"
                        AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="EmpName" HeaderText="EmpName" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Subject" HeaderText="Subject" ItemStyle-Width="250" />
                            <asp:BoundField DataField="DateTime" HeaderText="Date Time" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="80" />
                            <asp:BoundField DataField="FileName" HeaderText="FileName" ItemStyle-Width="80" Visible="false" />
                            
                            <asp:TemplateField HeaderText="Attachment" ItemStyle-Width="80">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <asp:Button Text="View" ID="btnview" runat="server" CssClass="btn btn-sm btn-info " CommandName="view" CommandArgument='<%# Eval("FileName") %>' />
                                        </button>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Action" ItemStyle-Width="80">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <asp:Button ID="btnselect" Text="Select" runat="server" CssClass="btn btn-sm btn-primary" CommandName="Select" CommandArgument='<%# Eval("QueryId") %>'></asp:Button>
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

