using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;

namespace DAL.PlanDbContext
{
	public class PlanDbContext : DbContext
	{
		public PlanDbContext(DbContextOptions<PlanDbContext> options)
			: base(options)
		{
		}

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

				entity.Property(p => p.Title)
					.IsRequired()
					.HasMaxLength(255);

				entity.Property(p => p.Description)
					.IsRequired(false);

				entity.Property(p => p.Finished)
					.IsRequired();
			});

			modelBuilder.Entity<PlanActivity>(entity =>
			{
				entity.ToTable("PlanActivities");
				entity.HasKey(pa => pa.Id);

				entity.Property(pa => pa.PlanId)
					.IsRequired();

				entity.Property(pa => pa.ActivityId)
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

				entity.Property(pd => pd.PlanId)
					.IsRequired();

				entity.Property(pd => pd.Date)
					.IsRequired();

				entity.HasOne<Plan>()
					.WithMany(p => p.PlanDates)
					.HasForeignKey(pd => pd.PlanId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			base.OnModelCreating(modelBuilder);
		}
	}
}
