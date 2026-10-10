<%@ Page Language="C#" AutoEventWireup="true" CodeFile="KRAGoalSetting.aspx.cs" Inherits="Admin_KRAGoalSetting" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <!-- Latest compiled and minified CSS -->
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css"/>

<!-- jQuery library -->
<script src="https://cdn.jsdelivr.net/npm/jquery@3.7.1/dist/jquery.slim.min.js"></script>

<!-- Popper JS -->
<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>

<!-- Latest compiled JavaScript -->
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script> 

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <title>Employee Data</title>

    <style>
        table {
            width: 100%;
            border-collapse: collapse;
        }
        table, th, td {
            border: 1px solid black;
        }
        th, td {
            padding: 8px;
            text-align: left;
        }
        th {
            background-color: #f2f2f2;
        }
      .center {
    display: block;
    margin-left: auto;
    margin-right: auto;
    width: fit-content;
  }
    </style>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.0/xlsx.full.min.js"></script>
</head>
<body>
   
    <form id="form1" runat="server"  enctype="multipart/form-data">
        
            <div class="ml-2">
            <%-- <button id="downloadBtn" class="btn btn-success mt-2 mr-2" style="display: none;float:right">Download</button>--%>
             <button type="button" class="btn btn-sm btn-primary mt-5 mr-2" style="float:right" id="btnImport">Import KRA</button>
                
                </div>
        
             <a href="Dashboard.aspx" style="float: right;left: -8px;position: relative;" class="btn btn-success btn-sm mt-5">Back To Portal</a>
       
              <input type="file" class="form-control col-2 mt-5 mr-4" style="float:right" id="fileInput" accept=".csv" />
         <h4 class="mt-5 mr-5" style="right: 13rem;
    float: inline-end;
    position: relative;
    font-family: ui-monospace;
    color: darkblue;">KRA Goal Setting</h4>
 <br />
<asp:Label ID="lblMessage" runat="server" Text="" />
            <div class="row">        
            <select id="selectList" class="form-select-lg ml-2 mb-2 col-4" runat="server"><option value="" disabled selected>Select RM</option></select>
                </div>
                
            <table id="tableID" style="zoom: 85%;">
                <thead>
                    <tr >
                        <th>Employee Name</th>
                        <th>ERPID</th>
                        <th>Department</th>
                        <th>RM Name</th>
                    </tr>
                </thead>
                <tbody id="tableBody" runat="server"></tbody>
            </table>
           <div>           
           
         <table id="detailsTable" class="center mt-3 mb-3" style="display: none; zoom: 72%;">
  <thead id="detailsHeader"></thead>
  <tbody id="detailsBody"></tbody>
</table>                        
        </div>
    </form>
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            let accessTable = document.getElementById('tableID');
            let accessTableRow = accessTable.getElementsByTagName('tr');
            let accessSelectList = document.getElementById('selectList');
            let detailsTable = document.getElementById('detailsTable');
            let detailsBody = document.getElementById('detailsBody');
            let detailsHeader = document.getElementById('detailsHeader');
          

            accessSelectList.classList.add('mt-3', 'mb-3', 'form-control');
         

            let rmSet = new Set();
            for (let i = 1; i < accessTableRow.length; i++) {
                let accessTD = accessTableRow[i].getElementsByTagName('td');
                let rmCell = accessTD[3].textContent;
                rmSet.add(rmCell);
            }

            rmSet.forEach(rm => {
                let option = document.createElement('option');
                option.value = rm;
                option.textContent = rm;
                accessSelectList.appendChild(option);
            });

            accessSelectList.addEventListener('change', function () {
                let selectedValue = this.value;
                detailsBody.innerHTML = '';
                detailsHeader.innerHTML = '';
               
                detailsTable.style.display = 'none';

                for (let i = 1; i < accessTableRow.length; i++) {
                    let accessTD = accessTableRow[i].getElementsByTagName('td');
                    let rmCell = accessTD[3].textContent;

                    if (rmCell === selectedValue) {
                        let employeeNameCell = accessTD[0];
                        employeeNameCell.style.cursor = 'pointer';
                        employeeNameCell.onclick = function () {
                            let erpId = accessTD[1].textContent;
                            fetchEmployeeDetails(erpId);
                        };
                        accessTableRow[i].style.display = '';
                    } else {
                        accessTableRow[i].style.display = 'none';

                    }
                }
            });

            function fetchEmployeeDetails(erpId) {
                detailsBody.innerHTML = '';
                detailsHeader.innerHTML = '';
                detailsTable.style.display = 'none';

                var xhr = new XMLHttpRequest();
                xhr.open("POST", "KRAGoalSetting.aspx/GetEmployeeDetails", true);
                xhr.setRequestHeader("Content-Type", "application/json");

                xhr.onload = function () {
                    if (xhr.status === 200) {
                        var responseText = xhr.responseText;

                        try {
                           
                            var response = JSON.parse(responseText);

                            if (response.d.startsWith("Error:")) {
                                alert(response.d);
                            } else {
                                var data = JSON.parse(response.d);

                                if (data.length === 0) {
                                    alert("No data found.");
                                    return;
                                }

                                populateDetailsTable(data);
                                detailsTable.style.display = 'block';
                            }
                        } catch (e) {
                            
                            console.error("Invalid JSON response:", responseText);
                            alert(responseText);
                        }
                    } else {
                        alert("An error occurred while fetching data.");
                    }
                };

                xhr.send(JSON.stringify({ erpId: erpId }));
            }


            function populateDetailsTable(data) {
                console.log("Data received:", data);
                
                if (!Array.isArray(data)) {
                    console.error("Expected an array but received:", data);
                    return;
                }

                detailsBody.innerHTML = '';
                detailsHeader.innerHTML = '';

                let headlineRow = document.createElement('tr');
                let headlineRow1 = document.createElement('tr');
                let headlineRow2 = document.createElement('tr');

                let headlineCell = createHeadlineCell('KRA Goal Setting', '#60d3ff', 'white');
                let headlineCell1 = createHeadlineCell('S M A R T : Specific- Measurable-Achievable-Relevant-Time Bounding', '#8ed1ea', '#2d4460');
                let headlineCell2 = createHeadlineCell('Measurement is the process of completing things that are desired.…!', '#245061', 'white');

                headlineRow.appendChild(headlineCell);
                headlineRow1.appendChild(headlineCell1);
                headlineRow2.appendChild(headlineCell2);

                detailsHeader.appendChild(headlineRow);
                detailsHeader.appendChild(headlineRow1);
                detailsHeader.appendChild(headlineRow2);

                let headerRow = document.createElement('tr');
                let headers = [
                    'ERPID', 'Descriptions of Personal Target / Main KRA\'s', 'UOM (No\'s)', 'UOP %', 'Quantification of target / Weightage Measure (Expected Outcome)',
                    'Weightage to be assigned out of 100%', 'Achievement', 'Score', 'Id', 'Action'
                ];

                headers.forEach(function (headerText) {
                    let headerCell = document.createElement('th');
                    headerCell.textContent = headerText;
                    headerRow.appendChild(headerCell);
                });

                detailsHeader.appendChild(headerRow);
                
                data.forEach(function (item) {
                   
                    if (item.TargetKRA === 'New / Additional Ownership- Accountality') {
                        appendSpecialRow('New / Additional Ownership- Accountality', '#f2f2f2', 'bold', 'center');
                        return;
                    } else if (item.TargetKRA === 'Past year Significant contribution') {
                        appendSpecialRow('Past year Significant contribution', '#bd87df', 'bold', 'center');
                        return;
                    }
                    else if (item.TargetKRA === 'Current Financial year Significant contribution')
                    {
                        appendSpecialRow('Current Financial year Significant contribution', '#bd87df', 'bold', 'center');
                        return;
                    }

                    let row = document.createElement('tr');               
                    appendDataCell(row, item.ERPID);
                    appendDataCell(row, item.TargetKRA);
                    appendDataCell(row, item.UOM);
                    appendDataCell(row, item.UOP);
                    appendDataCell(row, item.QTarget);
                    appendDataCell(row, item.Weightage);
                    appendDataCell(row, item.achievement);
                    appendDataCell(row, item.score);
                    appendDataCell(row, item.id);
                    
                    let actionCell = document.createElement('td');
                    let createEditButton = createButton('Edit', 'btn btn-primary', '55px', '35px');
                    let createSaveButton = createButton('Save', 'btn btn-success', '55px', '35px');
                    createSaveButton.style.display = 'none'; 
              
                    createEditButton.addEventListener('click', function (event) {
                        event.preventDefault();
                        toggleEditSave(true, createEditButton, createSaveButton, row);
                    });

                    createSaveButton.addEventListener('click', function (event) {
                        event.preventDefault();

                        toggleEditSave(false, createEditButton, createSaveButton, row);

                        let targetKRA = row.cells[1].innerText;
                        let uom = row.cells[2].innerText;
                        let uop = row.cells[3].innerText;
                        let qTarget = row.cells[4].innerText;
                        let weightage = row.cells[5].innerText;
                        let achievement = parseInt(row.cells[6].innerText.trim(), 10);
                        let score = parseInt(row.cells[6].innerText.trim(),10); 
                        let id = row.cells[8].innerText;

                        console.log('Request Payload:', JSON.stringify({ targetKRA, uom, uop, qTarget, weightage, achievement, score, id }));

                        fetch('KRAGoalSetting.aspx/SaveData', {
                            method: 'POST',
                            headers: { 'Content-Type': 'application/json' },
                            body: JSON.stringify({ targetKRA, uom, uop, qTarget, weightage, achievement, score, id })
                        })
                            .then(response => {
                                console.log('Response Status:', response.status);
                                return response.json().catch(() => ({ d: { success: false, message: 'Invalid JSON response' } }));
                            })
                           
                            .then(data => {
                                console.log('Response Data:', data);
                                
                                if (data.d && data.d.success) {
                                    console.log('Data saved successfully!', data);
                                } else {

                                    console.error('Failed to save data:', data.d ? data.d.message : "Unknown error");
                                }
                            })

                            .catch(error => console.error('Error:', error));                      
                    });

                    actionCell.appendChild(createEditButton);
                    actionCell.appendChild(createSaveButton);
                    row.appendChild(actionCell);
                    detailsBody.appendChild(row);

                });

                function createHeadlineCell(text, bgColor, color) {
                    let cell = document.createElement('th');
                    cell.setAttribute('colspan', 10);
                    cell.style.textAlign = 'center';
                    cell.style.fontSize = '14px';
                    cell.style.fontWeight = 'bold';
                    cell.style.backgroundColor = bgColor;
                    cell.style.color = color;
                    cell.textContent = text;
                    return cell;
                }

                function appendSpecialRow(text, bgColor, fontWeight, textAlign) {
                    let row = document.createElement('tr');
                    let cell = document.createElement('td');
                    cell.setAttribute('colspan', 10);
                    cell.textContent = text;
                    cell.style.fontWeight = fontWeight;
                    cell.style.backgroundColor = bgColor;
                    cell.style.textAlign = textAlign;
                    row.appendChild(cell);
                    detailsBody.appendChild(row);
                }

                function appendDataCell(row, textContent) {
                    let cell = document.createElement('td');
                    cell.textContent = textContent || '';
                    row.appendChild(cell);
                }

                function createButton(text, className, width, height) {
                    let button = document.createElement('button');
                    button.classList.add(...className.split(' '));
                    button.style.width = width;
                    button.style.height = height;
                    button.textContent = text;
                    return button;
                }

                function toggleEditSave(isEditing, editBtn, saveBtn, row) {
                    editBtn.style.display = isEditing ? 'none' : 'inline';
                    saveBtn.style.display = isEditing ? 'inline' : 'none';

                    for (let i = 1; i < 8; i++) {  
                        row.cells[i].contentEditable = isEditing;
                    }
                }
            }
        });

        $(document).ready(function () {
            $('#btnImport').click(async function () {
                var fileInput = $('#fileInput')[0];

                if (fileInput.files.length === 0) {
                    $('#lblMessage').text('Please select a file to upload.').css('color', 'red');
                    return;
                }

                var file = fileInput.files[0];
                var reader = new FileReader();

                reader.onload = async function (e) {
                    var data = new Uint8Array(e.target.result);
                    var workbook = XLSX.read(data, { type: 'array' });

                    var firstSheet = workbook.Sheets[workbook.SheetNames[0]];
                    var excelRows = XLSX.utils.sheet_to_json(firstSheet, { header: 1 });

                    for (var i = 1; i < excelRows.length; i++) {
                        var rowData = excelRows[i];

                       
                        while (rowData.length < 8) {
                            rowData.push('');
                        }

                        var data = {
                            ERPID: (rowData[0] !== undefined && rowData[0] !== null) ? String(rowData[0]).trim() : '',
                            TargetKRA: (rowData[1] !== undefined && rowData[1] !== null) ? String(rowData[1]).trim() : '',
                            UOM: (rowData[2] !== undefined && rowData[2] !== null) ? encodeURIComponent(String(rowData[2]).trim()) : '',
                            UOP: (rowData[3] !== undefined && rowData[3] !== null) ? String(rowData[3]).trim() : '',
                            QTarget: (rowData[4] !== undefined && rowData[4] !== null) ? String(rowData[4]).trim() : '',
                            Weightage: (rowData[5] !== undefined && rowData[5] !== null) ? String(rowData[5]).trim() : '',
                            Achievement: (rowData[6] !== undefined && rowData[6] !== null) ? String(rowData[6]).trim() : '',
                            Score: (rowData[7] !== undefined && rowData[7] !== null) ? String(rowData[7]).trim() : ''
                        };

                        await sendData(data, i);
                    }

                    $('#lblMessage').text('Data imported successfully.').css('color', 'green');
                };

                reader.readAsArrayBuffer(file);
            });

            function sendData(data, rowNumber) {
                return new Promise(function (resolve, reject) {
                    $.ajax({
                        type: "POST",
                        url: "KRAGoalSetting.aspx/ImportXlsxData",
                        data: JSON.stringify(data),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            console.log('Row ' + rowNumber + ' inserted successfully.');
                            resolve(response);
                        },
                        error: function (err) {
                            $('#lblMessage').text('Error at row ' + rowNumber + ': ' + err.responseText).css('color', 'red');
                            reject(err);
                        }
                    });
                });
            }
        });

    </script>
</body>
</html>
