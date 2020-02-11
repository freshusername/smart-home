let listOfHours = $("select#hours-select option");

$(document).ready(function () {
    $('select').niceSelect();
    get();
    $("select#type-select").change(function () {
        get();
    });
    $("#hours-select").change(function () {
        $('select').niceSelect('update');
        $('#hours-hidden').val($("#hours-select").children("option:selected").val());
    });
});


function get() {
    $.ajax({
        type: "Get",
        url: $(".row").data("save-url"),
        data: {
            type: $("select#type-select").children("option:selected").val(),
            dashboardId: $(".row").data("save-dashboardid")
        },
        success: function (res) {
            $('#sensor-select').empty()
            $.each(res, function (key, value) {
                $('#sensor-select')
                    .append($("<option></option>")
                        .attr("value", value.id)
                        .text(value.name));
            });
            set_hours()
            set_disable();
            set_title();
        },
        error: function () {
            $('#sensor-select').empty()
            $('#sensor-select')
                .append($("<option></option>")
                    .attr("selected", true)
                    .attr("value", "0")
                    .text("There is not suitable sensor"));
            set_hours()
            set_disable();
            set_title();
        }
    });
}




function set_disable() {
    var selected = $("select#type-select").children("option:selected").val();
    if (selected == "0" || selected == "7") {
        $("#sensor-select").attr('disabled', true);
        $("#hours-select").attr('disabled', true);
    } else if (selected == "6") {
        $('select#sensor-select').attr('disabled', false);
        $("#hours-select").attr('disabled', true);
    } else {
        $("#sensor-select").attr('disabled', false);
        $("#hours-select").attr('disabled', false);
    }
    $('select').niceSelect('update');
}

function set_title() {
    var selected = $("select#type-select").children("option:selected").val();
    if ($("select#sensor-select").children("option:selected").val() != "0") {
        $("#create-report-element").prop('disabled', false);
        $("#create-report-element").attr('title', "Create");
    } else {
        $("#create-report-element").prop('disabled', true);
        $("#create-report-element").attr('title', "There is no suitable sensor.");
    }
    if ($("select#sensor-select").children("option:selected").val() == "0" && (selected == "0" || selected == "7")) {
        $("#create-report-element").prop('disabled', false);
        $("#create-report-element").attr('title', "Create");
    }
    $('select').niceSelect('update');
}

function set_hours() {
    var selected = $("select#type-select").children("option:selected").val();
    $("select#hours-select").empty();
    $.each(listOfHours, function (key, value) {
        $("select#hours-select").append($(this));
    });
    if (selected == "3") {
        $.each($("select#hours-select option"), function (key, value) {
            if (key >= 0 && key < 8) {
                $(this).remove();
            }
        });
    }
    else if (selected == "8") {
        $.each($("select#hours-select option"), function (key, value) {
            if (key < 6 || key > 9) {
                $(this).remove();
            }
        });
    }
}