using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XrayKunden.Model
{
    internal class FileService
    {
        public async Task<string> PHPdataPath(string filename)
        {
            string? _urlstring = "";

            try
            {
                using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read)) // kein ";" da Dispose beide!! - geht auch nur mit Streamreader

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    _urlstring = await reader.ReadLineAsync();
                }

                if (string.IsNullOrEmpty(_urlstring))
                {
                    MessageBox.Show("Kein Pfad in Datei phpconnect.txt!");
                    return _urlstring = "";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("PHPdataInPath: Dateilesefehler aufgetreten!" + ex.Message);
                _urlstring = "";

            }


            return _urlstring;
        }


    }
}
