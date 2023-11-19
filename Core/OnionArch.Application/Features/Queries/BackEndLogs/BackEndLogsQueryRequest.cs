using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.BackEndLogs
{
    public class BackEndLogsQueryRequest:IRequest<BackEndLogsQueryResponse>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 8;
    }
}
