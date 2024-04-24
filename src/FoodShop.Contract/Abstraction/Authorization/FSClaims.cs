﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Contract.Abstraction.Authorization
{
    public static class FSClaims
    {
        public const string Fullname = "fullName";
        public const string Permission = "permission";
        public const string ImageUrl = "image_url";
        public const string IpAddress = "ipAddress";
        public const string Expiration = "exp";

    }
}
