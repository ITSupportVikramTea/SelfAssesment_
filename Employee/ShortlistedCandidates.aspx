<%@ Page Title="HR | Shortlited Candidates" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="ShortlistedCandidates.aspx.cs" Inherits="HR_ShortlistedCandidates" %>

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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <section class="content-header">
        <h1>Shortlisted Candidates
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
                    <asp:TextBox ID="txtsearchcand" runat="server"
                        CssClass="form-control" PlaceHolder="Search Candidate">
                    </asp:TextBox>
                </div>
                <div class="col-lg-3">
                    <asp:Button CssClass="btn btn-bitbucket" Text="Search" runat="server" OnClick="Search" />
                </div>
            </div>
            <br />
            <div class="box-body">
                <br />
                

                <div class="col-md-12" style="overflow-x: auto">
                    <asp:GridView ID="gdShortedCnds" runat="server" class="table table-striped table-bordered table-hover"
                        DataKeyNames="Id" OnRowCommand="gdShortedCands_rowcommand" AllowPaging="true" OnRowDataBound="OnRowDataBound"
                        OnPageIndexChanging="OnPageIndexChanging" PageSize="6"
                        AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="FullName" HeaderText="Full Name" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Department" HeaderText="Department" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Position" HeaderText="Position" ItemStyle-Width="250" />
                            <asp:BoundField DataField="HQ" HeaderText="HQ" ItemStyle-Width="250" />
                            <asp:BoundField DataField="IsLoginCreated" HeaderText="IsLoginCreated" ItemStyle-Width="250" Visible="false" />

                            <asp:TemplateField HeaderText="CV">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <asp:Button Text="View CV" ID="btncv1" runat="server" CssClass="btn btn-info " CommandName="CVView" CommandArgument='<%# Eval("FileName") %>' />
                                        </button>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Documents">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <asp:Button Text="View Docs" ID="vwdocs" runat="server" CssClass="btn btn-github" CommandName="viewdocs" CommandArgument='<%# Eval("Id") %>' />
                                        </button>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Login">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <asp:Button Text="Create" OnClientClick="return confirm('Create Login & Send Mail?');" ID="createlogin" runat="server" CssClass="btn btn-success" CommandName="createlogin" CommandArgument='<%# Eval("Id") %>' />
                                        </button>
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

