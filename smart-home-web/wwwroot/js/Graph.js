﻿"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/graphs").build();

connection.on("UpdateGraph", function (sensorId, value, date) {
    UpdateWordcloud(sensorId, value);
    UpdateTimeSeries(sensorId, value, date);
    UpdateGauge(sensorId, value);
    UpdateGraph(sensorId, value, date);
});

connection.start().catch(function (err) {
    return console.log(err.toString());
});