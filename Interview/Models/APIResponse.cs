using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interview.Models
{
    public class APIResponse<T>
    {
        public bool Success;
        public string Message;
        public T Data;
    }
}
