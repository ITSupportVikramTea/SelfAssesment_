<%@ Page Title="Add Assets" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="AddAssets.aspx.cs" Inherits="Employee_AddAssets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <section class="content-header">
        <h1>Add Assets
            <small>Master</small>
            <br />

        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Forms</a></li>
        </ol>
    </section>

    <section class="content">
        <div class="box box-default">
            <div class="box-header with-border">
                <asp:Button Text="Submit" CssClass="btn btn-success pull-left" ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />
            </div>

            <br />



            <div class="box-body">
                <br />

                <div class="container-fluid">
                    <div class="col-lg-6 col-md-6 m-auto my-5">
                        <div class="card">
                            <div class="form-group my-2 mx-3">
                                <label>Asset Name </label>
                                <asp:TextBox ID="txtassetname" runat="server" CssClass="form-control my-1"></asp:TextBox>
                            </div>
                            <div class="form-group mx-3">
                                <asp:Button ID="btnAdd" Text="Add" runat="server" CssClass="btn btn-bitbucket btn-primary my-3" OnClick="btnAdd_Click" />
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-6 col-md-6 m-auto my-5">
                        <div class="card">
                            <div class="form-group">
                                <label>Assets</label>
                                <asp:GridView ID="gdAssets" runat="server" class="table table-responsive table-hover table-striped  table-bordered"
                                    OnRowCommand="gdAssets_RowCommand"
                                    AutoGenerateColumns="false">
                                    <Columns>

                                        <asp:BoundField DataField="AssetName" HeaderText="Asset Name" ItemStyle-Width="250" />
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

            </div>
        </div>
    </section>

</asp:Content>

