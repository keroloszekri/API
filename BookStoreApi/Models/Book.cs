using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace BookStoreApi.Models
{
    public class Book
    {
        [Key]
        [Required]
        public String ISBN { get; set; }

        public double Price { get; set; }
        public String Title { get; set; }
        public int Year { get; set; }

        public String UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }

    }
}