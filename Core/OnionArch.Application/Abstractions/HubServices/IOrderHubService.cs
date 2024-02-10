using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Abstractions.HubServices
{
    public interface  IOrderHubService
    {
        //Sipariş oluştuğu anda çalışacak olan SignalR Hub'ının interface
        Task OrderAddedMessageAsync(string message);
    }
}
