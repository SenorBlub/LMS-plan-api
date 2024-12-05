using Logic.Models;

namespace Logic.IRepositories;

public interface IPlanDateRepository
{
	public Task CreateAsync(PlanDate planDate);
	public Task CreateAsync(List<PlanDate> planDates);
	public Task UpdateAsync(PlanDate planDate);
	public Task UpdateAsync(List<PlanDate> planDate);
	public Task DeleteAsync(PlanDate planDate);
	public Task DeleteAsync(Guid planId);
	public Task DeleteAsync(List<Guid> planIds);
	/*public Task DeleteRangeAsync(DateTime startDate, DateTime endDate);*/ //this method is never useful
	public Task<List<PlanDate>> GetAsync(Guid planId);
	public Task<PlanDate> GetAsync(DateTime date);
	public Task<List<PlanDate>> GetRangeAsync(DateTime startDate, DateTime endDate);
}