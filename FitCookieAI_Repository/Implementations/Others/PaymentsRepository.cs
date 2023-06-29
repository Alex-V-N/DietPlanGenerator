using FitCookieAI_Data.Entities.Others;
using FitCookieAI_Data.Entities.UserRelated;
using FitCookieAI_Repository.Implementations.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_Repository.Implementations.Others
{
	public class PaymentsRepository : BaseRepository<Payment>
	{
		public PaymentsRepository(MyUnitOfWork uow) : base(uow)
		{

		}
		public PaymentsRepository() : base()
		{

		}
		protected override IQueryable<Payment> CascadeInclude(IQueryable<Payment> query)
		{
			return query.Include(c => c.ParentUser);
		}
	}
}
