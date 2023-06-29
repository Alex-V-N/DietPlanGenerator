using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitCookieAI_Data.Context;

namespace FitCookieAI_Repository.Implementations.Base
{
	public class MyUnitOfWork : IDisposable
	{
		public DbContext Context { get; private set; }
		private IDbContextTransaction Transaction { get; set; }

		public MyUnitOfWork()
		{
			Context = new MyDbContext();
		}

		public void BeginTransaction()
		{
			Transaction = Context.Database.BeginTransaction();
		}

		public void Commit()
		{
			Transaction.Commit();
		}

		public void Rollback()
		{
			Transaction.Rollback();
		}

		public void Dispose()
		{
			Context.Dispose();
			Transaction.Dispose();
		}
	}
}
