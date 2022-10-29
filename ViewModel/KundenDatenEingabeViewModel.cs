using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.Json.Serialization;
using XrayKunden.Model;


namespace XrayKunden.ViewModel
{
    public class KundenDatenEingabeViewModel : ViewModelBase
    {
        
        //Binding: Properties aus View
        private string? _vorname;

        public string? Vorname      
        {
            get 
            {
                return _vorname;
            }
            set
            {
                if (_vorname != value)
                {
                    _vorname = value;
                    OnPropertyChanged();
                    ClearCommand?.RaiseCanExecuteChanged();
                    SaveCommand?.RaiseCanExecuteChanged();
                }
            }
        }

        private string? _nachname;

        public string? Nachname
        {
            get
            {
                return _nachname;
            }
            set
            {
                if (_nachname != value)
                {
                    _nachname = value;
                    OnPropertyChanged();
                    ClearCommand?.RaiseCanExecuteChanged();
                    SaveCommand?.RaiseCanExecuteChanged();
                }
            }
        }

        private string? _strasse;

        public string? Strasse
        {
            get
            {
                return _strasse;
            }
            set
            {
                if( _strasse != value) 
                {
                    _strasse = value;
                    OnPropertyChanged();
                    ClearCommand?.RaiseCanExecuteChanged();
                    SaveCommand?.RaiseCanExecuteChanged();
                }
              

            }
        }

        private string? _ort;

        public string? Ort
        {
            get
            {
                return _ort;
            }
            set
            {
                if (_ort != value)
                {
                    _ort = value;
                    OnPropertyChanged();
                    ClearCommand?.RaiseCanExecuteChanged();
                    SaveCommand?.RaiseCanExecuteChanged();
                }
            }
        }


        private string? _plz;

        public string? Plz
        {
            get
            {
                return _plz;
            }
            set
            {
                if (_plz != value)
                {
                    _plz = value;
                    OnPropertyChanged();
                    ClearCommand?.RaiseCanExecuteChanged();
                    SaveCommand?.RaiseCanExecuteChanged();
                }
            }
        }

        private string? _kundennr;

        public string? KundenNr
        {
            get
            {
                return _kundennr;
            }
            set
            {
                if (_kundennr != value)
                {
                    _kundennr = value;
                    OnPropertyChanged();
                    ClearCommand?.RaiseCanExecuteChanged();
                    SaveCommand?.RaiseCanExecuteChanged();
                }
            }
        }


        private string? _savecheck;
        public string? Savecheck
        {
            get { return _savecheck; }

            set
            {
                if(_savecheck != value)
                {
                    _savecheck = value;
                    ClearCommand?.RaiseCanExecuteChanged();
                    OnPropertyChanged(nameof(Savecheck));   // nameof(~) kann hier weggelassen werden wegen CompilerService [CallerMemberName] !!!
                }


            }

        }


        private bool _ischeckedjason;

        public bool Ischeckedjason { get ; set; } = true;

        private bool _ischeckedpostgres;
        public bool Ischeckedpostgres { get; set; }

        private bool _ischeckedmysqlinternet;
        public bool Ischeckedmysqlinternet { get; set; }


        //Commands aus View

        public DelegateCommand? ClearCommand { get; set; }

        public DelegateCommand? SaveCommand { get; set; }




        //Ende Binding


       

        //Methoden



        public void SetDefaultKundenEingaben()
        {

            this.Vorname = "";
            this.Nachname = "";
            this.Strasse = "";
            this.Ort = "";
            this.Plz = "";
            this.KundenNr = "";
            this.Savecheck = "Nicht gespeichert";

        }


        public async void SaveKundenDaten()
        {
            if (Ischeckedjason)
            {



                JsonWriter KundeToJson = new("KundenDaten.json");


                bool checkSave = await KundeToJson.JsonFileWriter(this.Vorname, this.Nachname, this.Strasse, this.Ort, this.Plz, this.KundenNr);
                if (checkSave)
                {
                    this.Savecheck = "Speichern JASON erfolgreich";
                }
                else
                {
                    this.Savecheck = "Speichern JSON fehlgeschlagen";
                }

            }
            else if (Ischeckedpostgres)
            {

                XrayPostgresConnector KundeToPostgres = new();

                bool checkSave = await KundeToPostgres.DataWriter("INSERT", this.Vorname, this.Nachname, this.Strasse, this.Ort, this.Plz, this.KundenNr);


                if (checkSave)
                {
                    this.Savecheck = "Speichern Postgres erfolgreich";
                }
                else
                {
                    this.Savecheck = "Speichern Postgres fehlgeschlagen";
                }
            }
            else if (Ischeckedmysqlinternet)
            {
                

                FileService FileService = new();

                string? _urlstring = await FileService.PHPdataPath("phpdatain.txt");

                
                Uri? Urlstring = new(_urlstring);


                Dictionary<string, string> Datensatz = new()
                {
                    ["softwareid"] = "yyyxxxccc",                      
                    ["vorname"] = this.Vorname,
                    ["nachname"] = this.Nachname,
                    ["strasse"] = this.Strasse,
                    ["ort"] = this.Ort,
                    ["plz"] = this.Plz,
                    ["kundennr"] = this.KundenNr
                };


                if (Urlstring != null && Urlstring.ToString() != "")
                {

                    bool checksave = await WebConnector.ClientServerComUpload(Urlstring, Datensatz);
                    if (checksave)
                    {
                        this.Savecheck = "MySQL Daten gespeichert!";  // nicht sicher, dass Daten gespeichert, nur Http response true!?

                    } else
                    {
                        this.Savecheck = "MySQL Daten - Probleme beim Speichern!";

                    }

                    
                    

                }
                else
                {
                    this.Savecheck = "MyUrl = null or empty!!!";

                }



            }
            
            
        }





        //Ende Methoden


        //Konstruktor
        public KundenDatenEingabeViewModel()
        {
            SetDefaultKundenEingaben();

            this.ClearCommand = new DelegateCommand(

               (obj) =>
               {
                   SetDefaultKundenEingaben();
               },

                (obj) => !string.IsNullOrEmpty(Vorname) ||
                !string.IsNullOrEmpty(Nachname) ||
                !string.IsNullOrEmpty(Strasse) ||
                !string.IsNullOrEmpty(Ort) ||
                !string.IsNullOrEmpty(Plz) ||
                !string.IsNullOrEmpty(KundenNr)

                ); //Ende new ClearCommand   


            this.SaveCommand = new DelegateCommand(

                (obj) =>
                {

                    SaveKundenDaten();
                },


                (obj) => !string.IsNullOrEmpty(Vorname) ||
                         !string.IsNullOrEmpty(Nachname) ||
                         !string.IsNullOrEmpty(Strasse) ||
                         !string.IsNullOrEmpty(Ort) ||
                         !string.IsNullOrEmpty(Plz) ||
                         !string.IsNullOrEmpty(KundenNr)



                ); //Ende new SaveCommand


           
            
        } //Ende Konstruktor

       
    }
}
