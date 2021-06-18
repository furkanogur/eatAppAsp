using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eatApp.ViewModel
{
    public class yemekKategoriModel
    {
        public string yemekKategoriId { get; set; }
        public string Yemek_id { get; set; }
        public string Kategori_yemek_id { get; set; }
        public kategoriYemekModel KategoriYemekBilgisi { get; set; }
    }
}