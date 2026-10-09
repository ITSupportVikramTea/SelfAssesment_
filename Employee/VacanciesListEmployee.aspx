<%@ Page Title="Employee | Vacancies List" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="VacanciesListEmployee.aspx.cs" Inherits="Employee_VacanciesList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css'
        media="screen" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>
    <script src="Scripts/jquery-1.3.2.min.js"></script>
    <script>

        $(document).ready(function () {
            //$('[id*=txtsearchvacancy]').keyup(function () {
            //    _this = this;
            //    $.each($('[id*=VacanciesList] tbody').find("tr:has(td)"), function () {
            //        if ($(this).text().toLowerCase().indexOf($(_this).val().toLowerCase()) != -1)
            //            $(this).show();
            //        else
            //            $(this).hide();

            //    });
            //});

            $('[id*=btnmodel]').click(function () {
                var row = $(this).closest("tr");
                //var message = "Row Index: " + (row[0].rowIndex - 1);
                var message2 = (row[0].rowIndex - 1);
                //alert(message2);
                $('#txtvcnid').val(message2);
                return true;
            })

            $('[id*=btnUpload]').click(function () {
                var fname = $("[id*=txtfname]").val();
                var mobno = $("[id*=txtmobno]").val();
                var fl = $("[id*=FileUpload1]").val();
                if (fname == '') {
                    $("[id*=rmme1]").remove()
                    $("[id*=txtfname] ").after('<span id="rmme1" class="error text-danger">Please enter name.</span>');
                    return false;
                }
                else {
                    $("[id*=rmme1]").remove()
                }
                if (mobno == '') {
                    $("[id*=rmme2]").remove()
                    $("[id*=txtmobno] ").after('<span id="rmme2" class="rmme error text-danger">Please enter mobile no.</span>');
                    return false;
                }
                else {
                    $("[id*=rmme2]").remove()
                }
                if (fl == '') {
                    $("[id*=rmme3]").remove()
                    $("[id*=FileUpload1] ").after('<span id="rmme3" class="error text-danger">Please select file.</span>');
                    return false;
                }
                else {
                    $("[id*=rmme3]").remove()
                }
                return true;
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
        <h1>Vacancies
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
                        CssClass="form-control" PlaceHolder="Search Vacancy">
                    </asp:TextBox>

                </div>
                <div class="col-lg-3">
                    <asp:Button CssClass="btn btn-bitbucket" Text="Search" runat="server" OnClick="Search" />
                </div>


                <div class="modal fade" id="myModal">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4 class="modal-title">Fill Form</h4>
                                <button type="button" class="close" data-dismiss="modal">×</button>
                            </div>
                            <div class="modal-body">
                                <div class="row">

                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <asp:HiddenField ID="txtvcnid" runat="server" ClientIDMode="Static"></asp:HiddenField>
                                            <lable>First Name </lable>
                                            <asp:TextBox ID="txtfname" placeholder="Enter First Name" class="form-control" runat="server">
                                            </asp:TextBox><span id="fname"></span>
                                        </div>
                                        <div class="form-group">
                                            <lable>Middle Name </lable>
                                            <asp:TextBox ID="txtmname" placeholder="Enter Middle Name" class="form-control" runat="server">
                                            </asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <lable>Last Name </lable>
                                            <asp:TextBox ID="txtlname" placeholder="Enter Last Name" class="form-control" runat="server">
                                            </asp:TextBox>
                                        </div>
                                        <div class="form-group input-group-append">
                                            <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-info" Text="Submit" OnClick="btnUpload_Click" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <lable>Email Id </lable>
                                            <asp:TextBox ID="txtemail" placeholder="Enter Email" class="form-control" runat="server">
                                            </asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <lable>Mobile </lable>
                                            <asp:TextBox ID="txtmobno" placeholder="Enter Mobile Number" class="form-control" runat="server">
                                            </asp:TextBox><span id="mobno"></span>
                                        </div>
                                        
                                        <div class="form-group">
                                            <label>Attach CV</label>
                                            <div class="input-group">
                                                <div class="custom-file">
                                                    <asp:FileUpload ID="FileUpload1" CssClass="custom-file-input" runat="server" />
                                                    <label class="custom-file-label"></label>
                                                </div>
                                                <label id="filename"></label>
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
                    <asp:GridView ID="VacanciesList" runat="server" class="table table-striped table-bordered table-hover"
                        DataKeyNames="Id" AllowPaging="true" OnRowDataBound="OnRowDataBound"
                        OnPageIndexChanging="OnPageIndexChanging" PageSize="6"
                        AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="Department" HeaderText="Department" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Position" HeaderText="Position" ItemStyle-Width="250" />
                            <asp:BoundField DataField="NoOfVC" HeaderText="No. Of Vacancies" ItemStyle-Width="250" />
                            <asp:BoundField DataField="HQ" HeaderText="HQ" ItemStyle-Width="250" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="card-body">
                                        <button type="button" id="btnmodel" class="btn btn-primary pull-right" data-toggle="modal" data-target="#myModal">
                                            </i>Submit CV 
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
