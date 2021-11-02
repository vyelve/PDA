var $Submitbtn = $('#btnSubmit');
var $Cancelbtn = $('#btnCancel');
window.existingModel = null;

if (jsonData !== null) {
    BindVesselGrid(jsonData);
}

function BindVesselGrid(jsonData) {
    for (var i = 0; i < jsonData.length; i++) {
        jsonData[i].isActive = jsonData[i].isActive === true ? "Active" : "In Active";
    }

    $('#tblVesselType').bootstrapTable('destroy');
    $('#tblVesselType').bootstrapTable({
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
                field: 'vesselTypeName',
                title: 'Vessel Type',
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
        '<a href="javascript:void(0);" class="editvessel fas fa-edit" data-title="Edit" title="Edit"></a>'
    ].join('');
}

function deleteRowFormatter(value, row, index) {
    return [
        '<a class="fas fa-trash" style="color: red;" href="javascript:void(0);" onclick="DeleteVesselType(\'' + row.vesselTypeID + '\');" data-title="Delete" title="Delete"></a>'
    ].join('');
}

function VesselTypeModel() {
    this.vesselType = {
        vesselTypeID: $('#hdnVesselTypeID').val(),
        vesselTypeName: $('#txtVesselTypeName').val(),
        isActive: $('#chkIsActive').prop('checked'),
    },
        this.getVesselTypeModel = function () {
        return this.vesselType;
        }
    this.setVesselTypeModel = function (Data) {
        $('#hdnVesselTypeID').val(Data.vesselTypeID);
        $('#txtVesselTypeName').val(Data.vesselTypeName);
        Data.isActive === "Active" ? $('#chkIsActive').prop('checked', true).change() : $('#chkIsActive').prop('checked', false).change();
        $("#btnSubmit").val('Update');
    }
    this.resetVesselTypeModel = function () {
        $("#hdnVesselTypeID").val('0');
        $('#txtVesselTypeName').val('');
        $('#chkIsActive').prop('checked', true).change();
        $("#btnSubmit").val('Submit');
    }
}


$(function () {
    $Submitbtn.bind('click', function (e) {
        e.preventDefault();
        var submit = true;
        if (!$("#vesselform").valid()) {
            submit = false;
        }
        if (submit)
            try {
                SaveVesselTypeData();
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
        var _vesselTypeModel = new VesselTypeModel();
        _vesselTypeModel.resetVesselTypeModel();
        Window.existingModel = null;
        ReloadPage();
    });
});

function ReloadPage() {
    var Url = '/VesselType/Index';
    location.href = Url;
}

var SaveVesselTypeData = function () {
    var _vesselTypeModel = new VesselTypeModel();
    var _model = _vesselTypeModel.getVesselTypeModel();
    var _command = $('#btnSubmit').val();
    var jsonData = JSON.stringify(_model);

    $.ajax({
        url: _command === "Submit" ? '/VesselType/CreateVesselType' : '/VesselType/EditVesselType',
        dataType: "JSON",
        type: "POST",
        cache: false,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        data: { vesselTypeModel: jsonData },
        async: false,
        success: function (result) {
            if (_command === "Submit") {
                SetAlert('success', 'Record has been added.');
            }
            else {
                SetAlert('success', 'Record has been updated.');
            }
            _vesselTypeModel.resetVesselTypeModel();
            BindVesselGrid(result.tblVesselType);
        },
        error: function (e) {
            SetAlert('error', 'Error Occured While Processing Data.');
        },
        complete: function () {
        }
    });
}

function DeleteVesselType(ID) {
    bootbox.confirm("Do you want to delete the Record?", function (result) {
        if (result) {
            if (ID !== "") {
                $.ajax({
                    url: '/VesselType/DeleteVesselType',
                    type: "PUT",
                    dataType: "JSON",
                    data: { "VesselTypeID": ID },
                    success: function (res) {
                        SetAlert('success', 'Record Deleted Successfully.');
                        BindVesselGrid(res.tblPort);
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
