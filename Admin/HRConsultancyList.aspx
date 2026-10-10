<%@ Page Title="Admin | Consultancies List" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="HRConsultancyList.aspx.cs" Inherits="Admin_HRConsultancyList" %>

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
        <h1>Consultancy
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
                <div class="btn-group pull-right">
                    <asp:Button Text="Add Consultancy" CssClass="btn btn-success" ID="btnaddcons" runat="server" OnClick="btnaddcons_click" />
                </div>
            </div>

            <br />



            <div class="box-body">
                <br />

                <div class="col-md-12" style="overflow-x: auto">
                    <asp:GridView ID="ConsultanciesList" runat="server" class="table table-striped table-bordered table-hover"
                        OnRowCommand="ConsultanciesList_RowCommand" OnRowDataBound="ConsultanciesList_RowDataBound"
                        AllowPaging="true" OnPageIndexChanging="OnPageIndexChanging" PageSize="10"
                        AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-Width="250" />
                            <asp:BoundField DataField="ConsName" HeaderText="Consultancy Name" ItemStyle-Width="250" />
                            <asp:BoundField DataField="EmailID" HeaderText="Email" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Location" HeaderText="Location" ItemStyle-Width="250" />
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

