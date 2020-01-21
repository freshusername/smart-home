"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/messages").build();

connection.on("ShowToastMessage", function (type, user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " returned value " + msg;
    //toastr.info(encodedMsg);
    toastr[type](encodedMsg);
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});