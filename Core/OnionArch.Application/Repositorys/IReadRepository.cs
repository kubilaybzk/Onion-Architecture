using System;
using System.Linq.Expressions;
using OnionArch.Domain.Entities.Common;

namespace OnionArch.Application.Abstractions
{
	public interface IReadRepository<T>:IRepository<T> where T: BaseEntity
    {
        //Burası Global olan değerlerede sahip olabilir.
        //Lakin burası sadece veri okumak için.
        /*Şimdi biz 3 tane şey düşünelim
			- şartlı bir istek
			- Tüm verilerin geldiği bir istek
			- Id ye göre bir istek.
			- Tek bir sonuç gönderen istek
		*/

        IQueryable<T> GetAll(bool tracking = true);

        IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true);

        Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true);

        Task<T> GetByIdAsync(string id, bool tracking = true);



    }
}
