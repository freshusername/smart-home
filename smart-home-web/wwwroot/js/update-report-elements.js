﻿function UpdateWordcloud(sensorId, sensorValue) {
    $.each($("div[id^='Wordcloud']"), function (key, value) {
        if (sensorId == $(value).data("save-sensorid")) {
            try {
                var result = Highcharts.charts.find(chart => chart.renderTo.id == $(value).attr("id"));
            }
            catch (error) {

            }
            if (typeof result !== 'undefined') {
                var series = result.series[0];
                var element = series.data.find(ar => ar.name == JSON.parse(sensorValue));
                if (typeof element !== 'undefined') {
                    element.update({ name: element.name, weight: ++element.weight });
                }
                else {
                    series.addPoint({ name: JSON.parse(sensorValue), weight: 1 });
                }
            }
        }
    });
}