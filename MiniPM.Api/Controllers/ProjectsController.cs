using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MiniPM.Api.Data;
using MiniPM.Api.DTOs;

namespace MiniPM.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProjectsController(AppDbContext db) { _db = db; }

        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _db.Projects
                .Where(p => p.UserId == UserId)
                .Select(p => new {
                    p.Id, p.Title, p.Description, p.CreatedAt,
                    TasksCount = p.Tasks.Count
                })
                .ToListAsync();
            return Ok(projects);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
        {
            var p = new Models.Project
            {
                Title = dto.Title,
                Description = dto.Description,
                UserId = UserId
            };
            _db.Projects.Add(p);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = p.Id }, p);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var p = await _db.Projects
                .Include(x => x.Tasks)
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == UserId);
            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.Projects.FirstOrDefaultAsync(x => x.Id == id && x.UserId == UserId);
            if (p == null) return NotFound();
            _db.Projects.Remove(p);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
