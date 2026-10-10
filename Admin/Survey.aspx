<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="Survey.aspx.cs" Inherits="Admin_Survey" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css'
        media="screen" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $('[id*=txtexpiores]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd-mm-yyyy",
                language: "tr"
            });

        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



    <section class="content-header">
        <h1>Add Survey
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
            <div class="box-body ">
                <br />

                <div class="col-md-4 row">
                    <div class="form-group">
                        <label>Survey Title</label>
                        <asp:TextBox Placeholder="Enter Survey Name" CssClass="form-control" ID="txtsureveytitle" runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Survey Description</label>
                        <asp:TextBox Placeholder="Enter Sureey Description" CssClass="form-control" ID="txtdesc" runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Valid Till</label>
                        <asp:TextBox Placeholder="Enter Date Of Joining" CssClass="form-control" ID="txtexpiores" runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>


                    <div class="form-group">
                        <asp:Button ID="btnsave" runat="server" Text="Save" class="btn btn-success" OnClick="btnsave_Click" />
                        <asp:Button ID="btncancel" runat="server" class="btn btn-danger" Text="Cancel" OnClick="btncancel_Click" />

                    </div>
                </div>


            </div>
            <!-- /.box-body -->

        </div>
        <!-- /.box -->


        <!-- /.row -->

    </section>




</asp:Content>

