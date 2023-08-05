using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.Odbc;

namespace WebApplication1.Pages.ClientiProiect
{
    [Authorize]
    public class ClientiProiectPageModel : PageModel
    {
        public List<ClientiProiect> projectClients = new List<ClientiProiect>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=D:\\Projects\\BD_Valasciuc_Bogdan_06.04.23.accdb;";
                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string odbc = "SELECT * FROM Clienti_Proiecte";
                    using (OdbcCommand command = new OdbcCommand(odbc, connection))
                    {
                        using (OdbcDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientiProiect clientiProiect = new ClientiProiect();
                                clientiProiect.ID_client = reader.GetInt32(0);
                                clientiProiect.Denumire_firma = reader.GetString(1);
                                clientiProiect.Nume = reader.GetString(2);
                                clientiProiect.Prenume = reader.GetString(3);
                                clientiProiect.Nr_telefon = reader.GetString(4);
                                clientiProiect.Adresa_email = reader.GetString(5);

                                projectClients.Add(clientiProiect);
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

    public class ClientiProiect
    {
        public int ID_client { get; set; }
        public string Denumire_firma { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Nr_telefon { get; set; }
        public string Adresa_email { get; set; }
    }
}