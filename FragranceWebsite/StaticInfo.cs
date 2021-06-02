using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FragranceWebsite
{
    public static class StaticInfo
    {
        public static string APIBaseUrl = "https://localhost:44387/";
        public static string FragranceAPIPath = APIBaseUrl+ "api/fragrances/";
        public static string CustomerAPIPath = APIBaseUrl + "api/customers/";
        public static string SaleAPIPath = APIBaseUrl + "api/sales/";
        public static string UserAPIPath = APIBaseUrl + "api/users/";

    }
}
