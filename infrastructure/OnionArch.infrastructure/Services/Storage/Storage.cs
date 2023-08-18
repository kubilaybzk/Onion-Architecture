using System;
using OnionArch.infrastructure.Operations;

namespace OnionArch.infrastructure.Services.Storage
{
	public class Storage
    {
        //Burası bizim base classımız olacak burada ortak olacak bazı alt davranışları taşımasını isteyeceğiz.
        //Mesela burada doyanın isminin yeniden değiştirilmesi gibi gibi .

        //Bunu neden interface içinde tanımlamadık sorusuna cevabımız şöyle olacak.
        /*Burada bir algoritmamız var ve heryerde onu kullanıyoruz yani örnek olarak AWS için yada Azure için
            Yada Local işlemler için bu değişmeyecek.
                Algoritma tamamen aynı olacak bundan dolayı.
    */



        //Sadece bu sınıftan türeyen sınıflar için kullanacak bir yöntem .


        /*
         Burada delagate denen bir yöntem kullanacağız,
                İleri seviye bir yöntem
                    C# programlama dilinde tanımlanan ve metot olarak adlandırılan işlevlerin bellek adresini tutmak için kullanılan yapıya delegate veya temsilci denir.
                            FrontEnd alanında bulunan useRef yada props olarak düşünebiliriz.
                    Bizim Global anlamda IStorage altında tanımladığımız değerler arasında HasFile olarak adlandırdığımız bir method mevcuttu.
                        Burada bu delagate yöntemini kullanarak
                    Sadece Local olarak geliştirdiğimiz FileRenameAsync fonksiyonunu burada Recursive olarak oluşturduğumuz koşulu burada  yeniden düzenleyeceğiz.
                            
                            
         */


        protected delegate bool HasFile(string FileName, string PathOrContainerName);

        // protected async Task<string> FileRenameAsync(string PathOrContainerName, string dosyaAdi, bool ilk = true)
        protected async Task<string> FileRenameAsync(string PathOrContainerName, string dosyaAdi, HasFile hasFileMethod,  bool ilk = true)
        {
            string yeniDosyaAdi = await Task.Run<string>(async () =>
            {
                string uzanti = Path.GetExtension(dosyaAdi);
                string yeniDosyaAdi = string.Empty;

                if (ilk)
                {
                    // İlk adımda, dosya adını belirli kurallara göre değiştir.
                    string eskiAd = Path.GetFileNameWithoutExtension(dosyaAdi);
                    yeniDosyaAdi = $"{NameOperation.CharacterRegulatory(eskiAd)}{uzanti}";
                    Console.WriteLine("İlk dosya adı düzenlemesi.");
                }
                else
                {
                    // Sonraki adımlarda, mevcut dosya adlarına göre dosya adı değişiklikleri yap.
                    yeniDosyaAdi = dosyaAdi;
                    int indexNo1 = yeniDosyaAdi.IndexOf("-");

                    if (indexNo1 == -1)
                    {
                        // Eğer tire bulunamazsa, dosya adına "-2" eklenir.
                        yeniDosyaAdi = $"{Path.GetFileNameWithoutExtension(yeniDosyaAdi)}-2{uzanti}";
                    }
                    else
                    {
                        int sonIndex = 0;
                        while (true)
                        {
                            sonIndex = indexNo1;
                            indexNo1 = yeniDosyaAdi.IndexOf("-", indexNo1 + 1);

                            if (indexNo1 == -1)
                            {
                                indexNo1 = sonIndex;
                                break;
                            }
                        }

                        int indexNo2 = yeniDosyaAdi.IndexOf(".");
                        string dosyaNo = yeniDosyaAdi.Substring(indexNo1 + 1, indexNo2 - indexNo1 - 1);

                        if (int.TryParse(dosyaNo, out int _dosyaNo))
                        {
                            // Dosya adındaki sayıyı artır (örneğin, -2'yi -3 yap).
                            _dosyaNo++;
                            yeniDosyaAdi = yeniDosyaAdi.Remove(indexNo1 + 1, indexNo2 - indexNo1 - 1)
                                                .Insert(indexNo1 + 1, _dosyaNo.ToString());
                        }
                        else
                        {
                            // Sayı çıkarılamazsa, dosya adına "-2" eklenir.
                            yeniDosyaAdi = $"{Path.GetFileNameWithoutExtension(yeniDosyaAdi)}-2{uzanti}";
                        }
                    }
                }
                // if (File.Exists($"{yol}\\{yeniDosyaAdi}"))
                if (hasFileMethod(yeniDosyaAdi, PathOrContainerName))
                {
                    // Yeni adıyla bir dosya zaten varsa, işlemi değiştirilmiş bir ad ile tekrar çağır.
                    return await FileRenameAsync(PathOrContainerName, yeniDosyaAdi,hasFileMethod, false);
                }
                else
                {
                    // Yeni ad uygunsa, son adı döndür.
                    return yeniDosyaAdi;
                }
            });

            return yeniDosyaAdi;
        }




    }
}

