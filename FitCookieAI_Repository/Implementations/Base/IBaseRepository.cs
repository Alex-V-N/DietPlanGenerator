using FitCookieAI_Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FitCookieAI_Repository.Implementations.Base
{
	internal interface IBaseRepository<T> where T : BaseEntity
	{
		Task<T> GetById(int id);
		Task<List<TResult>> GetReferences<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selector);
		Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null);
		Task<T> GetFirstOrDefault(Expression<Func<T, bool>> where);
		Task Save(T item);
		Task Delete(T item);
	}
}
