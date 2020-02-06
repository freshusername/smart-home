$(document).ready(function () {
    var hours = $('div#hours').data('save-hours');
    $('#hours-select').find('option[value=' + hours + ']').attr('selected', 'selected');
    $('select').niceSelect();
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
            if ($("input#type").val() == "Heatmap") {
                $('select#hours-select').attr('disabled', true);
                $('#hours-select').find('option[value=672]').attr('selected', 'selected');
                $('#hours-hidden').val(672);
            }
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
            if ($("input#type").val() == "3") {
                $('select#hours-select').attr('disabled', true);
                $('#hours-select').find('option[value=672]').attr('selected', 'selected');
                $('#hours-hidden').val(672);
            }
            if ($("input#type").val() == "OnOff") {
                $('select#hours-select').attr('disabled', true);
            }
            $('#sensor-select').val($('#sensor-id').val());
            $('select').niceSelect('update');

        },
    });
    $('#hours-hidden').val($("#hours-select").children("option:selected").val());
    $("#hours-select").change(function () {
        $('select').niceSelect('update');
        $('#hours-hidden').val($("#hours-select").children("option:selected").val());
    });
});