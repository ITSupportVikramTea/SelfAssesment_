<%@ Page Title="HR | Add Ideal Ratings" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="AddRatings.aspx.cs" Inherits="HR_AddRatings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">  
        function Calculation() {
            var grid = document.getElementById("<%= SkillsList.ClientID%>");
            for (var i = 0; i < grid.rows.length - 1; i++) {
                var txtAmountReceive = $("input[id*=txtidealrating]")
                if (txtAmountReceive[i].value != '') {
                    alert(txtAmountReceive[i].value);
                }
            }
        }
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
        <h1>Ideal Rating
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
                    <asp:GridView ID="SkillsList" runat="server" class="table table-bordered table-striped table-responsive"
                        AllowSorting="true" PageSize="6" OnRowDataBound="OnRowDataBound" AllowPaging="true"
                        OnPageIndexChanging="OnPageIndexChanging" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="EmpName" HeaderText="Employee Name" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Competency" HeaderText="Competency" ItemStyle-Width="250" />
                            <asp:BoundField DataField="ManagementSkills" HeaderText="ManagementSkills" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="250" />
                            <asp:BoundField DataField="RatingSelf" HeaderText="Self Rating" ItemStyle-Width="250" />
                            <asp:BoundField DataField="RMRating" HeaderText="Reporting Manager Rating" ItemStyle-Width="250" />
                            <asp:BoundField DataField="HODRating" HeaderText="HOD Rating" ItemStyle-Width="250" />
                            <asp:BoundField DataField="ConsentRating" HeaderText="Consent Rating" ItemStyle-Width="250" />
                            <asp:BoundField DataField="IdealRating" HeaderText="Ideal Rating" ItemStyle-Width="250" />
                            <asp:BoundField DataField="OverallScore" HeaderText="Overall Score" ItemStyle-Width="250" />

                            <asp:TemplateField HeaderText="Ideal Rating">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtidealrating" CssClass="form-control" Placeholder="IR" AutoComplete="off" runat="server"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="SkillId" runat="server" Text='<%# Eval("Id") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Year" HeaderText ="Year" ItemStyle-Width="250" />
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" CssClass="pagination-ys" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

