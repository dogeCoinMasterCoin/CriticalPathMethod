using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.Odbc;

namespace WebApplication1.Pages.PlanificareProiect
{
    [Authorize]
    public class PlanificareProiectPageModel : PageModel
    {
        public List<PlanificareProiect> projectPlanning = new List<PlanificareProiect>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=D:\\Projects\\BD_Valasciuc_Bogdan_06.04.23.accdb;";
                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string odbc = "SELECT * FROM Planificare_Proiect";
                    using (OdbcCommand command = new OdbcCommand(odbc, connection))
                    {
                        using (OdbcDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PlanificareProiect planificareProiect = new PlanificareProiect();
                                planificareProiect.ID_produs = reader.GetInt32(0);
                                planificareProiect.ID_proiect = reader.GetInt32(1);
                                planificareProiect.ID_activitate = reader.GetInt32(2);
                                planificareProiect.Durata_planificare_activitate = reader.GetInt32(3);
                                planificareProiect.UM_durata = reader.GetString(4);
                                planificareProiect.Cost_planificare_activitate = reader.GetInt32(5);
                                planificareProiect.UM_cost = reader.GetString(6);
                                planificareProiect.Drum_critic_proiect = reader.GetString(7);
                                planificareProiect.Durata_planificare_proiect = reader.GetInt32(8);
                                planificareProiect.UM_durata2 = reader.GetString(9);
                                planificareProiect.Cost_planificare_proiect = reader.GetInt32(10);
                                planificareProiect.UM_cost2 = reader.GetString(11);

                                projectPlanning.Add(planificareProiect);
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

    public class PlanificareProiect
    {
        public int ID_produs { get; set; }
        public int ID_proiect { get; set; }
        public int ID_activitate { get; set; }
        public int Durata_planificare_activitate { get; set; }
        public string UM_durata { get; set; }
        public int Cost_planificare_activitate { get; set; }
        public string UM_cost { get; set; }
        public string Drum_critic_proiect { get; set; }
        public int Durata_planificare_proiect { get; set; }
        public string UM_durata2 { get; set; }
        public int Cost_planificare_proiect { get; set; }
        public string UM_cost2 { get; set; }
    }
}
