using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccountingAPI.Model.ProductService
{

    [Table("ProductRestocks", Schema = "ProductService")]
    public class ProductRestock
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, provide product id.")]
        public int ProductId { get; set; }


    }
}
