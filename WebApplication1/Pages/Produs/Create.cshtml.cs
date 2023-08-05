using AAJControl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.Odbc;
using WebApplication1.Pages.ActivitatiProiect;

namespace WebApplication1.Pages.Produs
{
    public class CreateModel : PageModel
    {
        public Produs produs = new Produs();

        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            
        }
        public void OnPost()
        {
            produs.ID_produs = Convert.ToInt32(Request.Form["id_produs"]);
            produs.Denumire_produs = Request.Form["denumire_produs"];
            produs.Cod_produs = Request.Form["cod_produs"];

            if (produs.Denumire_produs.Length == 0 ||
               produs.Cod_produs.Length == 0)
            {
                errorMessage = "Toate campurile sunt obligatorii";
                return;
            }

            //Save data in database
            try
            {
                string connectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=D:\\Projects\\BD_Valasciuc_Bogdan_06.04.23.accdb;";
                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string odbcQuery = "INSERT INTO Produs (ID_produs, Denumire_produs, Cod_produs) VALUES ('" + produs.ID_produs + "','" + produs.Denumire_produs  + "','" + produs.Denumire_produs  + "')";

                    using (OdbcCommand command = new OdbcCommand(odbcQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id_produs", produs.ID_produs);
                        command.Parameters.AddWithValue("@denumire_produs", produs.Denumire_produs);
                        command.Parameters.AddWithValue("@cod_produs", produs.Cod_produs);

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

        [HttpPost]
        Tuple<int, string, string> storeUserInputProductID()
        {
            return new Tuple<int, string, string>(Convert.ToInt32(Request.Form["id_produs"]), Request.Form["denumire_produs"], Request.Form["cod_produs"]);
        }

    }
}
