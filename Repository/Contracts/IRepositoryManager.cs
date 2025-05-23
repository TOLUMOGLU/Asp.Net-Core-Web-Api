﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IRepositoryManager
    {
        IBookRepository Book { get; }
        Task SaveAsync();
    }
}
