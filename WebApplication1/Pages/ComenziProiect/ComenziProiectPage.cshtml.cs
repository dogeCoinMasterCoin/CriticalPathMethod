using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.Odbc;

namespace WebApplication1.Pages.ComenziProiect
{
    [Authorize]
    public class ComenziProiectPageModel : PageModel
    {
        public List<ComenziProiect> projectCommands = new List<ComenziProiect>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=D:\\Projects\\BD_Valasciuc_Bogdan_06.04.23.accdb;";
                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string odbc = "SELECT * FROM Comenzi_Proiecte";
                    using (OdbcCommand command = new OdbcCommand(odbc, connection))
                    {
                        using (OdbcDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ComenziProiect comenziProiect = new ComenziProiect();
                                comenziProiect.ID_produs = reader.GetInt32(0);
                                comenziProiect.ID_proiect = reader.GetInt32(1);
                                comenziProiect.ID_client = reader.GetInt32(2);
                                comenziProiect.Durata_cont = reader.GetInt32(3);
                                comenziProiect.UM_durata = reader.GetString(4);
                                comenziProiect.Pret_contractual_proiect = reader.GetInt32(5);
                                comenziProiect.UM_pret = reader.GetString(6);
                                comenziProiect.Adresa_livrare_produs = reader.GetString(7);

                                projectCommands.Add(comenziProiect);
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

    public class ComenziProiect
    {
        public int ID_produs { get; set; }
        public int ID_proiect { get; set; }
        public int ID_client { get; set; }
        public int Durata_cont { get; set; }
        public string UM_durata { get; set; }
        public int Pret_contractual_proiect { get; set; }
        public string UM_pret { get; set; }
        public string Adresa_livrare_produs { get; set; }
    }
}