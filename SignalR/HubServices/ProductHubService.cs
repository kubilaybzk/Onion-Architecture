using Microsoft.AspNetCore.SignalR;
using OnionArch.Application.Abstractions.HubServices;
using SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.HubServices
{
    public class ProductHubService : IProductHubService
    {
        readonly IHubContext<ProductHub> _hubContext;

          public ProductHubService(IHubContext<ProductHub> hubContext)
        {
            _hubContext = hubContext;
        }   

        public async Task ProductAddOperationMessage(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.ProductAddedMessage, message);
        }
    }
}
