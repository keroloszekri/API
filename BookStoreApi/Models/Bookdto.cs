using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStoreApi.Models
{
    public class Bookdto
    {

        public String ISBN { get; set; }

        public double Price { get; set; }
        public String Title { get; set; }
        public int Year { get; set; }
        public String UserID { get; set; }

    }
}