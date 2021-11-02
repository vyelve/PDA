var $Submitbtn = $('#btnSubmit');
var $Cancelbtn = $('#btnCancel');
window.existingModel = null;

if (jsonData !== null) {
    BindUserGrid(jsonData);
}

function BindUserGrid(jsonData) {
    for (var i = 0; i < jsonData.length; i++) {
        jsonData[i].isActive = jsonData[i].isActive === true ? "Active" : "In Active";
    }
    $('#tblUser').bootstrapTable('destroy');
    $('#tblUser').bootstrapTable({
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
        exportTypes: ['json', 'xml', 'csv', 'txt', 'sql', 'excel'],
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
                field: 'loginName',
                title: 'Login Name',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'firstName',
                title: 'First Name',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'lastName',
                title: 'Last Name',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'emailId',
                title: 'Email Id',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'gender',
                title: 'Gender',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'phoneNumber',
                title: 'Phone Number',
                align: 'left',
                valign: 'bottom',
                sortable: true,
                width: '30%'
            },
            {
                field: 'isActive',
                title: 'Active',
                align: 'center',
                valign: 'bottom',
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
        '<a href="javascript:void(0);" class="editUser fas fa-edit" data-title="Edit" title="Edit"></a>'
    ].join('');
}

function deleteRowFormatter(value, row, index) {
    return [
        '<a class="fas fa-trash" style="color: red;" href="javascript:void(0);" onclick="DeleteUser(\'' + row.userID + '\');" data-title="Delete" title="Delete"></a>'
    ].join('');
}

function UserModel() {
    this.user = {
        userID: $('#hdnUserID').val(),
        firstName: $('#txtFirstName').val(),
        lastName: $('#txtLastName').val(),
        fullName: $('#txtFullName').val(),
        emailId: $('#txtEmailId').val(),
        gender: $('#ddlGender option:selected').val(),
        phoneNumber: parseInt($('#txtPhoneNumber').val()),
        loginName: $('#txtLoginName').val(),
        password: $('#txtPassword').val(),
        confirmPassword: $('#txtConfirmPassword').val(),
        userPhotoPath: $('#hdnUserPhotoPath').val(),
        isActive: $('#chkIsActive').prop('checked'),
    },
        this.getUserModel = function () {
            return this.user;
        }
    this.setUserModel = function (Data) {
        $('#hdnUserID').val(Data.userID);
        $('#txtFirstName').val(Data.firstName);
        $('#txtLastName').val(Data.lastName);

        $('#txtFullName').prop('disabled', false);
        $('#txtFullName').val(Data.fullName);
        $('#txtFullName').prop('disabled', true);

        $('#txtEmailId').val(Data.emailId);
        $('#ddlGender').val(Data.gender.trim());
        $("#ddlGender option:contains(" + Data.gender.trim() + ")").attr('selected', true);

        $('#txtPhoneNumber').val(Data.phoneNumber);
        $('#txtLoginName').val(Data.loginName);
        $('#txtPassword').val(Data.password);
        $('#txtConfirmPassword').val(Data.confirmPassword);

        $('#txtLoginName').prop('disabled', true);
        $('#txtPassword').prop('disabled', true);
        $('#txtConfirmPassword').prop('disabled', true);

        $('#hdnUserPhotoPath').val(Data.userPhotoPath);

        Data.isActive === "Active" ? $('#chkIsActive').prop('checked', true).change() : $('#chkIsActive').prop('checked', false).change();
        $("#btnSubmit").val('Update');
    }
    this.resetUserModel = function () {
        $('#hdnUserID').val('0');
        $('#txtFirstName').val('');
        $('#txtLastName').val('');
        $('#txtFullName').val('');
        $('#txtEmailId').val('');

        $('#ddlGender option:eq(0)').attr('selected', 'selected');
        $('#ddlGender').prop('SelectedIndex', 0);
        $('#ddlGender').val('');

        $('#txtPhoneNumber').val('');
        $('#txtLoginName').val('');
        $('#txtPassword').val('');
        $('#txtConfirmPassword').val('');
        $('#txtUserPhoto').val('');
        $('#hdnUserPhotoPath').val('');

        $('#chkIsActive').prop('checked', true).change();
        $("#btnSubmit").val('Submit');

        $('#txtLoginName').prop('disabled', false);
        $('#txtPassword').prop('disabled', false);
        $('#txtConfirmPassword').prop('disabled', false);
    }
}

$(function () {

    $Submitbtn.bind('click', function (e) {
        e.preventDefault();
        var submit = true;
        if (!$("#userform").valid()) {
            submit = false;
            SetAlert('error', 'Validation failed.');
        }
        if (submit) {
            try {
                SaveUserDetails();
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
        var _userModel = new UserModel();
        _userModel.resetUserModel();
        Window.existingModel = null;
        ReloadPage();
    });
});

function ReloadPage() {
    var Url = '/User/Index';
    location.href = Url;
}

var SaveUserDetails = function () {
    var _userModel = new UserModel();
    var _model = _userModel.getUserModel();
    var _command = $('#btnSubmit').val();
    var jsonData = JSON.stringify(_model);

    $.ajax({
        url: _command === "Submit" ? '/User/CreateUser' : '/User/EditUser',
        dataType: "JSON",
        type: "POST",
        cache: false,
        async: false,
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        data: { userModel: jsonData },
        success: function (result) {
            if (_command === "Submit") {
                SetAlert('success', 'Record has been added.');
            }
            else {
                SetAlert('success', 'Record has been updated.');
            }
            _userModel.resetUserModel();
            BindUserGrid(result.tblUsers);
        },
        error: function (e) {
            SetAlert('error', 'Error Occured While Processing Data.');
        },
        complete: function () {
        }
    });
}

function DeleteUser(ID) {
    bootbox.confirm("Do you want to delete the Record?", function (result) {
        if (ID !== "") {
            $.ajax({
                url: '/User/DeleteUser',
                type: "PUT",
                dataType: "JSON",
                data: { "UserId": ID },
                success: function (res) {
                    SetAlert('success', 'Record Deleted Successfully.');
                    BindUserGrid(res.tblUsers);
                    setTimeout(function () { ReloadPage(); }, 4000);
                },
                error: function (e) {
                    SetAlert('error', 'Error Occured While Processing Data.');
                },
                complete: function () {
                }
            });
        }
    });
}

$("#txtUserPhoto").on('change', function () {
    var files = $('#txtUserPhoto').prop("files");
    var url = "/User/PhotoUpload?handler=MyUploader";
    formData = new FormData();
    formData.append("MyUploader", files[0]);

    $.ajax({
        type: 'POST',
        url: url,
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (repo) {
            if (repo.status == "success") {
                $('#hdnUserPhotoPath').val(repo.fileName);
                console.log(repo.fileName);
            }
        },
        error: function () {
            alert("Error occurs");
        }
    });
});

$('#txtLastName').keyup(function () {
    $('#txtFullName').val('');
    if ($('#txtFirstName').val() !== '' && $('#txtLastName').val() !== '') {
        var first = $('#txtFirstName').val();
        var second = $('#txtLastName').val();
        var combine = first + " " + second;
        $('#txtFullName').val(combine);
    }
});