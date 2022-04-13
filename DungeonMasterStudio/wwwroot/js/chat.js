"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var imageData;
//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message, fileData) {
    var p = document.createElement("p");
    document.getElementById("messages").appendChild(p);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    p.textContent = `(${user}) ${message}`;
    getBase64(dataURLtoFile(fileData));
});

function dataURLtoFile(url) {

    fetch(url)
        .then(res => res.blob())
        .then(blob => {
           return new File([blob], "File name", { type: "image/png" })
        })
}

connection.on("showErrorMessage", function(message) {
    var p = document.createElement("p");
    p.textContent = ` ${message}`;
    document.getElementById("messages").appendChild(p);
})

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;

}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("Users").value;
    var message = document.getElementById("messageInput").value;
    var p = document.createElement("p");
    p.textContent = ` ${message}`;
    document.getElementById("messages").appendChild(p);

    //const file = document.querySelector('#fileInput').files[0];
    //getBase64(file);

    connection.invoke("SendChatMessage", user, message, imageData).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});


//function getBase64(file) {
//    return new Promise((resolve, reject) => {
//        var reader = new FileReader();
//        reader.readAsDataURL(file)
//        reader.onload = function () {
//            resolve(reader.resolve);
//            console.log(reader.result);
//            var img = document.createElement("img");

//            img.setAttribute("src", reader.result)
//            console.log(img)
//            document.getElementById("messages").appendChild(img);
//            imageData = reader.result;

//        };
//        reader.onerror = function (error) {
//            console.log('Error: ', error);
//        };
//    });
//}
