using System;
using OnionArch.Domain.Entities.Common;

namespace OnionArch.Application.Abstractions
{
	public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
    {
        //Burası Global olan değerlerede sahip olabilir.
        //Lakin burası sadece veri yazmak ve update etmek için.
        Task<bool> AddAsync(T model);

        Task<bool> AddRangeAsync(List<T> datas);

        bool Remove(T datas);

        bool RemoveRange(List<T> datas);

        Task<bool> RemoveAsync(string id);

        bool Update(T model);

        Task<int> SaveAsync();

    }
}

