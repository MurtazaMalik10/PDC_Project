using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PDC_Backend.Hubs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDC_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotingController : ControllerBase
    {
        private static List<string> votes = new List<string>();
        private readonly IHubContext<VotingHub> _hubContext;

        // Inject the SignalR hub context
        public VotingController(IHubContext<VotingHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("vote")]
        public async Task<IActionResult> Vote([FromBody] string vote)
        {
            // Simulate parallel processing of multiple votes
            var task1 = ProcessVoteAsync(vote);
            var task2 = ProcessVoteAsync(vote);
            var task3 = ProcessVoteAsync(vote);

            await Task.WhenAll(task1, task2, task3); // Process all votes in parallel

            // Notify all clients about the vote update
            await _hubContext.Clients.All.SendAsync("ReceiveVoteUpdate", vote);

            return Ok("Vote processed!");
        }

        private async Task ProcessVoteAsync(string vote)
        {
            await Task.Delay(1000); // Simulate some processing time
            votes.Add(vote); // Simulate saving the vote
        }

        [HttpGet("results")]
        public IActionResult GetResults()
        {
            return Ok(votes);
        }
    }
}
