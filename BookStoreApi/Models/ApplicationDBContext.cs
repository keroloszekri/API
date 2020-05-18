using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BookStoreApi.Models
{
    public class ApplicationDBContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext():base("CS")
        {

        }
        public virtual DbSet<Book> Books { get; set; }

        //public System.Data.Entity.DbSet<BookStoreApi.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}