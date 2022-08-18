using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iakademi5_proje.Models;

namespace iakademi5_proje.Controllers
{
    public class AdminController : Controller
    {
        iakademi_projeEntities db = new iakademi_projeEntities();
        // GET: Admin
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(string Email,string Password)
        { 
            tbl_Users us = db.tbl_Users.FirstOrDefault(u => u.email == Email && u.parola == Password);

            if (us != null)
            {
                // dogru login ve sifre
                Session["email"] = us.email;
                Session["userID"] = us.userID;
                // web.config add key den geldi
                ViewBag.urungirisyetki = ConfigurationManager.AppSettings["urungirisyetki"].ToString();
                // Session["yetki"] = us.yetki;  // tbl_Users ta yetki kolonu
                return RedirectToAction("Anasayfa", "Admin");
            }
            return View(); //login.cshtml'ye git demek

        }

        public ActionResult Anasayfa()
        {
            if (Session["email"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }
            
        }

        public ActionResult Products()
        {
            if (Session["email"] != null)
            {
                List<tbl_Categories> catlist = db.tbl_Categories.ToList();
                ViewData["catlist"] = catlist.Select(a => new SelectListItem { Text = a.categoryname, Value = a.categoryID.ToString() }).ToList();

                List<tbl_Suppliers> marlist = db.tbl_Suppliers.ToList();
                ViewData["marlist"] = marlist.Select(a => new SelectListItem { Text = a.brandname, Value = a.supplierID.ToString() }).ToList();

                return View();
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }

        }

        [HttpPost]
        // public ActionResult Products(tbl_Products prd,HttpPostedFileBase fileuploader)
        // Birden fazla resim için (.cshtml de multiple) -> IEnumerable<HttpPostedFileBase> fileuploader
        // tek resim için HttpPostedFileBase fileuploader
        public ActionResult Products(tbl_Products prd, HttpPostedFileBase fileuploader)
        {
            prd.adddate = DateTime.Now;
            prd.tiklamasayisi = 0;
            prd.coksatanlar = 0;
            prd.benzer_urunler = 0;
            prd.aktif = true;
            db.tbl_Products.Add(prd);
            db.SaveChanges();

            int sonkayit = db.tbl_Products.Max(u => u.productID);

            //tek resim kaydı
            if (fileuploader != null)
            {
                string path = Path.Combine(Server.MapPath("~/Content/dosyalar"), Path.GetFileName(fileuploader.FileName));
                fileuploader.SaveAs(path);
                tbl_Images i = new tbl_Images();
                i.productID = sonkayit;
                i.resimyolu = fileuploader.FileName;
                i.aktif = true;
                db.tbl_Images.Add(i);
                db.SaveChanges();
            }

            //Birden fazla resim kaydı için
          /*  foreach (var item in fileuploader)
            {
                if (item != null && item.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/dosyalar"), Path.GetFileName(item.FileName));
                    item.SaveAs(path);
                    tbl_Images i = new tbl_Images();
                    i.productID = sonkayit;
                    i.resimyolu = item.FileName;
                    i.aktif = true;
                    db.tbl_Images.Add(i);
                    db.SaveChanges();
                    
                }
            } */


            return RedirectToAction("Products"); //HttpGet metodundan, .cshtml sayfasına gitsin çünkü .cshtml sayfası yüklenirken kategori ve marka dropdownlist içine veri gelmek zorunda
            

        }


        public ActionResult ProductsEdit()
        {
            if (Session["email"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }

        }

        public ActionResult ProductsList()
        {
            if (Session["email"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }

        }

        public ActionResult Categories()
        {
            if (Session["email"] != null)
            {
                //List<tbl_Categories> catlist = db.tbl_Categories.ToList();
                //ViewData["catlist"] = catlist.Select(a => new SelectListItem
                //{
                //    Text = a.categoryname,
                //    Value = a.categoryID.ToString()}).ToString();



                List<tbl_Categories> catlist = db.tbl_Categories.ToList();
                ViewData["catlist"] = catlist.Select(a => new SelectListItem { Text = a.categoryname, Value = a.categoryID.ToString() }).ToList();
                return View();
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }

        }

        [HttpPost]
        public ActionResult Categories(tbl_Categories cat)
        {
            //delete from * tbl_Categories
            //DBCC CHECKIDENT (tbl_Categories,reseed,0) kategori ıd si 1den başlıycak

            // int categoryID = Convert.ToInt32(Request.Form["sedat"]);

            /* List<tbl_Categories> catlist = db.tbl_Categories.ToList();
             ViewData["catlist"] = catlist.Select(a => new SelectListItem { Text = a.categoryname, Value = a.categoryID.ToString() }).ToList(); */
            // return View(""); //categories.cshtml'ye [HttpPost]'dan yani metottan gider. Dolayısıyla catliste ihtiyacı var.


            if (cat.categoryID > 0)
            {
                cat.parentID = cat.categoryID;
                cat.categoryID = 0;
            }
            else
            {
                cat.parentID = 0;
            }
            cat.aktif = true;

            db.tbl_Categories.Add(cat);
            db.SaveChanges();
            return RedirectToAction("Categories");  //şeklinde yazarsak, categories.cshtml ye [HttpGet] metodu üzerinden gider.
        }

        public ActionResult CategoriesEdit()
        {
            if (Session["email"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }

        }

        public ActionResult CategoriesList()
        {
            if (Session["email"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }

        }

        public ActionResult Suppliers()
        {
            if (Session["email"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }

        }

        [HttpPost]
        public ActionResult Suppliers(tbl_Suppliers sup)
        {
            if (Session["email"] != null)
            {
                sup.aktif = true;
                db.tbl_Suppliers.Add(sup);
                db.SaveChanges();
                return View();
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }

        }

        public ActionResult SuppliersEdit()
        {
            if (Session["email"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }

        }

        public ActionResult SuppliersList()
        {
            if (Session["email"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "Admin");
            }

        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Remove("email");
            return RedirectToAction("login", "Admin");
        }

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
    }
}