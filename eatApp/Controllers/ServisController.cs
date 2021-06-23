using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        #region Uye

     
        //üye listele
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
                uyeTelefon = x.uyeTelefon,
                uyeFoto = x.uyeFoto
            }).ToList();
            return liste;
        }

    //üyeyi id ye göre listele
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
                uyeTelefon = x.uyeTelefon,
                uyeFoto=x.uyeFoto
                

            }).SingleOrDefault();

            return kayit;
        }


        //üye ekleme
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
            yeni.uyeTelefon = model.uyeTelefon;
            yeni.uyeAdmin = false;
            db.uye_Tablosu.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Eklendi";

            return sonuc;
        }

     //üye düzenleme
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

        //üye silme
        [HttpDelete]
        [Route("api/uyesil/{uyeId}")]
        public sonucModel UyeSil(string uyeId)
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


        [HttpPost]
        [Route("api/uyefotoguncelle")]
        public sonucModel UyeFotoGuncelle(uyeFoto model)
        {
            uye_Tablosu uye = db.uye_Tablosu.Where(s => s.uyeId == model.uyeId).SingleOrDefault();


            if (uye == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            if (uye.uyeFoto != "profil.jpg")
            {
                string yol = System.Web.Hosting.HostingEnvironment.MapPath("~/Dosyalar/" + uye.uyeFoto);
                if (File.Exists(yol))
                {
                    File.Delete(yol);
                }
            }

            string data = model.fotoData;
            string base64 = data.Substring(data.IndexOf(',') + 1);
            base64 = base64.Trim('\0');
            byte[] imgbytes = Convert.FromBase64String(base64);
            string dosyaAdi = uye.uyeId + model.fotoUzanti.Replace("image/", ".");
            using (var ms = new MemoryStream(imgbytes, 0, imgbytes.Length))
            {
                Image img = Image.FromStream(ms, true);
                img.Save(System.Web.Hosting.HostingEnvironment.MapPath("~/Dosyalar/" + dosyaAdi));

            }
            uye.uyeFoto = dosyaAdi;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Fotoğraf Güncellendi";


            return sonuc;
        }


        #endregion

        #region Takipci
        //takipçileri listele
        [HttpGet]
        [Route("api/takipciliste")]
        public List<takipciModel> TakipciListe()
        {
            List<takipciModel> liste = db.takipci_Tablosu.Select(x => new takipciModel()
            {
                takipId = x.takipId,
                takipEdenUyeId = x.takipEdenUyeId,
                takipEdilenUyeId = x.takipEdilenUyeId,
           
            }).ToList();
            return liste;
        }
        //takip edilen kişi
        [HttpGet]
        [Route("api/takipedilenbyid/{takipedeniId}")]

        public uyeModel TakipedenById(string takipedenId)
        {
            uyeModel kayit = db.uye_Tablosu.Where(s => s.uyeId == takipedenId).Select(x => new uyeModel()
            {

                uyeId = x.uyeId,          
                uyeAdSoyad = x.uyeAdSoyad,
              

            }).SingleOrDefault();

            return kayit;
        }
        //takip eden kişi
        [HttpGet]
        [Route("api/takipedenbyid/{takipedilenId}")]

        public uyeModel TakipciById(string takipedilenId)
        {
            uyeModel kayit = db.uye_Tablosu.Where(s => s.uyeId == takipedilenId).Select(x => new uyeModel()
            {

                uyeId = x.uyeId,
                uyeAdSoyad = x.uyeAdSoyad,


            }).SingleOrDefault();

            return kayit;
        }

        //takipçi ekleme
        [HttpPost]
        [Route("api/takipciekle")]
        public sonucModel TakipciEkle(takipciModel model)
        {

            if (db.takipci_Tablosu.Count(s => s.takipEdenUyeId == model.takipEdenUyeId && s.takipEdilenUyeId ==model.takipEdilenUyeId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu Kişi Zaten Takip Ediliyor!";
                return sonuc;
            }

            takipci_Tablosu yeni = new takipci_Tablosu();
            yeni.takipId = Guid.NewGuid().ToString();
            yeni.takipEdenUyeId = model.takipEdenUyeId;
            yeni.takipEdilenUyeId = model.takipEdilenUyeId;
            db.takipci_Tablosu.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Takipci Eklendi";

            return sonuc;
        }
        //takipten çıkma
        [HttpDelete]
        [Route("api/takipcisil/{takipciId}")]
        public sonucModel TakipciSil(string takipciId)
        {

            takipci_Tablosu kayit = db.takipci_Tablosu.Where(s => s.takipId == takipciId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Takipci Bulunamadı";
                return sonuc;
            }

            db.takipci_Tablosu.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Takipden Çıkıldı";

            return sonuc;
        }

        #endregion

        #region Favori
        // favori id ye göre listeleme 
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


       


        // favori uyeid ye göre listeleme 
        [HttpGet]
        [Route("api/favoribyuyeid/{uyeıd}")]

        public List<favoriModel> FavoriByUyeId(string uyeıd)
        {
            List<favoriModel> kayit = db.favori_Tablo.Where(s => s.favoriUyeId == uyeıd).Select(x => new favoriModel()
            {

                favoriId = x.favoriId,
                favoriUyeId = x.favoriUyeId,
                favoriYemekId = x.favoriYemekId,
            }).ToList();
            foreach (var item in kayit)
            {
                item.yemekBilgisi = YemekFavById(item.favoriYemekId);
            }
            return kayit;
        }

        //favoriye ekleme
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
        // fovoriyi kaldırma
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

        // fovoriyi kaldırma ama yemekid
        [HttpDelete]
        [Route("api/favorisilyemekid/{yemekid}")]
        public sonucModel FavoriSilYemekId(string yemekid)
        {
            for (int i = 0; yemekid.Count() > 0; i++)
            {
            favori_Tablo kayit = db.favori_Tablo.Where(s => s.favoriYemekId == yemekid).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Favori Bulunamadı";
                return sonuc;
            }
           
            db.favori_Tablo.Remove(kayit);
            db.SaveChanges();
            }
            sonuc.islem = true;
            sonuc.mesaj = "Favorilerden Silindi";

            return sonuc;
        }
        #endregion

        //algoritmik yemek listele
        [HttpGet]
        [Route("api/algoritmikyemekliste/{takipedenid}")]

        public List<takipciModel> AlgoritmikYemekListe(string takipedenid)
        {
            List<takipciModel> liste = db.takipci_Tablosu.Where(s => s.takipEdenUyeId == takipedenid).Select(x => new takipciModel()
            {

             takipId=x.takipId,
             takipEdenUyeId=x.takipEdenUyeId,
             takipEdilenUyeId=x.takipEdilenUyeId

            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.UyeBilgisi = UyeById(kayit.takipEdilenUyeId);
                kayit.YemeklerBilgisi = YemekByIdUye(kayit.takipEdilenUyeId);
            }

            return liste;
        }


        #region KategoriYemek
        //yemeğin kategorileri(örn yemeğin tuzlu kategorisinde olması)
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

        //spesifik olarak kategori çağırma örn tatlı tuzlu vs
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




        // yemek kategorisi ekle örn tuzlu vs
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
        // yemeğin kategorisini düzenleme öneğin tuzlu yerine tuzlular
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
        // örneğin tuzlu kategorisini sil
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

        //Kategori listele
        [HttpGet]
        [Route("api/yemekkatliste")]
        public List<yemekKategoriModel> YemekKategoriListe()
        {
            List<yemekKategoriModel> liste = db.Yemek_kategori.Select(x => new yemekKategoriModel()
            {
                yemekKategoriId = x.yemekKategoriId,
                Kategori_yemek_id = x.Kategori_yemek_id,
                Yemek_id = x.Yemek_id
            }).ToList();
            return liste;
        }

        //bir yemek birden fazla kategoride olabilir yemeğin kategorilerini listeleme id ile
        //yemekdeki kategorileri listeliyor
        [HttpGet]
        [Route("api/yemekkategoriliste/{yemekkatıd}")]
        public List<yemekKategoriModel> yemekKategoriListebyid(string yemekkatıd)
        {
            List<yemekKategoriModel> liste = db.Yemek_kategori.Where(s => s.Yemek_id == yemekkatıd).Select(x => new yemekKategoriModel()
            {
                yemekKategoriId = x.yemekKategoriId,
                Kategori_yemek_id = x.Kategori_yemek_id,
                Yemek_id = x.Yemek_id
            }).ToList();
            
            //foreach lazım bura çünkü yemepin kategorisinin adı yok aynı şekil alttakine de tam tersini yazıns
            foreach (var kayit in liste)
            {
                kayit.KategoriYemekBilgisi = KatYemekById(kayit.Kategori_yemek_id);
            };
            return liste;
        }


        //kategorideki yemekleri listeliyor
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
            return liste;
        }


        [HttpPost]
        [Route("api/yemekkategoriekle")]
        public sonucModel YemekKategoriEkle(yemekKategoriModel model)
        {


            if (db.Yemek_kategori.Count(s => s.Yemek_id == model.Yemek_id && s.Kategori_yemek_id == model.Kategori_yemek_id) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu Yemek Bu Kategoriye Zaten Kayıtlıdır!";
                return sonuc;
            }

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
        [Route("api/yemekkategorisil/{yemekıd}")]
        public sonucModel YemekKategoriSilyemekid(string yemekıd)
        {             
            for (int i = 0; yemekıd.Count() > 0; i++)
            {
                Yemek_kategori kayit = db.Yemek_kategori.Where(s => s.Yemek_id == yemekıd).FirstOrDefault();

                if (kayit == null)
                {
                    sonuc.islem = false;
                    sonuc.mesaj = "Kayıt Bulunamadı";
                    return sonuc;
                }

                db.Yemek_kategori.Remove(kayit);
                db.SaveChanges();
            }
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
                Tarif = x.Tarif,
                yemekFoto = x.yemekFoto,
               
            }).ToList();
            foreach (var kayit in liste)
            {
                kayit.UyeBilgisi = UyeById(kayit.YemekUyeId);
                kayit.YemekKategori = yemekKategoriListebyid(kayit.yemekId);
                kayit.YemekMalzeme = YemekMalYemekId(kayit.yemekId);
               
            }
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
                Tarif = x.Tarif,
                yemekFoto = x.yemekFoto,
               
            }).FirstOrDefault();

           

            return kayit;
        }






        [HttpGet]
        [Route("api/yemekfavbyid/{yemekId}")]

        public yemeklerModel YemekFavById(string yemekId)
        {
            yemeklerModel liste = db.Yemekler.Where(s => s.yemekId == yemekId).Select(x => new yemeklerModel()
            {

                yemekId = x.yemekId,
                YemekAdi = x.YemekAdi,
                YemekUyeId = x.YemekUyeId,
                Tarif = x.Tarif,
                yemekFoto = x.yemekFoto,

            }).FirstOrDefault();
            return liste;
        }





        [HttpGet]
        [Route("api/yemekbyiduye/{uyeid}")]

        public List<yemeklerModel> YemekByIdUye(string uyeid)
        {
            List<yemeklerModel> kayit = db.Yemekler.Where(s => s.YemekUyeId == uyeid).Select(x => new yemeklerModel()
            {

                yemekId = x.yemekId,
                YemekAdi = x.YemekAdi,
                YemekUyeId = x.YemekUyeId,
                Tarif = x.Tarif,
                yemekFoto = x.yemekFoto,
              }).ToList();
            
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
            sonuc.id = yeni.yemekId;
            return sonuc;
        }

        [HttpPost]
        [Route("api/yemekfotoguncelle")]
        public sonucModel YemekFotoGuncelle(YemekFoto model)
        {
            Yemekler yemek = db.Yemekler.Where(s => s.yemekId == model.yemekId).SingleOrDefault();


            if (yemek == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            if (yemek.yemekFoto != "yemek.jpg")
            {
                string yol = System.Web.Hosting.HostingEnvironment.MapPath("~/Dosyalar/" + yemek.yemekFoto);
                if (File.Exists(yol))
                {
                    File.Delete(yol);
                }
            }

            string data = model.fotoData;
            string base64 = data.Substring(data.IndexOf(',') + 1);
            base64 = base64.Trim('\0');
            byte[] imgbytes = Convert.FromBase64String(base64);
            string dosyaAdi = yemek.yemekId + model.fotoUzanti.Replace("image/", ".");
            using (var ms = new MemoryStream(imgbytes, 0, imgbytes.Length))
            {
                Image img = Image.FromStream(ms, true);
                img.Save(System.Web.Hosting.HostingEnvironment.MapPath("~/Dosyalar/" + dosyaAdi));

            }
            yemek.yemekFoto = dosyaAdi;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Fotoğraf Güncellendi";


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
            YemekMalzemeSilyemekid(yemekId);
            FavoriSilYemekId(yemekId);
            YemekKategoriSilyemekid(yemekId);

            db.Yemekler.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Tarif Silindi";

            return sonuc;
        }

        #endregion

        #region yemekMalzeme

        [HttpGet]
        [Route("api/yemekmalliste")]
        public List<yemekMalzemeModel> YemekMalzemeListe()
        {
            List<yemekMalzemeModel> liste = db.Yemek_malzeme.Select(x => new yemekMalzemeModel()
            {
                yemekMalzemeId = x.yemekMalzemeId,
                Yemek_id = x.Yemek_id,
                Malzeme_id = x.Malzeme_id,
                Miktar = x.Miktar,
                Birim = x.Birim  
            }).ToList();
            return liste;
        }


        [HttpGet]
        [Route("api/yemekmalbyid/{yemekmald}")]

        public yemekMalzemeModel YemekMalById(string yemekmald)
        {
            yemekMalzemeModel kayit = db.Yemek_malzeme.Where(s => s.yemekMalzemeId == yemekmald).Select(x => new yemekMalzemeModel()
            {

                yemekMalzemeId = x.yemekMalzemeId,
                Yemek_id = x.Yemek_id,
                Malzeme_id = x.Malzeme_id,
                Miktar = x.Miktar,
                Birim = x.Birim

            }).SingleOrDefault();

            return kayit;
        }
        //yemek malzeme yemek ıd ile arama
        [HttpGet]
        [Route("api/yemekmalyemekid/{yemekıd}")]

        public List<yemekMalzemeModel> YemekMalYemekId(string yemekıd)
        {
            List<yemekMalzemeModel> kayit = db.Yemek_malzeme.Where(s => s.Yemek_id == yemekıd).Select(x => new yemekMalzemeModel()
            {

                yemekMalzemeId = x.yemekMalzemeId,
                Yemek_id = x.Yemek_id,
                Malzeme_id = x.Malzeme_id,
                Miktar = x.Miktar,
                Birim = x.Birim

            }).ToList();
            foreach (var kay in kayit)
            {
                kay.YemekMalzeme = MalzemeById(kay.Malzeme_id);
            }

            return kayit;
        }



        [HttpPost]
        [Route("api/yemekmalekle")]
        public sonucModel YemekMalEkle(yemekMalzemeModel model)
        {

            if (db.Yemek_malzeme.Count(s => s.Yemek_id == model.Yemek_id && s.Malzeme_id == model.Malzeme_id) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu Malzeme Zaten Kayıtlıdır!";

                return sonuc;
            }



            Yemek_malzeme yeni = new Yemek_malzeme();
            yeni.yemekMalzemeId = Guid.NewGuid().ToString();
            yeni.Yemek_id = model.Yemek_id;
            yeni.Malzeme_id = model.Malzeme_id;
            yeni.Birim = model.Birim;
            yeni.Miktar = model.Miktar;
 
            db.Yemek_malzeme.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Malzeme Tarife Eklendi";

            return sonuc;
        }


        [HttpPut]
        [Route("api/yemekmalduzenle")]

        public sonucModel YemMalDuzenle(yemekMalzemeModel model)
        {
            Yemek_malzeme kayit = db.Yemek_malzeme.Where(s => s.yemekMalzemeId == model.yemekMalzemeId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Tarifin Malzemesi Bulunamadı";
                return sonuc;
            }

            kayit.Birim = model.Birim;
            kayit.Miktar = model.Miktar;
            kayit.Malzeme_id = model.Malzeme_id;

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Tarifin Malzemesi Düzenlendi";

            return sonuc;
        }


        [HttpDelete]
        [Route("api/yemekmalzemesil/{yemekid}")]
        public sonucModel YemekMalzemeSilyemekid(string yemekid)
        {
           // List<Yemek_malzeme> sayi = db.Yemek_malzeme.Where(s => s.Yemek_id == yemekid).ToList();
            
            for (int i = 0; yemekid.Count() > 0; i++)
            {
                Yemek_malzeme kayit = db.Yemek_malzeme.Where(s => s.Yemek_id == yemekid).FirstOrDefault();

                if (kayit == null)
                {
                    sonuc.islem = false;
                    sonuc.mesaj = "Tarif Malzemesi Bulunamadı";
                    return sonuc;
                }

                db.Yemek_malzeme.Remove(kayit);
                db.SaveChanges();
            
         
            }
            sonuc.islem = true;
            sonuc.mesaj = "Tarif Malzemesi Silindi";
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

        [HttpPut]
        [Route("api/Katmalduzenle")]

        public sonucModel KatMalzDuzenle(kategoriMalzemeModel model)
        {
            Kategori_malzeme kayit = db.Kategori_malzeme.Where(s => s.katMalzemeId == model.katMalzemeId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Malzeme Bulunamadı";
                return sonuc;
            }

            kayit.Kategori_malzeme1 = model.Kategori_malzeme1;

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Malzeme Düzenlendi";
            return sonuc;
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

            foreach (var kayit in liste)
            {
                kayit.malzemeBilgi = MalzemeById(kayit.Malzeme_id);
                kayit.katMalBilgi = KatMalById(kayit.Kategori_malzeme_id);

            }

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

        [HttpGet]
        [Route("api/malzemebyid/{malId}")]
        public malzemelerModel MalzemeById(string malId)
        {
            malzemelerModel kayıt = db.Malzemeler.Where(s=>s.malzemeId == malId).Select(x => new malzemelerModel()
            {
                malzemeId = x.malzemeId,
                Malzemeler1 = x.Malzemeler1,
            }).SingleOrDefault();
            return kayıt;
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
        [Route("api/malzemeduzenle")]

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

            if (db.Yemek_malzeme.Count(s => s.Malzeme_id == malzemeid) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu Malzeme Yemekte Kullanılmaktadır!";
                return sonuc;
            }

            if (db.Malzeme_kategori.Count(s => s.Malzeme_id == malzemeid) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu Malzeme Bir Kategoriye Ait!";
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


