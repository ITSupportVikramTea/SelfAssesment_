<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Employee/MasterPage.master" CodeFile="TestAttendence.aspx.cs" Inherits="Employee_TestAttendence" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Google Sheets Data</title>
      <link rel="stylesheet" href="../bower_components/bootstrap/dist/css/bootstrap.min.css" />
     <style>
     .highlighted-cell {
         border: 2px solid #3498db; /* Blue border */
         background-color: #f0f8ff; /* Light blue background */
     }

     h5{
     font-family: cursive;
     display:inline-block;

        }

     .wrapper
     {
         max-height:auto;
         max-width:auto;
         overflow:auto;
        background-color: aliceblue;
     }

     #GridView1
     {
        width: 16rem;
        height: 60px;
     }

     table
     {
         border-collapse: collapse;
         background-color: ghostwhite;
         border-collapse: collapse;
         text-align: center;
         text-wrap: nowrap;
         zoom: 81%;
         /*border: blanchedalmond;*/
     }

     td, th {
    padding: 4px;
    text-align:center;
}
 </style>
 <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
 <script type="text/javascript">
     $(document).ready(function () {
         $('td').on('click', function () {
           
             $('td').removeClass('highlighted-cell');

           
             $(this).addClass('highlighted-cell');
         });
     });
 </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div class="text-center">
         <h5 class ="d-flex justify-content-center ">Monthly Attendence</h5>
          
                <p>P = PRESENT, W/O = WEEKLY OFF / SUNDAY, LWP / A = ABSENT / UNPAID LEAVE </br>
                    PH = PAID HOLIDAY,  PL = PAID LEAVE, CL = CASUAL LEAVESL  SL = SICK LEAVE </br>
               
                <p>HSL = HALF DAY SICK LEAVE, HCL = HALF DAY CASUAL LEAVE</p>
           <p>HPL = HALF DAY PAID LEAVE, C/O = COFF / COMPENSETORY OFF</p> 
   
          </div>

        <div class="wrapper justify-content-center" style="background-color: ghostwhite;">     
        <asp:Label ID="DataLabel" CssClass="text-center" runat="server" Text="" ></asp:Label>
       <asp:GridView ID="GridView1" runat="server" class="table table-bordered" AutoGenerateColumns="true">
    <HeaderStyle HorizontalAlign="Center" />
</asp:GridView>

            </div>
    </div>
</asp:Content>






 