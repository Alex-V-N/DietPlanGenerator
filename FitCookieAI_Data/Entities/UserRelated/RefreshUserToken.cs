using FitCookieAI_Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_Data.Entities.UserRelated
{
	public class RefreshUserToken : BaseToken
	{
		[Required]
		public int UserId { get; set; }

		[ForeignKey("UserId")]
		public User? ParentUser { get; set; }
	}
}
