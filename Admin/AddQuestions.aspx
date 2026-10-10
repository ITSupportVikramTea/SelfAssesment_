<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="AddQuestions.aspx.cs" Inherits="Admin_AddQuestions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css'
        media="screen" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



    <section class="content-header">
        <h1>Add Questions
            <small>Master</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Forms</a></li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">

        <div class="box box-default">
            <div class="box-header with-border">
                <div class="col-md-3">
                    <asp:Button Text="Submit" CssClass="btn btn-success pull-left" ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />
                </div>
                <div class="pull-right">
                    <asp:Button Text="Back" CssClass="btn btn-warning" ID="btnBack" runat="server" OnClick="btnBack_Click" />
                </div>
            </div>

            <br />



            <div class="box-body">
                <br />

                <div class="container-fluid">
                    <div class="col-lg-6 col-md-6 m-auto my-5">
                        <div class="card">
                            <div class="form-group my-2 mx-3">
                                <label>Question </label>
                                <asp:TextBox ID="txtquestion" runat="server" CssClass="form-control my-1"></asp:TextBox>
                            </div>

                             <div class="form-group my-2 mx-3">
                                <label>Type </label>
                                <asp:DropDownList ID="dpType" runat="server" CssClass="form-control my-1">
                                    <asp:ListItem Text="text" Value="text"></asp:ListItem>
                                    <asp:ListItem Text="select-option" Value="select-option"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                             <div class="form-group my-2 mx-3">
                                <label>Values </label>
                                <asp:TextBox TextMode="MultiLine" ID="txtValues" runat="server" CssClass="form-control my-1"></asp:TextBox>
                                 <p><b> Write values with comma seperated eg: Male, Female </b> </p>
                            </div>
                            <div class="form-group mx-3">
                                <asp:Button ID="btnAdd" Text="Add" runat="server" CssClass="btn btn-bitbucket btn-primary my-3" OnClick="btnAdd_Click" />
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-6 col-md-6 m-auto my-5">
                        <div class="card">
                            <div class="form-group">
                                <label>Questions</label>
                                <asp:GridView ID="gdQuestions" runat="server" class="table table-responsive table-hover table-striped  table-bordered"
                                    OnRowCommand="gdQuestions_RowCommand"
                                    AutoGenerateColumns="false">
                                    <Columns>

                                        <asp:BoundField DataField="Question" HeaderText="Questions" ItemStyle-Width="250" />
                                        <asp:BoundField DataField="Type" HeaderText="Type" ItemStyle-Width="250" />
                                        <asp:BoundField DataField="Options" HeaderText="Options" ItemStyle-Width="250" />
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


                    <div class="col-lg-6 col-md-6 m-auto my-5">
                        <div class="card">
                            <div class="form-group">
                                <label>Questions</label>
                                <asp:GridView ID="gdQuestionList" runat="server" class="table table-striped table-bordered table-hover"
                                    AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-Width="100" />
                                        <asp:BoundField DataField="Question" HeaderText="Question" ItemStyle-Width="300" />
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

