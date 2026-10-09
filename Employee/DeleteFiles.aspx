<%@ Page Title="" Language="C#" MasterPageFile="~/Employee/MasterPage.master" AutoEventWireup="true" CodeFile="DeleteFiles.aspx.cs" Inherits="Employee_Payregister" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

     <style type="text/css">
        .divrow
        {
            width: 100%;
        }
        .divcontrols
        {
            width: 100%;
        }
        /*.divcontrols 
        { float: left;
    margin: .5em;
    padding: 1em;
    width: 33%;
    height: 300px; 
    
    } */
        
        .ctrlwithCaption
        {
            float: left;
            margin: -1.5em 0.5em -1.5em -1.5em;
            padding: 0.6em; /* width: 33%;
    height: 300px;*/ /*border: .1em solid black;*/
        }
        
        .ctrlwithCaption2
        {
            float: left;
            margin: .5em;
            padding: 1em;
            width: 45%; /* width: 33%;
    height: 300px;*/ /*border: .1em solid black;*/
        }
        .ctrlwithCaption1
        {
            float: left;
            margin: .5em;
            padding: 1em;
            width: 100%; /* width: 33%;
    height: 300px;*/ /*border: .1em solid black;*/
        }
        .ctrlwithCaption div
        {
            float: left;
            margin: .5em;
            padding: 1em; /* width: 50%;
    height: 300px;*/ /*border: .1em solid black;*/
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

     <section class="content-header">
        <h1>Job Applications
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

                <div class="col-lg-3">
                    <asp:FileUpload ID="fileuploadSO" runat="server" CssClass="form-control">
                    </asp:FileUpload>
                </div>
                <div class="col-lg-3">
                    <asp:Button CssClass="btn btn-bitbucket" Text="Upload" runat="server" OnClick="fultr_click" />
                </div>
                  <div class="col-lg-3 pull-right">
                    <asp:Button CssClass="btn btn-success" Text="PayRegisterEmployeeList" runat="server" OnClick="btnEmpList_click" />
                </div>
            </div>

            <br />

            <div class="box-body">
                <br />
                
            </div>
        </div>
    </section>


</asp:Content>

