<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/MasterPage.master" CodeFile="UploadPolicy.aspx.cs" Inherits="Admin_UploadPolicy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
  .policy-container {
    min-height: 80vh;
    display: flex;
    justify-content: center;
    align-items: center;
    padding: 40px 15px;
    background: #f4f7fb;
}

.policy-card {
    width: 100%;
    max-width: 700px;
    background: #fff;
    border-radius: 20px;
    padding: 35px;
    box-shadow: 0 15px 40px rgba(0,0,0,0.08);
    border: 1px solid #eef2f7;
    transition: all 0.3s ease;
}

.policy-card:hover {
    transform: translateY(-3px);
    box-shadow: 0 20px 50px rgba(0,0,0,0.12);
}

.policy-header {
    text-align: center;
    margin-bottom: 30px;
}

.policy-header h2 {
    font-size: 28px;
    font-weight: 700;
    color: #1f2937;
    margin-bottom: 5px;
}

.policy-header p {
    color: #6b7280;
    margin: 0;
}

.form-group {
    margin-bottom: 22px;
}

.form-group label {
    display: block;
    margin-bottom: 8px;
    font-weight: 600;
    color: #374151;
}

.modern-input {
    width: 100%;
    border: 1px solid #dbe3ee;
    border-radius: 12px;
    padding: 12px 15px;
    font-size: 15px;
    transition: all .3s ease;
}

.modern-input:focus {
    outline: none;
    border-color: #4f46e5;
    box-shadow: 0 0 0 4px rgba(79,70,229,.12);
}

.modern-file {
    width: 100%;
    padding: 10px;
    border: 2px dashed #cbd5e1;
    border-radius: 12px;
    background: #fafbfc;
}

.modern-btn {
    background: linear-gradient(135deg,#4f46e5,#7c3aed);
    color: white;
    border: none;
    border-radius: 12px;
    padding: 12px 35px;
    font-size: 15px;
    font-weight: 600;
    cursor: pointer;
    transition: all .3s ease;
}

.modern-btn:hover {
    transform: translateY(-2px);
    box-shadow: 0 10px 25px rgba(79,70,229,.25);
}

.modern-btn:active {
    transform: translateY(0px);
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

  <div class="policy-container">

    <div class="policy-card">

        <div class="policy-header">
            <h2>📄 Company Policy Upload</h2>
            <p>Upload and manage company policies</p>
        </div>

        <div class="form-group">
            <label>Policy Title</label>
            <asp:TextBox ID="txtTitle"
                runat="server"
                CssClass="modern-input"
                placeholder="Enter policy title">
            </asp:TextBox>
        </div>

        <div class="form-group">
            <label>Description</label>
            <asp:TextBox ID="txtDescription"
                runat="server"
                CssClass="modern-input"
                TextMode="MultiLine"
                Rows="5"
                placeholder="Enter policy description">
            </asp:TextBox>
        </div>

        <div class="form-group">
            <label>Policy PDF</label>
            <asp:FileUpload ID="fuPolicy"
                runat="server"
                CssClass="modern-file" />
        </div>

        <div class="text-center mt-4">
            <asp:Button ID="btnSave"
                runat="server"
                Text="Upload Policy"
                CssClass="modern-btn"
                OnClick="btnSave_Click" />
        </div>

    </div>

</div>

</asp:Content>