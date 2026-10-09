<%@ Page Title="Resignations" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="ResingList.aspx.cs" Inherits="Employee_ResingList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

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

    <script>

        function showRemarksModal(resignationId) {
            if (resignationId != "") {
                try {
                    document.getElementById('<%= ddlRemarks.ClientID %>').value = "";
                    $('#remarksModal').modal('show');
                    document.getElementById('<%= resingId.ClientID %>').innerHTML = resignationId;
                } catch (exceptionVar) {
                    console.log(exceptionVar);
                }
            }
        }

        $(function () {
            $("[id*=btnSubRemarkAndApprove]").click(function () {

                var remark = $("#<%= ddlRemarks.ClientID %>").val();
                var resignId = $("#<%= resingId.ClientID %>").text();

                $.ajax({
                    url: "ResingList.aspx/AddRemarkAndApprove",
                    type: 'post',
                    dataType: "json",
                    data: '{"resingId": "' + resignId + '", "remark": "' + remark + '"}',
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var res = data.d;
                        console.log(res);
                        alert("Approved and Remark added.!");
                        location.reload();
                    },
                    error: function (err) {
                        alert(err.d);
                    }
                });
            });
        });

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="content-header">
        <h1>Resignation
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
                    <asp:TextBox ID="txtsearchresignation" runat="server"
                        CssClass="form-control" PlaceHolder="Search">
                    </asp:TextBox>
                </div>
                <div class="col-lg-3">
                    <asp:Button CssClass="btn btn-bitbucket" Text="Search" runat="server" OnClick="Search" />
                </div>
                <div class="pull-right">
                    <asp:Button CssClass="btn btn-success" ID="addresignation" Text="Resign Now" runat="server" OnClick="addresignation_Click" />
                </div>

            </div>

            <br />

            <div class="box-body">
                <br />

                <div class="container-fluid col-md-12" style="overflow-x: auto;">
                    <asp:GridView ID="gdResignationList" runat="server" class="table table-striped table-bordered table-hover"
                        DataKeyNames="ResignationId" OnRowCommand="gdResignationList_RowCommand" OnRowDataBound="gdResignationList_RowDataBound"
                        AllowPaging="true" OnPageIndexChanging="OnPageIndexChanging" PageSize="10"
                        AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="Reason" HeaderText="Reason" ItemStyle-Width="250" />
                            <asp:BoundField DataField="EmpName" HeaderText="EmpName" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="80" />
                            <asp:BoundField DataField="DatenTime" HeaderText="Resignation Date Time" ItemStyle-Width="250" DataFormatString="{0:dd/MM/yyyy HH:mm:ss tt}" />
                            <asp:BoundField DataField="EmpId" HeaderText="EmpId" ItemStyle-Width="250" Visible="false" />
                            <asp:BoundField DataField="RM" HeaderText="RM" ItemStyle-Width="250" Visible="false" />
                            <asp:BoundField DataField="HOD" HeaderText="HOD" ItemStyle-Width="250" Visible="false" />
                            <asp:BoundField DataField="IsSubmitted" HeaderText="IsSubmitted" ItemStyle-Width="250" Visible="false" />

                            <asp:TemplateField HeaderText="WithDraw" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <asp:Button ID="btnselect" Text="Select" runat="server" CssClass="btn btn-sm btn-primary" CommandName="Select" CommandArgument='<%# Eval("ResignationId") %>'></asp:Button>
                                        <asp:Button Text="WithDraw" ID="btnWithDraw" OnClientClick="return confirm('Are you sure?');" runat="server" CssClass="btn btn-sm btn-warning " CommandName="withdraw" CommandArgument='<%# Eval("ResignationId") %>' Visible="false" />
                                        </button>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Action" ItemStyle-Width="250">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <asp:Button ID="btnApproved" Text="Approve" runat="server" CssClass="btn btn-sm btn-success" CommandName="Approve" CommandArgument='<%# Eval("ResignationId") %>' OnClientClick='<%# "showRemarksModal(" + Eval("ResignationId") + "); return false;" %>' Visible="false"></asp:Button>
                                        <asp:Button Text="Reject" ID="btnReject" runat="server" CssClass="btn btn-sm btn-danger " CommandName="Reject" CommandArgument='<%# Eval("ResignationId") %>' Visible="false" />

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
        <!-- Modal Code -->
        <div id="remarksModalContainer">
            <div id="remarksModal" class="modal fade" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">Add Remarks & Approve</h4>
                        </div>
                        <div class="modal-body">
                            <asp:Label ID="resingId" runat="server" CssClass="form-control" Style="display: none;"></asp:Label>
                            <asp:DropDownList ID="ddlRemarks" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Select Remark" Value=""></asp:ListItem>
                                <asp:ListItem Text="Absconding" Value="Absconding"></asp:ListItem>
                                <asp:ListItem Text="Benefits" Value="Benefits"></asp:ListItem>
                                <asp:ListItem Text="Dissatisfied/Management" Value="Dissatisfied/Management"></asp:ListItem>
                                <asp:ListItem Text="Family Responsibilities" Value="Family Responsibilities"></asp:ListItem>
                                <asp:ListItem Text="For Further Studies" Value="For Further Studies"></asp:ListItem>
                                <asp:ListItem Text="Health Retirement" Value="Health Retirement"></asp:ListItem>
                                <asp:ListItem Text="Job Advancement" Value="Job Advancement"></asp:ListItem>
                                <asp:ListItem Text="Personal" Value="Personal"></asp:ListItem>
                                <asp:ListItem Text="Relocation" Value="Relocation"></asp:ListItem>
                                <asp:ListItem Text="Retain By Last Company" Value="Retain By Last Company"></asp:ListItem>
                                <asp:ListItem Text="Salary" Value="Salary"></asp:ListItem>
                                <asp:ListItem Text="Started Own Business" Value="Started Own Business"></asp:ListItem>
                                <asp:ListItem Text="Traveling Issue" Value="Traveling Issue"></asp:ListItem>
                                <asp:ListItem Text="Backedout" Value="Backedout"></asp:ListItem>
                                <asp:ListItem Text="Market Issue like Product Availability is not there" Value="Market Issue like Product Availability is not there"></asp:ListItem>
                                <asp:ListItem Text="Market Issue like No Credit Facility is there" Value="Market Issue like No Credit Facility is there"></asp:ListItem>
                                <asp:ListItem Text="Lack of Support & guidance from RM" Value="Lack of Support & guidance from RM"></asp:ListItem>
                                <asp:ListItem Text="Dealer Cum Employee" Value="Dealer Cum Employee"></asp:ListItem>
                                <asp:ListItem Text="Dismissal" Value="Dismissal"></asp:ListItem>
                                <asp:ListItem Text="Job Eliminated" Value="Job Eliminated"></asp:ListItem>
                                <asp:ListItem Text="Termination" Value="Termination"></asp:ListItem>
                                <asp:ListItem Text="Asked to Go" Value="Asked to Go"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnSubRemarkAndApprove" runat="server" Text="Submit" CssClass="btn btn-primary" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" onclick="hideRemarksModal();">Cancel</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

</asp:Content>

