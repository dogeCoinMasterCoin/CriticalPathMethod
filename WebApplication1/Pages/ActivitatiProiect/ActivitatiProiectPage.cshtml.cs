using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Data.Common;
using System.Data.Odbc;

namespace WebApplication1.Pages.ActivitatiProiect
{
    [Authorize]
    public class ActivitatiProiectPageModel : PageModel
    {
        public List<ActivitatiProiect> projectActivity = new List<ActivitatiProiect>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=D:\\Projects\\BD_Valasciuc_Bogdan_06.04.23.accdb;";
                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string odbc = "SELECT * FROM Activitati_Proiect";
                    using (OdbcCommand command = new OdbcCommand(odbc, connection))
                    {
                        using (OdbcDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ActivitatiProiect activitatiProiect = new ActivitatiProiect();
                                activitatiProiect.ID_produs = reader.GetString(0);
                                activitatiProiect.ID_activitate = reader.GetString(1);
                                activitatiProiect.ID_activitati_predecesoare = reader.GetString(2);
                                activitatiProiect.Descriere_activitate = reader.GetString(3);
                                activitatiProiect.Durata_standard_activitate = reader.GetString(4);
                                activitatiProiect.Durata_minima_activitate = reader.GetString(5);
                                activitatiProiect.UM_durata = reader.GetString(6);  
                                activitatiProiect.Cost_standard_activitate = reader.GetString(7);
                                activitatiProiect.Cost_maxim_activitate = reader.GetString(8);
                                activitatiProiect.UM_cost = reader.GetString(9);

                                projectActivity.Add(activitatiProiect);
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

    public class ActivitatiProiect
    {
        public string ID_produs;
        public string ID_activitate;
        public string ID_activitati_predecesoare;
        public string Descriere_activitate;
        public string Durata_standard_activitate;
        public string Durata_minima_activitate;
        public string UM_durata;
        public string Cost_standard_activitate;
        public string Cost_maxim_activitate;
        public string UM_cost;
    }
}