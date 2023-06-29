using FitCookieAI_Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_ApplicationService.DTOs.AdminRelated
{
	public class RefreshAdminTokenDTO : BaseToken
	{
		public int AdminId { get; set; }
	}
}
