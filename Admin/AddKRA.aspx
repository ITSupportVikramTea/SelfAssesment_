<%@ Page Title="Admin | Add KRA" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="AddKRA.aspx.cs" Inherits="Admin_AddKRA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

     <section class="content-header">
        <h1>Add KPI
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
                          <label>KPI *</label>
                           <asp:TextBox Placeholder="Enter KPI" CssClass="form-control" ID="txtkpi" runat="server" AutoComplete="Off" ></asp:TextBox>   
                      </div>
                      <div class="form-group">  
                          <label>Taget</label>
                           <asp:TextBox Placeholder="Enter Target" CssClass="form-control" ID="txttarget" runat="server" AutoComplete="Off" ></asp:TextBox>   
                      </div>
                      </div>
                <div class="col-md-6 ">
                       <div class="form-group">  
                          <label>Weightage</label>
                           <asp:TextBox Placeholder="Enter Weightage" CssClass="form-control" ID="txtweightage" runat="server" AutoComplete="Off" ></asp:TextBox>   
                      </div>

                        <div class="form-group">  
                          <label>UOM</label>
                           <asp:DropDownList runat="server" ID="dluom" CssClass="form-control">
                               <asp:ListItem> </asp:ListItem>
                           </asp:DropDownList>  
                      </div>
                      </div>
                 <div class="col-md-6 ">
                       <div class="form-group">   
                            <asp:Button ID="btnsave" runat="server" Text="Save" class="btn btn-success" OnClick="btnsave_Click" />
                           <asp:Button ID="btncancel" runat="server" class="btn btn-danger" Text="Cancel" OnClick="btncancel_click"/>
                           <!--<asp:HiddenField ID="empid" runat="server" />-->
                           </div>
                     </div>
     
            <!-- /.box-body -->

        </div>
        <!-- /.box -->
            </div>


        <!-- /.row -->

    </section>

</asp:Content>

