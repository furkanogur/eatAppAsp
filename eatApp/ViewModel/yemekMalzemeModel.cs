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
        public int Miktar { get; set; }
        public int Birim_id { get; set; }
    }
}