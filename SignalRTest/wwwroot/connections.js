/**
 * @author Carlos Paredes Márquez
 * @date Aug 05, 2025
 * Script to see the device connections during 
 * the development of the project
 * */

const connection = new signalR.HubConnectionBuilder()
    .withUrl("\gamehub")
    .build();

connection.on("UpdateClientList", function (clients) {
    const list = document.getElementById("clients");
    list.innerHTML = "";
    clients.forEach(id => {
        const li = document.createElement("li");
        li.textContent = id;
        list.appendChild(li);
    });
});

connection.start().catch(e => console.error(e));