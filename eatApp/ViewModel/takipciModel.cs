using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eatApp.ViewModel
{
    public class takipciModel
    {
        public string takipId { get; set; }
        public string takipEdenUyeId { get; set; }
        public string takipEdilenUyeId { get; set; }

        public uyeModel UyeBilgisi { get; set; }
        public List<yemeklerModel> YemeklerBilgisi { get; set; }
    //    public kategoriYemekModel KategoriYemek { get; set; }
      //  public yemekMalzemeModel YemekMalzeme { get; set; }
    }
}