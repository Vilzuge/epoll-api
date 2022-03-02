using ePollApi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ePollApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PollsController : ControllerBase
    {
        // GET: All polls without options
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Poll>>> GetAll()
        {
            var dbPolls = SqliteDataAccess.LoadPolls();
            return Ok(dbPolls);
        }

        // GET: Individual poll
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PollWithOptions>> GetOne(int id)
        {
            var dbPoll = SqliteDataAccess.LoadOne(id);
            return Ok(dbPoll);
        }

        // PUT: Vote a poll
        [HttpPut("{id}/vote/{option}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PollWithOptions>> VotePoll(int id, int option)
        {
            var pwo = SqliteDataAccess.VotePoll(id, option);
            return pwo;
        }

        // POST: Add poll
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public void AddPoll([FromBody] PollPost pollToPost)
        {
            SqliteDataAccess.SavePoll(pollToPost);
        }
    }
}