using OnionArch.Domain.Entities.Common;
using OnionArch.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Entities
{
    public class Basket:BaseEntity
    {
        //Her bir basket tek bir user'a bağlı olacak değil mi ?
        public AppUser User { get; set; }
        
        public string UserId { get; set; } // User'ı temsilen bir ForeignKey

        public ICollection<BasketItem> BasketItems { get; set; }

        public Order Order { get; set; }


    }
}
