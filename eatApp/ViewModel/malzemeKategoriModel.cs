using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eatApp.ViewModel
{
    public class malzemeKategoriModel
    {
        public string MalzemeKategoriId { get; set; }
        public string Malzeme_id { get; set; }
        public string Kategori_malzeme_id { get; set; }
        public malzemelerModel malzemeBilgi { get; set; }
        public kategoriMalzemeModel katMalBilgi { get; set; }


    }
}