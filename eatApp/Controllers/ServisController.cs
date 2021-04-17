using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using eatApp.Models;
using eatApp.ViewModel;

namespace eatApp.Controllers
{
    public class ServisController : ApiController
    {
        YemekAppEntities2 db = new YemekAppEntities2();
        sonucModel sonuc = new sonucModel();

        #region yemek
        [HttpGet]
        [Route("api/yemeklistele")]
        public List<yemeklerModel> YemekListele()
        {
            List<yemeklerModel> liste = db.Yemekler.Select(x => new yemeklerModel()
            {
                Id = x.Id,
                YemekAdi = x.YemekAdi,
                Tarif = x.Tarif,
            }).ToList();
            return liste;
        }

        //[HttpPost]
        //[Route("api/yemekekle")]
        //public sonucModel YemekEkle(urunBilgisiModel model)
        //{

        //    Urun_Bilgisi yeniUrun = new Urun_Bilgisi();
        //    yeniUrun.urun_Id = Guid.NewGuid().ToString();
        //    yeniUrun.urun_Admin_Bilgi = model.urun_Admin_Bilgi;
        //    yeniUrun.urun_Marka_Id = model.urun_Marka_Id;
        //    yeniUrun.urun_Kategori_Id = model.urun_Kategori_Id;
        //    yeniUrun.urun_Satis_Fiyat = model.urun_Satis_Fiyat;
        //    yeniUrun.urun_KDV = model.urun_KDV;
        //    yeniUrun.urun_Satılan = model.urun_Satılan;
        //    yeniUrun.urun_Stok = model.urun_Stok;
        //    yeniUrun.urun_İmage = model.urun_İmage;
        //    yeniUrun.urun_Gelis_Fiyat = model.urun_Gelis_Fiyat;
        //    yeniUrun.urun_Eklenme_Tarih = model.urun_Eklenme_Tarih;
        //    yeniUrun.urun_Adi = model.urun_Adi;
        //    db.Urun_Bilgisi.Add(yeniUrun);
        //    db.SaveChanges();
        //    sonuc.islem = true;
        //    sonuc.mesaj = "Yeni Üye Eklendi";
        //    return sonuc;
        //}

        #endregion
    }
}
