using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.IRepositories;
using Logic.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;

namespace DAL.Repositories
{
	public class PlanRepository : IPlanRepository
	{
		private readonly PlanDbContext.PlanDbContext _context;

		public PlanRepository(PlanDbContext.PlanDbContext context)
		{
			_context = context;
		}

		public async Task CreateAsync(Plan plan)
		{
			if (plan == null)
				throw new ArgumentNullException(nameof(plan));

			await _context.Plans.AddAsync(plan);
			await _context.SaveChangesAsync();
		}

		public async Task CreateAsync(List<Plan> plans)
		{
			if (plans == null)
				throw new ArgumentNullException(nameof(plans));

			await _context.Plans.AddRangeAsync(plans);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(Plan plan)
		{
			_context.Plans.Update(plan);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(List<Plan> plans)
		{
			_context.Plans.UpdateRange(plans);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(Plan plan)
		{
			_context.Plans.Remove(plan);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(Guid planId)
		{
			var plan = await _context.Plans.FindAsync(planId);
			if (plan != null)
			{
				_context.Plans.Remove(plan);
				await _context.SaveChangesAsync();
			}
		}

		public async Task DeleteAsync(List<Plan> plans)
		{
			_context.Plans.RemoveRange(plans);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(List<Guid> planIds)
		{
			var plans = await _context.Plans.Where(p => planIds.Contains(p.Id)).ToListAsync();
			_context.Plans.RemoveRange(plans);
			await _context.SaveChangesAsync();
		}

		public async Task<Plan> GetByIdAsync(Guid id)
		{
			return await _context.Plans.FindAsync(id);
		}

		public async Task<List<Plan>> GetAllAsync(List<Guid> ids)
		{
			return await _context.Plans.Where(p => ids.Contains(p.Id)).ToListAsync();
		}
	}
}
