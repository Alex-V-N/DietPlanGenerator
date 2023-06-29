using FitCookieAI_Data.Entities.Base;
using FitCookieAI_Data.Entities.UserRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_Data.Entities.Others
{
	public class PaymentPlanFeature : BaseEntity
	{
		[Required]
        [MaxLength(30)]
        public string? Title { get; set; }

		[Required]
		public int PaymentPlanId { get; set; }

		[ForeignKey("PaymentPlanId")]
		public PaymentPlan? ParentPaymentPlan { get; set; }
	}
}
