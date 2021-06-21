using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eatApp.ViewModel
{
    public class yemeklerModel
    {
        public string yemekId { get; set; }
        public string YemekUyeId { get; set; }
        public string YemekAdi { get; set; }
        public string Tarif { get; set; }
        public string yemekFoto { get; set; }

        // public List <yemekMalzemeModel> Malzemeler { get; set; }

        public uyeModel UyeBilgisi { get; set; }

        public List<yemekKategoriModel> YemekKategori { get; set; }
        public List<kategoriYemekModel> KategoriYemek { get; set; }
        public List<yemekMalzemeModel> YemekMalzeme { get; set; }


        public yemekKategoriModel YemekIdKategori { get; set; }
    
    }
}