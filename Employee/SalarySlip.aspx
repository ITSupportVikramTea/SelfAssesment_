<%@ Page Title="" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" Async="true" CodeFile="SalarySlip.aspx.cs" Inherits="Employee_SalarySlip" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .badge-success{
            background-color:#cee9dd !important;
            color:#0a3420 !important;
        }

        .bg-warning {
            background-color: #e9cece !important;
  color: #340a0a !important;
        }
        .sticky-top{
            border-top: 2px solid black;
        }

        .btn-primary {
            float: inline-end;
            border-radius: 13px;
        }
    </style>
  <script>
      function openModal() {
          document.getElementById("monthModal").style.display = "block";
      }

      function closeModal() {
          document.getElementById("monthModal").style.display = "none";
      }

      document.addEventListener("change", function (e) {

          if (e.target.classList.contains("select-checkbox")) {

            
              document.querySelectorAll(".select-checkbox").forEach(cb => {
                  if (cb !== e.target) cb.checked = false;
              });

           
              document.querySelectorAll("tr").forEach(r => r.classList.remove("table-primary"));

              if (e.target.checked) {
                  e.target.closest("tr").classList.add("table-primary");
              }
          }
      });
  </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

 <div id="monthModal" style="display:none; position:fixed; top:20%; left:30%; width:40%; background:white; padding:20px; border:1px solid #ccc; z-index:9999;">

    <h4>Select Month</h4>
     <div class="card shadow-sm p-3">
    <h5 class="mb-3" style="border-bottom: 2px solid #62c8a6;
  padding: 6px;">Salary Release Settings</h5>

  
    <div style="max-height: 400px; overflow-y: auto;">  

         <asp:GridView ID="month" runat="server"
            DataKeyNames="Id"
            AutoGenerateColumns="False"
            CssClass="table table-hover align-middle text-center mb-0"
            HeaderStyle-CssClass="table-dark sticky-top"
            RowStyle-CssClass="table-light"
            AlternatingRowStyle-CssClass="table-secondary"
            BorderStyle="None">

            <Columns>

               
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:CheckBox ID="rbSelect" runat="server"
                            CssClass="form-check-input select-checkbox" />
                    </ItemTemplate>
                    <ItemStyle Width="50px" />
                </asp:TemplateField>

             
                <asp:TemplateField HeaderText="Month">
                    <ItemTemplate>
                        <asp:TextBox ID="txtMonth" runat="server"
                            Text='<%# Eval("month") %>'
                            CssClass="form-control text-center fw-semibold" ReadOnly="true"/>
                    </ItemTemplate>
                </asp:TemplateField>

                
                <asp:TemplateField HeaderText="Year">
                    <ItemTemplate>
                        <asp:TextBox ID="txtYear" runat="server"
                            Text='<%# Eval("year") %>'
                            CssClass="form-control text-center" ReadOnly="true"/>
                    </ItemTemplate>
                </asp:TemplateField>

              
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <span class='<%# Eval("status").ToString() == "Released" ? "badge badge-success" : "badge bg-warning text-dark" %>'>
                            <%# Eval("status") %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>

        </asp:GridView>


    </div>
</div>
    <br />

    <asp:Button ID="btnUpdate" runat="server" Text="Update"
        CssClass="btn btn-success"
        OnClick="btnUpdate_Click"/>

    <button onclick="closeModal()" class="btn btn-danger">Close</button>

</div>



    <section class="content-header">
        <h1>Salary Slip
            <small>Report</small>
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
                <h3 class="box-title">Report</h3>
                <asp:Button ID="btnRelease" runat="server" Text="Release"
    CssClass="btn btn-primary"
    OnClientClick="openModal(); return false;" />
            </div>
            <!-- /.box-header -->
            <div class="box-body">
                <div class="row">

                    <div class="col-md-2">

                        <asp:DropDownList ID="drpSalaryYear" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="drpSalaryYear_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-2">
                        <asp:DropDownList ID="drpSalarySlip" runat="server" OnSelectedIndexChanged="drpSalarySlip_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control "></asp:DropDownList>
                    </div>

                     <div class="col-md-2">
                        <asp:Button ID="btnShowPreviousMonthSalary" runat="server" Text="Show Previous Month Salary" OnClick="btnShowPreviousMonthSalary_Click" CssClass="btn btn-info" Visible="false" />
                      </div>
                
                    <br />
                    <br />
                    <br />
                    <div class="col-md-12 form-group">

                        <asp:Panel ID="dvHtml" runat="server">
                        </asp:Panel>
                        <iframe runat="server" id="showpdf" height="1000" width="1000" scrolling="no"></iframe>

                    </div>
                       


                    
                </div>

               
            </div>
            <!-- /.box -->
        </div>

        <!-- /.row -->

    </section>

</asp:Content>

