namespace Logic.Models;

public class PlanDate
{
	public Guid Id { get; set; }
	public Guid PlanId { get; set; }
	public DateTime Date { get; set; }
}