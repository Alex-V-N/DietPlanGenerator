using FitCookieAI_Data.Entities.Others;
using FitCookieAI_Repository.Implementations.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_Repository.Implementations.Others
{
	public class PaymentPlansRepository : BaseRepository<PaymentPlan>
	{
		public PaymentPlansRepository(MyUnitOfWork uow) : base(uow)
		{

		}
		public PaymentPlansRepository() : base()
		{

		}
	}
}
