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

        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            var users = await dbClient.Select<User>(Table, cancellationToken);

            return Ok(users);
        }

        /// <summary>
        /// Get a single user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id, CancellationToken cancellationToken)
        {
            var user = await dbClient.Select<User>((Table, id), cancellationToken);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="newUser"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser(
            [FromBody] CreateUserDTO newUser,
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

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete an existing user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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