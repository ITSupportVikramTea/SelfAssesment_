<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="Scores.aspx.cs" Inherits="Admin_Scores" %>

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
        <h1>Scores
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
                    <asp:TextBox ID="txtsearchemp" runat="server"
                        CssClass="form-control" PlaceHolder="Search Employee">
                    </asp:TextBox>
                </div>
                <div class="col-lg-3">
                    <asp:Button CssClass="btn btn-bitbucket" Text="Search" runat="server" OnClick="Search" />
                </div>

                <div class="col-lg-2">
                    <div class="form-group">
                        <asp:DropDownList runat="server" ID="dlyear" CssClass="form-control " OnSelectedIndexChanged="dlyear_SelectedIndexChanged" AutoPostBack="true">

                            <asp:ListItem Value="" Text="Select Year"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="pull-right">
                    <asp:Button Text="Export Data" CssClass="btn btn-info" ID="btnexport" runat="server" OnClick="btnexport_Click" />
                    <asp:Button Text="Submit" CssClass="btn btn-success" ID="btnsubmit" runat="server" OnClick="btnsubmit_click" />
                    <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModal">
                        <i class="fa fa-plus-circle"></i>Import Excel  
                    </button>
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

            <div class="box-body">

                <br />

                <div class="col-md-12" style="overflow-x: auto; overflow-y: auto">
                    <asp:GridView ID="EmplooyewListScore" runat="server" class="table table-bordered table-striped table-responsive"
                        AllowSorting="true" PageSize="6" OnRowDataBound="EmplooyewListScore_RowDataBound"
                        AllowPaging="true" DataKeyNames="Id"
                        OnPageIndexChanging="OnPageIndexChanging" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-Width="100" Visible="false" />
                            <asp:BoundField DataField="EmpCode" HeaderText="PF Code" ItemStyle-Width="100" />
                            <asp:BoundField DataField="EmpName" HeaderText="Employee Name" ItemStyle-Width="250" />
                            <asp:BoundField DataField="DOJ" HeaderText="Date of Joining" ItemStyle-Width="250" />
                            <asp:BoundField DataField="EmpDepartment" HeaderText="Department" ItemStyle-Width="250" />
                            <asp:BoundField DataField="EmpDesignation" HeaderText="Designation" ItemStyle-Width="250" />
                            <asp:BoundField DataField="RM" HeaderText="Reporting Manager" ItemStyle-Width="250" />
                            <asp:BoundField DataField="SDM" HeaderText="SDM" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Location" HeaderText="Location" ItemStyle-Width="250" />
                            <asp:BoundField DataField="DOB" HeaderText="DOB" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Tenure" HeaderText="Tenure" ItemStyle-Width="300" />
                            <asp:TemplateField HeaderText="Organizational Score">
                                <ItemTemplate>
                                    <div class="form-group">
                                        <asp:TextBox Placeholder="Enter Target" CssClass="form-control" ID="txtogrscore" runat="server" AutoComplete="Off"></asp:TextBox>
                                        <asp:Label ID="lblorgscore" runat="server" Text='<%# Eval("ORGScore") %>' Visible="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Departmental Score">
                                <ItemTemplate>
                                    <div class="form-group">
                                        <asp:TextBox Placeholder="Enter Target" CssClass="form-control" ID="txtdepScore" runat="server" AutoComplete="Off"></asp:TextBox>
                                        <asp:Label ID="lbldepScore" runat="server" Text='<%# Eval("DepScore") %>' Visible="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Individual score">
                                <ItemTemplate>
                                    <div class="form-group">
                                        <asp:Label ID="lblkpiavg" runat="server" Text='<%# Eval("KPIAVG") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Consent Rating">
                                <ItemTemplate>
                                    <div class="form-group">
                                        <asp:Label ID="lblskillavg" runat="server" Text='<%# Eval("SkillAVG") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Final Score">
                                <ItemTemplate>
                                    <div class="form-group">
                                        <asp:Label ID="lblScore" runat="server" Text='<%# Eval("FinalScore") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PMS Category">
                                <ItemTemplate>
                                    <div class="form-group">
                                        <asp:Label ID="lblpmscategory" runat="server" Text='<%# Eval("PMSCategory") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="HOD Rating">
                                <ItemTemplate>
                                    <div class="form-group">
                                        <asp:Label ID="lblhodrating" runat="server" Text='<%# Eval("HODRating") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Year" HeaderText="Year" ItemStyle-Width="250" />

                        </Columns>
                        <PagerStyle HorizontalAlign="Right" CssClass="pagination-ys" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

