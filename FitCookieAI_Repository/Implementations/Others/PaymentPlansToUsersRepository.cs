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
	public class PaymentPlansToUsersRepository : BaseRepository<PaymentPlanToUser>
	{
		public PaymentPlansToUsersRepository(MyUnitOfWork uow) : base(uow)
		{

		}
		public PaymentPlansToUsersRepository() : base()
		{

		}
		protected override IQueryable<PaymentPlanToUser> CascadeInclude(IQueryable<PaymentPlanToUser> query)
		{
			return query.Include(c => c.ParentUser)
				.Include(c => c.ParentPaymentPlan);
		}
	}
}
