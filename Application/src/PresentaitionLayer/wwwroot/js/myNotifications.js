"use strict";

function ShowPopUp(title, msg, level) {
    const myNotification = window.createNotification({

    });
    myNotification({
        title: title,
        message: "",
        theme: "success"
    })
}