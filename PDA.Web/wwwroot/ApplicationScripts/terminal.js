var $Submitbtn = $('#btnSubmit');
var $Cancelbtn = $('#btnCancel');
window.existingModel = null;

if (jsonData !== null) {
    BindTerminalGrid(jsonData);
}

function BindTerminalGrid(jsonData) {
    for (var i = 0; i < jsonData.length; i++) {
        jsonData[i].isActive = jsonData[i].isActive === true ? "Active" : "In Active";
    }

    $('#tblTerminal').bootstrapTable('destroy');
    $('#tblTerminal').bootstrapTable({
        data: jsonData,
        height: 500,
        pagination: true,
        pageSize: 5,
        pageList: [5, 10, 20, 50, 100],
        search: true,
        showColumns: false,
        showRefresh: false,
        cache: false,
        striped: false,
        showExport: true,
        exportTypes: ['json', 'xml', 'csv', 'txt', 'sql', 'excel', 'pdf'],
        columns: [
            {
                field: 'Edit',
                title: 'Edit',
                align: 'Center',
                valign: 'bottom',
                sortable: false,
                editable: false,
                formatter: editRowFormatter,
                events: window.operateEvents,
                width: '5%'
            },
            {
                field: 'Delete',
                title: 'Delete',
                align: 'Center',
                valign: 'bottom',
                sortable: false,
                editable: false,
                formatter: deleteRowFormatter,
                width: '5%'
            },
            {
                field: 'terminalName',
                title: 'Terminal Name',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'portName',
                title: 'Port',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'isActive',
                title: 'Active',
                align: 'center',
                valign: 'middle',
                sortable: true,
                width: '5%'
            }]
    });
}

function getHeight() {
    return $(window).height();
}

function editRowFormatter(value, row, index) {
    return [

        '<a href="javascript:void(0);" class="editTerminal fas fa-edit" data-title="Edit" title="Edit"></a>'
    ].join('');
}

function deleteRowFormatter(value, row, index) {
    return [
        '<a class="fas fa-trash" style="color: red;" href="javascript:void(0);" onclick="DeleteTerminal(\'' + row.terminalID + '\');" data-title="Delete" title="Delete"></a>'
    ].join('');
}

function TerminalModel() {
    this.terminal = {
        terminalID: $('#hdnTerminalID').val(),
        terminalName: $('#txtTerminalName').val(),
        portID: $('#ddlPort option:selected').val(),
        portName: $('#ddlPort option:selected').text(),
        isActive: $('#chkIsActive').prop('checked'),
    },
    this.getTerminalModel = function () {
        return this.terminal;
    }
    this.setTerminalModel = function (Data) {
        $('#hdnTerminalID').val(Data.terminalID);
        $('#txtTerminalName').val(Data.terminalName);
        $('#ddlPort').val(Data.portID);
        Data.isActive === "Active" ? $('#chkIsActive').prop('checked', true).change() : $('#chkIsActive').prop('checked', false).change();
        $("#btnSubmit").val('Update');
    }
    this.resetTerminalModel = function () {
        $("#hdnTerminalID").val('0');
        $('#txtTerminalName').val('');
        $('#ddlPort option:eq(0)').attr('selected', 'selected');
        $('#ddlPort').prop('SelectedIndex', 0);
        $('#chkIsActive').prop('checked', true).change();
        $("#btnSubmit").val('Submit');
    }
}

$(function () {
    $Submitbtn.bind('click', function (e) {
        e.preventDefault();
        var submit = true;
        if (!$("#terminalform").valid()) {
            submit = false;
        }
        if (submit)
            try {
                SaveTerminalData();
                window.existingModel = null;
                setTimeout(function () { ReloadPage(); }, 4000);

            }
            catch (err) {
                if (arguments !== null && arguments.callee !== null && arguments.callee.trace)
                    logError(err, arguments.callee.trace());
            }
        else {
            SetAlert('error', 'Validation failed.');
        }
    });

    $Cancelbtn.bind('click', function (e) {
        e.preventDefault();
        var _terminalModel = new TerminalModel();
        _terminalModel.resetTerminalModel();
        Window.existingModel = null;
        ReloadPage();
    });
});

function ReloadPage() {
    var Url = '/Terminal/Index';
    location.href = Url;
}

var SaveTerminalData = function () {
    var _terminalModel = new TerminalModel();
    var _model = _terminalModel.getTerminalModel();
    var _command = $('#btnSubmit').val();
    var jsonData = JSON.stringify(_model);

    $.ajax({
        url: _command === "Submit" ? '/Terminal/CreateTerminal' : '/Terminal/EditTerminal',
        dataType: "JSON",
        type: "POST",
        cache: false,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        data: { terminalModel: jsonData },
        async: false,
        success: function (result) {
            if (_command === "Submit") {
                SetAlert('success', 'Record has been added.');
            }
            else {
                SetAlert('success', 'Record has been updated.');
            }
            _terminalModel.resetTerminalModel();
            BindTerminalGrid(result.tblTerminal);
        },
        error: function (e) {
            SetAlert('error', 'Error Occured While Processing Data.');
        },
        complete: function () {
        }
    });
}

function DeleteTerminal(ID) {
    bootbox.confirm("Do you want to delete the Record?", function (result) {
        if (result) {
            if (ID !== "") {
                $.ajax({
                    url: '/Terminal/DeleteTerminal',
                    type: "PUT",
                    dataType: "JSON",
                    data: { "terminalId": ID },
                    success: function (res) {
                        SetAlert('success', 'Record Deleted Successfully.');
                        BindTerminalGrid(res.tblTerminal);
                        setTimeout(function () { ReloadPage(); }, 4000);
                    },
                    error: function (e) {
                        SetAlert('error', 'Error Occured While Processing Data.');
                    },
                    complete: function () {
                    }
                });
            }
        }
    });
}



