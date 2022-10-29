using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;



namespace XrayKunden.Model
{
    internal class JsonWriter
    {

        private string _fileName;





        //Methoden
        
        public async Task<bool> JsonFileWriter(string vorname, string nachname, string strasse, string ort, string plz, string kundennr)
        {
            Kunde KundenDatensatz = new( vorname, nachname, strasse, ort, plz, kundennr);
                        
            JsonSerializerOptions jsonOptions = new()
            {
                WriteIndented = true,
                IncludeFields = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase

            };

            using FileStream fstream = new FileStream(_fileName, FileMode.Create, FileAccess.Write);     
            
            await JsonSerializer.SerializeAsync(fstream, KundenDatensatz, jsonOptions);  
            await fstream.DisposeAsync();
            

            return true;

        }


        
        //Konstruktor
        public JsonWriter(string filename)
        {
            
            if (filename != null)
            {
                _fileName = filename;

            }
            else
            {

                _fileName = "DefaultName.json";

            }
        }

    }
}
