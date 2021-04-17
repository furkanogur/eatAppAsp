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
        YemekAppEntities1 db = new YemekAppEntities1();
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

        [HttpPost]
        [Route("api/yemekekle")]
        public sonucModel YemekEkle(yemeklerModel kullanıcıdangelen)
        {

            Yemekler yeniyemek = new Yemekler();
            yeniyemek.Id = kullanıcıdangelen.Id;
            yeniyemek.YemekAdi = kullanıcıdangelen.YemekAdi;
            yeniyemek.Tarif = kullanıcıdangelen.Tarif;

            db.Yemekler.Add(yeniyemek);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Yeni yemek Eklendi";
            return sonuc;
        }

        #endregion
    }
}
