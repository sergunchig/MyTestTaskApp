using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTestTaskApp.Models
{
    public class OrderViewComparer : IEqualityComparer<OrderView>
    {
        public bool Equals(OrderView x, OrderView y)
        {
            return x.Number.Equals(y.Number);
        }

        public int GetHashCode(OrderView obj)
        {
            return obj.Number.GetHashCode();
        }
    }
}