using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medimall.Models
{
    public class ListProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public List<Product> listProducts { get; set; }
    }
}