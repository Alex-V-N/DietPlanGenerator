using FitCookieAI_Data.Entities.UserRelated;
using FitCookieAI_Repository.Implementations.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_Repository.Implementations.UserRelated
{
	public class RefreshUserTokensRepository : BaseRepository<RefreshUserToken>
	{
		public RefreshUserTokensRepository(MyUnitOfWork uow) : base(uow)
		{

		}
		public RefreshUserTokensRepository() : base()
		{

		}
		protected override IQueryable<RefreshUserToken> CascadeInclude(IQueryable<RefreshUserToken> query)
		{
			return query.Include(c => c.ParentUser);
		}
	}
}
