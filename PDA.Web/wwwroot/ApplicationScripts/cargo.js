var $Submitbtn = $('#btnSubmit');
var $Cancelbtn = $('#btnCancel');
window.existingModel = null;

if (jsonData !== null) {
    BindCargoGrid(jsonData);
}

function BindCargoGrid(jsonData) {
    for (var i = 0; i < jsonData.length; i++) {
        jsonData[i].isActive = jsonData[i].isActive === true ? "Active" : "In Active";
    }

    $('#tblCargo').bootstrapTable('destroy');
    $('#tblCargo').bootstrapTable({
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
                field: 'cargoName',
                title: 'Cargo Name',
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
        '<a href="javascript:void(0);" class="editcargo fas fa-edit" data-title="Edit" title="Edit"></a>'
    ].join('');
}

function deleteRowFormatter(value, row, index) {
    return [
        '<a class="fas fa-trash" style="color: red;" href="javascript:void(0);" onclick="DeleteCargo(\'' + row.cargoID + '\');" data-title="Delete" title="Delete"></a>'
    ].join('');
}

function CargoModel() {
    this.cargo = {
        cargoID: $('#hdnCargoID').val(),
        cargoName: $('#txtCargoName').val(),
        isActive: $('#chkIsActive').prop('checked'),
    },
        this.getCargoModel = function () {
            return this.cargo;
        }
    this.setCargoModel = function (Data) {
        $('#hdnCargoID').val(Data.portID);
        $('#txtCargoName').val(Data.cargoName);
        Data.isActive === "Active" ? $('#chkIsActive').prop('checked', true).change() : $('#chkIsActive').prop('checked', false).change();
        $("#btnSubmit").val('Update');
    }
    this.resetCargoModel = function () {
        $("#hdnCargoID").val('0');
        $('#txtCargoName').val('');
        $('#chkIsActive').prop('checked', true).change();
        $("#btnSubmit").val('Submit');
    }
}

$(function () {
    $Submitbtn.bind('click', function (e) {
        e.preventDefault();
        var submit = true;
        if (!$("#cargoform").valid()) {
            submit = false;
        }
        if (submit)
            try {
                SaveCargoData();
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
        var _cargoModel = new CargoModel();
        _cargoModel.resetCargoModel();
        Window.existingModel = null;
        ReloadPage();
    });
});

function ReloadPage() {
    var Url = '/Cargo/Index';
    location.href = Url;
}

var SaveCargoData = function () {
    debugger;
    var _cargoModel = new CargoModel();
    var _model = _cargoModel.getCargoModel();
    var _command = $('#btnSubmit').val();
    var jsonData = JSON.stringify(_model);

    $.ajax({
        url: _command === "Submit" ? '/Cargo/CreateCargo' : '/Cargo/EditCargo',
        dataType: "JSON",
        type: "POST",
        cache: false,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        data: { cargoModel: jsonData },
        async: false,
        success: function (result) {
            if (_command === "Submit") {
                SetAlert('success', 'Record has been added.');
            }
            else {
                SetAlert('success', 'Record has been updated.');
            }
            _cargoModel.resetCargoModel();
            BindCargoGrid(result.tblCargo);
        },
        error: function (e) {
            SetAlert('error', 'Error Occured While Processing Data.');
        },
        complete: function () {
        }
    });
}

function DeleteCargo(ID) {
    bootbox.confirm("Do you want to delete the Record?", function (result) {
        if (result) {
            if (ID !== "") {
                $.ajax({
                    url: '/Cargo/DeleteCargo',
                    type: "PUT",
                    dataType: "JSON",
                    data: { "CargoId": ID },
                    success: function (res) {
                        SetAlert('success', 'Record Deleted Successfully.');
                        BindCargoGrid(res.tblCargo);
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