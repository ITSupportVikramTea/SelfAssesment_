<%@ Page Title="HR | Add Vacancy" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="AddVacancies.aspx.cs" Inherits="HR_AddVacancies" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="content-header">
        <h1>Add Consaltancy
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
                        <label>Department</label>
                        <asp:TextBox Placeholder="Enter Department" CssClass="form-control" ID="dlDepartment" runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Position</label>
                        <asp:TextBox Placeholder="Avalable Position" CssClass="form-control" ID="txtposition" runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Vacancies</label>
                        <asp:TextBox Placeholder="No.Of Vacancies" CssClass="form-control" ID="txtnoofvacancies" runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-6 ">
                    <div class="form-group">
                        <label>ZONE</label>
                        <asp:TextBox Placeholder="ZONE" CssClass="form-control" ID="txtzone" runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>SDM</label>
                        <asp:TextBox Placeholder="SDM" CssClass="form-control" ID="txtsdm" runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>HQ</label>
                        <asp:TextBox Placeholder="Head Quarter" CssClass="form-control" ID="dlhq" runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>

                </div>
                <div class="form-group">
                        <asp:Button ID="btnsave" runat="server" Text="Save" class="btn btn-success" OnClick="btnsave_Click" />
                        <asp:Button ID="btncancel" runat="server" class="btn btn-danger" Text="Cancel" OnClick="btncancel_click" />
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

