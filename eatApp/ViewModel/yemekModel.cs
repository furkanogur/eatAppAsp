using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eatApp.ViewModel
{
    public class yemekModel
    {
        public string yeme_Id { get; set; }
        public string yemek_Adi { get; set; }
        public string yemek_Malezeme_Id { get; set; }
        public string yemek_Kategori_Id { get; set; }
        public string yemek_Yapan_Id { get; set; }
        public string yemek_Tarif { get; set; }
    }
}