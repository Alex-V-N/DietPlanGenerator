using FitCookieAI_Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_Data.Entities.AdminRelated
{
	public class AdminStatus : BaseEntity
	{
		[Required]
        [MaxLength(50)]
        public string? Name { get; set; }
	}
}
