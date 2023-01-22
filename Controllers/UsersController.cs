using Microsoft.AspNetCore.Mvc;

namespace RestSample.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    
    private readonly RestSampleContext _dbContext;

    public UsersController(
        ILogger<UsersController> logger,
        RestSampleContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpPost]
    public IActionResult Post([FromBody] User user)
    {
        var exists = _dbContext.Users
            .Any(u => u.Id == user.Id);
        if (exists)
        {
            return Conflict();
        }

        var entry = _dbContext.Add(user);
        _dbContext.SaveChanges();

        var newUser = entry.Entity;
        return CreatedAtAction(
            nameof(GetById),
            new { id = newUser.Id },
            newUser
        );
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_dbContext.Users.ToList());
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var exisitingUser = _dbContext.Users
            .FirstOrDefault(u => u.Id == id);
        if (exisitingUser == null)
        {
            return NotFound();
        }

        return Ok(exisitingUser);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, [FromBody] User user)
    {
        var existingUser = _dbContext.Users
            .FirstOrDefault(u => u.Id == id)!;
        if (existingUser == null)
        {
            return NotFound();
        }

        existingUser.Name = user.Name;
        _dbContext.Update(existingUser);
        _dbContext.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var existingUser = _dbContext.Users
            .FirstOrDefault(u => u.Id == id);
        if (existingUser == null)
        {
            return NotFound();
        }

        _dbContext.Users.Remove(existingUser);
        _dbContext.SaveChanges();

        return Ok();
    }
}
