﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOModels
{
    public class ProductRequestDTO
    {
        public string Name { get; init; }
        public double Price { get; init; }
        public string Image { get; init; }
        public int GroupId { get; init; }
    }
}
