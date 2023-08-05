using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.Odbc;

namespace WebApplication1.Pages.ActivitatiProiect
{
    public class NewActivityModel : PageModel
    {
        public ActivitatiProiect activitatiProiect = new ActivitatiProiect();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {

        }
        public void OnPost() 
        {
            activitatiProiect.ID_produs = Request.Form["id_produs"];
            activitatiProiect.ID_activitate = Request.Form["id_activitate_proiect"];
            activitatiProiect.ID_activitati_predecesoare = Request.Form["id_activitati_predecesoare"];
            activitatiProiect.Descriere_activitate = Request.Form["descriere_activitate"];
            activitatiProiect.Durata_standard_activitate = Request.Form["durata_standard_activitate"];
            activitatiProiect.Durata_minima_activitate = Request.Form["durata_minima_activitate"];
            activitatiProiect.UM_durata = Request.Form["um_durata"];
            activitatiProiect.Cost_standard_activitate = Request.Form["cost_standard_activitate"];
            activitatiProiect.Cost_maxim_activitate = Request.Form["cost_maxim_activitate"];
            activitatiProiect.UM_cost = Request.Form["um_cost"];

            if (activitatiProiect == null ||
                activitatiProiect.ID_produs == null ||
                activitatiProiect.ID_activitate == null ||
                activitatiProiect.ID_activitati_predecesoare == null ||
                activitatiProiect.Descriere_activitate == null ||
                activitatiProiect.Durata_standard_activitate == null ||
                activitatiProiect.Durata_minima_activitate == null ||
                activitatiProiect.UM_durata == null ||
                activitatiProiect.Cost_standard_activitate == null ||
                activitatiProiect.Cost_maxim_activitate == null ||
                activitatiProiect.UM_cost == null)
            {
                errorMessage = "Toate campurile sunt obligatorii";
                return;
            }

            //Save data in database
            try
            {
                String connectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=D:\\Projects\\BD_Valasciuc_Bogdan_06.04.23.accdb;";
                using( OdbcConnection connection = new OdbcConnection( connectionString ) )
                {
                    connection.Open();
                    string odbcQuery = "INSERT INTO Activitati_Proiect " +
                        " (ID_produs, ID_activitate_proiect, ID_activitati_predecesoare, Descriere_activitate, Durata_standard_activitate, Durata_minima_activitate, UM_durata, Cost_standard_activitate, Cost_maxim_activitate, UM_cost) VALUES "+
                        " ('" + activitatiProiect.ID_produs + "','" + activitatiProiect.ID_activitate + "','" + activitatiProiect.ID_activitati_predecesoare + "','" + activitatiProiect.Descriere_activitate + "','" + activitatiProiect.Durata_standard_activitate +
                        "','" + activitatiProiect.Durata_minima_activitate + "','" + activitatiProiect.UM_durata + "','" + activitatiProiect.Cost_standard_activitate + "','" + activitatiProiect.Cost_maxim_activitate + "','" + activitatiProiect.UM_cost + "');";

                    using(OdbcCommand command = new OdbcCommand(odbcQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id_produs", activitatiProiect.ID_produs);
                        command.Parameters.AddWithValue("@id_activitate_proiect", activitatiProiect.ID_activitate);
                        command.Parameters.AddWithValue("@id_activitati_predecesoare", activitatiProiect.ID_activitati_predecesoare);
                        command.Parameters.AddWithValue("@descriere_activitate", activitatiProiect.Descriere_activitate);
                        command.Parameters.AddWithValue("@durata_standard_activitate", activitatiProiect.Durata_standard_activitate);
                        command.Parameters.AddWithValue("@durata_minima_activitate", activitatiProiect.Durata_minima_activitate);
                        command.Parameters.AddWithValue("@um_durata", activitatiProiect.UM_durata);
                        command.Parameters.AddWithValue("@cost_standard_activitate", activitatiProiect.Cost_standard_activitate);
                        command.Parameters.AddWithValue("@cost_maxim_activitate", activitatiProiect.Cost_maxim_activitate);
                        command.Parameters.AddWithValue("@um_cost", activitatiProiect.UM_cost);
                        
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

            Response.Redirect("/ActivitatiProiect/ActivitatiProiectPage");
         }
    }
}
