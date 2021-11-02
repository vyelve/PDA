function SetAlert(_icon, message) {
    var alertPopUp = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000
    });

    alertPopUp.fire({ icon: _icon, title: message });
}


function SetAlertDiv(AlertDivID, isError, status, message) {
    var alertControl = $("#" + AlertDivID);
    if (isError) {
        alertControl.addClass('alert-danger').removeClass('');
    }
    else {
        alertControl.addClass('alert-success').removeClass('');
    }
    // uncomment when this function is in use
    //alertControl.find('span[id="lblStatus"]')[0].innerHTML = status;
    //alertControl.find('span[id="lblMessage"]')[0].innerHTML = message;
    //alertControl.fadeIn(1000).delay(5000).fadeOut(1000);
}


//$(function () {
//    $('.nav-sidebar > li').click(function (e) {
//        e.stopPropagation();
//        var $el = $('ul', this);
//        $('.nav > li > ul').not($el).slideUp();
//        $el.stop(true, true).slideToggle(400);
//    });
//    $('.nav-sidebar > li > ul > li').click(function (e) {
//        e.stopImmediatePropagation();
//    });
//});


