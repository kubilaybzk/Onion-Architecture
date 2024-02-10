using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR
{
    public static class ReceiveFunctionNames
    {
        //Bizim Hublarımızda hangi fonksiyona karşılık verilecek gelecek olan istekteki fonksiyon isimleri 

        public const string ProductAddedMessage = "receiveProductAddedMessage";
        public const string OrderAddedMessage = "receiveOrderAddedMessage";

    }
}
