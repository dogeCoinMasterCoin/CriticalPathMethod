using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.Odbc;
using WebApplication1.Pages.ActivitatiProiect;

namespace WebApplication1.Pages.PlanificareProiect
{
    public class CreateModel : PageModel
    {
        public PlanificareProiect planificareProiect = new PlanificareProiect();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {

        }
        public void OnPost()
        {
            planificareProiect.ID_produs = Convert.ToInt32(Request.Form["id_produs"]);
            planificareProiect.ID_proiect = Convert.ToInt32(Request.Form["id_proiect"]);
            planificareProiect.ID_activitate = Convert.ToInt32(Request.Form["id_activitate_proiect"]);
            planificareProiect.Durata_planificare_activitate = Convert.ToInt32(Request.Form["durata_planificata_activitate"]);
            planificareProiect.UM_durata = Request.Form["um_durata"];
            planificareProiect.Cost_planificare_activitate = Convert.ToInt32(Request.Form["cost_planificat_activitate"]);
            planificareProiect.UM_cost = Request.Form["um_cost"];
            planificareProiect.Drum_critic_proiect = Request.Form["drum_critic_proiect"];
            planificareProiect.Durata_planificare_proiect = Convert.ToInt32(Request.Form["durata_planificata_proiect"]);
            planificareProiect.UM_durata2 = Request.Form["um_durata2"];
            planificareProiect.Cost_planificare_proiect = Convert.ToInt32(Request.Form["cost_planificat_proiect"]);
            planificareProiect.UM_cost2 = Request.Form["um_cost2"];

            if (planificareProiect.ID_produs.ToString().Length == 0 ||
               planificareProiect.ID_proiect.ToString().Length == 0 ||
               planificareProiect.ID_activitate.ToString().Length == 0 ||
               planificareProiect.Durata_planificare_activitate.ToString().Length == 0 ||
               planificareProiect.UM_durata.Length == 0 ||
               planificareProiect.Cost_planificare_activitate.ToString().Length == 0 ||
               planificareProiect.UM_cost.Length == 0 ||
               planificareProiect.Drum_critic_proiect.ToString().Length == 0 ||
               planificareProiect.Durata_planificare_proiect.ToString().Length == 0 ||
               planificareProiect.UM_durata2.Length == 0 ||
               planificareProiect.Cost_planificare_proiect.ToString().Length == 0 ||
               planificareProiect.UM_cost2.Length == 0)
            {
                errorMessage = "Toate campurile sunt obligatorii";
                return;
            }
            //Save data in database
            try
            {
                String connectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=D:\\Projects\\BD_Valasciuc_Bogdan_06.04.23.accdb;";
                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string odbcQuery = "INSERT INTO Planificare_Proiect" +
                        "(ID_produs, ID_proiect, ID_activitate_proiect, Durata_planificata_activitate, UM_durata, Cost_planificat_activitate, UM_cost, Drum_critic_proiect, Durata_planificata_proiect, UM_durata2, Cost_planificat_proiect, UM_cost2) VALUES " +
                        "('" + planificareProiect.ID_produs + "','" + planificareProiect.ID_proiect + "','" + planificareProiect.ID_activitate + "','" + planificareProiect.Durata_planificare_activitate + "','" + planificareProiect.UM_durata + "','" + planificareProiect.Cost_planificare_activitate + "','" + planificareProiect.UM_cost + "','" + planificareProiect.Drum_critic_proiect + "','" + planificareProiect.Durata_planificare_proiect + "','" + planificareProiect.UM_durata2 + "','" + planificareProiect.Cost_planificare_proiect + "','" + planificareProiect.UM_cost2 + "')";

                    using (OdbcCommand command = new OdbcCommand(odbcQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id_produs", planificareProiect.ID_produs);
                        command.Parameters.AddWithValue("@id_proiect", planificareProiect.ID_proiect);
                        command.Parameters.AddWithValue("@id_activitate_proiect", planificareProiect.ID_activitate);
                        command.Parameters.AddWithValue("@durata_planificata_activitate", planificareProiect.Durata_planificare_activitate);
                        command.Parameters.AddWithValue("@um_durata", planificareProiect.UM_durata);
                        command.Parameters.AddWithValue("@cost_planificat_activitate", planificareProiect.Cost_planificare_activitate);
                        command.Parameters.AddWithValue("@um_cost", planificareProiect.UM_cost);
                        command.Parameters.AddWithValue("@drum_critic_proiect", planificareProiect.Drum_critic_proiect);
                        command.Parameters.AddWithValue("@durata_planificata_proiect", planificareProiect.Durata_planificare_proiect);
                        command.Parameters.AddWithValue("@um_durata2", planificareProiect.UM_durata2);
                        command.Parameters.AddWithValue("@cost_planificat_proiect", planificareProiect.Cost_planificare_proiect);
                        command.Parameters.AddWithValue("@um_cost2", planificareProiect.UM_cost2);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            successMessage = "Activitate adaugata cu succes!";

            Response.Redirect("/PlanificareProiect/PlanificareProiectPage");
        }
    }
}
