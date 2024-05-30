using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.GlobalResponse
{
    public class GlobalResponseResult
    {
        public string? Message { get; set; }
        public bool? HassError { get; set; }
        public string? ErrorMessage { get; set; }
        public HttpStatusCode? StatusCode { get; set; }
        public string? StatusCodeString { get; set; }
    }
}
