using FitCookieAI_Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_Data.Entities.Others
{
	public class PaymentPlan : BaseEntity
	{
		[Required]
        [MaxLength(30)]
        public string? Title { get; set; }

		[Required]
        [MaxLength(150)]
        public string? Description { get; set; }

		[Required]
		public decimal? PricePerMonth { get; set; }
	}
}
