using Microsoft.AspNetCore.SignalR;
using OnionArch.Application.Abstractions.HubServices;
using SignalR.Hubs;


namespace SignalR.HubServices
{
    //İnterface içinde kalıtım alıp gerekli işlemleri burada yapacağımız servise dosyamız
    public class OrderHubService : IOrderHubService
    {
        readonly IHubContext<OrderHub> _hubContext;

        public OrderHubService(IHubContext<OrderHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task OrderAddedMessageAsync(string message)
        =>_hubContext.Clients.All.SendAsync(ReceiveFunctionNames.OrderAddedMessage, message);
    }
}
