using FluentValidation.Validators;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.HubServices;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Repositories.BackEndLogsCrud;
using SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.HubServices
{
    public class ProductHubService : IProductHubService
    {

        /*
         Şimdi çalışmalara başlamadan önce bizim oluşturduğumuz ve IOC içinde kayıt ettiğimiz
           Bu IHhubContext interface referansı üzerinden içeride hangi Hub çalışacağını belirtmemiz gerekiyor.
                Biz daha önce IHhubContext tanımlamadık bu nereden geliyor diye düşünebiliriz?  
                    => Bu bizim serviseRegistration içinde tanımladığımız  addSingralR() üzerinden geliyor.
           
           
         */

        readonly IHubContext<ProductHub> _hubContext;
        private readonly IBackEndLogsReadRepository _backendReadRepository;

        public ProductHubService(IHubContext<ProductHub> hubContext, IBackEndLogsReadRepository backendReadRepository)
        {
            _hubContext = hubContext;
            _backendReadRepository = backendReadRepository;

        }

        /*
         Gerekli tanımlamaları yaptıktan sonra   IProductHubService içinde tanımladığımız
            imzaları uygulayıp geliştirmelere başlıyoruz.
         */
        public async Task ProductAddOperationMessage(string message)
        {
            /* 
             Burada  Clientler'in hepsine mesaj yollamak istediğimizi.
                => Clients.All ile tanımladık.

            SendAsync() Nedir? 
             Şimdi bu tanımlamayı yaptıktan hemen sonra hangi fonksiyon ile her yollacacağımız belirtmemiz
           gerekiyor.
                Bunun için şöyle bir yöntem izliyeceğiz,
                    İlk parametre hangi fonksiyona karşılık verilecek olan mesajı belirtmemizi, 
                    ikinci parametre ise ne yollayacağımızı belirteceğiz.

             */

            //await _hubContext.Clients.All.SendAsync("receiveProductAddedMessage", message);

            /*
              Normalde bizim client tarafından tetiklenecek olan fonksiyonların isimlerini 
                    string olarak ifade etmek bek kullanılır değil bunları constant bir yapıya çevirmek daha mantıklı
                 
              Bunun için ayrı bir class içinde  ReceiveFunctionNames adında bir değişken tanımlayıp
                    Bu değişken üzeriden bu değişkenleri ayarlamak daha mantıklı.
             */





            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.ProductAddedMessage, message);
        }



        public async Task ShowAllLogs()
        {
            var datas = _backendReadRepository.GetAll(false);
            var result = await datas.Select(p => new
            {
                p.MessageTemplate,
                p.Message,
                p.EmailOrUserNameLogs
            })
                .ToListAsync();
            Console.WriteLine(result);
            await _hubContext.Clients.All.SendAsync("GetAllLogs", result);
        }


    }
}
