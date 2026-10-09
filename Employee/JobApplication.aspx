<%@ Page Title="Employee | Job Applications" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="JobApplication.aspx.cs" Inherits="Employee_JobApplication" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.11.2.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css'
        media="screen" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>
    <script>

        $(document).ready(function () {
            $('[id*=btnseeintassment]').click(function () {
                var cid = $(this).closest("tr").find("input[type=hidden][id*=key]").val();
                $('#tblassessment tr td').remove();
                $.ajax({
                    url: 'JobApplication.aspx/getCandidateAssessment',
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

    </script>

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
                <div class="pull-right">
                    <asp:Button CssClass="btn btn-success" ID="btnshortcands" Text="Shortlisted" runat="server" OnClick="btnshortcands_Click" />
                </div>
            </div>

            <div class="modal fade" id="myModal">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">Assessment</h4>
                            <button type="button" class="close" data-dismiss="modal">×</button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12" style="overflow-x: auto;">
                                    <div class="form-group">
                                        <table class="table" id="tblassessment">
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
                        AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="FullName" HeaderText="Full Name" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Department" HeaderText="Department" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Position" HeaderText="Position" ItemStyle-Width="250" />
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
                                        <asp:Button Text="View CV" ID="btncv1" runat="server" CssClass="btn btn-sm btn-info " CommandName="viewcv" CommandArgument='<%# Eval("FileName") %>' />
                                        </button>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Interviewer Remarks">
                                <ItemTemplate>
                                    <div class="card-body btn-group">
                                        <asp:Button Visible="false" Text="Assessment" ID="btnintassess" runat="server" CssClass="btn btn-sm btn-bitbucket" CommandName="intassess" CommandArgument='<%# Eval("Id") %>' />
                                        </button>
                                        <asp:Button OnClientClick="return false" Visible="false" Text="See Assessment" ID="btnseeintassment" runat="server" CssClass="btn btn-sm btn-github" data-toggle="modal" data-target="#myModal" />
                                    </div>
                                    <asp:HiddenField ID="key" runat="server" Value='<%#Eval("Id") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </section>

</asp:Content>

