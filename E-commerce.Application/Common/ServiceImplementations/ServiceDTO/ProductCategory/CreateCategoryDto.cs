using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Common.ServiceImplementations.ServiceDTO.ProductCategory
{
    public class CreateCategoryDto
    {
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public DateTime CreatedAT { get; set; }

    }
}
