//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eatApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Begenilenler_Tablosu
    {
        public string begenilenler_Id { get; set; }
        public string begenilenler_Yemek_Id { get; set; }
        public string begenilenler_Uye_Id { get; set; }
    
        public virtual Uye_Tablosu Uye_Tablosu { get; set; }
        public virtual Yemek_Tablosu Yemek_Tablosu { get; set; }
    }
}