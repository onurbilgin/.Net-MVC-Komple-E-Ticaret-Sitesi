using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace iakademi5_proje.Models
{
    public class ClassUser
    {
        public string email { get; set; }
        public string parola { get; set; }
        public string isimsoyisim { get; set; }
        public string telefon { get; set; }
        public string faturaadresi { get; set; }

        //ctor
        public ClassUser()
        {

        }

        public ClassUser(int userID)
        {
            try
            {
                using (iakademi_projeEntities db = new iakademi_projeEntities())
                {
                    var list = db.tbl_Users.Where(u => u.userID == userID).ToList();
                    foreach (var item in list)
                    {
                        userID = item.userID;
                        email = item.email;
                        isimsoyisim = item.isimsoyisim;
                        telefon = item.telefon;
                        faturaadresi = item.faturaadresi;
                    }
                }
            }
            catch (Exception)
            {                
            }
        }

        //order sayfasında (kredi kartı onay sayfasında üyenin fatura adresi vb. diğer bilgilerinin özetinin göstereceğim)
        public List<ClassUser> uyegetir(int userID)
        {
            try
            {
                List<ClassUser> uy = new List<ClassUser>();
                using (iakademi_projeEntities db = new iakademi_projeEntities())
                {
                    var list = db.tbl_Users.Where(p => p.userID == userID).ToList();
                    foreach (var item in list)
                    {
                        ClassUser u = new ClassUser();
                        u.faturaadresi = item.faturaadresi;
                        u.isimsoyisim = item.isimsoyisim;
                        u.telefon = item.telefon;
                        u.email = item.email;
                        uy.Add(u);
                    }
                    return uy;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void uyeekle(tbl_Users u, string adminmi)
        {
            using (iakademi_projeEntities db = new iakademi_projeEntities())
            {
                tbl_Users uyler = new tbl_Users();
                uyler.email = u.email;
                uyler.parola = MD5sifrele(u.parola);
                uyler.faturaadresi = u.faturaadresi;
                uyler.isimsoyisim = u.isimsoyisim;
                uyler.telefon = u.telefon;
                if (adminmi == "true")
                {
                    uyler.adminmi = true;
                }
                else
                {
                    uyler.adminmi = false;
                }
                uyler.aktif = true;
                db.tbl_Users.Add(uyler);
                db.SaveChanges();

            }
        }

        public static string MD5sifrele(string metin)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            //girilen veriyi byte dizisine dönüştürelim ve hash hesaplamasını yapalım.
            byte[] btr = Encoding.UTF8.GetBytes(metin);
            btr = md5.ComputeHash(btr);

            // byte'ları biriktirmek için yeni StringBuilder ve string oluşturalım.
            StringBuilder sb = new StringBuilder();

            // hash yapılmış her bir byte'ı dizi içinden alalım ve her birini hexadecimal string olarak formatlayalım
            foreach (byte item in btr)
            {
                sb.Append(item.ToString("x2").ToLower());
            }
            // hexadecimal (16'lık sistem) string'inin geri döndürüyorum.
            return sb.ToString();
        }

        public static void mesajekle(string isim, string email, string konu, string mesaj)
        {
            using (iakademi_projeEntities db = new iakademi_projeEntities())
            {
                tbl_Mesaj m = new tbl_Mesaj();
                m.isim = isim;
                m.email = email;
                m.konu = konu;
                m.message = mesaj;

                db.tbl_Mesaj.Add(m);
                db.SaveChanges();
            
            }
        }

    }
}