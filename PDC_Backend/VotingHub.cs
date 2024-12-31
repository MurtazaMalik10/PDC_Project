using Microsoft.AspNetCore.SignalR;

namespace PDC_Backend.Hubs
{
    public class VotingHub : Hub
    {
        // Store votes on the server-side (you could use a database in a real application)
        private static List<string> votes = new List<string>();

        // This method will be called from the server to send the current list of votes to all connected clients
        public async Task SendVoteUpdate()
        {
            // Send the updated list of votes to all connected clients
            await Clients.All.SendAsync("ReceiveVoteUpdate", votes);
        }

        // This method will be called when a client submits a vote
        public async Task SubmitVote(string vote)
        {
            // Add the submitted vote to the list (you can add validation or other logic here)
            votes.Add(vote);

            // Broadcast the updated vote to all clients
            await Clients.All.SendAsync("ReceiveVoteUpdate", vote);
        }

        // This method can be used to retrieve the current list of votes (e.g., on initial connection)
        public async Task GetVotes()
        {
            // Send the list of votes to the caller (client that requested)
            await Clients.Caller.SendAsync("ReceiveVoteUpdate", votes);
        }
    }
}
