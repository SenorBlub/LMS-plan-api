using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Logic.IRepositories;
using Logic.IServices;
using Logic.Models;

namespace Logic.Services
{
	public class PlanService : IPlanService
	{
		private readonly IPlanRepository _planRepository;
		private readonly IPlanDateRepository _planDateRepository;
		private readonly IPlanActivityRepository _planActivityRepository;

		public PlanService(IPlanRepository planRepository, IPlanDateRepository planDateRepository, IPlanActivityRepository planActivityRepository)
		{
			_planRepository = planRepository;
			_planDateRepository = planDateRepository;
			_planActivityRepository = planActivityRepository;
		}

		public async Task CreateAsync(Plan plan)
		{
			await _planRepository.CreateAsync(plan);

			List<PlanDate> planDates = new List<PlanDate>();
			foreach (PlanDate planDate in plan.PlanDates)
			{
				planDates.Add(planDate);
			}

			await _planDateRepository.CreateAsync(planDates);

			List<PlanActivity> planActivities = new List<PlanActivity>();
			foreach (PlanActivity planActivity in plan.PlanActivities)
			{
				planActivities.Add(planActivity);
			}

			await _planActivityRepository.CreateAsync(planActivities);
		}

		public async Task CreateManyAsync(List<Plan> plans)
		{
			foreach (Plan plan in plans)
			{
				await _planRepository.UpdateAsync(plan);

				List<PlanDate> planDates = new List<PlanDate>();
				foreach (PlanDate planDate in plan.PlanDates)
				{
					planDates.Add(planDate);
				}

				await _planDateRepository.CreateAsync(planDates);

				List<PlanActivity> planActivities = new List<PlanActivity>();
				foreach (PlanActivity planActivity in plan.PlanActivities)
				{
					planActivities.Add(planActivity);
				}

				await _planActivityRepository.CreateAsync(planActivities);
			}
		}

		public async Task DeleteAsync(Plan plan)
		{
			foreach (PlanActivity planActivity in plan.PlanActivities)
			{
				await _planActivityRepository.DeleteAsync(planActivity);
			}

			foreach (PlanDate planDate in plan.PlanDates)
			{
				await _planDateRepository.DeleteAsync(planDate);
			}

			await _planRepository.DeleteAsync(plan);
		}

		public async Task DeleteAsync(Guid planId)
		{
			await _planActivityRepository.DeleteAsync(planId);
			await _planDateRepository.DeleteAsync(planId);
			await _planRepository.DeleteAsync(planId);
		}

		public async Task DeleteByDateAsync(DateTime date)
		{
			Guid planId = _planDateRepository.GetAsync(date).Result.Id;
			await _planActivityRepository.DeleteAsync(planId);
			await _planDateRepository.DeleteAsync(planId);
			await _planRepository.DeleteAsync(planId);
		}

		public async Task DeleteByDateRangeAsync(DateTime startDate, DateTime endDate, List<Guid> userPlanIds)
		{
			List<PlanDate> planDates = await _planDateRepository.GetRangeAsync(startDate, endDate);

			List<Guid> DeleteIds = new List<Guid>();
			foreach (Guid planId in userPlanIds)
			{
				int planDateIndex = 999999999;
				for (int i = 0; i < planDates.Count(); i++)
				{
					if (planDates[i].Id == planId)
					{
						DeleteIds.Add(planDates[i].PlanId);
						planDateIndex = i; break;
					}
				}
				planDates.RemoveAt(planDateIndex);
			}

			await _planDateRepository.DeleteAsync(DeleteIds);
			await _planActivityRepository.DeleteAsync(DeleteIds);
			await _planRepository.DeleteAsync(DeleteIds);
		}

		public async Task UpdateAsync(Plan plan)
		{
			List<PlanDate> planDateList = plan.PlanDates.ToList();
			List<PlanActivity> planActivityList = plan.PlanActivities.ToList();
			await _planDateRepository.UpdateAsync(planDateList);
			await _planActivityRepository.UpdateAsync(planActivityList);
			await _planRepository.UpdateAsync(plan);
		}

		public async Task UpdateAsync(List<Plan> plans)
		{
			foreach (Plan plan in plans)
			{
				List<PlanDate> planDateList = plan.PlanDates.ToList();
				List<PlanActivity> planActivityList = plan.PlanActivities.ToList();
				await _planDateRepository.UpdateAsync(planDateList);
				await _planActivityRepository.UpdateAsync(planActivityList);
			}
			await _planRepository.UpdateAsync(plans);
		}

		public async Task UpdateAsync(List<Guid> planIds, Plan plan)
		{
			//see from clause on microsoft, this was cool to code!
			var updatedPlanDates =
				from planDate in plan.PlanDates.ToList()
				from planId in planIds.ToList()
					select new PlanDate { PlanId = planId, Date = planDate.Date, Id = planDate.Id };

			var updatedPlanActivities =
				from planActivity in plan.PlanActivities.ToList()
				from planId in planIds.ToList()
					select new PlanActivity { PlanId = planId, ActivityId = planActivity.ActivityId, Id = planActivity.Id };

			var updatedPlans =
				from planId in planIds
				select new Plan
				{
					Id = planId,
					PlanActivities = plan.PlanActivities,
					PlanDates = plan.PlanDates,
					Description = plan.Description,
					Finished = plan.Finished,
					Title = plan.Title
				};

			List<Plan> updatedPlanList = updatedPlans.ToList();
			List<PlanDate> updatedPlanDateList = updatedPlanDates.ToList();
			List<PlanActivity> updatedPlanActivityList = updatedPlanActivities.ToList();

			await _planDateRepository.UpdateAsync(updatedPlanDateList);
			await _planActivityRepository.UpdateAsync(updatedPlanActivityList);
			await _planRepository.UpdateAsync(updatedPlanList);
		}

		public async Task UpdateAsync(Guid planId, Plan plan)
		{
			Plan newPlan = plan;
			newPlan.Id = planId;
			List<PlanDate> planDateList = newPlan.PlanDates.ToList();
			List<PlanActivity> planActivityList = newPlan.PlanActivities.ToList();
			await _planDateRepository.UpdateAsync(planDateList);
			await _planActivityRepository.UpdateAsync(planActivityList);
			await _planRepository.UpdateAsync(newPlan);
		}

		public async Task<Plan> GetByIdAsync(Guid planId)
		{
			Plan plan = await _planRepository.GetByIdAsync(planId);
			plan.PlanActivities = await _planActivityRepository.GetByPlanIdAsync(planId);
			plan.PlanDates = await _planDateRepository.GetAsync(planId);
			return plan;
		}

		public async Task<List<Plan>> GetByDateRange(DateTime startDate, DateTime endDate, List<Guid> userPlanIds)
		{
			List<PlanDate> planDates = await _planDateRepository.GetRangeAsync(startDate, endDate);

			List<Guid> planIds = new List<Guid>();
			foreach (Guid planId in userPlanIds)
			{
				int planDateIndex = 999999999;
				for (int i = 0; i < planDates.Count(); i++)
				{
					if (planDates[i].Id == planId)
					{
						planIds.Add(planDates[i].PlanId);
						planDateIndex = i; break;
					}
				}
				planDates.RemoveAt(planDateIndex);
			}

			List<Plan> planList = await _planRepository.GetAllAsync(planIds);
			List<List<PlanDate>> planPlanDateList = new List<List<PlanDate>>();
			List<List<PlanActivity>> planPlanActivityList = new List<List<PlanActivity>>();
			foreach (Guid planId in planIds)
			{
				planPlanDateList.Add(await _planDateRepository.GetAsync(planId));
				planPlanActivityList.Add(await _planActivityRepository.GetByPlanIdAsync(planId));
			}

			var plans =
				from plan in planList
				from planDateList in planPlanDateList
				from planActivityList in planPlanActivityList
				select new Plan
				{
					Id = plan.Id,
					Title = plan.Title,
					Description = plan.Description,
					Finished = plan.Finished,
					PlanActivities = planActivityList,
					PlanDates = planDateList
				};
			List<Plan> plansList = plans.ToList();
			return plansList;
		}

		public async Task<List<Plan>> GetAllAsync(List<Guid> userPlanIds)
		{
			List<List<PlanDate>> planPlanDateList = new List<List<PlanDate>>();
			List<List<PlanActivity>> planPlanActivityList = new List<List<PlanActivity>>();
			foreach (Guid planId in userPlanIds)
			{
				planPlanDateList.Add(await _planDateRepository.GetAsync(planId));
				planPlanActivityList.Add(await _planActivityRepository.GetByPlanIdAsync(planId));
			}
			var plans =
				from plan in await _planRepository.GetAllAsync(userPlanIds)
				from planDateList in planPlanDateList
				from planActivityList in planPlanActivityList
				select new Plan
				{
					Id = plan.Id,
					Title = plan.Title,
					Description = plan.Description,
					Finished = plan.Finished,
					PlanActivities = planActivityList,
					PlanDates = planDateList
				};

			List<Plan> planList = plans.ToList();
			return planList;
		}
	}
}
