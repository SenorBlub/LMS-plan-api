using Logic.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.PlanDbContext;

public class PlanDbContext : DbContext
{
	public PlanDbContext(DbContextOptions<PlanDbContext> options)
		: base(options) { }

	public DbSet<Plan>? Plans { get; set; }
	public DbSet<PlanActivity>? PlanActivities { get; set; }
	public DbSet<PlanDate>? PlanDates { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Plan>(entity =>
		{
			entity.ToTable("Plans");
			entity.HasKey(p => p.Id);

			entity.Property(p => p.Id)
				.HasColumnName("Id")
				.HasColumnType("char(36)")
				.IsRequired();

			entity.Property(p => p.Title)
				.HasColumnName("Title")
				.HasColumnType("varchar(255)")
				.IsRequired();

			entity.Property(p => p.Description)
				.HasColumnName("Description")
				.HasColumnType("text")
				.IsRequired(false);

			entity.Property(p => p.Finished)
				.HasColumnName("Finished")
				.HasColumnType("tinyint(1)")
				.IsRequired();
		});

		modelBuilder.Entity<PlanActivity>(entity =>
		{
			entity.ToTable("PlanActivities");
			entity.HasKey(pa => pa.Id);

			entity.Property(pa => pa.Id)
				.HasColumnName("Id")
				.HasColumnType("char(36)")
				.IsRequired();

			entity.Property(pa => pa.PlanId)
				.HasColumnName("PlanId")
				.HasColumnType("char(36)")
				.IsRequired();

			entity.Property(pa => pa.ActivityId)
				.HasColumnName("ActivityId")
				.HasColumnType("char(36)")
				.IsRequired();

			entity.HasOne<Plan>()
				.WithMany(p => p.PlanActivities)
				.HasForeignKey(pa => pa.PlanId)
				.OnDelete(DeleteBehavior.Cascade);
		});

		modelBuilder.Entity<PlanDate>(entity =>
		{
			entity.ToTable("PlanDate");
			entity.HasKey(pd => pd.Id);

			entity.Property(pd => pd.Id)
				.HasColumnName("Id")
				.HasColumnType("char(36)")
				.IsRequired();

			entity.Property(pd => pd.PlanId)
				.HasColumnName("PlanId")
				.HasColumnType("char(36)")
				.IsRequired();

			entity.Property(pd => pd.Date)
				.HasColumnName("Date")
				.HasColumnType("datetime")
				.IsRequired();

			entity.HasOne<Plan>()
				.WithMany(p => p.PlanDates)
				.HasForeignKey(pd => pd.PlanId)
				.OnDelete(DeleteBehavior.Cascade);
		});
	}
}
