<%@ Page Title="Employee | Dashboard" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="DashboardEmployee.aspx.cs" Inherits="Employee_Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css'
        media="screen" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>


    <style>
        .model {
            left: -21%;
        }

        .modal-content {
            left: -25%;
            width: 950px;
            height: auto;
        }

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

              .blink-ani {
                   animation: fadeBlink 1.5s linear infinite;
              }

              @keyframes fadeBlink {
  0% {
    opacity: 1; 
  }
  50% {
    opacity: 0; 
  }
  100% {
    opacity: 1;
  }
}
              .alert-success {
                  background-color: #d4edda !important;
  color: #155724 !important;
              }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">

        <h1>Dashboard
            <small>Master</small>
            <br />
        </h1>
        <div>

            <h5 class="text-center">Hi <b><u><% =FirstName %></u></b> Welcome Back! </h5>
            <ol class="breadcrumb" style="margin-top: 6rem;">
                <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
                <li><a href="#">Forms</a></li>
                <li class="blink-ani"> <a href="kpi.aspx" style="color:red;font-weight:bold;font-size: 18px;margin-left:19rem;"><img src="../Content/image-removebg-preview.png" style="    width: 197px;
    height: 66px;"/>Add KPIs. </a></li> 
            </ol>
        </div>
        <div class="alert alert-info" hidden>
            <p>
                The website is in its beta version, so you may encounter some problems. If you find any issues in using the website,
please contact itsupport@vikramtea.com.
            </p>
        </div>
        <center>
        <div class="alert alert-success" style="height: 58px;">
         <%--  <center><h4>आपसे निवेदन हैं की आपके सभी वर्तमान काम इस फॉर्म मैं संक्षित रूप मैं कल शनिवार १० जनवरी २०२६ तक भरे |</h4></center> --%>
            <center style="margin-top: 5px;"><h4>Add Your Self-Rating On Or Before 5 May 2026.</h4></center>
        </div>
            </center>
    </section>


    <section class="content">

        <div class="box box-default" style="visibility: hidden;">
            <div class="box-header with-border">

                <div class="col-lg-3">
                    <asp:TextBox ID="txtsearchemp" runat="server"
                        CssClass="form-control" PlaceHolder="Search">
                    </asp:TextBox>
                </div>
                <div class="btn-group">
                    <div class="form-group">
                        <asp:Button CssClass="btn btn-bitbucket" Text="Search User in Skills" runat="server" OnClick="SearchSkillUser" />
                        <asp:Button CssClass="btn btn-bitbucket" Text="Search User in KPI" runat="server" OnClick="SearchKPIUser" />
                    </div>
                </div>
                <div class="btn-group pull-right">
                    <div class="form-group">
                        <asp:Button ID="btnjobapp" Text="Job Applications" runat="server" CssClass="btn btn-primary" OnClick="btnjobapp_click" />
                        <asp:Button ID="btnChangepass" CssClass="btn btn-success" Text="Change Password" runat="server" OnClick="btnChangepass_click" />
                        <asp:Button Text="Download" CssClass="btn btn-github" ID="btndownload" runat="server" OnClick="ExportToExcel" />

                    </div>
                </div>
            </div>



            <div class="modal fade"
                id="exampleModal"
                tabindex="-1"
                role="dialog"
                aria-labelledby="exampleModalLabel"
                aria-hidden="true">
                <div class="modal-dialog"
                    role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <!-- w-100 class so that header
                div covers 100% width of parent div -->
                            <h5 class="modal-title w-100"
                                id="exampleModalLabel">Schemes
                            </h5>
                            <button type="button"
                                class="close"
                                data-dismiss="modal"
                                aria-label="Close">
                                <span aria-hidden="true">×
                                </span>
                            </button>
                        </div>

                        <!--Modal body with image-->
                        <div class="img-fluid ">
                            <img src="../Content/employee_scheme.PNG" style="width: 100%;" />
                        </div>

                        <div class="modal-footer">
                            <button type="button"
                                class="btn btn-danger"
                                data-dismiss="modal">
                                Close
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="box-body" runat="server">
                 
                <div class="col-md-6" style="overflow-x: auto;">
                    <h3 class="text-center"><b>Skills Report </b></h3>
                    <asp:GridView ID="gdPendingDetails" runat="server" class="table table-bordered table-striped table-responsive"
                        OnRowDataBound="gdPendingDetails_RowDataBound"
                        AllowPaging="true" OnPageIndexChanging="gdPendingDetails_PageIndexChanging" PageSize="10"
                        AllowSorting="true" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="EmpName" HeaderText="Employee Name" ItemStyle-Width="300" />
                            <asp:BoundField DataField="PendindgSelfRating" HeaderText="Pendindg Self Rating" ItemStyle-Width="30" />
                            <asp:BoundField DataField="PendingRMRating" HeaderText="Pending RMRating" ItemStyle-Width="30" />
                            <asp:BoundField DataField="PendingHODRating" HeaderText="Pending HODRating" ItemStyle-Width="30" />
                            <asp:BoundField DataField="ConsentRating" HeaderText="Consent Rating" ItemStyle-Width="30" />
                            <asp:BoundField DataField="TotalPenRating" HeaderText="Total Pending Rating" ItemStyle-Width="30" />

                        </Columns>
                        <PagerStyle HorizontalAlign="Right" CssClass="pagination-ys" />
                    </asp:GridView>
                </div>

                <div class="col-md-6" style="overflow-x: auto;">
                    <h3 class="text-center"><b>KPI Report </b></h3>
                    <asp:GridView ID="gdKPIPending" runat="server" class="table table-bordered table-striped table-responsive"
                        OnRowDataBound="gdKPIPending_RowDataBound"
                        AllowPaging="true" OnPageIndexChanging="gdKPIPending_PageIndexChanging" PageSize="10"
                        AllowSorting="true" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="EmpName" HeaderText="Employee Name" ItemStyle-Width="300" />
                            <asp:BoundField DataField="TotalPenKPI" HeaderText="Total Pending KPI" ItemStyle-Width="30" />

                        </Columns>
                        <PagerStyle HorizontalAlign="Right" CssClass="pagination-ys" />
                    </asp:GridView>
                </div>
              
            </div>
        </div>
    </section>


</asp:Content>

