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
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _db;
        public TasksController(AppDbContext db) { _db = db; }

        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost("api/projects/{projectId:int}/tasks")]
        public async Task<IActionResult> Create(int projectId, [FromBody] CreateTaskDto dto)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == UserId);
            if (project == null) return NotFound(new { message = "Project not found" });

            var task = new Models.TaskItem
            {
                Title = dto.Title,
                DueDate = dto.DueDate,
                ProjectId = projectId
            };
            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { taskId = task.Id }, task);
        }

        [HttpGet("api/tasks/{taskId:int}")]
        public async Task<IActionResult> Get(int taskId)
        {
            var t = await _db.Tasks
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x => x.Id == taskId && x.Project!.UserId == UserId);
            if (t == null) return NotFound();
            return Ok(t);
        }

        [HttpPut("api/tasks/{taskId:int}")]
        public async Task<IActionResult> Update(int taskId, [FromBody] UpdateTaskDto dto)
        {
            var t = await _db.Tasks.Include(x => x.Project).FirstOrDefaultAsync(x => x.Id == taskId && x.Project!.UserId == UserId);
            if (t == null) return NotFound();

            t.Title = dto.Title;
            t.DueDate = dto.DueDate;
            t.IsCompleted = dto.IsCompleted;
            await _db.SaveChangesAsync();
            return Ok(t);
        }

        [HttpDelete("api/tasks/{taskId:int}")]
        public async Task<IActionResult> Delete(int taskId)
        {
            var t = await _db.Tasks.Include(x => x.Project).FirstOrDefaultAsync(x => x.Id == taskId && x.Project!.UserId == UserId);
            if (t == null) return NotFound();
            _db.Tasks.Remove(t);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
