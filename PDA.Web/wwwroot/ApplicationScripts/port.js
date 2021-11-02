var $Submitbtn = $('#btnSubmit');
var $Cancelbtn = $('#btnCancel');
window.existingModel = null;

if (jsonData !== null) {
    BindPortGrid(jsonData);
}

function BindPortGrid(jsonData) {
    for (var i = 0; i < jsonData.length; i++) {
        jsonData[i].isActive = jsonData[i].isActive === true ? "Active" : "In Active";
    }

    $('#tblPort').bootstrapTable('destroy');
    $('#tblPort').bootstrapTable({
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
            }, {
                field: 'portName',
                title: 'Port Name',
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
        '<a href="javascript:void(0);" class="editport fas fa-edit" data-title="Edit" title="Edit"></a>'
    ].join('');
}

function deleteRowFormatter(value, row, index) {
    return [
        '<a class="fas fa-trash" style="color: red;" href="javascript:void(0);" onclick="DeletePort(\'' + row.portID + '\');" data-title="Delete" title="Delete"></a>'
    ].join('');
}

function PortModel() {
    this.port = {
        portID: $('#hdnPortID').val(),
        portName: $('#txtPortName').val(),
        isActive: $('#chkIsActive').prop('checked'),
    },
        this.getPortModel = function () {
        return this.port;
        }
    this.setPortModel = function (Data) {
        $('#hdnPortID').val(Data.portID);
        $('#txtPortName').val(Data.portName);
        Data.isActive === "Active" ? $('#chkIsActive').prop('checked', true).change() : $('#chkIsActive').prop('checked', false).change();
        $("#btnSubmit").val('Update');
    }
    this.resetPortModel = function () {
        $("#hdnPortID").val('0');
        $('#txtPortName').val('');
        $('#chkIsActive').prop('checked', true).change();
        $("#btnSubmit").val('Submit');
    }
}


$(function () {
    $Submitbtn.bind('click', function (e) {
        e.preventDefault();
        var submit = true;
        if (!$("#portform").valid()) {
            submit = false;
        }
        if (submit)
            try {
                SavePortData();
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
        var _portModel = new PortModel();
        _portModel.resetPortModel();
        Window.existingModel = null;
        ReloadPage();
    });
});

function ReloadPage() {
    var Url = '/Port/Index';
    location.href = Url;
}

var SavePortData = function () {
    var _portModel = new PortModel();
    var _model = _portModel.getPortModel();
    var _command = $('#btnSubmit').val();
    var jsonData = JSON.stringify(_model);

    $.ajax({
        url: _command === "Submit" ? '/Port/CreatePort' : '/Port/EditPort',
        dataType: "JSON",
        type: "POST",
        cache: false,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        data: { portModel: jsonData },
        async: false,
        success: function (result) {
            if (_command === "Submit") {
                SetAlert('success', 'Record has been added.');
            }
            else {
                SetAlert('success', 'Record has been updated.');
            }
            _portModel.resetPortModel();
            BindPortGrid(result.tblPort);
        },
        error: function (e) {
            SetAlert('error', 'Error Occured While Processing Data.');
        },
        complete: function () {
        }
    });
}

function DeletePort(ID) {
    bootbox.confirm("Do you want to delete the Record?", function (result) {
        if (result) {
            if (ID !== "") {
                $.ajax({
                    url: '/Port/DeletePort',
                    type: "PUT",
                    dataType: "JSON",
                    data: { "PortId": ID },
                    success: function (res) {
                        SetAlert('success', 'Record Deleted Successfully.');
                        BindPortGrid(res.tblPort);
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

