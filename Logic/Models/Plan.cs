using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
	public class Plan
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public bool Finished { get; set; }
		public ICollection<PlanActivity>? PlanActivities { get; set; }
		public ICollection<PlanDate>? PlanDates { get; set; }
	}
}
