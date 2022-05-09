using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PC_08
{
    internal class Utils
    {
        public static string conn = @"Data Source=DESKTOP-HUJGH1E\SQLEXPRESS;Initial Catalog=PC_08;Integrated Security=True";
    }

    public class Encrypt
    {
        public static string enc(string data)
        {
            using(SHA256 s = new SHA256Managed())
            {
                return Convert.ToBase64String(s.ComputeHash(Encoding.UTF8.GetBytes(data)));
            }
        }

        public static byte[] encode(Image img)
        {
            ImageConverter converter = new ImageConverter();
            byte[] image = (byte[])converter.ConvertTo(img, typeof(byte[]));
            return image;
        }
    }

    public class Model
    {
        public static string Name { get; set; }
        public static int id { set; get; }
        public static int jobID { set; get; }
    }

    public class Command
    {
        public static DataTable getdata(SqlCommand command)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);
            return dt;
        }
    }
}
