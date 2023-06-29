using FitCookieAI_Data.Entities.AdminRelated;
using FitCookieAI_Data.Entities.Others;
using FitCookieAI_Data.Entities.UserRelated;
using GlobalVariables.Encription;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_Data.Context
{
	public class MyDbContext : DbContext
	{
		public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Admin> Admins { get; set; }
		public DbSet<AdminStatus> AdminStatuses { get; set; }
		public DbSet<Payment> Payments { get; set; }
		public DbSet<PaymentPlan> PaymentPlans { get; set; }
		public DbSet<PaymentPlanFeature> PaymentPlanFeatures { get; set; }
		public DbSet<PaymentPlanToUser> PaymentPlansToUsers { get; set; }
		public DbSet<RefreshUserToken> RefreshUserTokens { get; set; }
        public DbSet<RefreshAdminToken> RefreshAdminTokens { get; set; }

        public MyDbContext()
		{
			Users = this.Set<User>();
			Admins = this.Set<Admin>();
			AdminStatuses = this.Set<AdminStatus>();
			Payments = this.Set<Payment>();
			PaymentPlans = this.Set<PaymentPlan>();
			PaymentPlanFeatures = this.Set<PaymentPlanFeature>();
			PaymentPlansToUsers = this.Set<PaymentPlanToUser>();
			RefreshAdminTokens = this.Set<RefreshAdminToken>();
			RefreshUserTokens = this.Set<RefreshUserToken>();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Seeding AdminStatus
			modelBuilder.Entity<AdminStatus>().HasData(new AdminStatus
			{
				Id = 1, 
				Name = "Developer" 
			});

			modelBuilder.Entity<AdminStatus>().HasData(new AdminStatus
			{
				Id = 2,
				Name = "Administrator"
			});

			modelBuilder.Entity<AdminStatus>().HasData(new AdminStatus
			{
				Id = 3,
				Name = "Unverified"
			});

			// Seeding Admin
			modelBuilder.Entity<Admin>().HasData(new Admin
			{
				Id = 1, 
				Email = "admin@mail.com",
				Password = StringCipher.Encrypt("123456", EncriptionVariables.PasswordEncriptionKey), 
				FirstName = "Admin",
				LastName = "User",
				DOB = DateTime.Now, 
				Gender = "Male",
				StatusId = 1, 
				ProfilePhotoName = ""
			});
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Data Source = AKIHIRO\\SQLEXPRESS; TrustServerCertificate = True; initial catalog = FitCookieAIDb; user id = alex; password = rexibexi1");
		}
	}
}
