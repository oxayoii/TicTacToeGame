﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Middleware
{
    public class CustomException : Exception
    {
        public class NotFoundException : Exception
        {
            public NotFoundException(string message) : base(message) { }
        }
        public class BadRequestException : Exception
        {
            public BadRequestException(string message) : base(message) { }
        }
    }
}
