<%@ Page Title="HR | Job Applications" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="HR_Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css'
        media="screen" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>

    <script src="../Content/ckeditor/ckeditor.js"></script>
    <script src="Scripts/jquery.dynDateTime.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('[id*=btnmodel]').click(function () {
                var row = $(this).closest("tr");
                var message2 = (row[0].rowIndex - 1);
                $('#hfrowin').val(message2);
                return true;
            });

            $('[id*=btnseehrassment]').click(function () {
                var cid = $(this).closest("tr").find("input[type=hidden][id*=key]").val();
                $('#tblassessment tr td').remove();
                $.ajax({
                    url: 'Dashboard.aspx/getCandidateAssessment',
                    method: 'post',
                    dataType: "json",
                    data: '{cid: ' + cid + '}',
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var res = data.d;
                        $("#tblassessment").append("<tr><td>" + res[0] + "</td><td>" + res[1] + "</td> <td>" + res[2] +
                            "</td><td>" + res[3] + "</td><td>" + res[4] + "</td><td>" + res[5] + "</td><td>" + res[6] + "</td></tr>");

                    },
                    error: function (err) {
                        alert(err);
                    }
                });
            })

        });
        function openInNewTab(fileName) {
            var url = '' + fileName; // Replace 'path_to_your_file/' with the actual path to your file
            var win = window.open(url, '_blank');
            win.focus();
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
        <h1>Job Applications
            <small>Master</small>
            <br />

        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Forms</a></li>
            <li><a href="DirectKRA.aspx" style="color:red;font-weight:bold">Important Notice : Employee KRA.</a></li>
        </ol>
    </section>


    <section class="content">
        <div class="box box-default">
            <div class="box-header with-border">

                <div class="col-lg-3">
                    <asp:TextBox ID="txtsearchuser" runat="server"
                        CssClass="form-control" PlaceHolder="Search User,Position,SDM,HQ">
                    </asp:TextBox>
                </div>
                <div class="col-lg-3">
                    <asp:Button CssClass="btn btn-bitbucket" Text="Search" runat="server" OnClick="Search" />
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
                                <div class="col-md-12" style="overflow-x: auto;">
                                    <div class="form-group">
                                        <table  class="table" id="tblassessment">
                                            <tr>
                                                <th>Appearance</th>
                                                <th>Job Knoledge</th>
                                                <th>Relavant Experience</th>
                                                <th>Matuarity</th>
                                                <th>Communication Skill</th>
                                                <th>Good Points</th>
                                                <th>Bad Points</th>
                                            </tr>

                                        </table>

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


            <br />

            <div class="box-body">
                <br />

                <div class="col-md-12" style="overflow-x: auto">
                    <asp:GridView ID="gdJobAppList" runat="server" class="table table-striped table-bordered table-hover"
                        DataKeyNames="Id" OnRowCommand="gdJobAppList_rowcommand" OnRowDataBound="gdJobAppList_RowDataBound"
                        AllowPaging="true" OnPageIndexChanging="OnPageIndexChanging" PageSize="6"
                        AutoGenerateColumns="false">
                        <Columns>

                            <asp:BoundField DataField="FullName" HeaderText="Full Name" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Department" HeaderText="Department" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Position" HeaderText="Position" ItemStyle-Width="250" />
                            <asp:BoundField DataField="SDM" HeaderText="SDM" ItemStyle-Width="250" />
                            <asp:BoundField DataField="HQ" HeaderText="HQ" ItemStyle-Width="250" />
                            <asp:BoundField DataField="CreatedDate" HeaderText="Applied Date" ItemStyle-Width="250" />
                            <asp:BoundField DataField="InterviewStatus" HeaderText="Interview Status" ItemStyle-Width="250" />
                            <asp:BoundField DataField="RefferedPerson" HeaderText="Reffered Person" ItemStyle-Width="250" />
                            <asp:BoundField DataField="HRFeedback" HeaderText="HR Feedback" ItemStyle-Width="250" />
                            <asp:BoundField DataField="IntFeedback" HeaderText="Int. Feedback" ItemStyle-Width="250" />
                            <asp:BoundField DataField="FinalResult" HeaderText="Final Result" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Interviewer" HeaderText="Interviewer" Visible="false" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Appearance" HeaderText="Appearance" Visible="false" ItemStyle-Width="250" />
                            <asp:BoundField DataField="RatingType" HeaderText="RatingType" Visible="false" ItemStyle-Width="250" />

                            <asp:TemplateField HeaderText="CV">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <%--<asp:Button Text="View CV" ID="btncv1" runat="server" CssClass="btn btn-sm btn-info " CommandName="CVView" CommandArgument='<%# Eval("FileName") %>' />
                                        </button>--%>
                                       <asp:Button Text="View CV" ID="btncv1" runat="server" CssClass="btn btn-sm btn-info" OnClientClick='<%# "openInNewTab(\"/CVUploads/" + Eval("FileName") + "\"); return false;" %>' />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
    <ItemTemplate>
        <asp:Button ID="btnSelect" runat="server" Text="Select" CommandName="Select" 
            CommandArgument='<%# Container.DataItemIndex %>' />
    </ItemTemplate>
</asp:TemplateField>


                            <asp:TemplateField HeaderText="HR Remarks">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <asp:Button Text="Assessment" ID="btnhrassmnt" runat="server" CssClass="btn btn-sm btn-bitbucket" CommandName="hrassessment" CommandArgument='<%# Eval("Id") %>' />
                                        </button>
                                        <asp:Button OnClientClick="return false" Visible="false" Text="View Assessment" ID="btnseehrassment" runat="server" CssClass="btn btn-sm btn-github" data-toggle="modal" data-target="#myModal" />
                                    </div>
                                    <asp:HiddenField ID="key" runat="server" Value='<%#Eval("Id") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Schedule">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <asp:Button ID="btnschedule" Text="Schedule" runat="server" CssClass="btn btn-sm btn-primary" CommandName="Schedule" CommandArgument='<%# Eval("Id") %>'></asp:Button>
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

