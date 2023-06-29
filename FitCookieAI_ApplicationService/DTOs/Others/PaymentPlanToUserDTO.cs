using FitCookieAI_Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_ApplicationService.DTOs.Others
{
	public class PaymentPlanToUserDTO : BaseEntity
	{
		public int PaymentPlanId { get; set; }
		public int UserId { get; set; }
	}
}
