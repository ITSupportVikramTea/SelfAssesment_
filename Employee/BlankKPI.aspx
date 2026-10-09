<%@ Page Title="Blank KPI" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="BlankKPI.aspx.cs" Inherits="Employee_BlankKPI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css'
        media="screen" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>

    <script>
        $(document).ready(function () {
            $('[id*=btnsubmit]').click(function () {
                var val = $('[id*=txtweightage]').val();
                if (val > 100) {
                    alert('Weightage should not be greater than 100!');
                    return false;
                }
            });
        });
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
        <h1>Blank KPI's
            <small>Master</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Forms</a></li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">

        <!-- SELECT2 EXAMPLE -->
        <div class="box box-default">
            <div class="box-header with-border">
                <div class="col-lg-3">
                    <asp:TextBox ID="txtsearchuser" runat="server"
                        CssClass="form-control" PlaceHolder="Search Employee, KPI">
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



            <!-- /.box-header -->
            <div class="box-body">
                <br />
                <br />
                <br />
                <div class="col-md-12" style="overflow-x: auto">
                    <asp:GridView ID="gdBlankKPI" runat="server" class="table table-striped table-bordered table-hover"
                        AllowPaging="true" OnRowDataBound="gdBlankKPI_RowDataBound" OnPageIndexChanging="OnPageIndexChanging" PageSize="10"
                        DataKeyNames="Id"
                        AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-Width="50" />
                            <asp:BoundField DataField="EmpName" HeaderText="Employee Name" ItemStyle-Width="250" />
                            <asp:BoundField DataField="KPI" HeaderText="KPI" ItemStyle-Width="300" />

                            <asp:TemplateField HeaderText="Target">
                                <ItemTemplate>
                                    <div class="form-group">
                                        <asp:TextBox Placeholder="Enter Target" CssClass="form-control" ID="txttarget" runat="server" AutoComplete="Off"></asp:TextBox>
                                        <asp:Label ID="lbltarget" runat="server" Text='<%# Eval("Target") %>' Visible="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Weightage">
                                <ItemTemplate>
                                    <div class="form-group">
                                        <asp:TextBox Placeholder="Enter Weightage" CssClass="form-control" ID="txtweightage" runat="server" AutoComplete="Off"></asp:TextBox>
                                        <asp:Label ID="lblwieghtage" runat="server" Text='<%# Eval("Weightage") %>' Visible="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="UOM">
                                <ItemTemplate>
                                    <div class="form-group">
                                        <asp:TextBox Placeholder="Enter UOM" CssClass="form-control" ID="txtuom" runat="server" AutoComplete="Off"></asp:TextBox>
                                        <asp:Label ID="lbluom" runat="server" Text='<%# Eval("UOM") %>' Visible="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Achievment">
                                <ItemTemplate>
                                    <div class="form-group">
                                        <asp:TextBox Placeholder="Enter Achievement" CssClass="form-control" ID="txtachievement" runat="server" AutoComplete="Off"></asp:TextBox>
                                        <asp:Label ID="lblachievment" runat="server" Text='<%# Eval("Achievement") %>' Visible="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtFormula" runat="server" Text='<%# Eval("Formula") %>' Visible="false"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" CssClass="pagination-ys" />
                    </asp:GridView>
                </div>

                <!-- /.box-body -->

            </div>
            <!-- /.box -->
        </div>


        <!-- /.row -->

    </section>


</asp:Content>

