using System;
namespace OnionArch.Application.RequestParamaters
{
    //Bu karşılayıcı bir nesne olacağı için bunu class olarak isimlendirmemize gerek yok aslında
    //Bu record yada struct olabilir.  Biz record olarak geliştirelim. (Maliyetimizi kısmak için)
    public class Pagination
	{
        //Pagination için bize toplam kaç sayfa istendiği ve toplam kaç adet verinin istenen sayfada listeleneceğini kurgulayalım Bunlara default değerler ekleyelim.

        //Şimdi Client tarafından buraya bu bilgiler gönderilmemiş olabilir böyle bir durumda hata ile karşılaşmamız için
            //Burada bulunan props değerlerine default olarak atamalar yapmamız gerekmekte .

        public int Page { get; set; } = 0;
        public int Size { get; set; } = 8;
    }
}

