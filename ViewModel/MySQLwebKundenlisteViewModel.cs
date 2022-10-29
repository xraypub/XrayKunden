using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using XrayKunden.Model;
using XrayKunden.Views;

namespace XrayKunden.ViewModel
{
    internal class MySQLwebKundenlisteViewModel : ViewModelBase
    {
        public WebConnector KundeToDataGrid = new();

        public WebConnector KundeToDatabase = new();     // geht auch mit nur einem WebConnector 

        public List<Kunde> TempList = new();

        public ObservableCollection<Kunde>? MySQLwebKundenListe { get; set; }

        private bool _filldata;
        public bool FillData { 
            get
            { 
                return _filldata; 
            }
            set 
            {
                _filldata = value;
                DataFillCommand?.RaiseCanExecuteChanged();
            } 
        }

        private  bool _storeback;
        public bool StoreBack {
            get
            {
                return _storeback;
            }
            set
            {
                _storeback = value;
                DataStoreBack?.RaiseCanExecuteChanged();
            } 
        }


        private string? _dataLoadStatus;
        public string? DataLoadStatus
        {

            get
            {

                return _dataLoadStatus;
            }

            set
            {

                if (_dataLoadStatus != value)
                {

                    _dataLoadStatus = value;
                    OnPropertyChanged();
                }
            }
        }


        // Methoden

                

        public async void DataFill()
        {
            StoreBack = false;

            FileService fileService = new FileService();
            string? _urlstring = await fileService.PHPdataPath("phpdataout.txt");
            Uri? Urlstring = new(_urlstring);
            Dictionary<string, string> Datensatz = new()
            {
                ["softwareid"] = "yyyxxxccc",
              
            };

            if (Urlstring != null)
            {

               TempList = await WebConnector.ClientServerComDownload(Urlstring, Datensatz);

                if (TempList != null)
                {

                    foreach (var item in TempList)
                    {
                        MySQLwebKundenListe?.Add(item);
                    }

                    DataLoadStatus = "MySQL Daten geladen! ";


                }
                else
                {

                    DataLoadStatus = "Fehler beim Laden der Daten! ";
                }

            }
            else
            {
                MessageBox.Show("Url = null !!!");

            }
            
            StoreBack = true;
            
        }

        public async void DataUpdater()
        {

            FillData = false;
            bool checksave = false;
            FileService fileService = new FileService();
            string? _urlstring = await fileService.PHPdataPath("phpdataupdate.txt");
            Uri? Urlstring = new(_urlstring);
            Dictionary<string, string> Datensatz = new();

            if (MySQLwebKundenListe != null)
            {

                foreach (var item in MySQLwebKundenListe)
                {
                    Datensatz["softwareid"] = "yyyxxxccc";
                    Datensatz["vorname"] = item.Vorname;
                    Datensatz["nachname"] = item.Nachname;
                    Datensatz["strasse"] = item.Strasse;
                    Datensatz["ort"] = item.Ort;
                    Datensatz["plz"] = item.Plz;
                    Datensatz["kundennr"] = item.KundenNr;
                    Datensatz["id"] = item.ID.ToString();
                    checksave = await WebConnector.ClientServerComUpload(Urlstring, Datensatz);
                }
            }
            else
            {
                MessageBox.Show("Keine Daten inder Liste -> null");
            }

            
            if (checksave)
            {
                DataLoadStatus = "MySQL Data Update erfolgreich!";

            }
            else
            {
                DataLoadStatus = "MySQL DAta Update-Fehler!!";
            }

           
           
            FillData = true;
           
        }



        // Commands aus View

        public DelegateCommand? DataFillCommand { get; set; }

        public DelegateCommand? DataStoreBack { get; set; }


        // Konstruktor
        public MySQLwebKundenlisteViewModel()
        {
            DataLoadStatus = "Keine Daten geladen!";
            MySQLwebKundenListe = new(TempList);

            
            DataFill();
            FillData = true;


            this.DataFillCommand = new DelegateCommand(

              (obj) =>
              {

                  MySQLwebKundenListe.Clear();
                  DataFill();




              },

              (obj) => FillData.Equals(true)


              ); //Ende new DataFillCommand  



            this.DataStoreBack = new DelegateCommand(

               (obj) =>
               {
                   DataUpdater();



               },

               (obj) => StoreBack.Equals(true)


                );  //Ende new DataStoreBack


        }

    }
}
