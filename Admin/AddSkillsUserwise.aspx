<%@ Page Title="Admin | Assing Skills" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="AddSkillsUserwise.aspx.cs" Inherits="Admin_AddSkillsUserwise" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="content-header">
        <h1>Assing Skills
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


                <div class="col-lg-12">
                    <asp:Button Text="Add Skills" CssClass="btn btn-success pull-left" ID="btnaddskills" runat="server" OnClick="btnaddskills_click" />
                    <asp:Button Text="List" CssClass="btn btn-primary pull-right" ID="btnskilllist" runat="server" OnClick="btnskilllist_click" />
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <h4>Select Role</h4>
                        <div class="form-group">
                            <asp:DropDownList runat="server" ID="dlroles" CssClass="form-control " OnTextChanged="dlroles_TextChanged" AutoPostBack="true">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="1">HOD</asp:ListItem>
                                <asp:ListItem Value="2">RM</asp:ListItem>
                                <asp:ListItem Value="3">Employee</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <h4>Select Department</h4>
                        <div class="form-group">
                            <asp:DropDownList runat="server" ID="dldepartments" AppendDataBoundItems="true" CssClass="form-control ">
                                <asp:ListItem Text="" Value="" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <!-- /.box-header -->
            <div class="box-body">
                <br />

                <div class="col-md-6">

                    <div class="form-group">
                        <label>Select Designations</label>

                        <asp:DropDownList runat="server" ID="dlDesg" AppendDataBoundItems="true" CssClass="form-control" OnSelectedIndexChanged="dlDesg_SelectedIndexChanged" OnTextChanged="dlDesg_TextChanged" AutoPostBack="true">
                            <asp:ListItem Text="Select" Value="" />
                        </asp:DropDownList>

                    </div>

                    <div class="form-group">
                        <label>Assign Skills</label>
                        <asp:DropDownList runat="server" ID="dlskills" CssClass="form-control " AppendDataBoundItems="true" OnSelectedIndexChanged="dlskills_SelectedIndexChanged" AutoPostBack="true">
                         <asp:ListItem Text="Select" Value="0" />
                        </asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <label>Skills</label>
                        <asp:GridView ID="grdskills" runat="server" class="table table-responsive table-hover table-striped  table-bordered"
                            AutoGenerateColumns="false">
                            <Columns>

                                <asp:BoundField DataField="Competency" HeaderText="Competency" ItemStyle-Width="250" />
                                <asp:BoundField DataField="ManagementSkills" HeaderText="Management Skills" ItemStyle-Width="250" />
                                <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="250" />

                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
                <div class="col-md-6 ">
                    <div class="form-group">
                        <label>Search Employee</label>
                        <asp:TextBox ToolTip="enter name to search" CssClass="form-control" ID="txtautosearch" runat="server" AutoPostBack="true" OnTextChanged="txtautosearch_TextChanged"></asp:TextBox>

                    </div>
                    <div class="form-group " style="max-height: 600px; overflow: auto; display: inline-block;">
                        <label>Select Employee(s)</label>
                        <asp:GridView ID="EmployeeList" runat="server" class="table table-responsive table-hover table-striped  table-bordered"
                            AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Select" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkRow" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="EmpName" HeaderText="Employee Name" ItemStyle-Width="250" />
                                <asp:TemplateField HeaderText="Employee Code" ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmpId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
                <div class="col-md-12 ">
                    <div class="form-group">
                        <asp:Button ID="btnAddEmp" runat="server" Text="Add" class="btn btn-success" OnClick="btnAddEmp_Click" />
                        <asp:Button ID="btnsubmit" runat="server" Text="Assign Skills" class="btn btn-info" OnClick="btnsubmit_Click" />
                    </div>
                </div>
                <div class="col-md-6 ">
                    <div class="form-group">
                        <label>Selected Employee(s)</label>
                        <asp:GridView ID="grdAssignSkills" runat="server" class="table table-responsive table-hover table-striped  table-bordered"
                            AutoGenerateColumns="false">
                            <Columns>

                                <asp:BoundField DataField="ManagementSkills" HeaderText="ManagementSkills" ItemStyle-Width="250" />
                                <asp:BoundField DataField="EmpName" HeaderText="EmpName" ItemStyle-Width="250" />
                                <asp:TemplateField HeaderText="Employee Code" ItemStyle-Width="150" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="EmpId" runat="server" Text='<%# Eval("EmpId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SkillsId" ItemStyle-Width="150" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="SkillId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>



                            </Columns>
                        </asp:GridView>

                    </div>
                </div>

            </div>
            <!-- /.box -->
        </div>

        <!-- /.row -->

    </section>

</asp:Content>

