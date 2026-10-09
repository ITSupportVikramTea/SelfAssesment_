<%@ Page Title="" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="PrintClearanceForm.aspx.cs" Inherits="Employee_PrintClearanceForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="content-header">
        <h1>Clearance Form
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
                <div class="col-md-12">
                    <h3 class="text-center" id="clearanceText" runat="server"><b>Clearance Form</b></h3>
                </div>
            </div>
            <br />

            <div class="box-body">
                <div class="row form-group">
                    <div class="container-fluid">
                        <div class="form-group col-md-6">
                            <lable>Employee Name: </lable>
                            <b><%=empName %> </b>
                        </div>
                        <div class="form-group col-md-6">
                            <lable>Designation: </lable>
                            <b><%=designation %> </b>
                        </div>
                        <div class="form-group col-md-6">
                            <lable>Department: </lable>
                            <b><%=department %> </b>
                        </div>
                        <div class="form-group col-md-6">
                            <lable>Date of Joining: </lable>
                            <b><%=doj %> </b>
                        </div>
                        <div class="form-group col-md-6">
                            <lable>ERPID: </lable>
                            <b><%=erpid %> </b>
                        </div>
                        
                        <div class="form-group col-md-6">
                            <lable>Date of Resignation: </lable>
                            <b><%=dor %> </b>
                        </div>
                        <div class="form-group col-md-6">
                            <lable>Date of Relieving: </lable>
                            <b><%=dorelieving %> </b>
                        </div>
                        <div class="form-group col-md-6">
                            <lable>Reporting to: </lable>
                            <b><%=rm %> </b>
                        </div>
                        <div class="form-group col-md-6">
                            <lable>Location: </lable>
                            <b><%=location %> </b>
                        </div>
                         <div class="form-group col-md-6">
                            <lable>Remarks: </lable>
                            <b><%=remark %> </b>
                        </div>
                        
                        <div class="form-group col-md-12">
                            <lable>
                                I have checked with the distributors & in market,  following is the status in respect with
                                <br />
                                Mr./Ms.<b> <%=empName %> </b>was working under me.</lable>
                        </div>
                        <div class="col-md-6 my-2" style="overflow-x: auto">
                            <asp:GridView ID="gdAssetData" runat="server" class="table table-striped table-bordered table-hover"
                                AllowPaging="true"
                                AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="AssesstName" HeaderText="Asset Name" ItemStyle-Width="250" />
                                    <asp:BoundField DataField="ReturnBackStatus" HeaderText="Return Back Status" ItemStyle-Width="250" />
                                    <asp:BoundField DataField="ReturnedToWhom" HeaderText="Returned To Whom" ItemStyle-Width="250" />
                                </Columns>
                            </asp:GridView>
                        </div>

                        <div class="col-md-6 my-2" style="overflow-x: auto">
                            <asp:GridView ID="gdSupiriorClearanceData" runat="server" class="table table-striped table-bordered table-hover"
                                AllowPaging="true"
                                AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="DistributorName" HeaderText="Distributor Name" ItemStyle-Width="250" />
                                    <asp:BoundField DataField="IsNOCRecieved" HeaderText="Is NOC Recieved" ItemStyle-Width="250" />
                                    <asp:BoundField DataField="BalanceStatus" HeaderText="Balance Status" ItemStyle-Width="250" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="100" />
                                </Columns>
                            </asp:GridView>
                        </div>

                        <div class="col-md-6 my-2" style="overflow-x: auto">
                            <asp:GridView ID="gdDeptNocData" runat="server" class="table table-striped table-bordered table-hover"
                                AllowPaging="true"
                                AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="department_name" HeaderText="Department Name" ItemStyle-Width="250" />
                                    <asp:BoundField DataField="description" HeaderText="Description" ItemStyle-Width="250" />
                                    <asp:BoundField DataField="is_noc_recieved" HeaderText="Is NOC Recieved" ItemStyle-Width="250" />
                                    <asp:BoundField DataField="amount_if_payable" HeaderText="Amount" ItemStyle-Width="250" />
                                </Columns>
                            </asp:GridView>
                        </div>

                        <div class="form-group col-md-12">
                            <lable>
                                I herewith confirm that there is nothing pending with Mr./Ms.<b> <%=empName %> </b>other than mentioned above										
                               <br />
                            His Full & final account may please be setteled.
                        </div>
                        <div class="col-md-3">
                            Thanking You.

                            <div class="form-group">

                                <img src="<%=imgsource %>" height="100px" width="200px" />
                            </div>


                            <div class="form-group">
                                <lable>Name: </lable>
                                <b><%=thankingName %> </b>
                            </div>
                            <div class="form-group">
                                <lable>Date: </lable>
                                <b><%=thankingdate %> </b>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>


</asp:Content>

