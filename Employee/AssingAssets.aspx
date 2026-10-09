<%@ Page Title="" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="AssingAssets.aspx.cs" Inherits="Employee_AssingAssets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="content-header">
        <h1>Assing Assets
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
                    <asp:Button Text="AssignedAssetsList" CssClass="btn btn-primary pull-right" ID="assgnAssets" runat="server" OnClick="assgnAssets_Click" />
                </div>
               

            </div>
            <!-- /.box-header -->
            <div class="box-body">
                <br />

                <div class="col-md-6">

                    <div class="form-group">
                        <label>Assign Assets</label>
                        <asp:DropDownList runat="server" ID="dlAssets" CssClass="form-control " OnSelectedIndexChanged="dlAssets_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>Assets</label>
                        <asp:GridView ID="grdAssets" runat="server" class="table table-responsive table-hover table-striped  table-bordered"
                            AutoGenerateColumns="false">
                            <Columns>

                                <asp:BoundField DataField="AssetName" HeaderText="Assets" ItemStyle-Width="250" />
                                 <asp:TemplateField HeaderText="Asset Id" ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetId" runat="server" Text='<%# Eval("AssetId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
                <div class="col-md-6 ">
                    <div class="form-group">
                        <label>Search Employee</label>
                        <asp:TextBox ToolTip="Search" CssClass="form-control" ID="txtautosearch" runat="server" AutoPostBack="true" OnTextChanged="txtautosearch_TextChanged"></asp:TextBox>

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
                        <asp:Button ID="btnsubmit" runat="server" Text="AssignAssets" class="btn btn-info" OnClick="btnsubmit_Click" />
                    </div>
                </div>
                <div class="col-md-6 ">
                    <div class="form-group">
                        <label>Selected Employee(s)</label>
                        <asp:GridView ID="grdAssingAssets" runat="server" class="table table-responsive table-hover table-striped  table-bordered"
                            AutoGenerateColumns="false">
                            <Columns>

                                <asp:BoundField DataField="AssetName" HeaderText="Asset Name" ItemStyle-Width="250" />
                                <asp:BoundField DataField="EmpName" HeaderText="Empployee Name" ItemStyle-Width="250" />
                                <asp:TemplateField HeaderText="Employee Code" ItemStyle-Width="150" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="EmployeeId" runat="server" Text='<%# Eval("EmpId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Asset Id" ItemStyle-Width="150" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="AssetId" runat="server" Text='<%# Eval("AssetId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                

                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
            </div>
        </div>
        <!-- /.box -->

        <!-- /.row -->

    </section>

</asp:Content>

