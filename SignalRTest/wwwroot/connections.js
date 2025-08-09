/**
 * @author Carlos Paredes Márquez
 * @date Aug 05, 2025
 * Script to see the device connections during 
 * the development of the project
 * */
const baseURL = window.location.origin;

const connection = new signalR.HubConnectionBuilder()
    .withUrl(`${baseURL}/gamehub`)
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

//  Create a bottom visible just for the HOST
connection.on("IdentifyHost", (isHost) => {
    if (isHost) {
        const createLobbyBtn = document.createElement("button");
        createLobbyBtn.textContent = "Create Lobby";
        createLobbyBtn.onclick = () => {
            connection.invoke("createLobby")
                .catch(error => console.error(error));
        };
        document.body.appendChild(createLobbyBtn);
    }
});

connection.start().catch(e => console.error(e));