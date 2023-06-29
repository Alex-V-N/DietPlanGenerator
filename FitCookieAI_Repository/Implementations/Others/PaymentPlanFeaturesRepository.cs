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
	public class PaymentPlanFeaturesRepository : BaseRepository<PaymentPlanFeature>
	{
		public PaymentPlanFeaturesRepository(MyUnitOfWork uow) : base(uow)
		{

		}
		public PaymentPlanFeaturesRepository() : base()
		{

		}
		protected override IQueryable<PaymentPlanFeature> CascadeInclude(IQueryable<PaymentPlanFeature> query)
		{
			return query.Include(c => c.ParentPaymentPlan);
		}
	}
}
