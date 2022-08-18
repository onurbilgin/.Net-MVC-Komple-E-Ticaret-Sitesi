using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iakademi5_proje.Models;
using PagedList.Mvc;
using PagedList;
using System.Collections.Specialized;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.Collections;

namespace iakademi5_proje.Controllers
{
    public class HomeController : BaseController
    {
        iakademi_projeEntities db = new iakademi_projeEntities();
        AnasayfaModel model = new AnasayfaModel();
        ClassUrunler u = new ClassUrunler();
        Classsiparisler s = new Classsiparisler();

        // GET: Home
        public ActionResult Index()
        {
            model.tek_ozel_urun = u.tek_ozel_urun_getir();
            model.slider_urunler = u.slider_urunler_getir();
            model.ozel_urunler = u.ozel_urunler_getir();
            model.yildizli_urunler = u.yildizli_urunler_getir();
            model.enyeni_urunler = u.enyeni_urunler_getir();
            model.indirimli_urunler = u.indirimli_urunler();
            model.onecikanlar = u.onecikanlar_getir();
            model.coksatanlar = u.coksatanlar_getir();
            return View(model);
        }

        
        public ActionResult detaylar(int id)
        {
            // Öne çıkanlar kolonu değerini arttıralım
            ClassUrunler.onecikanlar_kolonunu_arttir(id);
            model.detayli_urun = u.detayli_urun_getir(id);
            model.benzer_urunler = u.benzer_urunler(Convert.ToInt32(model.detayli_urun.benzer_urunler));
            model.buna_bakanlar = u.buna_bakanlar(Convert.ToInt32(model.detayli_urun.categoryID));
            model.yorumlar = u.yorumlari_getir(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult detaylar()
        {
            int productID = Convert.ToInt32(Request.Form["productID"].ToString());
            string yorum = Request.Form["txtyorum"].ToString();

            ClassUrunler.onecikanlar_kolonunu_arttir(productID);
            model.detayli_urun = u.detayli_urun_getir(productID);
            model.benzer_urunler = u.benzer_urunler(Convert.ToInt32(model.detayli_urun.benzer_urunler));
            model.buna_bakanlar = u.buna_bakanlar(Convert.ToInt32(model.detayli_urun.categoryID));
            ClassUrunler.yorumekle(productID, yorum);
            model.yorumlar = u.yorumlari_getir(productID);
            return View(model);
        }

        public ActionResult sepeteekle(int id)
        {
            // Request -> browser'ın serverdan talebi
            // Response -> Server'ın browser'a cevabı



            // Öne çıkanlar kolonu değerini arttıralım
            ClassUrunler.onecikanlar_kolonunu_arttir(id);


            // sepetim
            // 10=1 & 20=1 & 30=1
            HttpCookie cSetCookie;
            string sepet = "";
            //cookie'yi browser'da sepete ekliyoruz.
            s.productID = id;
            s.adet = 1;
            cSetCookie = Request.Cookies.Get("sepetim");
            // cSetCookie boşsa ilk kayıtta null kontrolü yapıyorum.
            if (cSetCookie == null)
            {
                cSetCookie = new HttpCookie("sepetim");
                s.sepet = sepet;
            }
            else // sepet doludur o zaman verileri okuyup sepet değişkenine atıyoruz
            {
                s.sepet = cSetCookie.Value;
            }

            // aynı ürün sepette yoksa ekleyeceğim.
            if (s.sepete_ekle(id.ToString()) == true)
            {
                cSetCookie.Values.Add(id.ToString(), "1");
                cSetCookie.Expires = DateTime.Now.AddMinutes(60 * 24 * 7); // 1 haftalık cookie... dakika,saat,gün
                Response.Cookies.Add(cSetCookie);
                Session["Mesaj"] = "Ürün Sepetinize Eklendi.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["Mesaj"] = "Ürün Sepetinizde Zaten Var.";
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult EnYeniUrunler()
        {
            model.enyeni_urunler = u.enyeni_urunler_getir_hepsi();
            return View(model);
        }

            
        public ActionResult _partial_EnYeniUrunler(string sonrakisayfa)
        {
            int sayfano = Convert.ToInt32(sonrakisayfa);
            model.enyeni_urunler = u.enyeni_urunler_getir_hepsi_loadmore(sayfano);
            return View(model);
        }

        public ActionResult OzelUrunler()
        {
            model.ozel_urunler = u.ozel_urunler_getir_hepsi();
            return View(model);
        }

        public ActionResult _partial_OzelUrunler(string sonrakisayfa)
        {
            int sayfano = Convert.ToInt32(sonrakisayfa);
            model.ozel_urunler = u.ozel_urunler_getir_hepsi_loadmore(sayfano);
            return View(model);
        }

        public ActionResult Encokindirim()
        {
            ViewData["header"] = "Encokindirim";
            model.indirimli_urunler = u.indirimli_urunler_getir_hepsi();
            return View(model);
        }

        public ActionResult _partial_Encokindirim(string sonrakisayfa)
        {
            int sayfano = Convert.ToInt32(sonrakisayfa);
            model.indirimli_urunler = u.indirimli_urunler_getir_hepsi_loadmore(sayfano);
            return View(model);
        }

        //int? -> null olabilir
        public ActionResult onecikanlar(int? page)
        {
            var urunler = db.vw_aktif_urunler.OrderByDescending(p => p.tiklamasayisi).ToList();

            //turner if 
            var pagenumber = page ?? 1; // page null ise page = 1 demek

            var sayfalidata = urunler.ToPagedList(pagenumber, 4);
            ViewBag.onecikanlar = sayfalidata;
            return View();
        }

        public ActionResult coksatanlar(int? page)
        {
            var urunler = db.vw_aktif_urunler.OrderByDescending(p => p.coksatanlar).ToList();

            //turner if 
            var pagenumber = page ?? 1; // page null ise page = 1 demek

            var sayfalidata = urunler.ToPagedList(pagenumber, 4);
            ViewBag.coksatanlar = sayfalidata;
            return View();
        }

        public ActionResult markalar(int id, int? page)
        {
            var urunler = db.vw_aktif_urunler.Where(p => p.supplierID == id).OrderBy(p => p.productname).ToList();

            //turner if 
            var pagenumber = page ?? 1; // page null ise page = 1 demek

            var sayfalidata = urunler.ToPagedList(pagenumber, 4); // sayfa 4 adet gösteriliyor.
            ViewBag.markalar = sayfalidata;
            return View();
        }

        public ActionResult kategoriler(int id, int? page)
        {
            var urunler = db.vw_aktif_urunler.Where(p => p.categoryID == id).OrderBy(p => p.productname).ToList();

            //turner if 
            var pagenumber = page ?? 1; // page null ise page = 1 demek

            var sayfalidata = urunler.ToPagedList(pagenumber, 4); // sayfa 4 adet gösteriliyor.
            ViewBag.kategoriler = sayfalidata;
            return View();
        }

        //header'da sepetim tıklanınca
        public ActionResult cart()
        {
            Classsiparisler s = new Classsiparisler();
            //sepetten silmek için geldiysem burası çalışacak sileceğim ürün icin scid gönderdim. Ürünün sil ikonuna tıkladım
            if (Request.QueryString["scid"] != null)
            {
                int scid = Convert.ToInt32(Request.QueryString["scid"]);
                HttpCookie cSetCookie = Request.Cookies.Get("sepetim");
                string sepetcookie = cSetCookie.Value;
                s.sepet = sepetcookie;
                s.sepetten_sil(scid.ToString());
                HttpCookie cKuki = new HttpCookie("sepetim", s.sepet);
                Response.Cookies.Add(cKuki);
                cKuki.Expires = DateTime.Now.AddMinutes(60 * 24 * 7); // 1 haftalık kuki
                Session["Mesaj"] = "Ürün sepetinizden silindi";
                List<Classsiparisler> sepet = s.sepetiyazdir();
                ViewBag.Sepetim = sepet;
                ViewBag.sepet_tablo_detay = sepet;
            }
            else
            {
                // header'da sepetim tıklanınca
                HttpCookie setCookie = Request.Cookies.Get("sepetim");
                List<Classsiparisler> sepet;
                // cookie sepeti boşsa
                if (setCookie == null)
                {
                    setCookie = new HttpCookie("sepetim");
                    s.sepet = "";
                    sepet = s.sepetiyazdir();
                    ViewBag.Sepetim = sepet;
                    ViewBag.sepet_tablo_detay = sepet;
                }
                else // cookie sepeti doluysa
                {
                    s.sepet = setCookie.Value.ToString();
                    sepet = s.sepetiyazdir();
                    ViewBag.Sepetim = sepet;
                    ViewBag.sepet_tablo_detay = sepet;
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult order()
        {
            int geciciuserID = Convert.ToInt32(Session["userID"]);
            ClassUser m = new ClassUser(geciciuserID);
            ViewBag.UyeBilgi = m.uyegetir(geciciuserID);
            if (geciciuserID == 0)
            {
                return RedirectToAction("hesabim", "Home");
            }
            return View();
        }

        string returnvalue = "";

        [HttpPost]
        public ActionResult order(FormCollection frm)
        {
            string creditCardNumber = Request.Form["creditCardNumber"].ToString();
            string creditCardMonth = Request.Form["creditCardMonth"].ToString();
            string creditCardYear = Request.Form["creditCardYear"].ToString();
            string creditCardCVC = Request.Form["creditCardCVC"].ToString();

            if (Request.Form["txttckimlikno"].ToString() != "")
            {
                string txttckimlikno = Request.Form["txttckimlikno"].ToString();
            }
            if (Request.Form["txtvergino"].ToString() != "")
            {
                string txtvergino = Request.Form["txtvergino"].ToString();
            }

            // iyzico , payu

            /* Burası şu an banka kontrolünü yapamayacağı için direk RedirectToAction("backref","Home"); yazdım */
            /*
            NameValueCollection data = new NameValueCollection();
            string backref = "https://www.sedattefci.com/backref";
            data.Add("BACK_REF", backref);
            data.Add("CC_CVV", creditCardCVC);
            data.Add("CC_NUMBER", creditCardNumber);
            data.Add("EXP_MONTH", creditCardMonth);
            data.Add("EXP_YEAR","20"+ creditCardYear);

            var deger = "";
            foreach (var val in data)
            {
                var value = val as string;
                var byteCount = Encoding.UTF8.GetByteCount(data.Get(value));
                deger += byteCount + data.Get(value);
            }

            var signatureKey = "payu'nun verdiği SECRET_KEY buraya gelecek";
            var hash = HashWithSignature(deger, signatureKey);

            data.Add("ORDER_HASH", hash);
            var x = POSTFormPayu("https://secure.payu.com.tr/order/alu/v3", data);

            //sanal kart ise
            if (x.Contains("<STATUS>SUCCESS</STATUS>") && x.Contains("<RETURN_CODE>3DS_ENROLLED</RETURN_CODE>"))
            {
                returnvalue = "Başarılı";
            }
            else // gerçek kredi kartı
            {
                if (x.Contains("<STATUS>SUCCESS</STATUS>") && x.Contains("<RETURN_CODE>AUTHORIZED</RETURN_CODE>"))
                {
                    returnvalue = "Başarılı";
                }
                else
                {
                    //kredi kartı bilgilerinde hata var
                    if (x.Contains("INVALID_PAYMENT_INFO"))
                    {
                        returnvalue = "Geçersiz kart bilgileri! Kontrol Ediniz.";
                    }
                    else
                    {
                        returnvalue = "Hata İşlemi tekrar deneyiniz";
                    }
                }
            }
            */


            return RedirectToAction("backref", "Home");
        }


        public static string HashWithSignature(string hashString, string signature)
        {
            var binaryHash = new HMACMD5(Encoding.UTF8.GetBytes(signature)).ComputeHash(Encoding.UTF8.GetBytes(hashString));

            var hash = BitConverter.ToString(binaryHash).Replace("-", string.Empty).ToLowerInvariant();
            return hash;
        }

        public class StringString
        {
            public string Text1 { get; set; }
            public string Text2 { get; set; }
        }

        public static string POSTFormPayu(string url, NameValueCollection data)
        {
            var result = new List<StringString>();
            var webClient = new WebClient();

            try
            {
                string request = System.Text.Encoding.UTF8.GetString(webClient.UploadValues(url, data)).Trim();
                return request;
            }
            catch (Exception)
            {
            }
            return "";
        }

       public static string siparisno = "";

        [HttpGet]
        public ActionResult backref()
        {
            //basarılı
            string siparis_kayit_durum = siparisi_kaydet();
            if (siparis_kayit_durum != "")
            {
                siparisno = siparis_kayit_durum;
                return RedirectToAction("basarili", "Home");
                
            }
            else
            {
                return RedirectToAction("hata", "Home");
            }
        }

        public string siparisi_kaydet()
        {
            HttpCookie cSetCookie;
            cSetCookie = Request.Cookies.Get("sepetim");
            if (cSetCookie != null)
            {
                Classsiparisler s = new Classsiparisler();
                s.sepet = cSetCookie.Value.ToString();
                string sonuc = s.cookie_sepetini_siparis_tablosuna_yaz(Convert.ToInt32(Session["userID"]));
                if (sonuc != "")
                {
                    //siparis kaydedildi
                    s.sepet = "";
                    HttpCookie cKuki = new HttpCookie("sepetim", s.sepet);
                    Response.Cookies.Add(cKuki);
                    cKuki.Expires = DateTime.Now.AddDays(1);
                    return sonuc;
                }
                else
                {
                    //sipariş kaydedilemedi
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public ActionResult basarili()
        {
            ViewBag.siparisno = siparisno;
            return View();
        }

        public ActionResult hata()
        {
            return View();
        }

        /* Bankanın sonuçları döneceği metod HttpPost olmak zorunda
        [HttpPost]
        public ActionResult backref()
        {
            
            return View();
        }
        */

        [HttpGet]
        public ActionResult uyeol()
        {
            return View();
        }

        [HttpPost]
        public ActionResult uyeol(tbl_Users u)
        {
            string adminmi = Request.Form["adminmi"].ToString();
            ClassUser.uyeekle(u,adminmi);
            return View();
        }

        [HttpGet]
        public ActionResult hesabim()
        {
            return View();
        }

        [HttpPost]
        public ActionResult hesabim(string email,string parola)
        {
            string md5parola = ClassUser.MD5sifrele(parola);
            tbl_Users usr = db.tbl_Users.FirstOrDefault(u => u.email == email && u.parola == md5parola);

            if (usr != null)
            {
                Session["email"] = usr.email;
                Session["userID"] = usr.userID;
                return RedirectToAction("Index", "Home");
            }
            Session["mesaj"] = "Giriş bilgileriniz yanlış.";
            return View();
        }

        public ActionResult siparislerim()
        {
            if (Session["userID"] != null)
            {
                List<vw_siparislerim> list = s.siparislerim(Convert.ToInt32(Session["userID"]));
                return View(list);
            }
            else
            {
                return RedirectToAction("hesabim", "Home");
            }
        }


        public ActionResult detayliarama()
        {
            return View();
        }

        ArrayList mr = new ArrayList();

        [HttpPost]
        public ActionResult dproducts(string kategoriler, string[] markalar, string stoktavarmi, string amount)
        {
            // int kat =Convert.ToInt32(Request.Form["kategoriler"].ToString());
            int categoryID = Convert.ToInt32(kategoriler);

            //int stok = Convert.ToInt32(Request.Form["stoktavarmi"].ToString());
            int stock = Convert.ToInt32(stoktavarmi);
            string stok = "";
            if (stock == 1)
            {
                stok = "stock> 0";
            }
            else
            {
                stok = "stock>= 0";
            }

            //string amt = Request.Form["amount"].ToString();
            string amounts = Request.Form["amount"].Replace("$", "").Replace(" ", "");
            string[] fiyatdizi = amounts.Split('-');
            string baslangicpara = fiyatdizi[0]; //28
            string bitispara = fiyatdizi[1]; //452

            //markalar
            if (markalar != null && markalar.Length > 0)
            {
                foreach (var item in markalar)
                {
                    mr.Add(item);
                }
            }

            string markaID = "";
            for (int i = 0; i < mr.Count; i++)
            {
                if (i != mr.Count - 1) // en son kayda kadar burada döner
                {
                    markaID += "supplierID =" + mr[i].ToString() + " or ";
                }
                else
                {
                    // en son seçilen marka için buraya kesin gelecek
                    markaID += "supplierID = " + mr[i].ToString();
                }
            }

            string all = "select * from tbl_Products where " + stok + "and(categoryID = " + categoryID + ") and(price >= " + baslangicpara + " and price < " + bitispara + ")" +
            " and(" + markaID + ") order by price";

            ViewBag.Urunlers = s.urunler_getir_detayli_sorgu(all);

            return View();
        }

        public ActionResult formdan_veri_okuma_yontemleri(string adi)
        {
            // .cshtml sayfasında gelen veriyi 1. yakalama yöntemi
            return View();
        }

        public ActionResult formdan_veri_okuma_yontemleri2()
        {
            // .cshtml sayfasında gelen veriyi 2. yakalama yöntemi
            string adi = Request.Form["adi"].ToString();
            return View();
                
        }

        // menu.cshtml'den arama kutucugu ajax burayı cagırıyor
        public ActionResult urunlerstring(string id)
        {
            id = id.ToUpper(new System.Globalization.CultureInfo("tr-TR", false));
            List<view_arama> ulist = ClassUrunler.arama_getir(id);
            return Json(ulist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult logout()
        {
            Session.Abandon();
            Session.Remove("email");
            Session.Remove("userID");
            return RedirectToAction("index", "Home");

        }

        public ActionResult hakkimizda()
        {
            return View();
        }

        public ActionResult iletisim()
        {
            return View();
        }

        public ActionResult mesaj()
        {
            return View();
        }

        [HttpPost]
        public ActionResult mesaj(string deneme)
        {
            string isim = Request.Form["txtisim"].ToString();
            string email = Request.Form["txtemail"].ToString();
            string konu = Request.Form["txtkonu"].ToString();
            string mesaj = Request.Form["txtmesaj"].ToString();

            ClassUser.mesajekle(isim, email, konu, mesaj);

            return View();
        }

    }
}