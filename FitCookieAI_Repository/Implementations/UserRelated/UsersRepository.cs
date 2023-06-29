using FitCookieAI_Data.Entities.UserRelated;
using FitCookieAI_Repository.Implementations.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_Repository.Implementations.UserRelated
{
	public class UsersRepository : BaseRepository<User>
	{
		public UsersRepository(MyUnitOfWork uow) : base(uow)
		{

		}
		public UsersRepository() : base()
		{

		}
	}
}
