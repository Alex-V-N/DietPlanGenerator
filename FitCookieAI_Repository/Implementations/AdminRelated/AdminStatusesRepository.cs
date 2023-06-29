using FitCookieAI_Data.Entities.AdminRelated;
using FitCookieAI_Repository.Implementations.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_Repository.Implementations.AdminRelated
{
	public class AdminStatusesRepository : BaseRepository<AdminStatus>
	{
		public AdminStatusesRepository(MyUnitOfWork uow) : base(uow)
		{

		}
		public AdminStatusesRepository() : base()
		{

		}
	}
}
