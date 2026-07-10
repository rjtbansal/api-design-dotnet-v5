using Microsoft.AspNetCore.Mvc;

namespace HabitTrackerApi.Controllers;

// Node comparison: src/routes/habitRoutes.ts, mounted at /api/habits
[ApiController]
[Route("api/habits")]
public class HabitsController : ControllerBase
{
    // Node comparison: router.get('/', (req, res) => { res.status(200).json(...) })
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(new { message = "Get all habits" });
    }

    // Node comparison: router.post('/', (req, res) => { res.status(201).json(...) })
    [HttpPost]
    public IActionResult Create()
    {
        return StatusCode(201, new { message = "Habit created" });
    }

    // Node comparison: router.post('/:id/complete', (req, res) => {
    //   res.json({ message: `Mark habit ${req.params.id} complete` })
    // })
    // Express's `:id` param becomes an ASP.NET Core route template
    // placeholder `{id}`, bound automatically to the method parameter of
    // the same name — no manual `req.params` lookup needed.
    [HttpPost("{id}/complete")]
    public IActionResult Complete(string id)
    {
        return Ok(new { message = $"Mark habit {id} complete" });
    }

    // Node comparison: router.get('/:id/stats', (req, res) => { res.json(...) })
    [HttpGet("{id}/stats")]
    public IActionResult GetStats(string id)
    {
        return Ok(new { message = $"Get stats for habit {id}" });
    }
}
