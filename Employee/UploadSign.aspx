<%@ Page Title="" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="UploadSign.aspx.cs" Inherits="Employee_UploadSign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #img_preview {
            height: 100%;
            width: 100%;
        }
    </style>
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $(".btnimgview").click(function () {
                var btnName = $(this).attr("id");
                var img_name = "";

                if (btnName.includes("viewSign")) {
                    img_name = "Sign";

                    // Access the value of the hidden field
                    var empId = $("#<%= hidEmpId.ClientID %>").val();
                    console.log(empId);
                    $.ajax({
                        url: "UploadSign.aspx/getImgPreviewLink",
                        type: 'post',
                        dataType: "json",
                        data: '{"id": "' + empId + '", "img_name": "' + img_name + '"}',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            var res = data.d;
                            console.log(res);

                            $("#img_preview").attr('src', res);
                            $("#modalDialogScrollable").modal('show');
                            $("#doc_name").html(img_name);
                        },
                        error: function (err) {
                            alert(err.d);
                        }
                    });
                }
            });

            $("[id*=SignImg]").change(function () {
                isValidImg(this);
            });

            function isValidImg(input) {
                var fileExtension = ['jpeg', 'jpg', 'png'];
                if ($.inArray($(input).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
                    alert("Only formats are allowed : " + fileExtension.join(', '));
                    $(input).val(null);
                }
            }
        });

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>Upload Signature
            <small>Master</small>
            <br />

        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Forms</a></li>
        </ol>
    </section>
    <asp:HiddenField ID="hidEmpId" runat="server" />
    <section class="content">
        <div class="box box-default">
            <div class="box-header with-border">
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-4">
                            <asp:FileUpload ID="SignImage" runat="server" CssClass="form-control" />

                        </div>

                        <div class="col-md-4">

                            <asp:Button runat="server" Text="Upload Signature" CssClass="btn btn-success" OnClick="UploadSignature_Click" />
                            <label class="input-group-text" for="panCard">Sign</label>
                            <b><i id="sign_img_msg" runat="server"></i></b>
                        </div>
                        <div class="col-md-4">
                            <asp:Button OnClientClick="return false;" runat="server" Text="View" ID="viewSign" CssClass="btnimgview btn btn-lg btn-outline-info" Visible="false" />
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

