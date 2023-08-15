using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Services;
using OnionArch.infrastructure.Operations;

namespace OnionArch.infrastructure.Services
{
    public class FileService : IFileService
    {
        readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }




        private async Task<string> FileRenameAsync(string yol, string dosyaAdi, bool ilk = true)
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

                if (File.Exists($"{yol}\\{yeniDosyaAdi}"))
                {
                    // Yeni adıyla bir dosya zaten varsa, işlemi değiştirilmiş bir ad ile tekrar çağır.
                    return await FileRenameAsync(yol, yeniDosyaAdi, false);
                }
                else
                {
                    // Yeni ad uygunsa, son adı döndür.
                    return yeniDosyaAdi;
                }
            });

            return yeniDosyaAdi;
        }






        public async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                //Gelen dosya için bir filestream işlemi ger
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                //Gelen dosyayı kopyalama işlemini gerçekleştiriyoruz.
                await file.CopyToAsync(fileStream);
                //Kopyalama işemi sonrası temizleme işlemi yapıyoruzç
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                //todo log!
                throw ex;
            }
        }





        public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
        {
            //Burada iki farklı path'i bileştirme işlemi yapıyruz.
            // wwwroot özel bir dosya sistemi bundan dolayı burada göresellerimizi depolamak istiyoruz.
            //Burada şimdi wwwroot'un pat bilgisini alıp içinde nasıl bir mantıkla bir dosyalama sistemi olacak onu düzenleyelim.

            //Path bizim dışarıdan aldığımız dizin.

            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);

            //Yani gelen dosyalar wwwroot/resource/gelen_path_bilgisi altında olsun demek istedik üst satırda.

            //path var mı yok mu kontrol ediyoruz.
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);




            List<(string fileName, string path)> datas = new();
            List<bool> results = new();


            foreach (IFormFile file in files)

            {
                //Şimdi burada Solid prensipinden yararlanacağız.
                //SRP yani single respon. prensciple
                //Bu ne anlama geliyordu bir sınıf içinde bulunan bir method tek bir sorunluluğa sahip olsun demekti değil mi ?
                //Şimdi burada bu yapının hem veri tabanı ile ilgilenmesi hemde dosya yüklemesi bu prensipe aykırı değil mi ?
                //Yani dosyanın ismini değiştirme dosyanın veri tabanına yazma işlemleri burada bu prensip açısından  uygun bir şey değil.
                //Bundan dolayı burada sadece dosyayı kayıt ettirme işlemi yapacağız.

                string fileNewName = await FileRenameAsync(uploadPath, file.FileName);
                //Burada File'ın yeni bir isme sahip olmasını sağlıyoruz.

                string CopyFilePath = $"{uploadPath}\\{fileNewName}";

                bool result = await CopyFileAsync(CopyFilePath, file);

                datas.Add((fileNewName, $"{uploadPath}\\{fileNewName}"));
                results.Add(result);


            }

            if (results.TrueForAll(r => r.Equals(true)))
                return datas;

            return null;
        }

        
    }
}
