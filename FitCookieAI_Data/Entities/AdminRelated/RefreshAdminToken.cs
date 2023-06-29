using FitCookieAI_Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_Data.Entities.AdminRelated
{
	public class RefreshAdminToken : BaseToken
	{
		public int AdminId { get; set; }

		[ForeignKey("AdminId")]
		public Admin? ParentAdmin { get; set; }
	}
}
