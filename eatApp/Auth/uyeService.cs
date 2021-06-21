using eatApp.Models;
using eatApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eatApp.Auth
{
    public class uyeService
    {
        YemekAppEntities db = new YemekAppEntities();
        public uyeModel UyeOturumAc(string kadi, string parola)
        {
            uyeModel uye = db.uye_Tablosu.Where(s => s.uyeEmail == kadi && s.uyeSifre == parola).Select(x => new uyeModel()
            {
                uyeId = x.uyeId,
                uyeAdSoyad = x.uyeAdSoyad,
                uyeEmail = x.uyeEmail,
                uyeSifre = x.uyeSifre,
                uyeTelefon = x.uyeTelefon,
                uyeAdmin = x.uyeAdmin,
                uyeFoto =x.uyeFoto,


            }).SingleOrDefault();
            return uye;
        }
    }
}