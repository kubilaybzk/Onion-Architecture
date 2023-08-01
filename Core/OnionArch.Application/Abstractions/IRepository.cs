using System;
using Microsoft.EntityFrameworkCore;
using OnionArch.Domain.Entities.Common;

namespace OnionArch.Application.Abstractions
{
	public interface IRepository<T> where T: BaseEntity
    {
		//Burada evrensel olan şeyleri eklememiz gerekmekte .

		DbSet<T> Table { get; }

	}
}

