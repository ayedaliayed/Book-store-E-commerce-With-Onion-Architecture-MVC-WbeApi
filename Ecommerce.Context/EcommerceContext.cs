using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Context
{

    public class EcommerceUser: IdentityUser
    {
       public string? Address { get; set; }
       public string? city { get; set; }
        public int? age { get; set; }  
    }

    public class EcommerceContext:IdentityDbContext<EcommerceUser>                                          // DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
     
        public EcommerceContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

       
    }
}
