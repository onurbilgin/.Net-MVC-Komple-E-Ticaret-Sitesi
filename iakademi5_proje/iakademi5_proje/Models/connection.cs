using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iakademi5_proje.Models
{
    public class connection
    {
        public static SqlConnection baglanti
        {
            get
            {
                SqlConnection sqlcon = new SqlConnection("Server=BILGIN\\SQLEXPRESS;trusted_connection=true;Database=iakademi_proje;");
                return sqlcon;
            }
        }

    }
}