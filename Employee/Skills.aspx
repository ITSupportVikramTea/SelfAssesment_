<%@ Page Title="Employee | Skills" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="Skills.aspx.cs" Inherits="Employee_Skills" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css'
        media="screen" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>
    <link rel="stylesheet"
href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css" />
  

    <script>
        $(document).ready(function () {
            $('[id*=btnsubmit]').click(function () {
                var selfRating = parseInt($('[id*=txtratingself]').val());
                var rmRating = parseInt($('[id*=txtratingrm]').val());
                var consentRating = parseInt($('[id*=txtconsentrating]').val());
                var hodRating = parseInt($('[id*=txthodrating]').val());

                if (selfRating < 1 || selfRating > 5 ||
                    rmRating < 1 || rmRating > 5 ||
                    consentRating < 1 || consentRating > 5 ||
                    hodRating < 1 || hodRating > 5) {
                    alert('Add ratings must be between 1 and 5.');
                    return false;
                }
            });
        });
    </script>

    <style>

body {
    background: #f5f7fb;
}

/* GRID */

.table {
    background: white;
    border-radius: 14px;
    overflow: hidden;
    border-collapse: separate !important;
    border-spacing: 0;
    box-shadow: 0 2px 12px rgba(0,0,0,0.06);
    font-size: 13px;
}

.table th {
    background: linear-gradient(90deg,#1e293b,#334155);
    color: white;
    text-align: center;
    font-weight: 600;
    padding: 12px !important;
    border: none !important;
   /*white-space: nowrap;*/
}

.table td {
    padding: 10px !important;
    vertical-align: middle !important;
    border-color: #edf1f7 !important;
}

.table-striped tbody tr:nth-of-type(odd) {
    background-color: #fafbfd;
}

.table tbody tr:hover {
    background-color: #eef4ff !important;
    transition: 0.2s;
}

/* TEXTBOX */

.form-control {
    border-radius: 10px !important;
  /*  border: 1px solid #dbe2ea;*/
    min-height: 36px;
    text-align: center;
    font-size: 13px;
    transition: 0.2s;
}

.form-control:focus {
    border-color: #2563eb !important;
    box-shadow: 0 0 0 3px rgba(37,99,235,.12) !important;
}

/* PAGINATION */

.pagination-ys {
    margin-top: 18px;
    text-align: right;
}

.pagination-ys table {
    margin-left: auto;
}

.pagination-ys table > tbody > tr > td {
    display: inline-block;
    margin: 0 2px;
}

.pagination-ys table > tbody > tr > td > a,
.pagination-ys table > tbody > tr > td > span {

    min-width: 36px;
    height: 36px;
    line-height: 36px;

    display: inline-block;
    text-align: center;

    border-radius: 10px;

    border: 1px solid #dbe2ea;

    background: white;
    color: #475569;

    font-weight: 500;

    text-decoration: none;

    transition: 0.2s;
}

.pagination-ys table > tbody > tr > td > a:hover {
    background: #2563eb;
    color: white;
    border-color: #2563eb;
}

.pagination-ys table > tbody > tr > td > span {

    background: #2563eb;
    color: white;
    border-color: #2563eb;
}

/* DESCRIPTION */

.nowrap-management td:nth-child(3),
.nowrap-management th:nth-child(3)
{
    white-space: nowrap;
}

/* RESPONSIVE */

.table-responsive {
    border-radius: 14px;
    overflow: auto;
}

/* TOGGLE BUTTON */

.btn-outline-secondary {

    border-radius: 10px;
    padding: 7px 16px;

    background: white;
    border: 1px solid #dbe2ea;

    transition: 0.2s;
}

.btn-outline-secondary:hover {

    background: #2563eb;
    color: white;
    border-color: #2563eb;
}

.table {
    border-collapse: collapse !important;
}

/* FORCE borders on merged rating cells */

.table td[rowspan] {
    position: relative;
}

/* Consent Rating */
.table td:nth-child(6)[rowspan]::after,

/* RM Rating */
.table td:nth-child(7)[rowspan]::after,

/* HOD Rating */
.table td:nth-child(8)[rowspan]::after
{
    content: "";
    position: absolute;
    top: 0;
    right: 0;
    width: 1px;
    height: 100%;
    /*background: #94a3b8;*/
    z-index: 2;
}

/* Overall Score stronger end border */

.table td:nth-child(9)[rowspan]
{
   /* border-right: 2px solid #64748b !important;*/
}

/* left start border */

.table td:nth-child(6)[rowspan]
{
   /* border-left: 2px solid #64748b !important;*/
}


</style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="content-header">
        <h1>Skills
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


                <div class="col-lg-3">
                    <asp:TextBox ID="txtsearchuser" runat="server"
                        CssClass="form-control" PlaceHolder="Search Employee">
                    </asp:TextBox>
                </div>
                <div class="col-lg-3">
                    <asp:Button CssClass="btn btn-bitbucket" Text="Search" runat="server" OnClick="Search" />
                </div>

                <div class="col-md-3">

                    <div class="form-group">
                        <asp:DropDownList runat="server" ID="dlyear" CssClass="form-control " OnSelectedIndexChanged="dlyears_SelectedIndexChanged" AutoPostBack="true">

                            <asp:ListItem Value="" Text="Select Year"></asp:ListItem>
                        </asp:DropDownList>

                    </div>
                </div>

                <div class="col-md-3">
                    <div class="pull-right">
                        <asp:Button Text="Submit" CssClass="btn btn-success" ID="btnsubmit" runat="server" OnClick="btnsubmit_click" />
                    </div>
                </div>
            </div>

            <br />
            <div class="box-body">
                <div class="text-bold text-capitalize col-lg-12">
                    <h2 class="text-center"><strong>Skills Rating </strong></h2>
                    <div class="text-white col-lg-6">
                        <h4>1 - Cannot Do and Cannot not learn </h4>
                        <h4>2 - Cannot Do but he can be trained </h4>
                        <h4>3 - Can Do but needs Supervision </h4>
                    </div>
                    <div class="col-lg-6">
                        <h4>4 - Can Do without Supervision </h4>
                        <h4>5 - Fully Competent and can train others </h4>
                    </div>
                </div>


                <br />
                <br />
                <br />
                <br />
                <br />
                <br />

                <div class="col-md-12" style="overflow-x: auto; overflow-y: auto">
                    <div style="margin-bottom:10px;text-align:right;">
    
    <asp:LinkButton ID="btnToggleDescription"
        runat="server"
        OnClick="btnToggleDescription_Click"
        CssClass="btn btn-sm btn-outline-secondary">

        <i class="fa fa-eye"></i> Description

    </asp:LinkButton>

</div>
                    <asp:GridView ID="SkillsList" runat="server" class="table table-bordered table-striped table-responsive"
                        AllowSorting="true" OnDataBound="SkillsList_DataBound" OnRowDataBound="SkillsList_RowDataBound" AllowPaging="true" PageSize="10" OnPageIndexChanging="OnPageIndexChanging" 
                        AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="EmpName" HeaderText="Employee Name" ItemStyle-Width="250" Visible="false"  />
                            <asp:BoundField DataField="Competency" HeaderText="Competency" ItemStyle-Width="250" />
                            <asp:BoundField DataField="ManagementSkills" HeaderText="ManagementSkills" ItemStyle-Width="250" />
                            <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="250" ItemStyle-Wrap="true" />
                            <asp:BoundField DataField="RatingSelf" HeaderText="Self Rating" ItemStyle-Width="80" />
                            <asp:BoundField DataField="ConsentRating" HeaderText="Consent Rating" ItemStyle-Width="80" />
                            <asp:BoundField DataField="RMRating" HeaderText="Reporting Manager Rating" ItemStyle-Width="80" />
                            <asp:BoundField DataField="HODRating" HeaderText="HOD Rating" ItemStyle-Width="80" />
                            <asp:BoundField DataField="OverallScore" HeaderText="Overall Score" ItemStyle-Width="80" />
                            <asp:BoundField DataField="HOD" HeaderText="HOD" ItemStyle-Width="250" Visible="false" />
                            <asp:BoundField DataField="RM" HeaderText="RM" ItemStyle-Width="250" Visible="false" />
                            <asp:BoundField DataField="SDM" HeaderText="SDM" ItemStyle-Width="250" Visible="false" />
                            <asp:BoundField DataField="ASM" HeaderText="ASM" ItemStyle-Width="250" Visible="false" />

                            <asp:TemplateField HeaderText="Add Self Rating">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtratingself" CssClass="form-control" PlaceHolder="SR" AutoComplete="off" runat="server"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Add RM Rating">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtratingrm" CssClass="form-control" PlaceHolder="RM" AutoComplete="off" runat="server"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Add HOD Rating">
                                <ItemTemplate>
                                    <asp:TextBox ID="txthodrating" CssClass="form-control" PlaceHolder="HOD" AutoComplete="off" runat="server"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="SkillId" runat="server" Text='<%# Eval("Id") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Add Consent Rating">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtconsentrating" CssClass="form-control" PlaceHolder="CR" AutoComplete="off" runat="server"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Year" HeaderText="Year" ItemStyle-Width="250" />
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" CssClass="pagination-ys" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </section>

</asp:Content>

