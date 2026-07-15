using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using HabitTrackerApi.Models;

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

    // Node comparison: router.post('/', validateBody(createHabitSchema), (req, res) => {
    //   res.status(201).json(...)
    // })
    // [FromBody] CreateHabitRequest binds + validates in one step — the
    // [Required] on CreateHabitRequest.Name is checked automatically
    // before this method runs, same effect as validateBody(createHabitSchema)
    // returning 400 before the Express handler executes.
    [HttpPost]
    public IActionResult Create([FromBody] CreateHabitRequest request)
    {
        return StatusCode(201, new { message = "Habit created" });
    }

    // Node comparison: router.post('/:id/complete',
    //   validateParams(completeParamsSchema), validateBody(createHabitSchema),
    //   (req, res) => { res.json({ message: `Mark habit ${req.params.id} complete` }) }
    // )
    // Two validators chained on one route, same idea on both sides:
    //   - validateParams(completeParamsSchema) ≈ [Required] on the `id`
    //     parameter below (in practice both are close to a no-op here,
    //     since `{id}` is a required route segment either framework would
    //     already refuse to match without — kept to mirror the pattern)
    //   - validateBody(createHabitSchema), reusing the same schema as
    //     Create above ≈ reusing CreateHabitRequest as the body type here
    // ASP.NET Core checks both through the same ModelState mechanism, so
    // there's no need for two separate chained middleware functions.
    [HttpPost("{id}/complete")]
    public IActionResult Complete([Required] string id, [FromBody] CreateHabitRequest request)
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
