using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iakademi5_proje.Models
{
    public class Classsiparisler
    {
        public int productID { get; set; }
        public int adet { get; set; }
        public string sepet { get; set; }
        public decimal sepettutar { get; set; }
        public int kalem { get; set; } //çeşit
        public int sepetmiktar { get; set; }
        public decimal price { get; set; }
        public string productname { get; set; }
        public int kdv { get; set; }
        public string resimyolu { get; set; }
        public int discount { get; set; }


        iakademi_projeEntities db = new iakademi_projeEntities();
        


        // sepetim
        // 10=1 & 20=1 & 30=1

        //sepete ürün ekliyorum
        public bool sepete_ekle(string scid)
        {
            bool varmi = true;
            string[] sepetdizi = sepet.Split('&'); // Ürünleri birbirinden ayırıyorum.
            for (int i = 0; i < sepetdizi.Length; i++)
            {
                string[] sepetdizi2 = sepetdizi[i].Split('='); // Ürün ile adet bilgisini ayırıyorum
                // sepetdizi2[0] -> productID 
                if (sepetdizi2[0] == scid)
                {
                    //aynı ürün var
                    varmi = false;
                    return varmi;
                }
            }
            return varmi;

        }


        // _header'da sağ üstte sepetim üstüne gelince bilgileri yazdırmak için
        public List<Classsiparisler> sepetiyazdir()
        {
            sepettutar = 0;
            kalem = 0;

            List<Classsiparisler> sip = new List<Classsiparisler>();
            string[] sepetdizi = sepet.Split('&');
            if (sepetdizi[0] != "")
            {
                sepetmiktar = sepetdizi.Length - 1;
                for (int i = 0; i < sepetdizi.Length; i++)
                {
                    string[] sepetdizi2 = sepetdizi[i].Split('=');
                    int sepetid = Convert.ToInt32(sepetdizi2[0]);
                    if (sepetdizi2[0] != "")
                    {
                        //ürün bilgilerini veritabanından getirelim.                     
                        tbl_Products urn = db.tbl_Products.FirstOrDefault(u => u.productID == sepetid);
                        Classsiparisler s = new Classsiparisler();
                        s.price = Convert.ToDecimal(urn.price);
                        s.productname = urn.productname;
                        s.productID = urn.productID;
                        s.kdv = Convert.ToInt32(urn.kdv);
                        s.adet = Convert.ToInt32(sepetdizi2[1]); // sepetdizi2[1] -> adet
                        tbl_Images img = db.tbl_Images.FirstOrDefault(im => im.productID == urn.productID);
                        s.resimyolu = img.resimyolu;
                        sip.Add(s);
                    }
                } // for bitis
                
            } // if bitis
            return sip;
        }

        // sepetim sayfasında bir ürünü silmek istediğimizde bu metod çalışacak
        //10=1&20=1&
        public void sepetten_sil(string scid)
        {
            string[] sepetdizi = sepet.Split('&');
            string yenisepet = "";

            int miktar = 0;
            int count = 1;

            for (int i = 0; i < sepetdizi.Length; i++)
            {
                string[] sepetdizi2 = sepetdizi[i].Split('=');

                if (count == 1)
                {
                    if (sepetdizi2[0] != scid)
                    {
                        miktar = Convert.ToInt32(sepetdizi2[1]);
                        yenisepet += sepetdizi2[0] + "=" + miktar;
                        count++;
                    }
                }
                else
                {
                    if (sepetdizi2[0] != scid)
                    {
                        miktar = Convert.ToInt32(sepetdizi2[1]);
                        yenisepet += "&" + sepetdizi2[0] + "=" + miktar;
                        count++;
                    }
                }
            }// for bitis
            sepet = yenisepet;

        }

        public string cookie_sepetini_siparis_tablosuna_yaz(int ID)
        {
            string kayit_durum = "";
            try
            {
                using (iakademi_projeEntities db = new iakademi_projeEntities())
                {
                    string[] sepetdizi = sepet.Split('&');
                    string ortak_siparis_no = DateTime.Now.ToString().Replace(".", "").Replace(":", "").Replace(" ", "");
                    DateTime OrderDate = DateTime.Now;
                    for (int i = 0; i < sepetdizi.Length; i++)
                    {
                        string[] sepetdizi2 = sepetdizi[i].Split('=');
                        tbl_Orders ords = new tbl_Orders();
                        ords.userID = ID;
                        // ords.OrderDate = DateTime.Now;
                        ords.OrderDate = OrderDate;
                        ords.orderGroupGUID = ortak_siparis_no;
                        ords.invoiceAddress = db.tbl_Users.FirstOrDefault(a => a.userID == ID).faturaadresi;
                        ords.ProductID = Convert.ToInt32(sepetdizi2[0]);
                        ords.quantity = Convert.ToInt32(sepetdizi2[1]);
                        ords.aktif = true;
                        db.tbl_Orders.Add(ords);
                        db.SaveChanges();
                        kayit_durum = ortak_siparis_no; 
                    }
                    return kayit_durum;
                    
                }
            }
            catch (Exception)
            {
                return "";               
            }
        }

        public List<vw_siparislerim> siparislerim(int userID)
        {
            List<vw_siparislerim> list = db.vw_siparislerim.Where(o => o.userID == userID).ToList();
            return list;
        }

        public List<Classsiparisler> urunler_getir_detayli_sorgu(string hepsi)
        {
            List<Classsiparisler> prd = new List<Classsiparisler>();
            SqlConnection sqlcon = connection.baglanti;
            SqlCommand sqlcmd = new SqlCommand(hepsi,sqlcon);
            sqlcon.Open();
            SqlDataReader sdr = sqlcmd.ExecuteReader();
            while (sdr.Read())
            {
                Classsiparisler p = new Classsiparisler();
                p.productID = Convert.ToInt32(sdr["productID"]);
                p.productname = sdr["productname"].ToString();
                p.price = Convert.ToDecimal(sdr["price"]);
                p.discount = Convert.ToInt32(sdr["discount"]);
                tbl_Images img = db.tbl_Images.FirstOrDefault(u => u.productID == p.productID);
                p.resimyolu = img.resimyolu;
                prd.Add(p);
            }
            sqlcon.Close();
            return prd;
        }
    }
}