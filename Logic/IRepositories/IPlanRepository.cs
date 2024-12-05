using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;

namespace Logic.IRepositories
{
	public interface IPlanRepository
	{
		public Task CreateAsync(Plan plan);
		public Task CreateAsync(List<Plan> plans);
		public Task UpdateAsync(Plan plan);
		public Task UpdateAsync(List<Plan> plans);
		public Task DeleteAsync(Plan plan);
		public Task DeleteAsync(Guid planId);
		public Task DeleteAsync(List<Plan> plans);
		public Task DeleteAsync(List<Guid> planIds);
		public Task<Plan> GetByIdAsync(Guid id);
		public Task<List<Plan>> GetAllAsync(List<Guid> ids);
	}
}