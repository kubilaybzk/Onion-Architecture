using OnionArch.Application.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.CQRS_Globals.Pagination
{
    public  class GlobalPaginationResponse: GlobalResponseResult
    {
        public int TotalCount { get; set; }        //Toplam bulunan ürün 

        public int TotalPageSize { get; set; }  //Toplam oluşan sayfa 

        public int CurrentPage { get; set; }    //O anki sayfa 

        public bool HasNext { get; set; }        //Sonraki sayfa var mı ?

        public bool HasPrev { get; set; }       //Önceki sayfa var mı  ?

        public int PageSize { get; set; }       //Sayfada gözükecek item sayısı
    }
}
