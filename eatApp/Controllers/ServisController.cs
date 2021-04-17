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
        eatAppEntities db = new eatAppEntities();
        sonucModel sonuc = new sonucModel();

        #region yemek
        [HttpGet]
        [Route("api/yemeklistele")]
        public List<yemekModel> YemekListele()
        {
            List<yemekModel> liste = db.Yemek_Tablosu.Select(x => new yemekModel()
            {
                yeme_Id = x.yeme_Id,
                yemek_Adi = x.yemek_Adi,
                yemek_Kategori_Id = x.yemek_Kategori_Id,
                yemek_Malezeme_Id = x.yemek_Malezeme_Id,
                yemek_Yapan_Id = x.yemek_Yapan_Id,
                yemek_Tarif = x.yemek_Tarif

            }).ToList();
            return liste;
        }

        [HttpPost]
        [Route("api/yemekekle")]
        public sonucModel YemekEkle(yemekModel model)
        {

            Yemek_Tablosu yeniyemek = new Yemek_Tablosu();
            yeniyemek.yeme_Id = Guid.NewGuid().ToString();
            yeniyemek.yemek_Adi = model.yemek_Adi;
            yeniyemek.yemek_Kategori_Id = model.yemek_Kategori_Id;
            yeniyemek.yemek_Malezeme_Id = model.yemek_Malezeme_Id;
            yeniyemek.yemek_Tarif = model.yemek_Tarif;
            yeniyemek.yemek_Yapan_Id = model.yemek_Yapan_Id;


            db.Yemek_Tablosu.Add(yeniyemek);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Yeni yemek Eklendi";
            return sonuc;
        }

        #endregion

        [HttpPost]
        [Route("api/uyeekle")]
        public sonucModel UyeEkle(uyeModel model)
        {
            if (db.Uye_Tablosu.Count(s => s.uye_E_Mail == model.uye_E_Mail) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıtlı kullanıcı tekrar kayıt edilemez!";
                return sonuc;
            }
            Uye_Tablosu yeniUye = new Uye_Tablosu();
            yeniUye.uye_Id = Guid.NewGuid().ToString();
            yeniUye.uye_Adı_Soyado = model.uye_Adı_Soyado;
            yeniUye.uye_E_Mail = model.uye_E_Mail;
            yeniUye.uye_Sifre = model.uye_Sifre;
            
            db.Uye_Tablosu.Add(yeniUye);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Yeni Üye Eklendi";
            return sonuc;
        }

        [HttpGet]
        [Route("api/uyelistele")]
        public List<uyeModel> UyeListele()
        {
            List<uyeModel> liste = db.Uye_Tablosu.Select(x => new uyeModel()
            {
                uye_Id = x.uye_Id,
                uye_Adı_Soyado = x.uye_Adı_Soyado,
                uye_E_Mail = x.uye_E_Mail,
                uye_Sifre = x.uye_Sifre
            }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/uyebyid/{uyeId}")]
        public uyeModel UyeById(string uyeId)
        {
            uyeModel kayit = db.Uye_Tablosu.Where(s => s.uye_Id == uyeId).Select(x => new uyeModel()
            {
                uye_Id = x.uye_Id,
                uye_Adı_Soyado = x.uye_Adı_Soyado,
                uye_E_Mail = x.uye_E_Mail,
                uye_Sifre = x.uye_Sifre
            }).SingleOrDefault();
            return kayit;
        }
        [HttpDelete]
        [Route("api/yemeksil/{yemekıd}")]
        public sonucModel YemekSil(string yemekıd)
        {

            Yemek_Tablosu yemeksil = db.Yemek_Tablosu.Where(s => s.yeme_Id == yemekıd).SingleOrDefault();
            if (yemeksil == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "yemek bulunamadığı için silinemedi";
                return sonuc;
            }
            db.Yemek_Tablosu.Remove(yemeksil);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "yemek silindi";
            return sonuc;
        }
    }
}
