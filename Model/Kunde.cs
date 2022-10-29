using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace XrayKunden.Model
{
    internal class Kunde
    {
        
        //Properties
        public string? Vorname { get; set; }
        public string? Nachname { get; set; }
        public string? Strasse { get; set; }
        public string? Ort { get; set; }
        public string? Plz { get; set; }

       // string? Email { get; set; }  not in use!!
       // string? Mobil { get; set; }  ~
        public string? KundenNr { get; set; }

        public int ID { get; set; } = 0;

       

        //Methoden


        public Kunde()
        {
            
        }


        public Kunde(string? vorname, string? nachname, string? strasse, string? ort, string? plz, string? kundennr)
        {
            this.Vorname = vorname;
            this.Nachname = nachname;
            this.Strasse = strasse;
            this.Ort = ort;
            this.Plz = plz;
            this.KundenNr = kundennr;

                     
           

        }

       
               
        


       


    }
}
