using Core.Issues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCookieAuth.Data
{
    public class OnEditedResult
    {
        public string UserId { get; set; }
        public Issue Issue { get; set; }
        public string Type { get; set; }
    }
}
