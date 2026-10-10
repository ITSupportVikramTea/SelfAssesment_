<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" CodeFile="OrganizationChart.aspx.cs" Inherits="Admin_OrganizationChart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--<script type="text/javascript" src="https://www.google.com/jsapi"></script> -->
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>


    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['corechart'], 'callback': drawChartDepartment });
        //google.charts.load('current', { 'packages': ['corechart'], 'callback': drawChartType });

        $(document).ready(function () {

            $('[id*=dldepartments]').on('change', function () {
                //google.load("visualization", "1", { packages: ["orgchart"] });
                //google.charts.load('current', { packages: ["corechart"] });
                google.charts.load('current', { 'packages': ['corechart'], 'callback': drawChartDepartment });
                //google.charts.setOnLoadCallback(drawChart);
            });

            $('[id*=dlcharttype]').on('change', function () {
                //google.load("visualization", "1", { packages: ["orgchart"] });
                //google.charts.load('current', { packages: ["corechart"] });
                google.charts.load('current', { 'packages': ['corechart'], 'callback': drawChartType });
                //google.charts.setOnLoadCallback(drawChart);
            });

        });




        function drawChartDepartment() {

            var department = $('[id*=dldepartments]').val();

            //google.charts.load('current', { packages: ["orgchart"] });
            //google.charts.setOnLoadCallback(drawChart);

            $.ajax({
                url: 'OrganizationChart.aspx/getCharData',
                type: 'post',
                dataType: "json",
                data: '{"department": "' + department + '"}',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var res = data.d;
                    var chartData = new google.visualization.DataTable();
                    chartData.addColumn('string', 'Name');
                    chartData.addColumn('string', 'HOD');
                    chartData.addColumn('string', 'RM');
                    chartData.addColumn('string', 'ASM');
                    chartData.addColumn('string', 'SDM');
                    chartData.addColumn('string', 'ToolTip');


                    $.each(JSON.parse(res), function (index, row) {
                        var Id = row.Id.toString() == "0" ? '' : row.Id.toString();


                        chartData.addRows([
                            [row.EmpName.toString(), row.HOD.toString(), row.RM.toString(), row.ASM.toString(), row.SDM.toString(), row.EmpDesignation.toString()],

                        ]);



                        /* chartData.addRows([
                             [{ 'v': 'Mike', 'f': 'Mike<div style="color:red; font-style:italic">President</div>' },
                                 '', 'The President'],
                             [{ 'v': 'Jim', 'f': 'Jim<div style="color:red; font-style:italic">Vice President</div>' },
                                 'Mike', 'VP'],
                             ['Alice', 'Mike', ''],
                             ['Bob', 'Jim', 'Bob Sponge'],
                             ['Carol', 'Bob', '']
                         ]);*/

                        /*chartData.addRows([[{
                            v: row.EmpName.toString() + '<div>(<span>' + row.EmpDesignation.toString() + '</span>)</div>',
                            f: row.RM.toString(),
                        }, Id, row.Id.toString()]]);*/
                    });






                    google.charts.load('current', { packages: ["orgchart"] });
                    var chart = new google.visualization.OrgChart(document.getElementById('chart_div'));
                    // Draw the chart, setting the allowHtml option to true for the tooltips.
                    chart.draw(chartData, { 'allowHtml': true });
                    //alert('Success');

                },
                error: function (err) {
                    alert('Failed');
                }


            })


        }

        function drawChartType() {

            var type = $('[id*=dlcharttype]').val();

            //google.charts.load('current', { packages: ["orgchart"] });
            //google.charts.setOnLoadCallback(drawChart);

            $.ajax({
                url: 'OrganizationChart.aspx/getCharDataType',
                type: 'post',
                dataType: "json",
                data: '{"type": "' + type + '"}',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var res = data.d;
                    var chartData = new google.visualization.DataTable();
                    chartData.addColumn('string', 'Name');
                    chartData.addColumn('string', 'HOD');
                    chartData.addColumn('string', 'RM');
                    chartData.addColumn('string', 'ASM');
                    chartData.addColumn('string', 'SDM');
                    chartData.addColumn('string', 'ToolTip');


                    $.each(JSON.parse(res), function (index, row) {
                        var Id = row.Id.toString() == "0" ? '' : row.Id.toString();


                        chartData.addRows([
                            [row.EmpName.toString(), row.HOD.toString() + '<div style="color:red; font-style:italic">President</div>', row.RM.toString(), row.ASM.toString(), row.SDM.toString(), row.EmpDesignation.toString()],

                        ]);



                        /* chartData.addRows([
                             [{ 'v': 'Mike', 'f': 'Mike<div style="color:red; font-style:italic">President</div>' },
                                 '', 'The President'],
                             [{ 'v': 'Jim', 'f': 'Jim<div style="color:red; font-style:italic">Vice President</div>' },
                                 'Mike', 'VP'],
                             ['Alice', 'Mike', ''],
                             ['Bob', 'Jim', 'Bob Sponge'],
                             ['Carol', 'Bob', '']
                         ]);*/

                        /*chartData.addRows([[{
                            v: row.EmpName.toString() + '<div>(<span>' + row.EmpDesignation.toString() + '</span>)</div>',
                            f: row.RM.toString(),
                        }, Id, row.Id.toString()]]);*/
                    });






                    google.charts.load('current', { packages: ["orgchart"] });
                    var chart = new google.visualization.OrgChart(document.getElementById('chart_div'));
                    // Draw the chart, setting the allowHtml option to true for the tooltips.
                    chart.draw(chartData, { 'allowHtml': true });
                    //alert('Success');

                },
                error: function (err) {
                    alert('Failed');
                }


            })


        }













    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="content-header">
        <h1>Organization Chart
            <small>Master</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Forms</a></li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">

        <!-- SELECT2 EXAMPLE -->
        <div class="box box-default">
            <div class="box-header with-border">
                <h3 class="box-title">Forms</h3>

                <div class="row">
                    <div class="col-lg-6 pull-right">

                        <h4>Select Department</h4>
                        <div class="form-group">
                            <asp:DropDownList runat="server" ID="dldepartments" AppendDataBoundItems="true" CssClass="form-control ">
                                <asp:ListItem Text="" Value="" />
                            </asp:DropDownList>

                        </div>
                    </div>

                    <!--<div class="col-lg-6 pull-left">

                        <h4>Select Type</h4>
                        <div class="form-group">
                            <asp:DropDownList runat="server" ID="dlcharttype" AppendDataBoundItems="true" CssClass="form-control ">
                                <asp:ListItem Text="" Value="" />
                                <asp:ListItem Text="All" Value="All" />
                                <asp:ListItem Text="Leaders" Value="Leaders" />
                                <asp:ListItem Text="Sales" Value="Sales" />
                                <asp:ListItem Text="Operations" Value="Operations" />
                            </asp:DropDownList>

                        </div>
                    </div>-->
                </div>
            </div>
            <!-- /.box-header -->
            <div class="box-body">
                <br />
                <div class="container">

                    <div id="chart_div" style="overflow-x: auto;"></div>
                </div>

            </div>
            <!-- /.box-body -->

        </div>
        <!-- /.box -->


        <!-- /.row -->

    </section>
</asp:Content>
