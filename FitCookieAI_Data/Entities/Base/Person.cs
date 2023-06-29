using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_Data.Entities.Base
{
	public class Person : BaseEntity
	{
		[Required]
		[MaxLength(80)]
		public string? Email { get; set; }

		[Required]
		[MaxLength(120)]
		public string? Password { get; set; }

		[Required]
		[MaxLength(60)]
		public string? FirstName { get; set; }

		[Required]
		[MaxLength(60)]
		public string? LastName { get; set; }

		[Required]
		public DateTime? DOB { get; set; }

		[Required]
		public string? Gender { get; set; }
	}
}
