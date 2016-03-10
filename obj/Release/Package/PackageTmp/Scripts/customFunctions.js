function addTableRow(tableID, cellId) {
    var table = document.getElementById(tableID);
    var count = (table.rows.length) -1;
    var cellCount = count * 2;
    if (count <= 10) {
        var row = table.insertRow(-1);
        var cell1 = row.insertCell(0);
        var cell2 = row.insertCell(1);
        cell1.innerHTML = '<span id=' + cellId + (cellCount + 1) + ' contenteditable></span>';
        cell2.innerHTML = '<span id=' + cellId + (cellCount + 2) + ' contenteditable></span>';
    }
    else {
        alert("Cannot have more than 10 rows");
        return;
    }
    

}

function addChecklistRows() {
    var table = document.getElementById('checkTable');
    var count = table.rows.length;
    while (count <= 10) {
        var row = table.insertRow(-1);
        var cell1 = row.insertCell(0);
        var cell2 = row.insertCell(1);
        var cell3 = row.insertCell(2);
        var cell4 = row.insertCell(3);
        var cell5 = row.insertCell(4);
        var cell6 = row.insertCell(5);
        var cell7 = row.insertCell(6);
        var cell8 = row.insertCell(7);
        var cell9 = row.insertCell(8);

        cell1.innerHTML = '<span id= areaCell' + count + ' runat="server"></span>';
        cell2.innerHTML = '<span id= parCell' + count + ' runat="server"></span>';
        cell3.innerHTML = '<span id= action' + count + ' runat="server" contenteditable></span>';
        cell4.innerHTML = '<span id= priority' + count + ' runat="server" contenteditable></span>';
        cell5.innerHTML = '<span id= client' + count + ' runat="server" contenteditable></span>';
        cell6.innerHTML = '<span id= mbAssign'  + count + ' runat="server" contenteditable></span>';
        cell7.innerHTML = '<span id= complete'  + count + ' runat="server" contenteditable></span>';
        cell8.innerHTML = '<span id= finDate'  + count + ' runat="server" contenteditable></span>';
        cell9.innerHTML = '<span id= comments' + count + ' runat="server" contenteditable></span>';

        count++;
    }

}




