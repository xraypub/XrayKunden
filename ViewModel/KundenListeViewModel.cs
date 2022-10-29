using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XrayKunden.Model;
using System.Windows;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Diagnostics.Tracing;
using System.Threading;

namespace XrayKunden.ViewModel
{
    internal class KundenListeViewModel : ViewModelBase
    {


      

      public XrayPostgresConnector KundeToDataGrid = new();

      public XrayPostgresConnector KundeToDatabase = new();     // geht auch mit nur einem XrayPosrgresConnector 
        
      public List<Kunde> TempList = new();

      public  ObservableCollection<Kunde>? DataGridKundenListe { get; set; }


        private bool _filldata;
        public bool FillData
        {
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

        private bool _storeback;
        public bool StoreBack
        {
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
      public string? DataLoadStatus { 
            
            get { 
                
                return _dataLoadStatus; 
            } 
            
            set {

                if (_dataLoadStatus != value)
                   {

                    _dataLoadStatus = value;
                    OnPropertyChanged();
                   }
                }
        }


        

        // Methoden

        public  async void DataFill()
        {
            StoreBack = false;
            
            TempList = await KundeToDataGrid.DataReader();

            if (TempList != null)
            {

                foreach (var item in TempList)
                {
                    DataGridKundenListe?.Add(item);
                }           
               
                DataLoadStatus = "Postgres Daten geladen! ";
                
            
            } else {

                DataLoadStatus = "Fehler beim Laden der Daten! ";
            }
            
            StoreBack = true;

        }


        public async void DataUpdater()
        {
            FillData = false;
            bool checkupdate = false;

            if (DataGridKundenListe != null)
            {
                foreach (var item in DataGridKundenListe)           // Update aller Datensätze!! Nicht nur die geänderten!!
                {
                    string vorname = item.Vorname;
                    string nachname = item.Nachname;
                    string strasse = item.Strasse;
                    string ort = item.Ort;
                    string plz = item.Plz;
                    string kundennr = item.KundenNr;
                    int id = item.ID;                     // int.Parse(item.ID);
                   
                    checkupdate = await KundeToDatabase.DataWriter("UPDATE", vorname, nachname, strasse, ort, plz, kundennr, id);

                    // MessageBox.Show("Check: " + checkupdate + "  " + vorname + " " + id);

                    if (checkupdate)
                    {
                        DataLoadStatus = "Postgres-Datensatz neu gespeichert!";

                    }
                    else
                    {
                        DataLoadStatus = "Fehler beim Postgres speichern!";

                    }
                }

            }
            else { DataLoadStatus = "Fehler DataGridKundenListe - null!"; }



            FillData = true;

        }


       

        // Commands aus View

        public DelegateCommand? DataFillCommand { get; set; }

        public DelegateCommand? DataStoreBack { get; set; }



        // Konstruktor

        public KundenListeViewModel()
        {
            DataLoadStatus = "Keine Daten geladen! ";

            DataGridKundenListe = new(TempList);



            FillData = true;
            DataFill();
            
            this.DataFillCommand = new DelegateCommand(

              (obj) =>
              {
                  DataGridKundenListe.Clear();
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
