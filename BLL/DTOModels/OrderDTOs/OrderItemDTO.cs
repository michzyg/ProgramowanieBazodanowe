﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOModels.OrderDTOs
{
    public class OrderItemDTO
    {
        public string ProductName { get; init; }
        public double Price { get; init; }
        public int Amount { get; init; }
        public double TotalValue { get; init; }
    }
}
