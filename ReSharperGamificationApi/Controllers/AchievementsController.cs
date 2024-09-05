using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReSharperGamificationApi.DTO;
using ReSharperGamificationApi.Models;
using ReSharperGamificationApi.Services;

namespace ReSharperGamificationApi.Controllers
{
    [Route("api/v1/achievements")]
    [ApiController]
    public class AchievementsController(IMapper mapper, AchievementContext context, IAchievementService service) : ControllerBase
    {
        // GET: api/v1/achievements
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AchievementDTO>>> GetAchievements()
        {
            return await context.Achievements
                .Select(achievement => mapper.Map<AchievementDTO>(achievement))
                .ToListAsync();
        }

        // GET: api/v1/achievements/5
        [HttpGet("{id:long}")]
        public async Task<ActionResult<AchievementDTO>> GetAchievement(long id)
        {
            var achievement = await context.Achievements.FindAsync(id);

            if (achievement == null)
            {
                return NotFound();
            }

            return mapper.Map<AchievementDTO>(achievement);
        }

        // PUT: api/v1/achievements/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id:long}")]
        public async Task<IActionResult> PutAchievement(long id, AchievementDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var achievement = mapper.Map<Achievement>(dto);
            context.Entry(achievement).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
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

        // POST: api/v1/achievements
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<AchievementDTO>>> PostAchievement(AchievementWithGradesDTO dto)
        {
            var saved = await service.SaveAll(dto.UserId, dto.Group, dto.Grades);
            return CreatedAtAction(nameof(GetAchievements), saved.Select(mapper.Map<AchievementDTO>));
        }

        // DELETE: api/v1/achievements/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAchievement(int id)
        {
            var achievement = await context.Achievements.FindAsync(id);
            if (achievement == null)
            {
                return NotFound();
            }

            context.Achievements.Remove(achievement);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool AchievementExists(long id)
        {
            return context.Achievements.Any(e => e.Id == id);
        }
    }
}
