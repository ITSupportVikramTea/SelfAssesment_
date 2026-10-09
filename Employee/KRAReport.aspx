<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/Employee/MasterPage.master"
    CodeFile="KRAReport.aspx.cs"
    Inherits="Employee_KRAReport" %>

<asp:Content ID="c1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<div class="box box-primary">
<div class="box-header with-border">
   <center><h3 style="font-weight: bold;font-size: x-large;" class="box-title">
        <i class="fa fa-bar-chart"></i> KRA Report
    </h3>
       </center>
</div>


<div class="table-responsive">

<asp:GridView ID="gvKRA" runat="server"
    CssClass="table table-bordered table-striped table-hover"
    AutoGenerateColumns="False"
    DataKeyNames="Id"
    OnRowEditing="gvKRA_RowEditing"
    OnRowCancelingEdit="gvKRA_RowCancelingEdit"
    OnRowUpdating="gvKRA_RowUpdating">

    <Columns>
        <asp:TemplateField HeaderText="No.">
    <ItemTemplate>
        <%# Container.DataItemIndex + 1 %>
    </ItemTemplate>
</asp:TemplateField>

        <asp:BoundField DataField="EmployeeName" HeaderText="Employee" ReadOnly="true" />
        <%--<asp:BoundField DataField="EntryDate" HeaderText="Date" ReadOnly="true" />--%>

        <asp:TemplateField HeaderText="KRA Description">
            <ItemTemplate>
                <span class="text-bold"><%# Eval("DailyKra") %></span>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtKra" runat="server"
                    CssClass="form-control"
                    Text='<%# Bind("DailyKra") %>' />
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Type">
            <ItemTemplate>
                <span class="label label-info">
                    <%# Eval("DailyType") %>
                </span>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList ID="ddlType" runat="server"
                    CssClass="form-control">
                    <asp:ListItem>Daily</asp:ListItem>
                    <asp:ListItem>Weekly</asp:ListItem>
                    <asp:ListItem>Monthly</asp:ListItem>
                    <asp:ListItem>Need Based</asp:ListItem>
                </asp:DropDownList>
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Action">
            <ItemTemplate>
                <asp:LinkButton runat="server" CommandName="Edit"
                    CssClass="btn btn-xs btn-warning">
                    <i class="fa fa-edit"></i>
                </asp:LinkButton>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:LinkButton runat="server" CommandName="Update"
                    CssClass="btn btn-xs btn-success">
                    <i class="fa fa-check"></i>
                </asp:LinkButton>

                <asp:LinkButton runat="server" CommandName="Cancel"
                    CssClass="btn btn-xs btn-danger">
                    <i class="fa fa-times"></i>
                </asp:LinkButton>
            </EditItemTemplate>
        </asp:TemplateField>

    </Columns>
</asp:GridView>

</div>
</div>
</div>

</asp:Content>