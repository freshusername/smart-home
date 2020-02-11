$(document).ready(function () {
    var hours = $('div#hours').data('save-hours');
    $('#hours-select').find('option[value=' + hours + ']').attr('selected', 'selected');
    $('select').niceSelect();
    set_hours();
    $.ajax({
        type: "Get",
        url: $(".row").data("save-url"),
        data: {
            type: $("input#type").val(),
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
            if ($("input#type").val() == "OnOff") {
                $('select#hours-select').attr('disabled', true);
            }
            $('#sensor-select').val($('#sensor-id').val());
            $('select').niceSelect('update');
        },
        error: function () {
            $('#sensor-select')
                .append($("<option></option>")
                    .attr("selected", true)
                    .attr("value", "0")
                    .text("There is not suitable sensor"));
            if ($("input#type").val() == "Clock") {
                $('select#sensor-select').attr('disabled', true);
            }
            if ($("input#type").val() == "OnOff" || $("input#type").val() == "StatusReport" || $("input#type").val() == "Clock") {
                $('select#hours-select').attr('disabled', true);
            }
            $('#sensor-select').val($('#sensor-id').val());
            $('select').niceSelect('update');
        },
    });
});

function set_hours() {
    var selected = $("input#type").val();
    if (selected == "Heatmap") {
        $.each($("select#hours-select option"), function (key, value) {
            if (key >= 0 && key < 8) {
                $(this).remove();
            }
        });
    }
    else if (selected == "BoolHeatmap") {
        $.each($("select#hours-select option"), function (key, value) {
            if (key < 6 || key > 9) {
                $(this).remove();
            }
        });
    }
    $('select').niceSelect('update');
}