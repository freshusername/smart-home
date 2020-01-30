function UpdateGraph(sensorId, sensorValue, date) {
    $.each($("div#Graph"), function (key, value) {
        if (sensorId == $(value).data("save-sensorid")) {
            try {
                var result = Highcharts.charts.find(chart => chart.renderTo.id == $(value).attr("id"));
            }
            catch (error) {

            }
            if (typeof result !== 'undefined') {
                var xAxis = result.xAxis[0];
                var categories = xAxis.categories
                var series = result.series[0];
                categories.push(date);
                xAxis.setCategories(categories);
                series.addPoint({ y: JSON.parse(sensorValue) }, true);
            }
        }
    });
}

function UpdateWordcloud(sensorId, sensorValue) {
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


function UpdateTimeSeries(sensorId, sensorValue, date) {
    $.each($("div[id^='TimeSeries']"), function (key, value) {
        if (sensorId == $(value).data("save-sensorid")) {
            try {
                var result = Highcharts.charts.find(chart => chart.renderTo.id == $(value).attr("id"));
            }
            catch (error) {

            }
            if (typeof result !== 'undefined') {
                var xAxis = result.xAxis[0];
                var categories = xAxis.categories
                var series = result.series[0];
                categories.push(date);
                xAxis.setCategories(categories);
                series.addPoint({ y: JSON.parse(sensorValue)},true);
            }
        }
    });
}

function UpdateGauge(sensorId, sensorValue) {
    $.each($("div[id^='Gauge']"), function (key, value) {
        if (sensorId == $(value).data("save-sensorid")) {
            try {
                var result = Highcharts.charts.find(chart => chart.renderTo.id == $(value).attr("id"));
            }
            catch (error) {

            }
            if (typeof result !== 'undefined') {
                var series = result.series[0];
                series.points[0].update(JSON.parse(sensorValue));
                if (JSON.parse(sensorValue) < result.yAxis[0].min) {
                    result.yAxis[0].update({
                        min: JSON.parse(sensorValue)
                    });
                }
                else if (JSON.parse(sensorValue) > result.yAxis[0].max) {
                    result.yAxis[0].update({
                        max: JSON.parse(sensorValue) 
                    });
                }
                result.yAxis[0].tickPositions = [result.yAxis[0].min, result.yAxis[0].max];
                result.yAxis[0].update({
                    tickPositions: result.yAxis[0].tickPositions
                });
            }
        }
    });
}
