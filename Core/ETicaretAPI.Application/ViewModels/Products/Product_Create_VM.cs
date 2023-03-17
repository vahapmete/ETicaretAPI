using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.ViewModels.Products
{
    public class Product_Create_VM
    {
        public string Name { get; set; }
        public int Stock{ get; set; }
        public float Price { get; set; }
    }
}
