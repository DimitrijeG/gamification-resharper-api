using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReSharperGamificationApi.DTO;
using ReSharperGamificationApi.Models;
using ReSharperGamificationApi.Services;

namespace ReSharperGamificationApi.Controllers
{
    [Route("api/v1/achievements")]
    [ApiController]
    public class AchievementsController(AchievementContext context, IAchievementService service) : ControllerBase
    {
        private readonly AchievementContext _context = context;

        // GET: api/Achievement
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AchievementDTO>>> GetAchievements()
        {
            return await _context.Achievements
                .Select(achievement => MapToDTO(achievement))
                .ToListAsync();
        }

        // GET: api/Achievement/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AchievementDTO>> GetAchievement(long id)
        {
            var achievement = await _context.Achievements.FindAsync(id);

            if (achievement == null)
            {
                return NotFound();
            }

            return MapToDTO(achievement);
        }

        // PUT: api/Achievement/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAchievement(long id, AchievementDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var achievement = MapToModel(dto);

            _context.Entry(achievement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AchievementExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Achievement
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IEnumerable<AchievementDTO>>> PostAchievement(AchievementWithGradesDTO dto)
        {
            var saved = await service.SaveAll(dto.Group, dto.Grades, "meow");
            return CreatedAtAction(nameof(GetAchievements), saved.Select(MapToDTO));
        }

        // DELETE: api/Achievement/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAchievement(int id)
        {
            var achievement = await _context.Achievements.FindAsync(id);
            if (achievement == null)
            {
                return NotFound();
            }

            _context.Achievements.Remove(achievement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AchievementExists(long id)
        {
            return _context.Achievements.Any(e => e.Id == id);
        }

        private static AchievementDTO MapToDTO(Achievement achievement) => new()
        {
            Id = achievement.Id,
            User = achievement.User,
            Group = achievement.Group,
            Grade = achievement.Grade,
        };

        private static Achievement MapToModel(AchievementDTO dto) => new()
        {
            Id = dto.Id,
            User = dto.User,
            Group = dto.Group,
            Grade = dto.Grade,
        };
    }
}
