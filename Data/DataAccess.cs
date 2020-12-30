using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDocuments.Data
{
    class DataAccess
    {
        private string DataFile;

        public DataAccess(string file)
        {
            this.DataFile = file;
            if (!File.Exists(this.DataFile))
            {
                //Si no existe crea el archivo
                FileStream fs = File.Create(this.DataFile);
                fs.Close();
            }

        }

        public void Save(string values)
        {
            //Borro el archivo anterior
            if (File.Exists(this.DataFile))
            {
                File.Delete(this.DataFile);
            }
            //Creo el archivo nuevo con la informacion actualizadad
            using (FileStream fs = File.Create(this.DataFile))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(values);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
        }

        public string Read()
        {
            // Open the stream and read it back.
            string s = "";
            using (StreamReader sr = File.OpenText(this.DataFile))
            {
                s = sr.ReadLine();
            }
            return s;
        }
    }
}
