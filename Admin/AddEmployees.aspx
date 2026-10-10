<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true"
    CodeFile="AddEmployees.aspx.cs" Inherits="webpages_AddEmployees" Async="true" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
        <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
        <script type="text/javascript"
            src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
        <link rel="stylesheet"
            href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css'
            media="screen" />
        <link rel="stylesheet"
            href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css"
            type="text/css" />
        <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js"
            type="text/javascript"></script>

        <script src=" https://cdn.jsdelivr.net/npm/choices.js@11.2.1/public/assets/scripts/choices.min.js "></script>
        <link href=" https://cdn.jsdelivr.net/npm/choices.js@11.2.1/public/assets/styles/choices.min.css"
            rel="stylesheet">

        <script type="text/javascript">

            document.addEventListener('DOMContentLoaded', function () {
                const employees = document.getElementById('<%=txtfullname.ClientID%>');
                const choices = new Choices(employees, {
                    removeItemButton: true,
                });
            });

            $(function () {
                $('[id*=txtdoj]').datepicker({
                    changeMonth: true,
                    changeYear: true,
                    format: "dd-mm-yyyy",
                    language: "tr"
                });

                $('[id*=txtpromodate]').datepicker({
                    changeMonth: true,
                    changeYear: true,
                    format: "dd-mm-yyyy",
                    language: "tr"
                });
                $('[id*=desgchangedate]').datepicker({
                    changeMonth: true,
                    changeYear: true,
                    format: "dd-mm-yyyy",
                    language: "tr"
                });
                $('[id*=txtdob]').datepicker({
                    changeMonth: true,
                    changeYear: true,
                    format: "dd-mm-yyyy",
                    language: "tr"
                });
            });



        </script>

        <style>
            label {
                position: absolute;
                left: 3rem;
                margin-top: -10px;
                background-color: white;
                font-weight: inherit;

            }

            .form-control {
                height: 45px;
                margin-bottom: 35px;
            }
        </style>
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

        <section class="content-header">
            <h1>Add Employee
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
                    <h3 class="box-title">Forms</h3>
                </div>
                <!-- /.box-header -->
                <div class="box-body">
                    <br />

                    <div class="col-md-6 ">
                        <div class="form-group">
                            <label style="z-index: 1;">Full Name</label>
                            <asp:DropDownList runat="server" ID="txtfullname" CssClass="form-control"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <%--<div class="form-group">
                            <label>Full Name</label>
                            <asp:TextBox Placeholder="Enter Employee Name" CssClass="form-control" ID="txtfullname"
                                runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>--%>
                    <div class="form-group">
                        <label>Email ID</label>
                        <asp:TextBox Placeholder="Enter Employee Email Id" CssClass="form-control" ID="txtemailid"
                            runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Date Of Joining</label>
                        <asp:TextBox Placeholder="Enter Date Of Joining" CssClass="form-control" ID="txtdoj"
                            runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Employee Code</label>
                        <asp:TextBox Placeholder="Enter Employee Code" CssClass="form-control" ID="txtempcode"
                            runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>ERPId</label>
                        <asp:TextBox Placeholder="Enter Employee ERPId" CssClass="form-control" ID="txterpid"
                            runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>

                    <%-- <div class="form-group">
                        <label>Promotion Date</label>
                        <asp:TextBox Placeholder="Enter Promotion Date" CssClass="form-control" ID="txtpromodate"
                            runat="server" AutoComplete="Off"></asp:TextBox>
                </div>--%>
                <%--<div class="form-group">
                    <label>Designation Change Date</label>
                    <asp:TextBox Placeholder="Enter Designation Change Date" CssClass="form-control" ID="desgchangedate"
                        runat="server" AutoComplete="Off"></asp:TextBox>
            </div>--%>
            <div class="form-group">
                <label>DOB</label>
                <asp:TextBox runat="server" PlaceHolder="Enter DOB" ID="txtdob" CssClass="form-control"
                    AutoComplete="Off">
                </asp:TextBox>
            </div>

            <div class="form-group">
                <label>Status</label>
                <asp:DropDownList runat="server" ID="dlstatus" CssClass="form-control">
                    <asp:ListItem>Active</asp:ListItem>
                    <asp:ListItem>InActive</asp:ListItem>
                </asp:DropDownList>
            </div>


            <div class="form-group">
                <lable><strong>Is HOD </strong></lable>
                <asp:CheckBox ID="chkHOD" runat="server" />
                <lable><strong>Is RM </strong></lable>
                <asp:CheckBox ID="chkRM" runat="server" />
                <lable><strong>Is HR </strong></lable>
                <asp:CheckBox ID="chkHR" runat="server" />
            </div>

            <div class="form-group">
                <asp:Button ID="btnsave" runat="server" Text="Save" class="btn btn-success" OnClick="btnsave_Click" />
                <asp:Button ID="btncancel" runat="server" class="btn btn-danger" Text="Cancel"
                    OnClick="btncancel_click" />

                <asp:HiddenField ID="empid" runat="server" />
            </div>
            </div>
            <div class="col-md-6 ">
                <div class="form-group">
                    <label>Role</label>
                    <asp:DropDownList runat="server" ID="dltype" CssClass="form-control">
                        <asp:ListItem>Employee</asp:ListItem>
                        <asp:ListItem>Admin</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <%-- <div class="form-group">
                    <label>Existing CTC</label>
                    <asp:TextBox Placeholder="Enter Existing CTC" CssClass="form-control" ID="existingctc"
                        runat="server" AutoComplete="Off"></asp:TextBox>
            </div>--%>
            <div class="form-group">
                <label>HOD</label>
                <asp:TextBox Placeholder="HOD" CssClass="form-control" ID="dlhod" runat="server" AutoComplete="Off">
                </asp:TextBox>
                <%-- <asp:DropDownList runat="server" ID="dlhod" AppendDataBoundItems="true" CssClass="form-control">
                    <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>--%>
            </div>
            <div class="form-group">
                <label>Reporting Manager</label>
                <asp:TextBox Placeholder="Enter RM" CssClass="form-control" ID="dlrm" runat="server" AutoComplete="Off">
                </asp:TextBox>

                <%--<asp:DropDownList runat="server" ID="dlrm" AppendDataBoundItems="true" CssClass="form-control">
                    <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>--%>
            </div>
            <%-- <div class="form-group">
                <label>SDM</label>
                <asp:DropDownList runat="server" ID="dlsdm" AppendDataBoundItems="true" CssClass="form-control">
                    <asp:ListItem Text="" Value="" />
                </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label>ASM</label>
                    <asp:DropDownList runat="server" ID="dlasm" AppendDataBoundItems="true" CssClass="form-control">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                </div>
                </div>
                --%>
                <div class="form-group">
                    <label>Designations</label>
                    <asp:TextBox runat="server" PlaceHolder="Enter Designation" ID="dldesignation"
                        AppendDataBoundItems="true" CssClass="form-control">
                    </asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Department</label>
                    <asp:TextBox runat="server" PlaceHolder="Enter Department" ID="dlDepartment"
                        CssClass="form-control">
                    </asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Location</label>
                    <asp:TextBox runat="server" PlaceHolder="Enter Location" ID="txtlocation" CssClass="form-control">
                    </asp:TextBox>
                </div>

                <div class="form-group">
                    <label>Notice Period(In Months)</label>
                    <asp:DropDownList runat="server" ID="dlnoticeperiod" CssClass="form-control">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                    </asp:DropDownList>
                </div>

                </div>


                <!-- /.col -->



                <!-- /.col -->

                <!-- /.row -->

                </div>
                <!-- /.box-body -->

                </div>
                <!-- /.box -->


                <!-- /.row -->

        </section>
    </asp:Content>