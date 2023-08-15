using System;
using OnionArch.Domain.Entities;

namespace OnionArch.Application.Abstractions.OrderCrud
{
	public interface IOrderWriteRepository :IWriteRepository<Order>
	{
	}
}

