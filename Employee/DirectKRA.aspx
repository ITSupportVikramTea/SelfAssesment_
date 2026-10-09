<%@ Page Title="" Language="C#" AutoEventWireup="true"
    MasterPageFile="~/Employee/MasterPage.master"
    CodeFile="DirectKRA.aspx.cs"
    Inherits="Employee_DirectKRA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

<style>
    .kra-box .box-body { padding: 15px; }
    .kra-label { font-weight: 600; }
    .action-col { width:70px; text-align:center; }
    textarea.form-control { min-height:60px; }

</style>

<script>
    document.addEventListener('DOMContentLoaded', function () {
        var tbody = document.getElementById("kraTable").getElementsByTagName("tbody")[0];
        var template = tbody.rows[0];

        for (var i = 1; i < 3; i++) {  
            var newRow = template.cloneNode(true);

            var inputs = newRow.querySelectorAll("input, textarea, select");
            for (var j = 0; j < inputs.length; j++) {
                inputs[j].value = "";
            }

            tbody.appendChild(newRow);
        }
    });


    function addRow() {
        var tbody = document.getElementById("kraTable").getElementsByTagName("tbody")[0];
        var newRow = tbody.rows[0].cloneNode(true);

        var inputs = newRow.querySelectorAll("input, textarea, select");
        for (var i = 0; i < inputs.length; i++) {
            inputs[i].value = "";
        }

        tbody.appendChild(newRow);
    }

    function removeRow(btn) {
        var tbody = document.getElementById("kraTable").getElementsByTagName("tbody")[0];
        if (tbody.rows.length > 1) {
            btn.closest("tr").remove();
        }
    }
</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<section class="content-header">
    <center><b><h1 style="font-weight: 700;">Job Description</h1></b></center>
</section>

<section class="content">
<div class="row">
<div class="col-md-10 col-md-offset-1">

<div class="box box-primary kra-box">
<div class="box-header with-border">
    <h3 class="box-title">KRA Form</h3>
</div>

<div class="box-body">

<div class="row">
    <div class="col-md-3">
        <label class="kra-label">Employee Name</label>
        <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="form-control" />
    </div>

    <div class="col-md-3">
        <label class="kra-label">Date</label>
        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Date" />
    </div>

    <div class="col-md-3">
        <label class="kra-label">Department</label>
        <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control" />
    </div>

    <div class="col-md-3">
        <label class="kra-label">Designation</label>
        <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control" />
    </div>
</div>

<hr />

<div class="table-responsive">
<table id="kraTable" class="table table-bordered table-striped">
<thead>
<tr>
    <th>Job Description (Fill in english language)</th>
    <th>Frequency</th>
    <th class="action-col">Action</th>
</tr>
</thead>
<tbody>
<tr>
    <td>
          <input class="form-control" name="dailyKra[]" />
      
    </td>

    <td>
        <select class="form-control" name="daily[]">
      <option value="">Select</option>
      <option value="Daily">Daily</option>
      <option value="Weekly">Weekly</option>
      <option value="Monthly">Monthly</option>
      <option value="Need Based">Need Based</option>
  </select>
    </td>

    <td class="action-col">
        <button type="button" class="btn btn-danger btn-sm" onclick="removeRow(this)">
            <i class="fa fa-trash"></i>
        </button>
    </td>
</tr>
</tbody>
</table>
</div>

<button type="button" class="btn btn-primary" onclick="addRow()">
    <i class="fa fa-plus"></i> Add Row
</button>

<asp:Button ID="btnSave" runat="server" Text="Submit KRA" CssClass="btn btn-success pull-right" OnClick="btnSave_Click" />

</div>
</div>

</div>
</div>
</section>

</asp:Content>