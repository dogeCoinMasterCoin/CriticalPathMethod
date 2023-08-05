using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.Odbc;
using System.Net.Sockets;

namespace WebApplication1.Pages.ClientiProiect
{
    public class CreateModel : PageModel
    {
        public ClientiProiect clientiProiect = new ClientiProiect();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {

        }
        public void OnPost()
        {
            clientiProiect.ID_client = Convert.ToInt32(Request.Form["id_client"]);
            clientiProiect.Denumire_firma = Request.Form["denumire_firma"];
            clientiProiect.Nume = Request.Form["nume"];
            clientiProiect.Prenume = Request.Form["prenume"];
            clientiProiect.Nr_telefon = Request.Form["nr_telefon"];
            clientiProiect.Adresa_email = Request.Form["adresa_mail"];

            if (clientiProiect.ID_client.ToString().Length == 0 ||
               clientiProiect.Denumire_firma.Length == 0 ||
               clientiProiect.Nume.Length == 0 ||
               clientiProiect.Prenume.Length == 0 ||
               clientiProiect.Nr_telefon.Length == 0 ||
               clientiProiect.Adresa_email.Length == 0)
            {
                errorMessage = "Toate campurile sunt obligatorii";
                return;
            }

            try
            {
                String connectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=D:\\Projects\\BD_Valasciuc_Bogdan_06.04.23.accdb;";
                using(OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string odbcQuery = "INSERT INTO Clienti_Proiecte" +
                                       "(ID_client, Denumire_firma, Nume, Prenume, Nr_telefon, Adresa_mail) VALUES " +
                                       "('" + clientiProiect.ID_client + "','" + clientiProiect.Denumire_firma + "','" + clientiProiect.Nume + "','" + clientiProiect.Prenume + "','" + clientiProiect.Nr_telefon + "','" + clientiProiect.Adresa_email + "')";
                    using (OdbcCommand command = new OdbcCommand(odbcQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id_client", clientiProiect.ID_client);
                        command.Parameters.AddWithValue("@denumire_firma", clientiProiect.Denumire_firma);
                        command.Parameters.AddWithValue("@nume", clientiProiect.Nume);
                        command.Parameters.AddWithValue("@prenume", clientiProiect.Prenume);
                        command.Parameters.AddWithValue("@nr_telefon", clientiProiect.Nr_telefon);
                        command.Parameters.AddWithValue("@adresa_mail", clientiProiect.Adresa_email);

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

            Response.Redirect("/ClientiProiect/ClientiProiectPage");
        }
    }
}
