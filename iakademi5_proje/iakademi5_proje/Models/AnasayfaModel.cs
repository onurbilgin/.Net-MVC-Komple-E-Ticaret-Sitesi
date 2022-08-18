using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iakademi5_proje.Models
{
    public class AnasayfaModel
    {

        public vw_aktif_urunler tek_ozel_urun { get; set; }
        public List<vw_aktif_urunler> slider_urunler { get; set; }
        public List<vw_aktif_urunler> ozel_urunler { get; set; }
        public List<vw_aktif_urunler> yildizli_urunler { get; set; }
        public List<vw_aktif_urunler> enyeni_urunler { get; set; }
        public List<vw_aktif_urunler> indirimli_urunler { get; set; }
        public List<vw_aktif_urunler> onecikanlar { get; set; }
        public List<vw_aktif_urunler> coksatanlar { get; set; }
        public vw_aktif_urunler detayli_urun { get; set; }
        public List<vw_aktif_urunler> benzer_urunler { get; set; }
        public List<vw_aktif_urunler> buna_bakanlar { get; set; }
        public List<tbl_Comments> yorumlar { get; set; }
    }


}