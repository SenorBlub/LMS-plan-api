using Logic.Models;

namespace Logic.IRepositories;

public interface IPlanActivityRepository
{
	public Task CreateAsync(PlanActivity planActivity);
	public Task CreateAsync(List<PlanActivity> planActivities);
	public Task UpdateAsync(PlanActivity planActivity);
	public Task UpdateAsync(List<PlanActivity> planActivity);
	public Task DeleteAsync(PlanActivity planActivity);
	public Task DeleteAsync(Guid planId);
	public Task DeleteAsync(List<Guid> planIds);
	public Task<List<PlanActivity>> GetByPlanIdAsync(Guid planId);
	public Task<List<PlanActivity>> GetByActivityIdAsync(Guid activityId);
	public Task<PlanActivity> GetByIdAsync(Guid planActivityId);
}