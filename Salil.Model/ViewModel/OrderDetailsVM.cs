﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salil.Model.ViewModel
{
    public class OrderDetailsVM
    {
        public OrderHeader OrderHeader { get; set; }

        public List<OrderDetails> OrderDetails { get; set; }
    }
}
