using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FragranceWebsite.ViewModels
{
    public class SaleVM
    {
        public IEnumerable<SelectListItem> FragranceList { get; set; }
        public IEnumerable<SelectListItem> CustomerList { get; set; }

        public Sale Sale { get; set; }

    }
}
