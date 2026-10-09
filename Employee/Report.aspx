<%@ Page Title="" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="Report.aspx.cs" Inherits="webpages_Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <style>
   
  .row{background-color: white;} 
  .alt-row{background-color: gray;}
  
</style> 
    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
<script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
<link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css'
    media="screen" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css"/>
<script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>
     <script type="text/javascript">
         $(function () {
             $('[id*=txtfromdate]').datepicker({
                 changeMonth: true,
                 changeYear: true,
                 format: "dd-mm-yyyy",
                 language: "tr"
             });
         });
         $(function () {
             $('[id*=txttodate]').datepicker({
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
        <h1>Add Attendance
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
                  <div class="col-md-12 form-group">
                      <div class="col-md-2">                          
                           <asp:TextBox Placeholder="Select From Date" CssClass="form-control" ID="txtfromdate" runat="server" AutoComplete="Off" ></asp:TextBox>   
                          
                      </div>
                       <div class="col-md-2">                          
                           <asp:TextBox Placeholder="Select To Date"  CssClass="form-control" ID="txttodate" runat="server" AutoComplete="Off"></asp:TextBox>                        
                      </div>
                       <div class="col-md-2">                          
                           <asp:DropDownList runat="server" id="Attype" CssClass="form-control"> 
                               <asp:ListItem Value="All">All</asp:ListItem>
                               <asp:ListItem Value="Y">Present</asp:ListItem>
                               <asp:ListItem Value="N">Absent</asp:ListItem>
                           </asp:DropDownList>                       
                      </div>
                       <div class="col-md-2">   
                            <asp:Button ID="btnsearch" runat="server" Text="Search" class="btn btn-success"  OnClick="btnsearch_Click" />
                           <asp:Button Text="Export" OnClick="btnExport_Click" runat="server" id="btnExport" class="btn btn-danger" />

                           </div>
                     </div>

                <div class="col-md-6">
                    <asp:GridView ID="AttendanceData" runat="server"    class="table"
 
                        AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="Adate" HeaderText="Date" ItemStyle-Width="250" />
                            <asp:BoundField DataField="EmployeeName" HeaderText="Employee Name" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Department" HeaderText="Department" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Role" HeaderText="Role" ItemStyle-Width="250" />
                            <asp:BoundField DataField="InTime" HeaderText="InTime" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Location" HeaderText="Location" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="250" />
                        </Columns>
                    </asp:GridView>

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


