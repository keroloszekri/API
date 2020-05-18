using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStoreApi.Models
{
    public class ApplicationUser:IdentityUser
    {
        public String Name { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Book> Books { get; set; }

    }
}