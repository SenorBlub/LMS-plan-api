using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Logic.IServices;
using Logic.Models;
using Pomelo.EntityFrameworkCore.MySql;

namespace LMS_plan_api.Controllers
{
	[Route("plan/[controller]")]
	[ApiController]
	public class PlanController : ControllerBase
	{
		private readonly IPlanService _planService;

		public PlanController(IPlanService planService)
		{
			_planService = planService;
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] Plan plan)
		{
			var Id = Guid.NewGuid();
			Plan TempPlan = plan;
			TempPlan.Id = Id;
			await _planService.CreateAsync(TempPlan);
			return CreatedAtAction(nameof(GetById), new { id = TempPlan.Id }, TempPlan);
		}

		[HttpPost("batch")]
		public async Task<IActionResult> CreateMany([FromBody] List<Plan> plans)
		{
			await _planService.CreateManyAsync(plans);
			return Ok();
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(Guid id)
		{
			var plan = await _planService.GetByIdAsync(id);
			if (plan == null)
				return NotFound();
			return Ok(plan);
		}

		[HttpGet("range")]
		public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] List<Guid> userPlanIds)
		{
			var plans = await _planService.GetByDateRange(startDate, endDate, userPlanIds);
			return Ok(plans);
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAll([FromQuery] List<Guid> userPlanIds)
		{
			var plans = await _planService.GetAllAsync(userPlanIds);
			return Ok(plans);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(Guid id, [FromBody] Plan plan)
		{
			if (id != plan.Id)
				return BadRequest();

			await _planService.UpdateAsync(plan);
			return NoContent();
		}

		[HttpPut("batch")]
		public async Task<IActionResult> UpdateMany([FromBody] List<Plan> plans)
		{
			await _planService.UpdateAsync(plans);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			await _planService.DeleteAsync(id);
			return NoContent();
		}

		[HttpDelete("by-date")]
		public async Task<IActionResult> DeleteByDate([FromQuery] DateTime date)
		{
			await _planService.DeleteByDateAsync(date);
			return NoContent();
		}

		[HttpDelete("date-range")]
		public async Task<IActionResult> DeleteByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] List<Guid> userPlanIds)
		{
			await _planService.DeleteByDateRangeAsync(startDate, endDate, userPlanIds);
			return NoContent();
		}
	}
}
