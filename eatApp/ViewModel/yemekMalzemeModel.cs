using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eatApp.ViewModel
{
    public class yemekMalzemeModel
    {
        public string yemekMalzemeId { get; set; }
        public string Yemek_id { get; set; }
        public string Malzeme_id { get; set; }
        public string Miktar { get; set; }
        public string Birim { get; set; }
        public malzemelerModel YemekMalzeme { get; set; }


    }
}