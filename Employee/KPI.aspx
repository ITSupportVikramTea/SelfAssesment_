<%@ Page Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="KPI.aspx.cs" Inherits="Employee_KPI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   
    <style>

        body {
            background: #f5f7fb;
            font-family: 'Segoe UI', sans-serif;
            color: #1f2937;
        }

        .page-container {
            max-width: 1200px;
            margin: 40px auto;
            padding: 0 15px;
        }

        .main-card {
            background: #ffffff;
            border-radius: 14px;
            box-shadow: 0 2px 15px rgba(0,0,0,0.06);
            overflow: hidden;
            border: 1px solid #e5e7eb;
        }

        .card-header-custom {
            padding: 22px 30px;
            border-bottom: 1px solid #edf0f5;
            background: #ffffff;
        }

        .title {
            font-size: 24px;
            font-weight: 600;
            margin-bottom: 4px;
            color: #111827;
        }

        .subtitle {
            color: #6b7280;
            font-size: 14px;
        }

        .year-badge {
            background: #eef2ff;
            color: #4338ca;
            padding: 8px 18px;
            border-radius: 8px;
            font-size: 14px;
            font-weight: 600;
            margin-top: 10px;
        }

        .table-section {
            padding: 25px 30px;
        }

        .table {
            margin-bottom: 0;
        }

        .table thead th {
            background: #f9fafb;
            color: #374151;
            font-size: 13px;
            font-weight: 600;
            border-bottom: 1px solid #e5e7eb;
            padding: 16px;
            text-transform: uppercase;
            letter-spacing: .4px;
        }

        .table tbody td {
            padding: 14px 16px;
            vertical-align: middle;
            border-bottom: 1px solid #f1f5f9;
        }

        .form-control-custom {
            border: 1px solid #d1d5db;
            border-radius: 8px;
            height: 42px;
            padding: 10px 14px;
            width: 100%;
            font-size: 14px;
            transition: all .2s ease;
            background: #ffffff;
        }

        .form-control-custom:focus {
            border-color: #2563eb;
            outline: none;
            box-shadow: 0 0 0 3px rgba(37,99,235,0.10);
        }

        .readonly-input {
            background: #f9fafb;
            color: #4b5563;
            font-weight: 600;
        }

        .bottom-section {
            padding: 22px 30px;
            border-top: 1px solid #edf0f5;
            background: #fcfcfd;
        }

        .btn-custom {
            min-width: 140px;
            height: 42px;
            border-radius: 8px;
            font-size: 14px;
            font-weight: 600;
            border: none;
            transition: .2s ease;
        }

        .btn-add {
            background: #ffffff;
            border: 1px solid #d1d5db;
            color: #374151;
        }

        .btn-add:hover {
            background: #f3f4f6;
        }

        .btn-save {
            background: #2563eb;
            color: white;
        }

        .btn-save:hover {
            background: #1d4ed8;
        }

        .info-text {
            font-size: 13px;
            color: #6b7280;
        }

        .success-message {
            display: block;
            margin-top: 15px;
            font-size: 14px;
            font-weight: 600;
            color: #059669;
        }

        @media(max-width:768px) {

            .card-header-custom {
                padding: 20px;
            }

            .table-section {
                padding: 20px;
            }

            .bottom-section {
                padding: 20px;
            }
        }

     



.table td {

    vertical-align: middle !important;
}

.table-hover tbody tr:hover {

    background: #f8fafc;
}

.btn {

    border-radius: 8px !important;
}

.card {

    border-radius: 12px;
}
.content-wrapper {
    padding-bottom: 5px !important;
}

    </style>
    </asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

   <section class="content-header">
     <h1>KPIs
         <small>Master</small>
         <br />

     </h1>
     <ol class="breadcrumb">
         <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
         <li><a href="#">Forms</a></li>
     </ol>
 </section>

        <div class="page-container">

            <div class="main-card">

                <!-- Header -->

                <div class="card-header-custom">

                    <div class="d-flex justify-content-between align-items-center flex-wrap gap-3">

                        <div>

                            <div class="title">
                               New Employee KPIs
                            </div>

                            <div class="subtitle">
                                Create KPIs
                            </div>

                        </div>

                        <div class="year-badge mt-3">
                           You Can Add Maximum 10 KPIs - Add Weightage To Every KPI Between (1-10) - All KPIs Weightage Total Must Be 100. 
                        </div>

                    </div>

                </div>

                <!-- Table -->

                <div class="table-section">

                    <div class="table-responsive">

                        <asp:GridView ID="gvKPI"
                            runat="server"
                            AutoGenerateColumns="False"
                            CssClass="table align-middle"
                            GridLines="None"
                            ShowHeader="true">

                            <Columns>

                                <asp:TemplateField HeaderText="KPI">

                                    <ItemTemplate>

                                        <asp:TextBox ID="txtKPI"
                                            runat="server"
                                            CssClass="form-control-custom"
                                            placeholder="Enter KPI">
                                        </asp:TextBox>

                                    </ItemTemplate>

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Weightage">

                                    <ItemTemplate>

                                        <asp:TextBox ID="txtWeightage"
                                            runat="server"
                                            CssClass="form-control-custom"
                                            placeholder="Enter Weightage"  >
                                        </asp:TextBox>

                                    </ItemTemplate>

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="UOM">

                                    <ItemTemplate>

                                       <%-- <asp:TextBox ID="txtUOM"
                                            runat="server"
                                            CssClass="form-control-custom"
                                            placeholder="Enter UOM">
                                        </asp:TextBox>--%>
                                          <asp:DropdownList ID="txtUOM"
                                                             runat="server"
                                                    CssClass="form-control-custom"
                                           placeholder="Enter UOM">
                                              <asp:ListItem>a</asp:ListItem>
                                                                                            <asp:ListItem>b</asp:ListItem>
                                                                                            <asp:ListItem>c</asp:ListItem>
                                          </asp:DropdownList>
                                    </ItemTemplate>

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Year">

                                    <ItemTemplate>

                                        <asp:TextBox ID="txtYear"
                                            runat="server"
                                            Text="2026"
                                            ReadOnly="true"
                                            CssClass="form-control-custom readonly-input">
                                        </asp:TextBox>

                                    </ItemTemplate>

                                </asp:TemplateField>

                            </Columns>

                        </asp:GridView>

                    </div>

                </div>

      

                <!-- Footer -->

                <div class="bottom-section">
                    <asp:Label ID="lblTotalWeightage"
    runat="server"
    Font-Bold="true">
</asp:Label>
                    <div class="d-flex justify-content-between align-items-center flex-wrap gap-3">

                        <div class="info-text">
                            Maximum 10 KPI rows can be added
                        </div>

                        <div class="d-flex gap-2">

                            <asp:Button ID="btnAddRow"
                                runat="server"
                                Text="+ Add Row"
                                CssClass="btn-custom btn-add"
                                OnClick="btnAddRow_Click" />

                            <asp:Button ID="btnSubmit"
                                runat="server"
                                Text="Save KPI"
                                CssClass="btn-custom btn-save"
                                OnClick="btnSubmit_Click" />

                        </div>

                    </div>

                    <asp:Label ID="lblMessage"
                        runat="server"
                        CssClass="success-message">
                    </asp:Label>

                </div>

              
</div>

</div>

    <div class="page-container" style="margin-top:30px; margin-bottom:120px;">

    <div class="main-card">

        <div class="card-header-custom">

            <div class="d-flex justify-content-between align-items-center flex-wrap gap-3">

                <div>

                    <div class="title">
                        Added KPIs
                    </div>

                    <div class="subtitle">
                        Modify KPIs
                    </div>

                </div>

                <div class="year-badge mt-3">
                    You Can Add Or Modify KPIs.
                </div>

            </div>

        </div>

        <div class="table-section">

            <div class="table-responsive">

                <asp:GridView ID="gvKPIReport"
                    runat="server"
                    AutoGenerateColumns="false"
                    CssClass="table table-bordered table-hover"
                    DataKeyNames="Id"
                    OnRowEditing="gvKPIReport_RowEditing"
                    OnRowCancelingEdit="gvKPIReport_RowCancelingEdit"
                    OnRowUpdating="gvKPIReport_RowUpdating"
                    OnRowDeleting="gvKPIReport_RowDeleting">

                    <Columns>

                        <asp:BoundField DataField="KPI"
                            HeaderText="KPI" />

                        <asp:BoundField DataField="Weightage"
                            HeaderText="Weightage" />

                        <asp:BoundField DataField="UOM"
                            HeaderText="UOM" />

                        <asp:BoundField DataField="Year"
                            HeaderText="Year" />

                        <asp:CommandField
                            ShowEditButton="true"
                            ShowDeleteButton="true"
                            ButtonType="Button"
                            ControlStyle-CssClass="btn btn-sm btn-primary"
                            HeaderText="Action" />

                    </Columns>

                </asp:GridView>

            </div>

        </div>

    </div>

</div>


  
</asp:Content>