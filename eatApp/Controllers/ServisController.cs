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

        YemekAppEntities4 db = new YemekAppEntities4();
        sonucModel sonuc = new sonucModel();

        #region Uye

        //UyeListele

        [HttpGet]
        [Route("api/uyeliste")]
        public List<uyeModel> UyeListe()
        {
            List<uyeModel> liste = db.uye_Tablosu.Select(x => new uyeModel()
            {
                uyeId = x.uyeId,
                uyeEmail = x.uyeEmail,
                uyeSifre = x.uyeSifre,
                uyeAdSoyad = x.uyeAdSoyad,
                uyeAdmin = x.uyeAdmin,
                uyeTelefon = x.uyeTelefon
            }).ToList();
            return liste;
        }

        //UyeById

        [HttpGet]
        [Route("api/uyebyid/{uyeId}")]

        public uyeModel UyeById(string uyeId)
        {
            uyeModel kayit = db.uye_Tablosu.Where(s => s.uyeId == uyeId).Select(x => new uyeModel()
            {

                uyeId = x.uyeId,
                uyeEmail = x.uyeEmail,
                uyeSifre = x.uyeSifre,
                uyeAdSoyad = x.uyeAdSoyad,
                uyeAdmin = x.uyeAdmin,
                uyeTelefon = x.uyeTelefon

            }).SingleOrDefault();

            return kayit;
        }

        //Uye EKle

        [HttpPost]
        [Route("api/uyeekle")]
        public sonucModel UyeEkle(uyeModel model)
        {

            if (db.uye_Tablosu.Count(s => s.uyeEmail == model.uyeEmail) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu Üye Zaten Kayıtlıdır!";
                return sonuc;
            }

            uye_Tablosu yeni = new uye_Tablosu();
            yeni.uyeId = Guid.NewGuid().ToString();
            yeni.uyeAdSoyad = model.uyeAdSoyad;
            yeni.uyeSifre = model.uyeSifre;
            yeni.uyeEmail = model.uyeEmail;
            yeni.uyeAdmin = model.uyeAdmin;
            db.uye_Tablosu.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Eklendi";

            return sonuc;
        }

        //Uye Duzenle

        [HttpPut]
        [Route("api/uyeduzenle")]

        public sonucModel UyeDuzenle(uyeModel model)
        {
            uye_Tablosu kayit = db.uye_Tablosu.Where(s => s.uyeId == model.uyeId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üye Bulunamadı";
                return sonuc;
            }

            kayit.uyeAdSoyad = model.uyeAdSoyad;
            kayit.uyeSifre = model.uyeSifre;
            kayit.uyeTelefon = model.uyeTelefon;

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Düzenlendi";

            return sonuc;
        }

        //Uye Sil
        [HttpDelete]
        [Route("api/uyesil/{uyeId}")]
        public sonucModel OgrenciSil(string uyeId)
        {

            uye_Tablosu kayit = db.uye_Tablosu.Where(s => s.uyeId == uyeId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üye Bulunamadı";
                return sonuc;
            }

            db.uye_Tablosu.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Silindi";

            return sonuc;
        }

        #endregion

        #region Favori

        //UyeById

        [HttpGet]
        [Route("api/favoribyid/{favoriId}")]

        public favoriModel FavoriById(string favoriId)
        {
            favoriModel kayit = db.favori_Tablo.Where(s => s.favoriId == favoriId).Select(x => new favoriModel()
            {

                favoriId = x.favoriId,
                favoriUyeId = x.favoriUyeId,
                favoriYemekId = x.favoriYemekId,
            }).SingleOrDefault();

            return kayit;
        }

        [HttpPost]
        [Route("api/favoriekle")]
        public sonucModel FavoriEkle(favoriModel model)
        {

            if (db.favori_Tablo.Count(s => s.favoriUyeId == model.favoriUyeId && s.favoriYemekId == model.favoriYemekId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu Tarif Zaten Favorilere Kayıtlıdır!";
                return sonuc;
            }

            favori_Tablo yeni = new favori_Tablo();
            yeni.favoriId = Guid.NewGuid().ToString();
            yeni.favoriUyeId = model.favoriUyeId;
            yeni.favoriYemekId = model.favoriYemekId;

            db.favori_Tablo.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Favorilere Eklendi";

            return sonuc;
        }

        [HttpDelete]
        [Route("api/favorisil/{favoriId}")]
        public sonucModel FavoriSil(string favoriId)
        {

            favori_Tablo kayit = db.favori_Tablo.Where(s => s.favoriId == favoriId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Favori Bulunamadı";
                return sonuc;
            }

            db.favori_Tablo.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Favorilerden Silindi";

            return sonuc;
        }

        #endregion

        #region KategoriYemek

        [HttpGet]
        [Route("api/katYemekliste")]
        public List<kategoriYemekModel> KatYemekListe()
        {
            List<kategoriYemekModel> liste = db.Kategori_yemek.Select(x => new kategoriYemekModel()
            {
                katYemekId = x.katYemekId,
                Kategori_yemek1 = x.Kategori_yemek1,   
            }).ToList();
            return liste;
        }

        [HttpPost]
        [Route("api/katyemekekle")]
        public sonucModel KatYemekEkle(kategoriYemekModel model)
        {

            if (db.Kategori_yemek.Count(s => s.Kategori_yemek1 == model.Kategori_yemek1) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu Yemek Kategorisi Kayıtlıdır!";
                return sonuc;
            }

            Kategori_yemek yeni = new Kategori_yemek();
            yeni.katYemekId = Guid.NewGuid().ToString();
            yeni.Kategori_yemek1 = model.Kategori_yemek1;
            
            db.Kategori_yemek.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Yemek Kategorisi Eklendi";

            return sonuc;
        }

        [HttpPut]
        [Route("api/katyemekduzenle")]

        public sonucModel KatYemekDuzenle(kategoriYemekModel model)
        {
            Kategori_yemek kayit = db.Kategori_yemek.Where(s => s.katYemekId == model.katYemekId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Yemek Kategorisi Bulunamadı";
                return sonuc;
            }

            kayit.Kategori_yemek1 = model.Kategori_yemek1;

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Yemek Kategori Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/katyemeksil/{katyemekId}")]
        public sonucModel KatYemekSil(string katyemekId)
        {

            Kategori_yemek kayit = db.Kategori_yemek.Where(s => s.katYemekId == katyemekId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Yemeğin Kategorisi Bulunamadı";
                return sonuc;
            }

            db.Kategori_yemek.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Yemek Kategorisi Silindi";

            return sonuc;
        }

        #endregion



    }
}


