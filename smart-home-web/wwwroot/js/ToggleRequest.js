function set_active(clicked_id) {
    var url = $('#btn_' + clicked_id).attr('data-request-url');
    debugger;
    var isActiveAttribute = $('#btn_' + clicked_id).attr('aria-pressed');
    var isActive = (isActiveAttribute === 'true');
    $.ajax({
        url: url,
        data: { 'id': clicked_id, 'isActive': !isActive},
        type: "post",
        cache: false,
        success: function (savingStatus) {
        },
        error: function (xhr, ajaxOptions, thrownError) {
            window.alert("Error encountered while sending request");
            console.log(thrownError);
        }
    });
}


function remove(clicked_id) {
    var url = $('.link').attr('data-request-url');
    $("#hide_" + clicked_id).hide(function () {
        
    });
   
    $.ajax({
        url: url,
        data: { 'id': clicked_id},
        type: "post",
        cache: false,

        success: function (savingStatus) {
           
        },
        error: function (xhr, ajaxOptions, thrownError) {
            window.alert("Error encountered while sending request");
            console.log(thrownError);
        }
    });
}



    

   
