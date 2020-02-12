"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/graphs").build();

connection.on("UpdateGraph", function (sensorId, value, date, type) {
    UpdateWordcloud(sensorId, value);
    UpdateGauge(sensorId, value);
    UpdateTimeSeries(sensorId, value, date, type);
    UpdateGraph(sensorId, value, date, type);
});

connection.on("UpdateOnOff", function (sensorId, value) {
    UpdateOnOff(sensorId, value);
});

connection.start().catch(function (err) {
    return console.log(err.toString());
});