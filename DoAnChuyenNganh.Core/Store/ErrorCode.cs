﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnChuyenNganh.Core.Store
{
    public class ErrorCode
    {
        public const string BadRequest = "Bad Request";
        public const string Unauthorized = "Unauthorized";
        public const string Forbidden = "Forbidden";
        public const string NotFound = "Not Found";
        public const string ServerError = "Internal Server Error";
    }
}