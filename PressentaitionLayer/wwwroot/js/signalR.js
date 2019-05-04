"use strict";


var connection = new signalR.HubConnectionBuilder().withUrl("/notificationsHub").build();

function ShowPopUp(title, msg, level) {
    const myNotification = window.createNotification({

    });
    myNotification({
        title: title,
        message: msg,
        theme: level
    })
}

connection.on("ReceiveMessage", function (title, message, theme) {
    ShowPopUp(title, message, theme);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});