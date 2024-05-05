using Microsoft.AspNetCore.Mvc;
using SurrealCrud.Models;
using SurrealDb.Net;

namespace SurrealCrud.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private const string Table = "User";

        private readonly ISurrealDbClient dbClient;
        private readonly ILogger<UserController> _logger;

        public UserController(
            ISurrealDbClient dbClient,
            ILogger<UserController> logger)
        {
            this.dbClient = dbClient;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            var users = await dbClient.Select<User>(Table, cancellationToken);

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id, CancellationToken cancellationToken)
        {
            var user = await dbClient.Select<User>((Table, id), cancellationToken);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(
            [FromBody]CreateUserDTO newUser, 
            CancellationToken cancellationToken)
        {
            var user = new User
            {
                Firstname = newUser.Firstname,
                Lastname = newUser.Lastname,
                Gender = newUser.Gender,
                Age = newUser.Age,
                IsConfirmed = false
            };

            var createdUser = await dbClient.Create(Table, user, cancellationToken);

            return Ok(createdUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(
            [FromBody] UpdateUserDTO user,
            string id,
            CancellationToken cancellationToken)
        {
            var existingUser = await dbClient.Select<User>((Table, id), cancellationToken);

            if (existingUser == null)
                return NotFound();

            existingUser.Firstname = user.Firstname;
            existingUser.Lastname = user.Lastname;
            existingUser.Gender = user.Gender;

            var updatedUser = await dbClient.Upsert(existingUser, cancellationToken);

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id, CancellationToken cancellationToken)
        {
            var existingUser = await dbClient.Select<User>((Table, id), cancellationToken);

            if (existingUser == null)
                return NotFound();

            bool isDeleted = await dbClient.Delete((Table, id), cancellationToken);

            return Ok(isDeleted);
        }
    }
}