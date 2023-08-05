using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.Odbc;

namespace WebApplication1.Pages.Produs
{
    [Authorize]
    public class ProdusPageModel : PageModel
    {
        public List<Produs> products = new List<Produs>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=D:\\Projects\\BD_Valasciuc_Bogdan_06.04.23.accdb;";
                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string odbc = "SELECT * FROM Produs";
                    using (OdbcCommand command = new OdbcCommand(odbc, connection))
                    {
                        using (OdbcDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Produs produse = new Produs();
                                produse.ID_produs = reader.GetInt32(0);
                                produse.Denumire_produs = reader.GetString(1);
                                produse.Cod_produs = reader.GetString(2);

                                products.Add(produse);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class Produs
    {
        public int ID_produs { get; set; }
        public string Denumire_produs { get; set; }
        public string Cod_produs { get; set; }
    }
}