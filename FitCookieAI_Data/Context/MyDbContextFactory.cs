using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_Data.Context
{
	public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
	{
		public MyDbContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
			optionsBuilder.UseSqlServer("Data Source = AKIHIRO\\SQLEXPRESS; TrustServerCertificate = True; initial catalog = FitCookieAIDb; user id = alex; password = rexibexi1");

			return new MyDbContext(optionsBuilder.Options);
		}
	}

}
