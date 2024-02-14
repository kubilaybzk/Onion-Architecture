﻿using OnionArch.Application.Abstractions;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Repositories.AddressCrud
{
    public interface IAddressReadRepository:IReadRepository<Address>
    {
    }
}
