<%@ Page Title="HR | Add Consultancy" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="HRAddConsultancy.aspx.cs" Inherits="Employee_HRAddConsultancy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                        <label>Name</label>
                        <asp:TextBox Placeholder="Enter Name" CssClass="form-control" ID="txtname" runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Email ID</label>
                        <asp:TextBox Placeholder="Enter Employee Email Id" CssClass="form-control" ID="txtemailid" runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>
                    
                </div>
                <div class="col-md-6 ">
                    <div class="form-group">
                        <label>Location</label>
                        <asp:TextBox Placeholder="Enter Location" CssClass="form-control" ID="txtlocation" runat="server" AutoComplete="Off"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Type</label>
                        <asp:DropDownList runat="server" ID="dltype" CssClass="form-control">
                            <asp:ListItem>Consultancy</asp:ListItem>
                        </asp:DropDownList>
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

