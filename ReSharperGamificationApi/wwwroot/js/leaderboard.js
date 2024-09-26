var script = $('script[src*=leaderboard');
var pageNumber = parseInt(script.attr('pageNumber'));
var pageSize = parseInt(script.attr('pageSize'));

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/leaderboardHub")
    .build();

connection.on("UpdateLeaderboard", async () => {
    try {
        const entries = await connection.invoke("GetUpdatedLeaderboard", pageNumber, pageSize);
        updateLeaderboard(entries);
    } catch (error) {
        console.error("Error retrieving leaderboard:", error);
    }
});

connection.start().catch(err => console.error(err.toString()));

function updateLeaderboard(entries) {
    const tbody = document.getElementById("leaderboardEntries");
    tbody.innerHTML = "";
    entries.forEach(entry => {
        const row = document.createElement("tr");
        row.innerHTML = `
                <td class="text-center">${entry.position}</td>
                <td>${entry.firstName}</td>
                <td>${entry.lastName}</td>
                <td class="text-right">${entry.points}</td>
            `;
        tbody.appendChild(row);
    });
}
