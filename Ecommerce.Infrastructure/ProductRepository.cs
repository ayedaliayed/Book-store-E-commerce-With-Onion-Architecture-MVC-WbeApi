using Ecommerce.Application.Contract;
using Ecommerce.Context;
using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure
{
    public class ProductRepository : Repository<Product, int>, IProductRepository
    {
        public ProductRepository(EcommerceContext ecommerceContext) : base(ecommerceContext)
        {

        }
    }
}
