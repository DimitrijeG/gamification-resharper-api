var script = $("script[src*=league");
var leagueId = parseInt(script.attr("leagueId"));

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/leagueHub")
    .build();

connection.on("UpdateLeague", async () => {
    try {
        const entries = await connection.invoke("GetUpdatedLeague", leagueId);
        console.log(entries);
        updateLeaderboard(entries);
    } catch (error) {
        console.error("Error retrieving league:", error);
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
