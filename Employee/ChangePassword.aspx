<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="HR_ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css'
        media="screen" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>


    <script>
        $(document).ready(function () {
            $('[id*=btnsave]').click(function () {
                var pass1 = $('[id*=txtpass1]').val();
                var pass2 = $('[id*=txtpass2]').val();
                if (pass1 != pass2) {
                    $('[id*=pass2]').text("Password does not match");
                    return false;
                }
                
            });

        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>Change Password
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
                        <label>Old Password</label>
                        <asp:TextBox Placeholder="Old Password" TextMode="Password" CssClass="form-control" ID="txtoldpass" runat="server" AutoComplete="Off"></asp:TextBox>

                    </div>
                    <div class="form-group">
                        <label>New Password</label>
                        <asp:TextBox Placeholder="New Password" TextMode="Password" CssClass="form-control" ID="txtpass1" runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Confirm Password</label>
                        <asp:TextBox Placeholder="Confirm Password" TextMode="Password" CssClass="form-control" ID="txtpass2" runat="server" AutoComplete="Off"></asp:TextBox>
                        <span class="text-danger" id="pass2"></span>
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btnsave" runat="server" Text="Save" class="btn btn-success" OnClick="btnsave_Click" />
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

