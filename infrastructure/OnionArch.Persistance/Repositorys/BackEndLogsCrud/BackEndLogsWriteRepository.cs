using OnionArch.Application.Repositories.BackEndLogsCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Persistance.Repositorys.BackEndLogsCrud
{
    public class BackEndLogsWriteRepository : WriteRepository<BackEndLogs>, IBackEndLogsWriteRepository
    {
        //Şimdi burada sadece IBackEndLogsWriteRepository uygularsak bütün içerikleri implement eder ve tekrardan oluşturmamız gerekir,
        //ama biz burada oluşturduğumuz ReadRepositoy içinde bunu zaten yaptın buna gerek yok  o zaman bunu şöyle yapamazmıyız
        //Tüm bu düzenlemeleri sen git readRepository içinde yap.
        //Aynı zamanda  IBackEndLogsWriteRepository senin soyut nesnen olsun.

        public BackEndLogsWriteRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}
