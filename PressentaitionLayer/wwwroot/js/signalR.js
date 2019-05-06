"use strict";


var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

function ShowPopUp(title, msg, level) {
    const myNotification = window.createNotification({

    });
    myNotification({
        title: title,
        message: "",
        theme: "success"
    })
}

connection.on("RecieveNotification", function (title) {
    ShowPopUp(title);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});