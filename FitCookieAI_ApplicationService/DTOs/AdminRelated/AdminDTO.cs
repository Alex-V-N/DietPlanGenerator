using FitCookieAI_Data.Entities.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_ApplicationService.DTOs.AdminRelated
{
	public class AdminDTO : Person
	{
		public int StatusId { get; set; }
		public string? ProfilePhotoName { get; set; }
		public IFormFile? ImageFileName { get; set; }
	}
}
