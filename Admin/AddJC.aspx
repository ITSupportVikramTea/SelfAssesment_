<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="AddJC.aspx.cs" Inherits="Admin_AddJC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <section class="content-header">
        <h1>Forward JC
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
                    <asp:Button Text="Submit" CssClass="btn btn-success pull-left" ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />
                </div>

                <%-- for future to hide all these like hr interviewer assessment show on particular click --%>
                <asp:Panel ID="pnlRoleDep" runat="server">
                    <div class="row">
                        <div class="col-md-6">
                            <h4>Select JC</h4>
                            <div class="form-group">
                                <asp:DropDownList runat="server" ID="dljcs" CssClass="form-control" required>
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem Value="1">JC1 TO JC4</asp:ListItem>
                                    <asp:ListItem Value="2">JC5 TO JC7</asp:ListItem>
                                    <asp:ListItem Value="3">JC8 TO JC10</asp:ListItem>
                                    <asp:ListItem Value="4">JC11 TO JC13</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-6" id="dvEmp" runat="server">
                            <h4>Select Employee</h4>
                            <div class="form-group">
                                <asp:DropDownList runat="server" ID="dlemployees" AppendDataBoundItems="true" OnTextChanged="dlemployees_TextChanged" AutoPostBack="true" CssClass="form-control ">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div id="dvdep" runat="server" class="col-md-6">
                            <h4 runat="server">Select Department</h4>
                            <div class="form-group">
                                <asp:DropDownList runat="server" ID="dldepartments" AppendDataBoundItems="true" OnTextChanged="dldepartments_TextChanged" AutoPostBack="true" CssClass="form-control ">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </div>

                            <div class="form-group">
                                <div class="form-check">
                                    <asp:RadioButton class="form-check-input" ID="jc1" runat="server" GroupName="JC" />
                                    <lable class="form-check-label"><strong>JC1 TO JC4 </strong></lable>

                                </div>
                                <div class="form-check">
                                    <asp:RadioButton class="form-check-input" ID="jc2" runat="server" GroupName="JC"  />
                                    <lable class="form-check-label"><strong>JC5 TO JC7 </strong></lable>

                                </div>
                                <div class="form-check">
                                    <asp:RadioButton class="form-check-input" ID="jc3" runat="server" GroupName="JC"  />
                                    <lable class="form-check-label"><strong>JC8 TO JC10 </strong></lable>

                                </div>
                                <div class="form-check">
                                    <asp:RadioButton class="form-check-input" ID="jc4" runat="server" GroupName="JC"  />
                                    <lable class="form-check-label"><strong>JC11 TO JC13 </strong></lable>

                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">

                        <div class="col-lg-6 col-md-6 m-auto my-5">
                            <div class="card">
                                <div class="form-group my-2 mx-3">
                                    <label>KPI Name </label>
                                    <asp:DropDownList runat="server" ID="dlKPI" AppendDataBoundItems="true" CssClass="form-control ">
                                        <asp:ListItem Text="" Value="" />
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group mx-3">
                                    <asp:Button ID="btnAdd" Text="Add" runat="server" CssClass="btn btn-bitbucket btn-primary my-3" OnClick="btnAdd_Click" />
                                </div>
                            </div>
                        </div>


                    </div>
                </asp:Panel>


            </div>
            <!-- /.box-header -->
            <div class="box-body">
                <br />

                <div class="col-md-6">

                    <div class="form-group">
                        <label>KPI</label>
                        <asp:GridView ID="grdkpi" runat="server" class="table table-responsive table-hover table-striped  table-bordered"
                            AutoGenerateColumns="false" OnRowCommand="grdkpi_RowCommand">
                            <Columns>

                                <asp:BoundField DataField="KPI" HeaderText="KPI" ItemStyle-Width="250" />
                                <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-Width="250" />

                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <div class="btn-group">
                                            <asp:Button ID="btnRemove" Text="Remove" runat="server" CssClass="btn btn-sm btn-danger" CommandName="Remove" CommandArgument="<%# Container.DataItemIndex %>"></asp:Button>
                                            </button>
                                        </div>
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

