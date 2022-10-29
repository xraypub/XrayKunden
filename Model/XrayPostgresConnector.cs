using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Npgsql;

namespace XrayKunden.Model
{
    internal class XrayPostgresConnector
    {

        readonly string dbConnection = "Host=localhost;Username=postgres;Password=admin;Database=postgres";  // Individuelle POSTGRES Zugangsdaten eingeben!!

        string cmdString ="";

        // Methoden

        public async Task<bool> DataWriter(string checkcmd, string vorname, string nachname, string strasse, string ort, string plz, string kundennr, int id = 0)
        {

            bool checksave = false;
            string _checkcmd = checkcmd;

            switch (_checkcmd)
            {
                case "INSERT":
                    cmdString = "INSERT INTO public.\"XrayKundenDaten\" (\"Vorname\", \"Nachname\", \"Strasse\", \"Ort\", \"Plz\", \"KundenNr\") VALUES (($1), ($2), ($3), ($4), ($5), ($6)) ";
                    break;
                
                case "UPDATE":
                    cmdString = "UPDATE public.\"XrayKundenDaten\" SET (\"Vorname\", \"Nachname\", \"Strasse\", \"Ort\", \"Plz\", \"KundenNr\") = ($1, $2,$3,$4,$5,$6) WHERE \"ID\"=$7 ";
                    break;

                default:
                    MessageBox.Show("Postgres SQL COMMAND-String Fehler!");
                    break;

            }

            



            try
            {
                await using var conn = new NpgsqlConnection(dbConnection);
                await conn.OpenAsync();


                if(_checkcmd == "INSERT")
                {
                    await using var cmd = new NpgsqlCommand(cmdString, conn)
                    {
                        Parameters =
                         {
                              new() { Value = vorname },
                              new() { Value = nachname },
                              new() { Value = strasse },
                              new() { Value = ort },
                              new() { Value = plz },
                              new() { Value = kundennr }
                   
                         }
                    };

                    await cmd.ExecuteNonQueryAsync();

                }
                else
                {


                    await using var cmd = new NpgsqlCommand(cmdString, conn)
                    {

                       Parameters =
                        {
                            new() { Value = vorname },
                            new() { Value = nachname },
                            new() { Value = strasse },
                            new() { Value = ort },
                            new() { Value = plz },
                            new() { Value = kundennr },
                            new() { Value = id}
                         } 
                    };

                    await cmd.ExecuteNonQueryAsync();


                }

                              

                
                checksave = true;
            }
            catch (Exception ex)
            {
                checksave = false;  
                MessageBox.Show(ex.Message);    
            }

            

            return checksave;
        }

        public async Task<List<Kunde>> DataReader()
        {
            

            List<Kunde> kundeList = new();  

            try
            {
                await using var conn = new NpgsqlConnection(dbConnection);
                await conn.OpenAsync();
                await using var cmd = new NpgsqlCommand("SELECT * FROM public.\"XrayKundenDaten\" LIMIT 100;", conn); // LIMIT 100 - es werden nur die ersten 100 Datensätze ausgelesen!!!
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())            
                        {
                            kundeList.Add(new Kunde { Vorname =  reader.GetValue(0).ToString(),
                                                      Nachname = reader.GetValue(1).ToString(),
                                                      Strasse = reader.GetValue(2).ToString(),
                                                      Ort = reader.GetValue(3).ToString(),
                                                      Plz = reader.GetValue(4).ToString(),
                                                      KundenNr = reader.GetValue(5).ToString(),
                                                      ID = (int) reader.GetValue(6)

                            });   

                        }


                    }



                }


               
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                
            }

            return kundeList;
        }

        // Konstruktor

        public XrayPostgresConnector()
        {

        }

    }
}
