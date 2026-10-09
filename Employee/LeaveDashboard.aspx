<%@ Page Title="Leave | Dashboard" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="LeaveDashboard.aspx.cs" Inherits="Employee_LeaveDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .flex-container > div {
            margin:10px;
            padding:10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <section class="content-header">
       <h1>Dashboard
           <small>Master</small>
           <br/>
       </h1>
   </section>
    <section class="content">
        <div class="box box-header with-border">
            <div class="flex-container" style="display:flex">
                <div style="border: groove;
    border-width: thin; box-shadow: 4px 4px;">
                    <h3 class="text-center">Absent</h3>
                    <h3 class="text-center">12</h3>
                </div>
                            <div style="border: groove;
border-width: thin; box-shadow: 4px 4px;">
                <h3 class="text-center">Present</h3>
                <h3 class="text-center">100</h3>
            </div>
            </div>
                            <div class="col-md-6" style="overflow-x: auto;">
                    <h3 class="text-center">Total Leave</h3>
  <asp:GridView ID="leaves" runat="server" class="table table-bordered table-striped table-responsive">
      <Columns>
       <%--   <asp:BoundField DataField="employeeName" HeaderText="Emp Name" />--%>
      </Columns>
  </asp:GridView>
</div>
        </div>
    </section>
</asp:Content>

