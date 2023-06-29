using FitCookieAI_Data.Entities.AdminRelated;
using FitCookieAI_Data.Entities.UserRelated;
using FitCookieAI_Repository.Implementations.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_Repository.Implementations.AdminRelated
{
	public class AdminsRepository : BaseRepository<Admin>
	{
		public AdminsRepository(MyUnitOfWork uow) : base(uow)
		{

		}
		public AdminsRepository() : base()
		{

		}
		protected override IQueryable<Admin> CascadeInclude(IQueryable<Admin> query)
		{
			return query.Include(c => c.ParentAdminStatus);
		}
	}
}
