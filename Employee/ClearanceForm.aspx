<%@ Page Title="" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="ClearanceForm.aspx.cs" Inherits="Employee_ClearanceForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css'
        media="screen" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <style type="text/css">
        .readonly-gridview {
            background-color: #ffffff;
        }

        .disabled-link {
            color: gray;
            pointer-events: none;
            cursor: not-allowed;
            text-decoration: none;
        }

        #img_preview {
            height: 100%;
            width: 100%;
        }
    </style>
    <script>
        $(document).ready(function () {

            var isUserRM = <%= IsUserRMOrHODOfResignedEmployee().ToString().ToLower() %>;
            if (!isUserRM) {
                console.log(isUserRM);
                $(".readonly-gridview :input").prop('disabled', true);
            }
        });

        $(function () {
            $('[id*=txtthankingdate]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd-mm-yyyy",
                language: "tr"
            });

            $('[id*=txtdorelieving]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd-mm-yyyy",
                language: "tr"
            });

            $(".btnimgview").click(function () {
                event.preventDefault();
                // Assuming 'this' is the button that was clicked
                var imgPath = $(this).closest('tr').find('[id*=IsNocImg]').val();
                console.log(imgPath);
                imgPath = imgPath.replace("~", "");
                $("#img_preview").attr('src', imgPath);

                $("#modalDialogScrollable").modal('show');
                $("#doc_name").html(img_name);
            });

            //check images are valid 
            $("[id*=fuNocImage]").change(function () {
                isValidImg(this);
            });

        });

        function get_query() {
            var url = document.location.href;
            var qs = url.substring(url.indexOf('?') + 1).split('&');
            for (var i = 0, result = {}; i < qs.length; i++) {
                qs[i] = qs[i].split('=');
                result[qs[i][0]] = decodeURIComponent(qs[i][1]);
            }
            return result;
        }

        function isValidImg(input) {
            var fileExtension = ['jpeg', 'jpg', 'png'];
            if ($.inArray($(input).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
                alert("Only formats are allowed : " + fileExtension.join(', '));
                $(input).val(null);

            }
        }


    </script>
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
                    <h3 class="text-center" id="clearanceText" runat="server"><b>Complete Clearance Form</b></h3>
                </div>

                <div class="pull-right">
                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="SAVE" OnClick="btnSave_Click" />
                    <asp:Button ID="btnSubmit" OnClientClick="return confirm('Are you sure? no one will be able to Edit.');" runat="server" CssClass="btn btn-warning" Text="Submit" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" Text="Cancel" OnClick="btnCancel_Click" />
                </div>
            </div>
            <br />

            <div class="box-body">
                <div class="row form-group">
                    <div class="container-fluid">
                        <div class="form-group col-md-4">
                            <lable>Employee Name </lable>
                            <asp:TextBox ID="txtempname" runat="server" CssClass="form-control">
                            </asp:TextBox>
                        </div>

                        <div class="form-group col-md-4">
                            <lable>ERP ID </lable>
                            <asp:TextBox ID="txterpid" runat="server" CssClass="form-control">
                            </asp:TextBox>
                        </div>

                        <div class="form-group col-md-4">
                            <lable>Designation </lable>
                            <asp:TextBox ID="txtdesgnation" runat="server" CssClass="form-control">
                            </asp:TextBox>
                        </div>
                        <div class="form-group col-md-4">
                            <lable>Department </lable>
                            <asp:TextBox ID="txtdepartment" runat="server" CssClass="form-control">
                            </asp:TextBox>
                        </div>
                        <div class="form-group col-md-4">
                            <lable>Date of Joining </lable>
                            <asp:TextBox ID="txtdoj" runat="server" CssClass="form-control">
                            </asp:TextBox>
                        </div>
                        <div class="form-group col-md-4">
                            <lable>Location </lable>
                            <asp:TextBox ID="txtlocation" runat="server" CssClass="form-control">
                            </asp:TextBox>
                        </div>
                        <div class="form-group col-md-4">
                            <lable>Date of Resignation </lable>
                            <asp:TextBox ID="txtdor" runat="server" CssClass="form-control">
                            </asp:TextBox>
                        </div>
                        <div class="form-group col-md-4">
                            <lable>Date of Relieving </lable>
                            <asp:TextBox ID="txtdorelieving" runat="server" CssClass="form-control">
                            </asp:TextBox>
                        </div>
                        <div class="form-group col-md-4">
                            <lable>Reporting to </lable>
                            <asp:TextBox ID="txtrm" runat="server" CssClass="form-control">
                            </asp:TextBox>
                        </div>
                        <div class="form-group col-md-4">
                            <lable>Remarks </lable>
                            <asp:TextBox ID="txtremark" runat="server" CssClass="form-control">
                            </asp:TextBox>
                        </div>
                        <div class="form-group col-md-12">
                            <lable>
                                I have checked with the distributors & in market,  following is the status in respect with
                                Mr./Ms.<b> <%=txtempname.Text %> </b>was working under me.</lable>
                        </div>


                        <div class="col-md-10" style="overflow-x: auto">
                            <asp:GridView ID="gdSuperiorClearance" runat="server" CssClass="table table-striped table-bordered table-hover readonly-gridview"
                                AllowPaging="true" OnRowCreated="gdSuperiorClearance_RowCreated" ShowFooter="true"
                                AutoGenerateColumns="false">
                                <Columns>

                                    <asp:TemplateField HeaderText="Distributor Child Id">
                                        <ItemTemplate>
                                            <div class="btn-group">
                                                <asp:Label ID="dccId" runat="server" Width="100%" CssClass="form-control " ReadOnly="true" Text='<%# Eval("Column1") %>'>
                                                </asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Distributor List" ItemStyle-Width="450">
                                        <ItemTemplate>
                                            <div class="btn-group">
                                                <asp:DropDownList ID="ddlDistributorList" runat="server" Width="100%" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="NOC Receieved Yes/No">
                                        <ItemTemplate>
                                            <div class="btn-group">
                                                <asp:DropDownList ID="ddlnoc" runat="server" CssClass="form-control ">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem>Yes</asp:ListItem>
                                                    <asp:ListItem>No</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Desription" ItemStyle-Width="200">
                                        <ItemTemplate>
                                            <div class="btn-group">
                                                <asp:TextBox ID="txtdescription" runat="server" CssClass="form-control">
                                                </asp:TextBox>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Balance Nill or (Payable / Recoverable)" ItemStyle-Width="50">
                                        <ItemTemplate>
                                            <div class="btn-group">
                                                <asp:TextBox ID="txtbill" runat="server" CssClass="form-control ">
                                                </asp:TextBox>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Upload Image" ItemStyle-Width="400">
                                        <ItemTemplate>
                                            <div class="row">
                                                <div class="col-md-8">
                                                    <asp:FileUpload ID="fuNocImage" runat="server" CssClass="form-control" />
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:Button runat="server" Text="View" ID="viewNoc" CssClass="btnimgview btn btn-sm btn-outline-info" Visible="false" />
                                                </div>
                                            </div>
                                            <asp:HiddenField ID="IsNocImg" runat="server" />

                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <FooterTemplate>
                                            <asp:Button ID="ButtonAdd" runat="server"
                                                Text="Add New Row" CssClass="btn btn-sm btn-bitbucket"
                                                OnClick="ButtonAdd_Click" />
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server"
                                                OnCommand="RemoveRow_Command" CommandName="Remove" CommandArgument='<%# Container.DataItemIndex %>' CssClass='<%# IsUserRMOrHODOfResignedEmployee() ? "" : "disabled-link" %>'>
                                                Remove</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        
                        <div class="col-md-8 my-2" style="overflow-x: auto">
                            <asp:GridView ID="gdAssetList" runat="server" CssClass="table table-striped table-bordered table-hover readonly-gridview"
                                AllowPaging="true" OnRowDataBound="gdAssetList_RowDataBound"
                                AutoGenerateColumns="false">
                                <Columns>
                                     <asp:TemplateField HeaderText="Asset Noc Id">
                                        <ItemTemplate>
                                            <div class="btn-group">
                                                <asp:Label ID="assetId" runat="server" Width="100%" CssClass="form-control " ReadOnly="true" Text='<%# Eval("Id") %>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="AssesstName" HeaderText="Asset Name" ItemStyle-Width="250" />

                                    <asp:TemplateField HeaderText="Return Back Status">
                                        <ItemTemplate>
                                            <div class="btn-group">
                                                <asp:TextBox ID="txtrbs" runat="server" CssClass="form-control" AutoComplete="Off"></asp:TextBox>
                                                <asp:Label ID="lblBs" runat="server" Text='<%# Eval("ReturnBackStatus") %>' Visible="false"></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Returned to Whom">
                                        <ItemTemplate>
                                            <div class="btn-group">
                                                <asp:TextBox ID="txtrtw" runat="server" CssClass="form-control" AutoComplete="Off"></asp:TextBox>
                                                <asp:Label ID="lblRtw" runat="server" Text='<%# Eval("ReturnedToWhom") %>' Visible="false"></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="col-md-8" style="overflow-x: auto">
                            <asp:GridView ID="gdDeptNOC" runat="server" class="table table-striped table-bordered table-hover"
                                AllowPaging="true" OnRowDataBound="gdDeptNOC_RowDataBound"
                                AutoGenerateColumns="false">
                                <Columns>

                                    <asp:TemplateField HeaderText="Dept Noc Id">
                                        <ItemTemplate>
                                            <div class="btn-group">
                                                <asp:Label ID="dncId" runat="server" Width="100%" CssClass="form-control " ReadOnly="true" Text='<%# Eval("id") %>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="department_name" HeaderText="Department Name" ItemStyle-Width="250" />

                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <div class="form-group">
                                                <asp:TextBox Placeholder="Enter Description" CssClass="form-control" ID="txtDesc" runat="server" AutoComplete="Off"></asp:TextBox>
                                                <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("description") %>' Visible="false"></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Is NOC Received">
                                        <ItemTemplate>
                                            <div class="form-group">

                                                <asp:DropDownList ID="ddlNoc" runat="server" CssClass="form-control">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem>Yes</asp:ListItem>
                                                    <asp:ListItem>No</asp:ListItem>
                                                </asp:DropDownList>

                                                <asp:Label ID="lblNoc" runat="server" Text='<%# Eval("is_noc_recieved") %>' Visible="false"></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Amount Payable(if any)">
                                        <ItemTemplate>
                                            <div class="form-group">
                                                <asp:TextBox Placeholder="Enter Amount" CssClass="form-control" ID="txtAmt" runat="server" AutoComplete="Off"></asp:TextBox>
                                                <asp:Label ID="lblAmt" runat="server" Text='<%# Eval("amount_if_payable") %>' Visible="false"></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>

                        <div class="form-group col-md-12">
                            <lable>
                                I herewith confirm that there is nothing pending with Mr./Ms.<b> <%=txtempname.Text %> </b>other than mentioned above										
                               <br />
                            His Full & final account may please be setteled.
                        </div>
                        <div class="col-md-3">
                            Thanking You.
                            <asp:Panel ID="bottomFields" runat="server">
                                <div class="form-group">
                                    <lable>Signature </lable>
                                    <asp:FileUpload ID="fusign" runat="server"></asp:FileUpload>
                                </div>

                                <div class="form-group">
                                    <lable>Name </lable>
                                    <asp:TextBox ID="txtthankingname" runat="server" CssClass="form-control">
                                    </asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <lable>Date </lable>

                                    <asp:TextBox ID="txtthankingdate" runat="server" CssClass="form-control">
                                    </asp:TextBox>
                                </div>

                            </asp:Panel>

                            <asp:Panel ID="bottomFilledFields" runat="server" Visible="false">
                                <div class="form-group">
                                    <lable>Signature </lable>
                                    <img src="<%=imgsource %>" height="100px" width="200px" />
                                </div>

                                <div class="form-group">
                                    <lable>Name </lable>
                                    <b><%=thankingName %> </b>
                                </div>
                            </asp:Panel>
                            <div class="form-group">
                                <lable>Date </lable>
                                <b><%=thankingDate %></b>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <div class="modal fade" id="modalDialogScrollable" tabindex="-1">
        <div class="modal-dialog modal-dialog-scrollable modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h2 class="modal-title">Document Preview</h2>
                    <h3 id="doc_name"></h3>
                </div>
                <div class="modal-body">
                    <img src="#" alt="img" id="img_preview" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" onclick="$('#modalDialogScrollable').modal('hide');">Close</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>


