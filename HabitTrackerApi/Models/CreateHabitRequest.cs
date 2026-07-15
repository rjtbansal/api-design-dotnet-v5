using System.ComponentModel.DataAnnotations;

namespace HabitTrackerApi.Models;

// Node comparison: src/routes/habitRoutes.ts
//   const createHabitSchema = z.object({ name: z.string() });
// Zod schemas are validated by hand, in middleware, at request time.
// Data Annotations attach the same rules directly to the shape of the
// data instead — [ApiController] on the controller checks them
// automatically via ModelState before the action body ever runs, so
// there's no separate validateBody(schema) middleware to write.
public class CreateHabitRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
