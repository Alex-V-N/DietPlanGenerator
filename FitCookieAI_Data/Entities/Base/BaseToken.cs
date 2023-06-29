using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_Data.Entities.Base
{
	public class BaseToken : BaseEntity
	{
		public string? Token { get; set; }
		public string? JwtId { get; set; }
		public bool IsUsed { get; set; }
		public bool IsRevorked { get; set; }
		public DateTime AddedDate { get; set; }
		public DateTime ExpiryDate { get; set; }
	}
}
