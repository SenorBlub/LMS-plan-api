using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.IRepositories;
using Logic.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
	public class PlanActivityRepository : IPlanActivityRepository
	{
		private readonly PlanDbContext.PlanDbContext _context;

		public PlanActivityRepository(PlanDbContext.PlanDbContext context)
		{
			_context = context;
		}

		public async Task CreateAsync(PlanActivity planActivity)
		{
			await _context.PlanActivities.AddAsync(planActivity);
			await _context.SaveChangesAsync();
		}

		public async Task CreateAsync(List<PlanActivity> planActivities)
		{
			await _context.PlanActivities.AddRangeAsync(planActivities);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(PlanActivity planActivity)
		{
			_context.PlanActivities.Update(planActivity);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(List<PlanActivity> planActivities)
		{
			_context.PlanActivities.UpdateRange(planActivities);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(PlanActivity planActivity)
		{
			_context.PlanActivities.Remove(planActivity);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(Guid planId)
		{
			var activities = await _context.PlanActivities.Where(pa => pa.PlanId == planId).ToListAsync();
			_context.PlanActivities.RemoveRange(activities);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(List<Guid> planIds)
		{
			var activities = await _context.PlanActivities.Where(pa => planIds.Contains(pa.PlanId)).ToListAsync();
			_context.PlanActivities.RemoveRange(activities);
			await _context.SaveChangesAsync();
		}

		public async Task<List<PlanActivity>> GetByPlanIdAsync(Guid planId)
		{
			return await _context.PlanActivities.Where(pa => pa.PlanId == planId).ToListAsync();
		}

		public async Task<List<PlanActivity>> GetByActivityIdAsync(Guid activityId)
		{
			return await _context.PlanActivities.Where(pa => pa.ActivityId == activityId).ToListAsync();
		}

		public async Task<PlanActivity> GetByIdAsync(Guid planActivityId)
		{
			return await _context.PlanActivities.FindAsync(planActivityId);
		}
	}
}
