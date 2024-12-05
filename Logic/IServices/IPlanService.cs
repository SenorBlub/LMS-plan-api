using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;

namespace Logic.IServices
{
	public interface IPlanService
	{
		public Task CreateAsync(Plan plan);
		public Task CreateManyAsync(List<Plan> plans);
		public Task DeleteAsync(Plan plan);
		public Task DeleteAsync(Guid planId);
		public Task DeleteByDateAsync(DateTime date);
		public Task DeleteByDateRangeAsync(DateTime startDate, DateTime endDate, List<Guid> userPlanIds);
		public Task UpdateAsync(Plan plan);
		public Task UpdateAsync(List<Plan> plans);
		public Task UpdateAsync(List<Guid> planIds, Plan plan);
		public Task UpdateAsync(Guid planId, Plan plan);	
		public Task<Plan> GetByIdAsync(Guid planId);
		public Task<List<Plan>> GetByDateRange(DateTime startDate, DateTime endDate, List<Guid> userPlanIds);
		public Task<List<Plan>> GetAllAsync(List<Guid> userPlanIds);
	}
}
