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
	public class PlanDateRepository : IPlanDateRepository
	{
		private readonly PlanDbContext.PlanDbContext _context;

		public PlanDateRepository(PlanDbContext.PlanDbContext context)
		{
			_context = context;
		}

		public async Task CreateAsync(PlanDate planDate)
		{
			await _context.PlanDates.AddAsync(planDate);
			await _context.SaveChangesAsync();
		}

		public async Task CreateAsync(List<PlanDate> planDates)
		{
			await _context.PlanDates.AddRangeAsync(planDates);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(PlanDate planDate)
		{
			_context.PlanDates.Update(planDate);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(List<PlanDate> planDates)
		{
			_context.PlanDates.UpdateRange(planDates);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(PlanDate planDate)
		{
			_context.PlanDates.Remove(planDate);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(Guid planId)
		{
			var dates = await _context.PlanDates.Where(pd => pd.PlanId == planId).ToListAsync();
			_context.PlanDates.RemoveRange(dates);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(List<Guid> planIds)
		{
			var dates = await _context.PlanDates.Where(pd => planIds.Contains(pd.PlanId)).ToListAsync();
			_context.PlanDates.RemoveRange(dates);
			await _context.SaveChangesAsync();
		}

		public async Task<List<PlanDate>> GetAsync(Guid planId)
		{
			return await _context.PlanDates.Where(pd => pd.PlanId == planId).ToListAsync();
		}

		public async Task<PlanDate> GetAsync(DateTime date)
		{
			return await _context.PlanDates.FirstOrDefaultAsync(pd => pd.Date == date);
		}

		public async Task<List<PlanDate>> GetRangeAsync(DateTime startDate, DateTime endDate)
		{
			return await _context.PlanDates
				.Where(pd => pd.Date >= startDate && pd.Date <= endDate)
				.ToListAsync();
		}
	}
}
