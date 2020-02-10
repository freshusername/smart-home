"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/messages").build();

connection.on("ShowToastMessage", function (type, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    toastr[type](msg);
});

connection.on("NotifyAboutInvalidSensor", function (message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});