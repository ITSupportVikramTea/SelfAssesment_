<%@ Page Title="" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="AddSkills.aspx.cs" Inherits="webpages_AddSkills" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

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
                           <asp:TextBox Placeholder="Enter Competency" CssClass="form-control" ID="txtcompetency" runat="server" AutoComplete="Off" ></asp:TextBox>   
                      </div>
                      <div class="form-group">  
                          <label>Management Skills</label>
                           <asp:TextBox Placeholder="Enter Management Skills" CssClass="form-control" ID="txtmngtskills" runat="server" AutoComplete="Off" ></asp:TextBox>   
                      </div>
                       <div class="form-group">  
                          <label>Description</label>
                           <asp:TextBox Placeholder="Enter Description" CssClass="form-control" ID="txtdesc" runat="server" AutoComplete="Off" ></asp:TextBox>   
                      </div>
                        <div class="form-group">  
                          <label>Email Id</label>
                           <asp:TextBox Placeholder="Enter Email Id" CssClass="form-control" ID="txtemailid" runat="server" AutoComplete="Off" ></asp:TextBox>   
                      </div>
                      </div>
                <div class="col-md-6 ">
                      <div class="form-group">  
                          <label>Self Rating</label>
                           <asp:TextBox Placeholder="Enter Self Rating" CssClass="form-control" ID="txtselfrating" runat="server" AutoComplete="Off" ></asp:TextBox>   
                      </div>
                      <div class="form-group">  
                          <label>Reporting Manager Rating</label>
                           <asp:TextBox Placeholder="Enter Reporting Manager Rating" CssClass="form-control" ID="txtrm" runat="server" AutoComplete="Off" ></asp:TextBox>   
                      </div>
                      <div class="form-group">  
                          <label>HOD Rating</label>
                           <asp:TextBox Placeholder="Enter HOD Rating" CssClass="form-control" ID="txthod" runat="server" AutoComplete="Off" ></asp:TextBox>   
                      </div>
                      <div class="form-group">  
                          <label>Overall Score</label>
                           <asp:TextBox Placeholder="Enter Self Rating" CssClass="form-control" ID="txtoverallscore" runat="server" AutoComplete="Off" ></asp:TextBox>   
                      </div>
                      <div class="form-group">  
                          <label>HOD</label>                     
                           <asp:DropDownList runat="server" id="dlhod" CssClass="form-control"> 
                                
                           </asp:DropDownList>                       
                      </div>
                
                      
                <div class="col-md-6 ">
                       <div class="form-group">   
                            <asp:Button ID="btnupdate" runat="server" Text="Update" class="btn btn-success"  OnClick="btnUpdate_Click" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" class="btn btn-success"  OnClick="btnsave_Click" />
                           <asp:HiddenField ID="empid" runat="server" />
                           </div>
                     </div>

                <div class="col-md-12">
                    <asp:GridView ID="EmployeeList" runat="server" class="table table-striped"
 OnRowCommand="EmployeeList_RowCommand"
                        AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="EmpName" HeaderText="Employee Name" ItemStyle-Width="250" />
                            <asp:BoundField DataField="EmpDepartment" HeaderText="Department" ItemStyle-Width="250" />
                            <asp:BoundField DataField="EmpDesignation" HeaderText="Designation" ItemStyle-Width="250" />
                            <asp:BoundField DataField="HOD" HeaderText="HOD" ItemStyle-Width="250" />
                             <asp:TemplateField>
            <ItemTemplate>
                <asp:Button Text="Select" runat="server" CommandName='<%# Eval("Id") %>' CommandArgument="<%# Container.DataItemIndex %>" />
            </ItemTemplate>
        </asp:TemplateField>
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

