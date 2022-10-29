using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace XrayKunden.Model
{
    internal class WebConnector
    {

        private static readonly HttpClient Client = new();


        internal static async Task<bool> ClientServerComUpload(Uri urlstring, Dictionary<string, string> datensatz)
        {
            bool checksave = false;

            try
            {
                if (!(datensatz["softwareid"] == null || datensatz["softwareid"] == "" ||
                      datensatz["vorname"] == null || datensatz["vorname"] == "" ||
                      datensatz["nachname"] == null || datensatz["nachname"] == "" ||
                      datensatz["strasse"] == null || datensatz["strasse"] == "" ||
                      datensatz["ort"] == null || datensatz["ort"] == "" ||
                      datensatz["plz"] == null || datensatz["plz"] == "" ||
                      datensatz["kundennr"] == null || datensatz["kundennr"] == ""))
                {
                    
                    using FormUrlEncodedContent Content = new(datensatz);
                    HttpResponseMessage response = await Client.PostAsync(urlstring, Content);
                    checksave = response.IsSuccessStatusCode;
                    // MessageBox.Show(response.Headers + "\n" + response.RequestMessage + "\n" + response.StatusCode + "\n" + response.Content + "\n" + response.ToString());

                }
                else
                {
                    MessageBox.Show("Datensatz-Elemente waren null oder empty");
                    checksave = false;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + " \n" + ex.ToString());
                checksave = false;
            }

            return checksave;
        }

        internal static async Task<List<Kunde>> ClientServerComDownload(Uri urlstring, Dictionary<string,string> datensatz)
        {
            bool checkdownload;
            List<Kunde>? kundenliste = new();

            try
            {
                if (!(datensatz["softwareid"] == null || datensatz["softwareid"] == ""))
                {

                    using FormUrlEncodedContent Content = new(datensatz);
                    HttpResponseMessage response = await Client.PostAsync(urlstring, Content);
                    checkdownload = response.IsSuccessStatusCode;
                    // MessageBox.Show(response.Headers + "\n" + response.RequestMessage + "\n" + response.StatusCode + "\n" + response.Content + "\n" + response.ToString());
                    if (checkdownload)
                    {

                        string tempData = await response.Content.ReadAsStringAsync();
                        kundenliste = JsonSerializer.Deserialize<List<Kunde>>(tempData);

                    }

                    if (kundenliste != null)
                    {

                        return kundenliste;
                        

                    }
                    else

                    {
                        MessageBox.Show("Datensatz-Elemente waren null oder empty");
                        kundenliste.Add(new Kunde { ID = 0, Vorname = "Fehler", Nachname = "Fehler" , Strasse = "Fehler", Ort = "Fehler", Plz = "Fehler", KundenNr = "" });
                        
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + " \n" + ex.ToString());
                kundenliste.Add(new Kunde { ID = 0, Vorname = "Fehler", Nachname = "Fehler", Strasse = "Fehler", Ort = "Fehler", Plz = "Fehler", KundenNr = "" });

            }
             
            return kundenliste;




        }


    }
}
