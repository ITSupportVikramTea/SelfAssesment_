<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Employee/MasterPage.master" CodeFile="LeaveSheet.aspx.cs" Inherits="Employee_LeaveSheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const cancelButtons = document.querySelectorAll(".cancelbtn");


            cancelButtons.forEach(function (cancelButton) {


                cancelButton.addEventListener("click", function (e) {
                    e.preventDefault();
                    if (confirm("Are you sure you want to cancel?")) {
                        const row = cancelButton.closest("tr");
                        const hiddenButton = row.querySelector(".submitbtn");

                        if (hiddenButton) {
                            hiddenButton.click();
                            cancelButton.disabled = true;
                            hiddenButton.disabled = true;
                        } else {
                            console.log("No hidden button found in this row.");
                        }
                    }
                });
            });

                document.querySelectorAll(".editBtn").forEach((EditBTN) => {
                    EditBTN.addEventListener("click", function () {
                        let EditRow = this.closest("tr");
                        console.log('EditRow Data BEFORE edit mode:', EditRow.innerHTML);
                        let LTS = EditRow.querySelector(".LVT");
                        if (LTS) {
                            LTS.addEventListener("change", function () {
                                console.log("LTS dropdown changed to:", LTS.value);
                                debugger;
                                let toDateField = EditRow.querySelector(".toDate");

                                if (toDateField) {
                                    console.log("Triggering change event on .toDate");

                                    let event = new Event("change", { bubbles: true });
                                    toDateField.dispatchEvent(event);
                                } else {
                                    console.error("No `.toDate` field found in this row.");
                                }
                            });
                        } else {
                            console.error("LTS dropdown not found in row:", EditRow);
                        }
                    });
                });

            document.addEventListener("change", function (e) {
                if (e.target.classList.contains("toDate")) {
                    console.log("Change event triggered on:", e.target);
                    let editRow = e.target.closest("tr");

                    let fromDate = editRow.querySelector(".from");
                    let toDate = editRow.querySelector(".toDate");
                    let leaveDays = editRow.querySelector(".leaveDay");

                    console.log("From Date:", fromDate ? fromDate.value : "Not found");
                    console.log("To Date:", toDate ? toDate.value : "Not found");
                    console.log("Leave Days:", leaveDays ? leaveDays.value : "Not found");
                }
            });

            document.querySelectorAll(".editBtn").forEach((EditBTN) => {
                EditBTN.addEventListener("click", function () {
                    let EditRow = this.closest("tr");
                    console.log('EditRow Data', EditRow.innerHTML);
                    let LType = EditRow.querySelector(".LVT");

                    if (LType) {
                        let D1Value = LType.textContent.trim();
                        document.cookie = `D1Value=${D1Value}; path=/;`;

                        console.log("Stored D1Value in cookie:", D1Value);
                    } else {
                        console.error("LType element not found in row:", EditRow);
                    }
                  
                });
            });

            function getCookie(name) {
                let cookieArr = document.cookie.split("; ");
                for (let i = 0; i < cookieArr.length; i++) {
                    let cookiePair = cookieArr[i].split("=");
                    if (cookiePair[0] === name) {
                        return decodeURIComponent(cookiePair[1]);
                    }
                }
                return null;
            }

            function clearCookie(name) {
                document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`;
                console.log(`Cookie ${name} has been cleared.`);
            }

            let D1 = { value: getCookie("D1Value") };
            console.log("Retrieved D1 value:", D1.value);

            if (D1.value !== "LWP") {
                console.log("D1 value in checking LWP:", D1.value);
            }
            const editBtns = document.querySelectorAll('.toDate');


            if (editBtns.length > 0) {
                editBtns.forEach(function (button) {
                    button.addEventListener("change", function (e) {
                        console.log("Edit button clicked:", button);
                        e.preventDefault();
                        let editRow = button.closest("tr");
                        let toDat = null;
                        let frm = null;
                        let dateElements = editRow.querySelectorAll(".from");
                        let frmDate = editRow.querySelectorAll(".toDate");
                        let countDay = editRow.querySelectorAll(".leaveDay");

                        let inValue = 0;
                      
                        countDay.forEach(function (leave) {
                            inValue = leave.value;
                            console.log('leave inspect',leave.value);
                        });

                        frmDate.forEach(function (fromDate) {
                            frm = fromDate.value;
                        });

                        dateElements.forEach(function (element) {
                            toDat = element.value;
                        });

                        let fromDate = new Date(frm);
                        let toDate = new Date(toDat);
                   
                        var CalDiff = fromDate - toDate;
                        CalDi = CalDiff / (1000 * 60 * 60 * 24) + 1;
                      
                        countDay.forEach(function (dateCount) {                     
                            dateCount.value = CalDi;
                        });

                        console.log('initial value-', inValue, 'new value -', CalDi);
                        const lB = editRow.querySelectorAll('.LB');
                        const lS = editRow.querySelectorAll('.LTS');
                        const LT = editRow.querySelector('.dayType');
                  
                        let getLTS = "";
                        lS.forEach(function (lType) {
                            getLTS = lType.value;
                        });
                        console.log("D1.value before checking LWP:", D1.value);

                        if (D1.value !== "LWP") {
                            console.log("D1 value in checking LWP", D1.value);
                            clearCookie("D1Value");
                            lB.forEach(function (LB) {
                               
                                if (LB.value > 0) { 
                                    console.log('leave balance', LB.value);
                                    console.log('after deduction.', CalDi - inValue);
                                   
                                var xhttp = new XMLHttpRequest();
                                xhttp.open("POST", "LeaveSheet.aspx/LeaveBalance", true);
                                xhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
                              
                                xhttp.onreadystatechange = function () {
                                    if (this.readyState === 4 && this.status === 200) {
                                        var x = JSON.parse(xhttp.responseText);
                                        if (x.d) {                                        
                                            console.log('x value', x.d);
                                            var data = JSON.parse(x.d);
                                            if (data.balance !== null && data.balance !== 0) {
                                                debugger;
                                                if (data.success) {
                                                    const LDB = CalDi - data.balance;
                                                    console.log('LDB data', LDB);                                                
                                                    console.log('both invalue and CalDi', inValue, CalDi);
                                                    let extraLe = inValue - CalDi;
                                              
                                                    if (CalDi > inValue) {

                                                        if (data.balance > extraLe) {

                                                            const trim = extraLe.toString().replace('-', '');
                                                            const updateLeave = data.balance - trim;
                                                            console.log('about to deduct balance', extraLe);
                                                            const hiddenField = document.getElementById('<%=LU.ClientID%>');

                                                            hiddenField.value = updateLeave;
                                                            console.log('hidden Field', hiddenField);
                                                        }

                                                        else {
                                                            alert("Insufficient Leave Balance");
                                                        }
                                                    }
                                                    else {
                                                        const plusLeave = Number(data.balance) + Number(extraLe);
                                                        const hidden = document.getElementById('<%=LU.ClientID%>');
                                                        hidden.value = plusLeave;
                                                        console.log('plust leave', hidden);
                                                    }                                                  
                                                }
                                            }
                                            else {
                                                console.log('data balance not found');
                                                alert("Insufficient Leave Balance");
                                            }
                                        }
                                        else {
                                            console.log('error', data.error);
                                        }
                                    }

                                    else {
                                        console.log('AJAX Request in Progress or Failed:', xhttp.readyState, xhttp.status);
                                    }   
                                }                             
                                var data = JSON.stringify({SelectedLeave: getLTS});
                                    xhttp.send(data);
                                }
                            });     
                        }
                    });
                });

            }
            else {
                console.log("No edit buttons found.");
            }
        });
    </script>

    <style>
        th {
            text-align: center;
        }

        span {
            text-align: center;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content">
        <section class="content-header">
            <h1>Leave / OD Applications
                <small>Master</small>
                <center>
                    <span id="message" runat="server" class="text-primary ml-3"></span>
                </center>
            </h1>
            <ol class="breadcrumb">
                <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
                <li><a href="#">Forms</a></li>
            </ol>
        </section>
        <h4 id="approveMsg" runat="server" class="text-success"></h4>
        <div style="overflow-x: auto; overflow-y: auto">
            <asp:GridView ID="GridViewLeaves" runat="server" CssClass="table table-striped mt-2" Style="border: dashed; text-align: center" AutoGenerateColumns="false" OnRowDataBound="GridViewLeaves_RowDataBound" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" AllowSorting="true">
                <Columns>
                       <asp:TemplateField HeaderText="Leave">
       <ItemTemplate>
           <asp:Label runat="server" CssClass="LVT" Text='<%#Eval("LeaveType") %>'></asp:Label>
       </ItemTemplate>
       <EditItemTemplate>
           <asp:DropDownList runat="server" ID="LT" Text='<%#Eval("LeaveType") %>' CssClass="form-control LTS">
               <asp:ListItem Value="">Select Leave Type</asp:ListItem>
               <asp:ListItem Value="CL">CL</asp:ListItem>
               <asp:ListItem Value="PL">PL</asp:ListItem>
               <asp:ListItem Value="SL">SL</asp:ListItem>
               <asp:ListItem Value="LWP">LWP</asp:ListItem>
               <asp:ListItem Value="C/O">C/O</asp:ListItem>
           </asp:DropDownList>
       </EditItemTemplate>
   </asp:TemplateField>
                    <asp:TemplateField HeaderText="From">
                        <ItemTemplate>
                            <asp:Label ID="lblFromDate" CssClass="frmDate" runat="server" Text='<%# Eval("FromDate", "{0:yyyy-MM-dd}") %>'>From</asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="FromDate" runat="server" CssClass="form-control from" Text='<%# Bind("FromDate", "{0:yyyy-MM-dd}") %>' TextMode="Date"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="To">
                        <ItemTemplate>
                            <asp:Label runat="server" CssClass="toDated" Text='<%# Eval("ToDate","{0:yyyy-MM-dd}") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="toDate" runat="server" CssClass="form-control toDate" TextMode="Date" Text='<%# Eval("ToDate","{0:yyyy-MM-dd}")%>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Day">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("dayType","{0:yyyy-MM-dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="dayType" CssClass="form-control dayType" runat="server" Text='<%# Eval("dayType") %>'>
                                <asp:ListItem Value="">Select</asp:ListItem>
<%--                                <asp:ListItem Value="Half Day">Half Day</asp:ListItem>--%>
                                <asp:ListItem Value="Full Day">Full Day</asp:ListItem>
                                <asp:ListItem Value="OD">OD</asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Session">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("halfDaySession") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="hds" runat="server" CssClass="form-control" Text='<%#Eval("halfDaySession")%>' readonly>
                                <asp:ListItem Value="">Select</asp:ListItem>
                                <asp:ListItem Value="Morning">Morning</asp:ListItem>
                                <asp:ListItem Value="Afternoon">Afternoon</asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                 

                    <asp:TemplateField HeaderText="TLD">
                        <ItemTemplate>
                            <asp:Label runat="server" CssClass="TotalLeave" Text='<%#Eval("TotalLeaveDay")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--Text='<%#Eval("TotalLeaveDay")%>'--%>
                            <asp:TextBox runat="server" ID="leaveDays" Text='<%#Eval("TotalLeaveDay")%>' CssClass="form-control leaveDay"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="TLB">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("TotalLeaveBalance")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" CssClass="form-control LB" Text='<%#Eval("TotalLeaveBalance")%>' ReadOnly></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Reason">
                        <ItemTemplate>
                            <asp:Label Text='<%#Eval("Reason") %>' runat="server"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" CssClass="form-control" Text='<%#Eval("Reason") %>' ReadOnly></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" SortExpression="Status">

                        <ItemTemplate>
                            <span style="<%# GetStatusClass(Eval("Status").ToString()) %>">
                                <%# Eval("Status") %>
                            </span>
                            <asp:HiddenField ID="hfLeaveId" runat="server" Value='<%# Eval("Id") %>' />
                            <asp:Button ID="HiddenSubmitButton" CssClass="submitbtn" runat="server" OnClick="UpdateStatus" Style="display: none;" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Apply Date">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("CreatedDate")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" Text='<%#Eval("CreatedDate")%>' ReadOnly></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:Button ID="CancelButton" runat="server" Text="Cancel" CssClass="btn btn-danger cancelbtn" />
                            <asp:Button ID="Edit" runat="server" Text="Edit" CssClass="btn btn-info editBtn" CommandName="Edit" />
                            <div style="display: flex; gap: 3px;">
                                <asp:Button ID="Update" runat="server" CssClass="btn btn-success updateBtn" Text="Update" CommandName="Update" Visible="false" />
                                <asp:Button ID="EditCancel" runat="server" CssClass="btn btn-warning" Text="Cancel Edit" CommandName="Cancel" Visible="false" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <%--update leave--%>

                <asp:HiddenField ID="LU" runat="server" value="" />
        </div>
    </section>
</asp:Content>

                                                                                               