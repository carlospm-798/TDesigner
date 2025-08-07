/**
 * @author Carlos Paredes Márquez
 * @date Aug 05, 2025
 * Script to see the device connections during 
 * the development of the project
 * */
const baseURL = window.location.origin;

let hostIp = '';
fetch('/host-ip')
    .then(res => res.text())
    .then(ip => {
        hostIp = ip.trim();
    });

const connection = new signalR.HubConnectionBuilder()
    .withUrl(`${baseURL}/gamehub`)
    .build();
connection.on("UpdateClientList", function (clients) {
    const list = document.getElementById("clients");
    list.innerHTML = "";
    
    /*if (hostIp){
        const liHost = document.createElement("li");
        liHost.textContent = `Host (${hostIp})`;
        list.appendChild(liHost);
    }*/
    
    clients.forEach(id => {
        const li = document.createElement("li");
        li.textContent = id;
        list.appendChild(li);
    });
});

connection.on("youAreHost", () => {
    document.getElementById("host-message").style.display = "block";
});
connection.start().catch(e => console.error(e));

