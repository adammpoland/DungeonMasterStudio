"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").withAutomaticReconnect().configureLogging(signalR.LogLevel.Information).build();
var imageData;
//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {

    var p = document.createElement("p");
    document.getElementById("messages").appendChild(p);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    console.log(user + " " + message)
    p.textContent = `(${user}) ${message}`;
    //getBase64(dataURLtoFile(fileData));
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

    //connection.invoke("JoinGroup",PartyName, user).catch(function (err) {
    //    return console.error(err.toString());
    //});

}).catch(function (err) {
    return console.error(err.toString());
});


document.getElementById("sendButton").addEventListener("click", function (event) {
    var chkAll = document.getElementById("All")
    var PartyName = document.getElementById("PartyName")
   
    if (chkAll.checked) {
        var message = document.getElementById("messageInput").value;
        var p = document.createElement("p");
        p.textContent = ` ${message}`;
        document.getElementById("messages").appendChild(p);
        console.log(message)
        console.log(PartyName)
        connection.invoke("SendChatMessageToAll", PartyName.value, message).catch(function (err) {
            return console.error(err.toString());
        });
    } else {
        var form = this.form;
        var cbs = Array.from(form.querySelectorAll('input[type="checkbox"]:checked'));
        var ids = cbs.map(function (cb) { return cb.id });
        console.log(ids)
        
        //var user = document.getElementById("Users").value;
        var message = document.getElementById("messageInput").value;
        var p = document.createElement("p");
        document.getElementById("messages").appendChild(p);
        p.textContent = ` ${message}`;

        //const file = document.querySelector('#fileInput').files[0];
        //getBase64(file);
        ids.forEach(function (id) {
            console.log(id.toString())
            console.log(id.value)
            console.log(id.id)
            connection.invoke("SendChatMessage", id.toString(), message).catch(function (err) {
                return console.error(err.toString());
            });
        })
        
    }
    
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
