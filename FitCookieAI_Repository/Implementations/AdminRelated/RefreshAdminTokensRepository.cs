using FitCookieAI_Data.Entities.AdminRelated;
using FitCookieAI_Repository.Implementations.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_Repository.Implementations.AdminRelated
{
	public class RefreshAdminTokensRepository : BaseRepository<RefreshAdminToken>
	{
		public RefreshAdminTokensRepository(MyUnitOfWork uow) : base(uow)
		{

		}
		public RefreshAdminTokensRepository() : base()
		{

		}
		protected override IQueryable<RefreshAdminToken> CascadeInclude(IQueryable<RefreshAdminToken> query)
		{
			return query.Include(c => c.ParentAdmin);
		}
	}
}
