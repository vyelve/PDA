var $Submitbtn = $('#btnSubmit');
var $Cancelbtn = $('#btnCancel');
var $Addbtn = $('#btnAdd');
var $Resetbtn = $('#btnReset');

window.existingModel = null;

if (jsonData !== null) {
    BindPilotageGrid(jsonData);
}

function BindPilotageGrid(jsonData) {

    for (var i = 0; i < jsonData.length; i++) {
        jsonData[i].isActive = jsonData[i].isActive === true ? "Active" : "In Active";
    }

    $('#tblPilotage').bootstrapTable('destroy');
    $('#tblPilotage').bootstrapTable({
        data: jsonData,
        height: 500,
        pagination: true,
        pageSize: 10,
        pageList: [10, 20, 30, 50, 100],
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
                field: 'portName',
                title: 'Port Name',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'pilotType',
                title: 'Type',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'stage',
                title: 'Stage',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'fixed_Tariff',
                title: 'Fixed Tariff',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'tariff',
                title: 'Tariff',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'range1',
                title: 'Range From',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'range2',
                title: 'Range Two',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            }]
    });
}

function getHeight() {
    return $(window).height();
}

function editRowFormatter(value, row, index) {
    return [
        '<a href="javascript:void(0);" class="editPilotage fas fa-edit" data-title="Edit" title="Edit"></a>'
    ].join('');
}

function deleteRowFormatter(value, row, index) {
    return [
        '<a class="fas fa-trash" style="color: red;" href="javascript:void(0);" onclick="DeletePilotage(\'' + row.pilotageID + '\');" data-title="Delete" title="Delete"></a>'
    ].join('');
}

function PilotageModel() {
    this.pilotage = {
        pilotageID: $('#hdnPilotageID').val(),
        stage: $('#hdnStage').val(),
        portID: $('#ddlPort option:selected').val(),
        portName: $('#ddlPort option:selected').text(),
        pilotType: $('#ddlPilotType option:selected').val(),
        fixed_Tariff: $('#txtFixedTariff').val(),
        tariff: $('#txtTariff').val(),
        range1: $('#txtRange1').val(),
        range2: $('#txtRange2').val()
    },
        this.getPilotageModel = function () {
            return this.pilotage;
        }
    this.setPilotageModel = function (Data) {
        $('#hdnPilotageID').val(Data.pilotageID);
        $('#hdnStage').val(Data.stage);
        $('#ddlPort').val(Data.portID);
        $('#ddlPilotType').val(Data.pilotType.trim());
        $("#ddlPilotType option:contains(" + Data.pilotType.trim() + ")").attr('selected', true);
        $('#txtFixedTariff').val(Data.fixed_Tariff);
        $('#txtTariff').val(Data.tariff);
        $('#txtRange1').val(Data.range1);
        $('#txtRange2').val(Data.range2);
        //$("#btnSubmit").val('Update');
        $("#btnAdd").html('Update');


    }
    this.resetPilotageModel = function () {
        $('#hdnPilotageID').val('0');
        $('#hdnStage').val('0');
        $('#ddlPort option:eq(0)').attr('selected', 'selected');
        $('#ddlPort').prop('SelectedIndex', 0);

        $('#ddlPilotType option:eq(0)').attr('selected', 'selected');
        $('#ddlPilotType').prop('SelectedIndex', 0);
        $('#ddlPilotType').val('');
        $('#txtFixedTariff').val('');
        $('#txtTariff').val('');
        $('#txtRange1').val('');
        $('#txtRange2').val('');

        $("#btnSubmit").val('Submit');
        $("#btnAdd").html('Add');

    }
    this.resetTempSavePilotageModel = function () {
        $('#hdnPilotageID').val('0');
        $('#hdnStage').val('0');
        $('#txtFixedTariff').val('');
        $('#txtTariff').val('');
        $('#txtRange1').val('');
        $('#txtRange2').val('');

        $("#btnAdd").html('Add');
    }
}


$(function () {

    var _rowsCount = $('#tblPilotage').bootstrapTable('getData').length;
    if (_rowsCount <= 10) {
        $("#btnSubmit").prop('disabled', true);
        $("#btnCancel").prop('disabled', true);
    }
    else {
        $("#btnSubmit").prop('disabled', false);
        $("#btnCancel").prop('disabled', false);
    }

    $Submitbtn.bind('click', function (e) {
        e.preventDefault();
        var submit = true;
        if (!$("#pilotageform").valid()) {
            submit = false;
            SetAlert('error', 'Validation failed.');
        }
        if (submit) {
            try {
                //SavePilotageDetails();
                window.existingModel = null;
                setTimeout(function () { ReloadPage(); }, 4000);
            }
            catch (err) {
                if (arguments !== null && arguments.callee !== null && arguments.callee.trace)
                    logError(err, arguments.callee.trace());
                SetAlert('error', 'Error Occured While Processing Data.');
            }
        }
    });

    $Cancelbtn.bind('click', function (e) {
        e.preventDefault();
        var _pilotageModel = new PilotageModel();
        _pilotageModel.resetPilotageModel();
        Window.existingModel = null;
        ReloadPage();
    });

    $Addbtn.bind('click', function (e) {
        e.preventDefault();
        saveTempDataToGrid();
        $('#ddlPort').prop('disabled', true);
        $('#ddlPilotType').prop('disabled', true);

    });

    $Resetbtn.bind('click', function (e) {
        e.preventDefault();
        var _pilotageModel = new PilotageModel();
        _pilotageModel.resetTempSavePilotageModel();
        Window.existingModel = null;
    });
});

function ReloadPage() {
    var Url = '/Pilotage/Index';
    location.href = Url;
}

$('#ddlPilotType').change(function () {
    $("#h3_card-title").empty();
    if (this.value != '') {
        var selectedText = 'Tariff ' + $(this).find("option:selected").text();
        $("#h3_card-title").html(selectedText);

        var _rowsCount = $('#tblPilotage').bootstrapTable('getData').length;
        if (_rowsCount == 0) {
            $('#txtFixedTariff').val('0');
            $('#txtRange1').val('0');
        }
    }
});


function saveTempDataToGrid() {
    
    var _rowsCount = $('#tblPilotage').bootstrapTable('getData').length;
    if (_rowsCount <= 10) {

        $('#hdnStage').val(_rowsCount + 1);
        $('#hdnPilotageID').val(_rowsCount + 1);

        var _pilotageModel = new PilotageModel();
        var _model = _pilotageModel.getPilotageModel();

        if ($('#btnAdd').html().toLowerCase() == 'add') {
            $('#tblPilotage').bootstrapTable('insertRow', { index: 0, row: _model });
            SetAlert('success', 'Record has been added.');
        }
        else {
            $('#tblPilotage').bootstrapTable('updateRow', { index: 0, row: _model });
            SetAlert('success', 'Record has been updated.');
        }
        _pilotageModel.resetTempSavePilotageModel();

    }
    else {
        SetAlert('error', 'Cannot Add more than 10 records.');
    }

}