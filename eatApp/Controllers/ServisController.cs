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

        YemekAppEntities db = new YemekAppEntities();
        sonucModel sonuc = new sonucModel();

        #region Uye

     

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
            yeni.uyeAdmin = false;
            db.uye_Tablosu.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Eklendi";

            return sonuc;
        }

     
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


        [HttpGet]
        [Route("api/katyemekbyid/{katyemekId}")]
        public kategoriYemekModel KatYemekById(string katyemekId)
        {
            kategoriYemekModel kayit = db.Kategori_yemek.Where(s => s.katYemekId == katyemekId).Select(x => new kategoriYemekModel()
            {
                katYemekId = x.katYemekId,
                Kategori_yemek1 = x.Kategori_yemek1
                

            }).SingleOrDefault();
            return kayit;
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

        #region YemekKategori


        [HttpGet]
        [Route("api/yemekkategoriliste/{yemekKatId}")]
        public List<yemekKategoriModel> yemekKategoriListe(string yemekKatId)
        {
            List<yemekKategoriModel> liste = db.Yemek_kategori.Where(s => s.Yemek_id == yemekKatId).Select(x => new yemekKategoriModel()
            {
                yemekKategoriId = x.yemekKategoriId,
                Kategori_yemek_id = x.Kategori_yemek_id,
                Yemek_id = x.Yemek_id

            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.yemekBilgi = YemekById(kayit.Yemek_id);
                kayit.katYemekBilgi = KatYemekById(kayit.yemekKategoriId);

            }

            return liste;
        }



        [HttpGet]
        [Route("api/kategoriyemekliste/{katYemekId}")]
        public List<yemekKategoriModel> KategoriYemekListe(string katYemekId)
        {
            List<yemekKategoriModel> liste = db.Yemek_kategori.Where(s => s.Kategori_yemek_id == katYemekId).Select(x => new yemekKategoriModel()
            {
                yemekKategoriId = x.yemekKategoriId,
                Kategori_yemek_id = x.Kategori_yemek_id,
                Yemek_id = x.Yemek_id

            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.yemekBilgi = YemekById(kayit.Yemek_id);
                kayit.katYemekBilgi = KatYemekById(kayit.yemekKategoriId);

            }

            return liste;
        }


        [HttpPost]
        [Route("api/yemekkategoriekle")]
        public sonucModel YemekKategoriEkle(yemekKategoriModel model)
        {   

            Yemek_kategori yeni = new Yemek_kategori();
            yeni.yemekKategoriId = Guid.NewGuid().ToString();
            yeni.Yemek_id = model.Yemek_id;
            yeni.Kategori_yemek_id = model.Kategori_yemek_id;

            db.Yemek_kategori.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Yemek Kategoriye Eklendi";

            return sonuc;
        }


        [HttpDelete]
        [Route("api/yemekkategorisil/{yemekKatId}")]
        public sonucModel YemekKategoriSil(string yemekKatId)
        {

            Yemek_kategori kayit = db.Yemek_kategori.Where(s => s.yemekKategoriId == yemekKatId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı";
                return sonuc;
            }

            db.Yemek_kategori.Remove(kayit);
            db.SaveChanges();
            sonuc.mesaj = "Kategorideki Yemek Silindi";
            sonuc.islem = true;
            return sonuc;
        }

        #endregion

        #region Yemekler

        [HttpGet]
        [Route("api/yemekliste")]
        public List<yemeklerModel> YemekListe()
        {
            List<yemeklerModel> liste = db.Yemekler.Select(x => new yemeklerModel()
            {
                yemekId = x.yemekId,
                YemekAdi = x.YemekAdi,
                YemekUyeId = x.YemekUyeId,
                Tarif = x.Tarif
            }).ToList();
            return liste;
        }



        [HttpGet]
        [Route("api/yemekbyid/{yemekId}")]

        public yemeklerModel YemekById(string yemekId)
        {
            yemeklerModel kayit = db.Yemekler.Where(s => s.yemekId == yemekId).Select(x => new yemeklerModel()
            {

                yemekId = x.yemekId,
                YemekAdi = x.YemekAdi,
                YemekUyeId = x.YemekUyeId,
                Tarif = x.Tarif

            }).SingleOrDefault();

            return kayit;
        }

     

        [HttpPost]
        [Route("api/yemekekle")]
        public sonucModel YemekEkle(yemeklerModel model)
        {
            Yemekler yeni = new Yemekler();
            yeni.yemekId = Guid.NewGuid().ToString();
            yeni.YemekAdi = model.YemekAdi;
            yeni.YemekUyeId = model.YemekUyeId;
            yeni.Tarif = model.Tarif;
           
            db.Yemekler.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Tarif Eklendi";

            return sonuc;
        }



        [HttpPut]
        [Route("api/yemekduzenle")]

        public sonucModel YemekDuzenle(yemeklerModel model)
        {
            Yemekler kayit = db.Yemekler.Where(s => s.yemekId == model.yemekId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Tarif Bulunamadı";
                return sonuc;
            }

            kayit.Tarif = model.Tarif;
            kayit.YemekAdi = model.YemekAdi;
            

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Tarif Düzenlendi";

            return sonuc;
        }

        
        [HttpDelete]
        [Route("api/yemeksil/{yemekId}")]
        public sonucModel YemekSil(string yemekId)
        {

            Yemekler kayit = db.Yemekler.Where(s => s.yemekId == yemekId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Tarif Bulunamadı";
                return sonuc;
            }

            db.Yemekler.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Tarif Silindi";

            return sonuc;
        }

        #endregion

        #region kategori malzeme

        [HttpGet]
        [Route("api/katmalbyid/{katmalıd}")]

        public kategoriMalzemeModel KatMalById(string katmalıd)
        {
            kategoriMalzemeModel kayit = db.Kategori_malzeme.Where(s => s.katMalzemeId == katmalıd).Select(x => new kategoriMalzemeModel()
            {

                katMalzemeId = x.katMalzemeId,
                Kategori_malzeme1 = x.Kategori_malzeme1

            }).SingleOrDefault();

            return kayit;
        }

        [HttpGet]
        [Route("api/katmalliste")]
        public List<kategoriMalzemeModel> KatMalListe()
        {
            List<kategoriMalzemeModel> liste = db.Kategori_malzeme.Select(x => new kategoriMalzemeModel()
            {
                katMalzemeId = x.katMalzemeId,
                Kategori_malzeme1 = x.Kategori_malzeme1
            }).ToList();
            return liste;
        }

        [HttpPost]
        [Route("api/kategorimalzemeekle")]
        public sonucModel KategoriMalzemeEkle(kategoriMalzemeModel model)
        {

            if (db.Kategori_malzeme.Count(s => s.Kategori_malzeme1 == model.Kategori_malzeme1) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu malzeme kategorisi Zaten Kayıtlıdır!";
                return sonuc;
            }

            Kategori_malzeme yeni = new Kategori_malzeme();
            yeni.katMalzemeId = Guid.NewGuid().ToString();
            yeni.Kategori_malzeme1 = model.Kategori_malzeme1;


            db.Kategori_malzeme.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "malzeme kategorisine Eklendi";

            return sonuc;
        }

        [HttpDelete]
        [Route("api/kategorimalzemesil/{katmalıd}")]
        public sonucModel kategoriMalzemeSil(string katmalıd)
        {

            Kategori_malzeme kayit = db.Kategori_malzeme.Where(s => s.katMalzemeId == katmalıd).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Malzeme Kategorisi Bulunamadı";
                return sonuc;
            }

            db.Kategori_malzeme.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Malzeme Kategorisi Silindi";

            return sonuc;
        }


        # endregion

        #region Malzeme Kategori

        [HttpGet]
        [Route("api/malkatliste")]
        public List<malzemeKategoriModel> MalzemeKategoriListe()
        {
            List<malzemeKategoriModel> liste = db.Malzeme_kategori.Select(x => new malzemeKategoriModel()
            {
                MalzemeKategoriId = x.MalzemeKategoriId,
                Malzeme_id = x.Malzeme_id,
                Kategori_malzeme_id = x.Kategori_malzeme_id
            }).ToList();
            return liste;
        }

        [HttpPost]
        [Route("api/malkatekle")]
        public sonucModel MalzemeKategoriEkle(malzemeKategoriModel model)
        {

            if (db.Malzeme_kategori.Count(s => s.MalzemeKategoriId == model.MalzemeKategoriId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Malzeme Kategorisi Kayıtlıdır!";
                return sonuc;
            }

            Malzeme_kategori yeni = new Malzeme_kategori();
            yeni.MalzemeKategoriId = Guid.NewGuid().ToString();
            yeni.Malzeme_id = model.Malzeme_id;
            yeni.Kategori_malzeme_id = model.Kategori_malzeme_id;

            db.Malzeme_kategori.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Malzeme Kategorisi Eklendi";

            return sonuc;
        }

        [HttpPut]
        [Route("api/malkatduzenle")]

        public sonucModel MalzemeKategoriDuzenle(malzemeKategoriModel model)
        {
            Malzeme_kategori kayit = db.Malzeme_kategori.Where(s => s.MalzemeKategoriId == model.MalzemeKategoriId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Malzeme Kategorisi Bulunamadı";
                return sonuc;
            }

            kayit.Malzeme_id = model.Malzeme_id;
            kayit.Kategori_malzeme_id = model.Kategori_malzeme_id;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Malzeme Kategori Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/malkatsil/{malkatıd}")]
        public sonucModel MalzemeKategoriSil(string malkatıd)
        {

            Malzeme_kategori kayit = db.Malzeme_kategori.Where(s => s.MalzemeKategoriId == malkatıd).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Malzeme Kategorisi Bulunamadı";
                return sonuc;
            }

            db.Malzeme_kategori.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Malzeme Kategorisi Silindi";

            return sonuc;
        }

        #endregion

        #region Malzemeler

        [HttpGet]
        [Route("api/malzemeliste")]
        public List<malzemelerModel> MalzemeListe()
        {
            List<malzemelerModel> liste = db.Malzemeler.Select(x => new malzemelerModel()
            {
                malzemeId = x.malzemeId,
                Malzemeler1 = x.Malzemeler1,
            }).ToList();
            return liste;
        }

        [HttpPost]
        [Route("api/malzemeekle")]
        public sonucModel MalzemeEkle(malzemelerModel model)
        {

            if (db.Malzemeler.Count(s => s.Malzemeler1 == model.Malzemeler1) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu Malzeme Kayıtlıdır!";
                return sonuc;
            }

            Malzemeler yeni = new Malzemeler();
            yeni.malzemeId = Guid.NewGuid().ToString();
            yeni.Malzemeler1 = model.Malzemeler1;

            db.Malzemeler.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Malzeme Eklendi";

            return sonuc;
        }

        [HttpPut]
        [Route("api/malzemedüzenle")]

        public sonucModel MalzemeDüzenle(malzemelerModel model)
        {
            Malzemeler kayit = db.Malzemeler.Where(s => s.malzemeId == model.malzemeId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Malzeme Bulunamadı";
                return sonuc;
            }

            kayit.Malzemeler1 = model.Malzemeler1;

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Malzeme Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/malzemesil/{malzemeid}")]
        public sonucModel MalzemeSil(string malzemeid)
        {

            Malzemeler kayit = db.Malzemeler.Where(s => s.malzemeId == malzemeid).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Malzeme Bulunamadı";
                return sonuc;
            }

            db.Malzemeler.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Malzeme Silindi";

            return sonuc;
        }



        #endregion
    }
}


