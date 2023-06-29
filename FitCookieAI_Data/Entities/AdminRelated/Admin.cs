using FitCookieAI_Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_Data.Entities.AdminRelated
{
	public class Admin : Person
	{
		[Required]
		public int StatusId { get; set; }
		public string? ProfilePhotoName { get; set; }

		[ForeignKey("StatusId")]
		public AdminStatus? ParentAdminStatus { get; set; }
	}
}
