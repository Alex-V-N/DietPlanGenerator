using FitCookieAI_Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_ApplicationService.DTOs.Others
{
	public class PaymentPlanDTO : BaseEntity
	{
		public string? Title { get; set; }
		public string? Description { get; set; }
		public decimal? PricePerMonth { get; set; }
	}
}
