<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Log in | ESS</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <!-- Bootstrap 3.3.7 -->
    <link rel="stylesheet" href="bower_components/bootstrap/dist/css/bootstrap.min.css" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="bower_components/font-awesome/css/font-awesome.min.css" />
    <!-- Ionicons -->
    <link rel="stylesheet" href="bower_components/Ionicons/css/ionicons.min.css" />

    <!-- Theme style -->
    <link rel="stylesheet" href="dist/css/AdminLTE.min.css" />
    <!-- iCheck -->
    <link rel="stylesheet" href="plugins/iCheck/square/blue.css" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic" />
    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>

    <script>

</script>

</head>
<body class="hold-transition login-page">
    <form runat="server">
        <div class="login-box">

            <!-- /.login-logo -->
            <div class="login-box-body">
                <div class="login-logo" width="180px" height="90">
                    <img src="/Content/vtppl_logo_2.jpg" width="200" height="150" alt="VTPPL Logo" />
                    <h3>Enter email below</h3>
                </div>
                <p><b>Note:</b> it may take 5 minutes to recieve email. </p>
                <div class="form-group has-feedback">
                    <asp:TextBox ID="txtemail" runat="server" class="form-control" placeholder="Emaild Id"></asp:TextBox>
                    <span class="glyphicon glyphicon-envelope form-control-feedback"></span>
                </div>

                <div class="row">
                    <div class="col-xs-8">
                    </div>
                    <!-- /.col -->
                    <div class="col-xs-4">
                        <asp:Button ID="btnSendEmail" runat="server" Text="Reset" class="btn btn-primary btn-block btn-flat" OnClick="btnSendEmail_Click" />
                    </div>
                    <!-- /.col -->
                </div>


            </div>
            <!-- /.login-box-body -->
        </div>
        <!-- /.login-box -->
    </form>
    <!-- jQuery 3 -->
    <script src="../../bower_components/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap 3.3.7 -->
    <script src="../../bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- iCheck -->
    <script src="../../plugins/iCheck/icheck.min.js"></script>

</body>
</html>
