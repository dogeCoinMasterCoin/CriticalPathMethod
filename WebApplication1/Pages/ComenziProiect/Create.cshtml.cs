using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.Odbc;
using WebApplication1.Pages.ActivitatiProiect;

namespace WebApplication1.Pages.ComenziProiect
{
    public class CreateModel : PageModel
    {
        public ComenziProiect comenziProiect = new ComenziProiect();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {

        }

        public void OnPost() 
        {
            comenziProiect.ID_produs = Convert.ToInt32(Request.Form["id_produs"]);
            comenziProiect.ID_proiect = Convert.ToInt32(Request.Form["id_proiect"]);
            comenziProiect.ID_client = Convert.ToInt32(Request.Form["id_client"]);
            comenziProiect.Durata_cont = Convert.ToInt32(Request.Form["durata_contractuala_proiect"]);
            comenziProiect.UM_durata = Request.Form["um_durata"];
            comenziProiect.Pret_contractual_proiect = Convert.ToInt32(Request.Form["pret_contractual_proiect"]);
            comenziProiect.UM_pret = Request.Form["um_pret"];
            comenziProiect.Adresa_livrare_produs = Request.Form["adresa_livrare_produs"];

            if (comenziProiect.ID_produs.ToString().Length == 0 ||
               comenziProiect.ID_proiect.ToString().Length == 0 ||
               comenziProiect.ID_client.ToString().Length == 0 ||
               comenziProiect.Durata_cont.ToString().Length == 0 ||
               comenziProiect.UM_durata.Length == 0 ||
               comenziProiect.Pret_contractual_proiect.ToString().Length == 0 ||
               comenziProiect.UM_pret.Length == 0 ||
               comenziProiect.Adresa_livrare_produs.Length == 0)
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
                    string odbcQuery = "INSERT INTO Comenzi_Proiecte" +
                        "(ID_produs, ID_proiect, ID_client, Durata_contractuala_proiect, UM_durata, Pret_Contractual_proiect, UM_pret, Adresa_livrare_produs) VALUES " +
                        "('" + comenziProiect.ID_produs + "','" + comenziProiect.ID_proiect + "','" + comenziProiect.ID_client + "','" + comenziProiect.Durata_cont + "','" + comenziProiect.UM_durata + "','" + comenziProiect.Pret_contractual_proiect + "','" + comenziProiect.UM_pret + "','" + comenziProiect.Adresa_livrare_produs + "')";

                    using (OdbcCommand command = new OdbcCommand(odbcQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id_produs", comenziProiect.ID_produs);
                        command.Parameters.AddWithValue("@id_proiect", comenziProiect.ID_proiect);
                        command.Parameters.AddWithValue("@id_client", comenziProiect.ID_client);
                        command.Parameters.AddWithValue("@durata_contractuala_proiect", comenziProiect.Durata_cont);
                        command.Parameters.AddWithValue("@um_durata", comenziProiect.UM_durata);
                        command.Parameters.AddWithValue("@pret_contractual_proiect", comenziProiect.Pret_contractual_proiect);
                        command.Parameters.AddWithValue("@um_pret", comenziProiect.UM_pret);
                        command.Parameters.AddWithValue("@adresa_livrare_produs", comenziProiect.Adresa_livrare_produs);


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

            Response.Redirect("/ComenziProiecte/ComenziProiectePage");
        }
    }
}
