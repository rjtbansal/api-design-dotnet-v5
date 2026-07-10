using Microsoft.AspNetCore.Mvc;

namespace HabitTrackerApi.Controllers;

// Node comparison: src/routes/userRoutes.ts, mounted at /api/users
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    // Node comparison: router.get('/', (req, res) => { res.status(200).json(...) })
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(new { message = "Get all users" });
    }

    // Node comparison: router.get('/:id', (req, res) => {
    //   const { id } = req.params
    //   res.status(200).json({ message: `Get user with id ${id}` })
    // })
    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        return Ok(new { message = $"Get user with id {id}" });
    }

    // Node comparison: router.post('/', (req, res) => { res.status(201).json(...) })
    [HttpPost]
    public IActionResult Create()
    {
        return StatusCode(201, new { message = "Create a new user" });
    }

    // Node comparison: router.put('/:id', (req, res) => { ... })
    [HttpPut("{id}")]
    public IActionResult Update(string id)
    {
        return Ok(new { message = $"Update user with id {id}" });
    }

    // Node comparison: router.delete('/:id', (req, res) => { ... })
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        return Ok(new { message = $"Delete user with id {id}" });
    }
}
