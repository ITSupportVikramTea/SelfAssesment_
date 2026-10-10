<%@ Page Title="Admin | Assinged Skills List" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="AssingedSkillsList.aspx.cs" Inherits="Admin_AssingedSkillsList" %>

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

                .table td {
    vertical-align: middle !important;
}

.table th
{
    text-align: center !important;
    vertical-align: middle !important;
}


.btn-primary {

    border-radius: 8px !important;

    background: #2563eb !important;

    border: none !important;

    padding: 6px 14px !important;
}

.btn-primary:hover {

    background: #1d4ed8 !important;
}



.bordered-ratings td,
.bordered-ratings th
{
    border: 1px solid #94a3b8 !important;
}


    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Literal ID="litPreview" runat="server" />
    <section class="content-header">
        <h1>Assigned Skills List
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
                    <asp:TextBox ID="txtsearchskill" runat="server"
                        CssClass="form-control" PlaceHolder="Search Skills">
                    </asp:TextBox>

                </div>
                <div class="col-lg-3">

                    <asp:Button CssClass="btn btn-bitbucket" Text="Search" runat="server" OnClick="Search" />


                </div>
                <div class="col-lg-2">
                    <div class="form-group">
                        <asp:DropDownList runat="server" ID="dlyear" CssClass="form-control " OnSelectedIndexChanged="dlyears_SelectedIndexChanged" AutoPostBack="true">

                            <asp:ListItem Value="" Text=""></asp:ListItem>

                        </asp:DropDownList>
                    </div>
                </div>


                <div class="card-body btn-group pull-right">
                    <asp:Button Text="Forward Previous Data" CssClass="btn btn-info" ID="btnForPrev" runat="server" OnClick="btnForPrev_Click" />
                    <asp:Button Text="Assing Skills" CssClass="btn btn-success" ID="btnassgnskill" runat="server" OnClick="btnassgnskills_click" />
                    <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModal">
                        <i class="fa fa-plus-circle"></i>Import Excel  
                    </button>
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
            </div>

            <br />



            <div class="box-body">
                <br />

                <div class="col-md-12" style="overflow-x: auto">
                    <asp:Button ID="btnExportExcel"
    runat="server"
    Text="Export Excel"
    CssClass="btn btn-success"
    OnClick="btnExportExcel_Click" />
                    <div style="margin-bottom:10px;text-align:right;">

    <asp:LinkButton ID="btnToggleDescriptionAssigned"
        runat="server"
        CssClass="btn btn-sm btn-outline-secondary"
        OnClick="btnToggleDescriptionAssigned_Click">

        <i class="fa fa-eye"></i> Description

    </asp:LinkButton>

</div>
                    <asp:GridView ID="AssingSkillList" runat="server" class="table table-bordered table-striped table-responsive"
                        OnRowCommand="AssingSkillList_RowCommand" AllowPaging="true" OnRowDataBound="AssingSkillList_RowDataBound"
                        OnPageIndexChanging="OnPageIndexChanging" PageSize="10"
                        AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="EmpName" HeaderText="Employee Name" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Competency" HeaderText="Competency" ItemStyle-Width="250" />
                            <asp:BoundField DataField="ManagementSkills" HeaderText="Management Skills" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Year" HeaderText="Year" ItemStyle-Width="250" />
                            <asp:TemplateField HeaderText="Action" ItemStyle-Width="120">
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

