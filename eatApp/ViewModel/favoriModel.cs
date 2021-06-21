using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eatApp.ViewModel
{
    public class favoriModel
    {
        public string favoriId { get; set; }
        public string favoriYemekId { get; set; }
        public string favoriUyeId { get; set; }

        public yemeklerModel yemekBilgisi { get; set; }
     

    }
}