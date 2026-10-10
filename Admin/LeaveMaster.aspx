<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Admin/MasterPage.master" CodeFile="LeaveMaster.aspx.cs" Inherits="Admin_LeaveMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <style>
     .reason-column {
    white-space: normal; /* Allows text wrapping */
    max-width: 200px;
    padding: 5px;
    background-color: #f9f9f9;
}
 </style>
<script>
    document.addEventListener("DOMContentLoaded", function () {
        const gridRows = document.querySelectorAll("#<%= GridViewLeaves.ClientID %> tbody tr");

        gridRows.forEach(function (row) {
            const statusCell = row.querySelector("td:nth-child(6)"); 
            const btnSubmit = row.querySelector("button[id$='btnSubmit']");
            if (statusCell && btnSubmit) {
                const statusValue = statusCell.textContent.trim();

                if (statusValue === "Approved" || statusValue === "Rejected") {
                    btnSubmit.disabled = true; 
                }
            }
        });
    });

    function onInputChange() { 
    const gridview = document.getElementById('<%=GridViewLeaves.ClientID%>');
        const searchBox = document.getElementById("searchbox");

        const tr = gridview.getElementsByTagName('tr');

        for (let i = 1; i < tr.length; i++) {
            let match = false;
            const td = tr[i].getElementsByTagName('td');

            for (let j = 0; j < td.length; j++) {
                const dataValue = td[j].innerText.toLowerCase();
                const getSearchBox = searchBox.value.toLowerCase();

                if (dataValue.includes(getSearchBox)) {
                    match = true;
                    break;
                }
            }

            tr[i].style.display = match ? "" : "none";
        }
    }
</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <section class="content">
     <section class="content-header">
     <h1>Leave / OD Application</h1>
      <ol class="breadcrumb">
         <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
         <li><a href="#">Forms</a></li>
     </ol>
 </section>
    <center>
        <input type="text" class="form-control" placeholder="Filter Data" onkeyup="onInputChange()" id="searchbox" />

    <h4 id="approveMsg" runat="server" class="text-success"></h4>
        </center>
         <div style="overflow-x:auto;overflow-y:auto">
    <asp:GridView ID="GridViewLeaves" runat="server" CssClass="table table-striped" AutoGenerateColumns="false" style="border: dashed;text-wrap: auto;" EnableViewState="true" OnRowDataBound="GridViewLeaves_RowDataBound">
    <Columns>
         <asp:BoundField DataField="employeeName" HeaderText="Employee Name" />
        <asp:BoundField DataField="FromDate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="From" />
        <asp:BoundField DataField="ToDate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="To" />
        <asp:BoundField DataField="LeaveType" HeaderText="Type" />
        <asp:BoundField DataField="halfDaySession" HeaderText="HDS" />
        <asp:BoundField DataField="TotalLeaveDay" HeaderText="TLD" />
        <asp:BoundField DataField="TotalLeaveBalance" HeaderText="TLB" />
        <asp:BoundField DataField="CreatedDate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="Applied On" />
        <asp:BoundField DataField="Reason" HeaderText="Reason">
    <ItemStyle Wrap="true" CssClass="reason-column" />
    <HeaderStyle CssClass="reason-header" />
</asp:BoundField>
        <asp:BoundField DataField="Status" HeaderText="Status" />
        <asp:TemplateField  HeaderText="Action">
            <ItemTemplate>
                <asp:HiddenField ID="hfLeaveId" runat="server" Value='<%# Eval("Id") %>' />
                <asp:DropDownList ID="ddlStatus" runat="server">
                      <asp:ListItem Text="Select" Value=""></asp:ListItem>
                    <asp:ListItem Text="Approve" Value="Approved"></asp:ListItem>
                    <asp:ListItem Text="Cancel" Value="Cancelled"></asp:ListItem>
                  <%--<asp:ListItem Text="Hold" Value="Hold"></asp:ListItem>
                    <asp:ListItem Text="Reject" Value="Rejected"></asp:ListItem>--%>
                </asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>
     <asp:TemplateField HeaderText="Status" SortExpression="Status">

      <ItemTemplate>
          <span style="<%# GetStatusClass(Eval("Status").ToString()) %>">
              <%# Eval("Status") %>
          </span>
      </ItemTemplate>
  </asp:TemplateField>
        <asp:TemplateField HeaderText="Remark">
            <ItemTemplate>
                <asp:Label ID="lblRemark" runat="server" Text='<%# Eval("Remark") %>'></asp:Label>
                <asp:TextBox ID="txtRemark" CssClass="form-control" style="text-wrap:auto" placeholder="Remark if any" runat="server" Visible="false"></asp:TextBox>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtEditRemark" CssClass="form-control" runat="server" Text='<%# Eval("Remark") %>'></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Action">
              <ItemTemplate>
                     <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-success" OnClick="UpdateStatus" />
                    </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
             </div>
<%--<asp:Button ID="btnUpdateStatus" runat="server" Text="Update Status" OnClick="UpdateStatus" />--%>
        </section>
 </asp:Content>