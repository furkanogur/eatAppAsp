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
        YemekAppEntities2 db = new YemekAppEntities2();
        sonucModel sonuc = new sonucModel();
    }
}
