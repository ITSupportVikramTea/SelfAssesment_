<%@ Page Title="" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="CandidateDocs.aspx.cs" Inherits="HR_CandidateDocs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="content-header">
        <h1>Candidate Documents
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
                <h3 class="text-blue"><b>Documents of <% =CandName %></b></h3>
            </div>

            <br />

            <div class="box-body">
                <br />

                <div class="col-md-12" style="overflow-x: auto">
                    <asp:GridView ID="gdDocuments" runat="server" class="table table-striped table-bordered table-hover"
                        OnRowCommand="gdDocuments_rowcommand"
                        AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="Aadhar Card">
                                <ItemTemplate>
                                    <div class="col-md-2">
                                        <asp:Button Text="View" ID="btnaadhar" runat="server" CssClass="btn btn-bitbucket " CommandName="AadharCard" CommandArgument='<%# Eval("AadharCard") %>' />
                                        </button>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="10th Mark Sheet">
                                <ItemTemplate>
                                    <div class="col-md-2">
                                        <asp:Button Text="View" ID="btntenth" runat="server" CssClass="btn btn-bitbucket " CommandName="tenthCert" CommandArgument='<%# Eval("TenthCert") %>' />
                                        </button>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="12th Mark Sheet">
                                <ItemTemplate>
                                    <div class="col-md-2">
                                        <asp:Button Text="View" ID="btntwevle" runat="server" CssClass="btn btn-bitbucket " CommandName="TwevleCert" CommandArgument='<%# Eval("TwevleCert") %>' />
                                        </button>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Graduation Cert.">
                                <ItemTemplate>
                                    <div class="col-md-2">
                                        <asp:Button Text="View" ID="btngraduation" runat="server" CssClass="btn btn-bitbucket " CommandName="GraduationCert" CommandArgument='<%# Eval("GraduationCert") %>' />
                                        </button>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Experience Cert.">
                                <ItemTemplate>
                                    <div class="col-md-2">
                                        <asp:Button Text="View" ID="btnexpcert" runat="server" CssClass="btn btn-bitbucket " CommandName="ExpCert" CommandArgument='<%# Eval("ExpCert") %>' />
                                        </button>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Salary Slip">
                                <ItemTemplate>
                                    <div class="col-md-2">
                                        <asp:Button Text="View" ID="btnsalslip" runat="server" CssClass="btn btn-bitbucket " CommandName="SalarySlip" CommandArgument='<%# Eval("SalarySlip") %>' />
                                        </button>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Driving Licence">
                                <ItemTemplate>
                                    <div class="col-md-2">
                                        <asp:Button Text="View" ID="btndriving" runat="server" CssClass="btn btn-bitbucket " CommandName="DrivingLicence" CommandArgument='<%# Eval("DrivingLicence") %>' />
                                        </button>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </section>

</asp:Content>

