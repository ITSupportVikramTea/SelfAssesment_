<%@ Page Title="" Language="C#" MasterPageFile="~/Employee/MasterPage.master" Async="true" AutoEventWireup="true" CodeFile="Leave.aspx.cs" Inherits="Employee_Leave" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<%--    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>--%>

    <script type="text/javascript">

        //function loadBootstrap5() {
        //    var bootstrap5Link = document.createElement('link');
        //    bootstrap5Link.rel = 'stylesheet';
        //    bootstrap5Link.href = 'https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css';

        //    document.head.appendChild(bootstrap5Link);

        //    bootstrap5Link.onload = function () {

        //        var elements = document.querySelectorAll('.use-bootstrap-5');
        //        elements.forEach(function (element) {

        //            element.classList.add('container', 'mt-5');
        //        });
        //    };
        //}

        //window.onload = loadBootstrap5;

     

        document.addEventListener('DOMContentLoaded', function () {

           

            const setMsg = document.getElementById('<%=lblMessage.ClientID%>');
            const getClick = document.getElementById('<%=btnaddleave.ClientID%>');


            //getClick.addEventListener('click', function () {
                
            //    setMsg.innerHTML = "Leave sent successfully.";
            //    setMsg.textContent = "Leave sent successfully.";
            //    setMsg.innerText = "";
            //    alert("Leave sent successfully.");
            //});

            const getSession = document.getElementById('<%=SessionDropDown.ClientID %>');
            const getLeaveType = document.getElementById('<%=dayType.ClientID%>');
            let getFromDate = document.querySelectorAll('.fromDate');
            const createLabel = document.createElement('label');
            createLabel.textContent = "Applied On";
            createLabel.setAttribute("for", "Date1");
            createLabel.style.display = 'none';
            getSession.style.display = 'none';
            const crLable = document.createElement('label');
            crLable.textContent = 'Select Session';
            crLable.style.display = 'none';

            getLeaveType.addEventListener('change', function () {
                console.log("event executed");

                if (this.value === 'Half Day') {
                    getSession.style.display = 'inline';
                    getSession.required = "true";
                    crLable.style.display = 'inline';
                    getSession.parentNode.insertBefore(crLable, getSession);
                }

                else if (this.value !== 'Half Day') {
                    getSession.style.display = 'none';
                    crLable.remove();
                }
            });

            getFromDate.forEach(element => {

                element.addEventListener('change', function () {

                    let current = this.value;
                   
                    /*let selectedDate = new Date(current);*/
                    selectedDate = current;
                    let defineDate = new Date().toLocaleDateString('en-GB').slice(0, 10).split('/').reverse().join('-');
                    const applied = document.querySelectorAll('.dated');

                    applied.forEach(data => {
                        data.style.display = 'none';
                    });

                    if (selectedDate < defineDate) {
                        createLabel.style.display = 'inline';

                        applied.forEach(data => {
                            data.style.display = "inline";

                            if (data.parentNode.contains(createLabel)) {
                                data.parentNode.removeChild(createLabel);
                            }
                            data.parentNode.insertBefore(createLabel, data);
                        });

                        console.log("selected date is less than current date");
                    } else {
                        createLabel.style.display = 'none';
                    }
                });
            });

            const getTo = document.getElementById('<%=txttodate.ClientID%>');
            const getTotalCount = document.getElementById('<%=totalCount.ClientID%>');

            getLeaveType.addEventListener('change', function () {
               
                if (getLeaveType.value !== "Half Day") {
                    const getFrm = document.getElementById('<%=txtfromdate.ClientID%>').value;
                    let getTOD = getTo.value;
                    const date1 = new Date(getTOD);
                    const date2 = new Date(getFrm);
                    const calDiff = date1.getTime() - date2.getTime();
                    var CalD = calDiff / (1000 * 60 * 60 * 24) + 1;
                    getTotalCount.value = CalD;
                    getTotalCount.textContent = CalD;
                }
                else
                {
                    getTotalCount.value = 0.5;
                }
            });

            const dropdownLeave = document.getElementById('<%=DropDownLeave.ClientID%>');
            const leaveBalance = document.getElementById('<%=Lb.ClientID%>');
            const spanMessage = document.getElementById('<%=lblMessage.ClientID%>');
            dropdownLeave.addEventListener('change', function () {
                const selectedLeave = this.value;

                if (selectedLeave) {
                    var xhr = new XMLHttpRequest();
                    xhr.open("POST", "Leave.aspx/LeaveBalance", true);
                    xhr.setRequestHeader("Content-Type", "application/json; charset=utf-8");

                    xhr.onreadystatechange = function () {
                        if (xhr.readyState === 4) {
                            if (xhr.status === 200) {
                                
                                var x = JSON.parse(xhr.responseText);
                                if (x.d) {
                                    var data = JSON.parse(x.d);
                                    if (data.success) {
                                        leaveBalance.value = data.balance;
                                        leaveBalance.textContent = data.balance;
                                        if (data.balance < 0) {
                                            spanMessage.textContent = 'Insufficient leave balance. LWP will be applicable. For more details, please contact the HR Department.';
                                            spanMessage.style.color = 'red';
                                        }
                                    } else {
                                        alert("Error: " + data.error);
                                    }
                                }
                            } else {
                                alert("Error: " + xhr.statusText);
                            }
                        }
                    };                  
                    var data = JSON.stringify({ selectedLeave: selectedLeave });
                    xhr.send(data);
                }
            });
        });
       
    </script>
    <style>
        .form-control{
            height: 45px;
            margin-bottom:30px;
        }
        label{
            position:absolute;
            margin-left:12px;
            margin-top:-11px;
            font-weight: initial;
  background-color: white;
        }
        @keyframes early {
            from {
                opacity: 1;
            }

            to {
                opacity: 0;
            }
        }
         .early{
     animation:fadeIn ease-in 1;
     animation-fill-mode:forwards;
     animation-duration:1s;
     animation-delay:1.5s;
 }
         article{
             max-width:800px;
             margin:1em auto;
             overflow:hidden;
             position:relative;
            
         }
         .example-left{
             white-space:nowrap;
             position:absolute;
             text-transform:uppercase;
         }
         .example-left{
             -webkit-animation:mymove 15s linear infinite;
             white-space:nowrap;
             animation: mymove 15s linear infinite alternate;
         }

         @-webkit-keyframes mymove{
             from{
                 left:0;
             }
             to{
                 left:-140px;
             }
         }
         @keyframes mymove{
             from{
                 left:0;
             }
             to{
                 left:-140px;
             }
         }
         .sidebar-toggle
         {
             position:relative;
             right:42rem;
         }
         img{
             position: relative;
    left: 43rem;
         }
         .tld {
             background: linear-gradient(180deg, #FFF 50%,#EEE 50%);
         }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <section class="content-header">
        <h1>Leave / OD Application 
            <small>Master</small>
        </h1>
       
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Forms</a></li>
      
        </ol>

    </section>
            <section class="early">
            <article>
                <marquee behavior="scroll" direction="left" class="text-danger b-80 fw-bolder" scrollamount="3">
            <h5 style="font-weight: 600;line-height:1.5;">Note: The application is in the initial development phase. As a result, you may encounter bugs, incorrect data, or data discrepancies. Kindly raise queries through the support section of this portal. टीप: पोर्टल प्रारंभिक विकास टप्प्यात आहे. त्यामुळे तुम्हाला काही त्रुटी, चुकीची माहिती किंवा डेटा विसंगती 
आढळू शकतात.आढळल्यास कृपया या पोर्टलच्या सपोर्ट सेक्शनद्वारे तक्रार करावी.</h5> 
                 </marquee>
                    </article>
                </section>
    <section class="content">
        <!-- Specific section to apply Bootstrap 5 styles -->
        <div class="box box-default use-bootstrap-5">
            <div class="box-header with-border" style="height: 2rem;">
                <h3 class="box-title">Forms</h3>
                  <span id="LastLeaved" runat="server" style="float:right" class="alert alert-primary mb-3" role="alert"></span>
            </div>
             <center>
     <h3 id="lblMessage" runat="server" class=""></h3>

<asp:Label ID="Label1" runat="server" ForeColor="Green"></asp:Label>
 </center>
  
            <div class="box-body">
                <div id="bootstrap5-wrapper">
                    <div class="col-md-12 form-group">
                        <div class="col-md-6 mb-5">
                            <label>From</label>
                            <input type="date" class="form-control fromDate h-100" id="txtfromdate" runat="server" autocomplete="Off" required />
                        </div>
                        <div class="col-md-6 mb-5">
                            <label>To</label>
                            <input type="date" class="form-control h-100" id="txttodate" runat="server" autocomplete="Off" required />
                        </div>
                    </div>        
                         <div class="col-md-12">
                        <div class="col-md-12 mb-5">
                            <%-- <asp:Label runat="server">Applied On</asp:Label>--%>
                            <input type="date" class="form-control dated h-100" id="Date1" runat="server" style="display: none" autocomplete="Off" />
                        </div>
                      </div>
                    <div class="col-md-12">
                        <div class="col-md-6 mb-5">
                            <label runat="server">Leave Type</label>
                            <div class="form-floating">
                                <asp:DropDownList runat="server" ID="DropDownLeave" CssClass="form-control" required>
                                    <asp:ListItem Value="">Select Leave Type</asp:ListItem>
                                    <asp:ListItem Value="CL">CL</asp:ListItem>
                                    <asp:ListItem Value="PL">PL</asp:ListItem>
                                    <asp:ListItem Value="SL">SL</asp:ListItem>
                                    <asp:ListItem Value="C/O">C/O</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-6 mb-5">
                            <label>Leave</label>
                            <div class="form-floating">
                                <asp:DropDownList runat="server" ID="dayType" CssClass="form-control">
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                    <asp:ListItem Value="Half Day">Half Day</asp:ListItem>
                                    <asp:ListItem Value="Full Day">Full Day</asp:ListItem>
                                    <asp:ListItem Value="OD">OD</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="col-md-12 mb-5">
                                <label>Session</label>
                            <asp:DropDownList runat="server" ID="SessionDropDown" CssClass="form-control" >
                                <asp:ListItem Value=""></asp:ListItem>
                                <asp:ListItem Value="Morning">Morning</asp:ListItem>
                                <asp:ListItem Value="Afternoon">Afternoon</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="col-md-6 mb-5">
                            <label class="tld">Total Leave Day</label>
                            <div class="form-floating">
                                <input type="text" id="totalCount" runat="server" class="form-control" readonly/>
                            </div>
                        </div>
                        <div class="col-md-6 mb-5">
                            <label class="tld">Total Leave Balance</label>
                            <div class="form-floating">
                                <input type="text" id="Lb" runat="server" class="form-control" readonly/>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">

                        <div class="col-md-12 mb-5">
                            <label>Reason / C/Off working against </label>
                            <textarea id="reason" runat="server" class="form-control mt-2" style="height:67px;" required></textarea>
                        </div>
                    </div>
                    <div class="col-md-12 mb-5">
                        <div class="col-md-12">
                            <asp:Button ID="btnaddleave" runat="server" Text="Send" class="btn btn-success mt-4 form-control" OnClick="btnaddleave_Click"/>
                        </div>
                    </div>
                    <asp:HiddenField ID="empid" runat="server" />

                </div>
            </div>
        </div>
    </section>
</asp:Content>
