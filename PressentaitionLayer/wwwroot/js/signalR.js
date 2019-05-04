"use strict";


var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

function ShowPopUp(title, msg, level) {
    const myNotification = window.createNotification({

    });
    myNotification({
        title: title,
        message: msg,
        theme: level
    })
}

connection.on("RecieveNotification", function (title, message, theme) {
    ShowPopUp(title, message, theme);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});