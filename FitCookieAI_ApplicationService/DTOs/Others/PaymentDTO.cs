using FitCookieAI_Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_ApplicationService.DTOs.Others
{
	public class PaymentDTO : BaseEntity
	{
		public long Amount { get; set; }
		public string? Description { get; set; }
		public string? Currency { get; set; }
		public string? Source { get; set; }
		public int UserId { get; set; }
		public DateTime? CreatedAt { get; set; } = DateTime.Now;
	}
}
