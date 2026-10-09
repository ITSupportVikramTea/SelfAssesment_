<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Employee/MasterPage.master" CodeFile="ViewPolicies.aspx.cs" Inherits="Employee_ViewPolicies" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/7.0.1/css/all.min.css" integrity="sha512-2SwdPD6INVrV/lHTZbO2nodKhrnDdJK9/kg2XD1r9uGqPo1cUbujc+IYdlYdEErWNu69gVcYgdxlmVmzTWnetw==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/7.0.1/js/all.min.js" integrity="sha512-6BTOlkauINO65nLhXhthZMtepgJSghyimIalb+crKRPhvhmsCdnIuGcVbR5/aQY2A+260iC1OPy1oCdB6pSSwQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
  <style>
    .policy-container {
    padding: 30px;
}

.policy-header {
    text-align: center;
    margin-bottom: 40px;
}

.policy-header h2 {
    font-size: 34px;
    font-weight: 700;
    color: #1e293b;
}

.policy-header p {
    color: #64748b;
    margin-top: 10px;
}

.policy-card {
    background: #fff;
    border-radius: 18px;
    padding: 25px;
    height: 100%;
    box-shadow: 0 8px 30px rgba(0,0,0,.08);
    transition: all .3s ease;
    border: 1px solid #eef2f7;
}

.policy-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 15px 40px rgba(0,0,0,.12);
}

.policy-icon {
    width: 65px;
    height: 65px;
    line-height: 65px;
    text-align: center;
    font-size: 32px;
    background: linear-gradient(135deg,#4f46e5,#7c3aed);
    border-radius: 15px;
    margin-bottom: 20px;
}

.policy-title {
    font-weight: 700;
    color: #1e293b;
    margin-bottom: 10px;
}

.policy-description {
    color: #64748b;
    min-height: 70px;
    font-size: 14px;
}

.policy-footer {
    margin-top: 20px;
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.policy-date {
    color: #94a3b8;
}

.view-btn {
    background: linear-gradient(135deg,#4f46e5,#7c3aed);
    color: white !important;
    text-decoration: none !important;
    padding: 10px 18px;
    border-radius: 10px;
    font-weight: 600;
    transition: .3s;
}

.view-btn:hover {
    color: white !important;
    transform: scale(1.05);
    box-shadow: 0 10px 20px rgba(79,70,229,.25);
}
      </style>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<div class="policy-container">

    <div class="policy-header">
        <h2>📚 Company Policies</h2>
        <p>View company policies and guidelines</p>
    </div>

    <div class="row">

        <asp:Repeater ID="rptPolicies" runat="server">

            <ItemTemplate>

                <div class="col-md-6 col-lg-4 mb-4">

                    <div class="policy-card">

                        <div class="policy-icon">
                          <i class="fa-solid fa-paperclip fa-2x"></i>
                        </div>

                        <h5 class="policy-title">
                            <%# Eval("PolicyTitle") %>
                        </h5>

                        <p class="policy-description">
                            <%# Eval("Description") %>
                        </p>

                        <div class="policy-footer">

                            <small class="policy-date">
                                <%# Convert.ToDateTime(Eval("UploadDate")).ToString("dd MMM yyyy") %>
                            </small>

                            <a href='<%# ResolveUrl(Eval("FilePath").ToString()) %>'
                                target="_blank"
                                class="view-btn">
                                View PDF
                            </a>

                        </div>

                    </div>

                </div>

            </ItemTemplate>

        </asp:Repeater>

    </div>

</div>
    
</asp:Content>