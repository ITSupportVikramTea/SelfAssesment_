<%@ Page Title="KPI" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="KRA.aspx.cs" Inherits="Employee_KRA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css'
        media="screen" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>


    <script>
        $(document).ready(function () {
            $('[id*=KRAList] tr').each(function () {
                $(this).find('td:eq(8), th:eq(8)').hide();
                $(this).find('td:eq(9), th:eq(9)').hide();
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
        <h1>KPI
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
                <div class="row">
                    <div class="col-lg-3">
                        <asp:TextBox ID="txtsearchuser" runat="server"
                            CssClass="form-control" PlaceHolder="Search Employee, KPI">
                        </asp:TextBox>
                    </div>
                    <div class="col-lg-3">
                        <asp:Button CssClass="btn btn-bitbucket" Text="Search" runat="server" OnClick="Search" />
                    </div>

                    <div class="col-md-2">

                        <div class="form-group">
                            <asp:DropDownList runat="server" ID="dlyear" CssClass="form-control " OnSelectedIndexChanged="dlyear_SelectedIndexChanged" AutoPostBack="true">

                                <asp:ListItem Value="" Text="Select Year"></asp:ListItem>
                            </asp:DropDownList>

                        </div>
                    </div>


                    <div class="col-lg-2">
                        <div class="form-group">
                            <asp:DropDownList runat="server" ID="dljcs" CssClass="form-control" OnTextChanged="dljcs_TextChanged" AutoPostBack="true">
                                <asp:ListItem Value="1">JC1 TO JC4</asp:ListItem>
                                <asp:ListItem Value="2">JC5 TO JC7</asp:ListItem>
                                <asp:ListItem Value="3">JC8 TO JC10</asp:ListItem>
                                <asp:ListItem Value="4">JC11 TO JC13</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        
                    </div>
                     <div class="col-lg-2">
                        <div class="form-group">
                             <asp:Button Text="Export Data" CssClass="btn btn-info" ID="btnexport" runat="server" OnClick="btnexport_Click" />
                            </div>
                         </div>
                    <div class="col-lg-3 pull-right">
                        
                        <asp:Button Text="Employee's KPI" CssClass="btn btn-primary" ID="btnBlankKPI" runat="server" OnClick="btnBlankKPI_Click" />
                        <asp:Button Text="Submit" CssClass="btn btn-success" ID="btnsubmit" runat="server" OnClick="btnsubmit_click" />
                    </div>
                </div>

            </div>
            <br />
            <div class="box-body">
                <h4>Score:<%=kpiavg %> </h4>

                <br />

                <div class="col-md-12" style="overflow-x: auto">
                    <asp:GridView ID="KRAList" runat="server" class="table table-bordered table-striped table-responsive"
                        OnRowDataBound="KRAList_RowDataBound" AllowPaging="true" PageSize="10" OnPageIndexChanging="OnPageIndexChanging"
                        AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="EmpName" HeaderText="Employee" ItemStyle-Width="250" />
                            <asp:BoundField DataField="KPI" HeaderText="KPI" ItemStyle-Width="300" />
                            <asp:BoundField DataField="Weightage" HeaderText="Weightage" ItemStyle-Width="100" />
                            <asp:BoundField DataField="UOM" HeaderText="UOM" ItemStyle-Width="50" />
                            <asp:BoundField DataField="Target" HeaderText="Target" ItemStyle-Width="100" />
                            <asp:BoundField DataField="Achievement" HeaderText="Achievement" ItemStyle-Width="100" />
                            <asp:BoundField DataField="Scores" HeaderText="Score" ItemStyle-Width="100" />
                            <asp:BoundField DataField="SDM" HeaderText="SDM" Visible="false" />
                            <asp:BoundField DataField="ASM" HeaderText="ASM" Visible="false" />
                            <asp:BoundField DataField="HOD" HeaderText="HOD" Visible="false" />
                            <asp:BoundField DataField="RM" HeaderText="HOD" Visible="false" />

                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtFormula" runat="server" Text='<%# Eval("Formula") %>' Visible="false"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Add Achievement">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtachievement" CssClass="form-control" AutoComplete="off" runat="server"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="KpiId" runat="server" Text='<%# Eval("Id") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtTarget" runat="server" Text='<%# Eval("Target") %>' Visible="false"></asp:TextBox>
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

