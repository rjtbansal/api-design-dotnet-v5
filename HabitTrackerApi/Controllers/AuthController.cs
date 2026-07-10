using Microsoft.AspNetCore.Mvc;

namespace HabitTrackerApi.Controllers;

// Node comparison: src/routes/authRoutes.ts
//   const router = Router()
//   router.post('/register', handler)
//   export default router   // then app.use('/api/auth', authRoutes) in server.ts
//
// [ApiController] enables automatic model validation + binding conventions.
// [Route("api/auth")] is the mount point — the attribute-based equivalent
// of `app.use('/api/auth', authRoutes)`, declared on the class instead of
// centrally in server.ts.
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    // Node comparison: router.post('/register', (req, res) => {
    //   res.status(201).json({ message: 'User registered' })
    // })
    [HttpPost("register")]
    public IActionResult Register()
    {
        // Stub only, same as the Express version — real logic (hash password
        // with BCrypt.Net, persist via EF Core, issue a JWT) comes later.
        return StatusCode(201, new { message = "User registered" });
    }

    // Node comparison: router.post('/login', (req, res) => { res.json(...) })
    [HttpPost("login")]
    public IActionResult Login()
    {
        return Ok(new { message = "User logged in" });
    }

    // Node comparison: router.post('/logout', (req, res) => { res.json(...) })
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok(new { message = "User logged out" });
    }

    // Node comparison: router.post('/refresh', (req, res) => { res.json(...) })
    [HttpPost("refresh")]
    public IActionResult Refresh()
    {
        return Ok(new { message = "Token refreshed" });
    }
}
