<%@ Page Title="Admin | Add Skills" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="AddSkills.aspx.cs" Inherits="webpages_AddSkills" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!--
<script src="http://code.jquery.com/jquery-1.11.0.min.js"></script>
<script type="text/javascript">
    $(function () {
        $('#lsskills').multiselect({
            includeSelectAllOption: true
        });
    });
</script>>
    -->


    <section class="content-header">
        <h1>Add Employee Skills
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
                <br />
                <br />
                <div class="col-md-6 ">
                    <div class="form-group">
                        <label>Competency</label>
                        <asp:TextBox Placeholder="Enter Competency" CssClass="form-control" ID="txtcompetency" runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Management Skills</label>
                        <asp:TextBox Placeholder="Enter Skills" CssClass="form-control" ID="txtmgmtskills" runat="server" AutoComplete="Off"></asp:TextBox>

                    </div>
                </div>
                <div class="col-md-6 ">
                    <div class="form-group">
                        <label>Description</label>
                        <asp:TextBox Placeholder="Enter Description" CssClass="form-control" ID="txtdesc" runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Email Id</label>
                        <asp:TextBox Placeholder="Enter Email Id" CssClass="form-control" ID="txtemailid" runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>
                    </div>
                <div class="col-md-6 ">
                    <div class="form-group">
                        <asp:Button ID="btnsave" runat="server" Text="Save" class="btn btn-success" OnClick="btnsave_click" />
                        <asp:Button ID="btncancel" runat="server" class="btn btn-danger" Text="Cancel" OnClick="btncancel_click" />
                       <!-- <asp:HiddenField ID="Id" runat="server" />-->
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


