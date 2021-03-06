﻿using System.Collections.Generic;
using System.Linq;
using ApiDocuments.Data;
using ApiDocuments.Models;
using Newtonsoft;
using Newtonsoft.Json;

namespace ApiDocuments
{
    partial class DocumentsDL
    {
        private string File { get; set; }
        //Clase que administra los archivos guardados
        private DataAccess DataAccess;
        private List<Documents> FileList;
        private string FileData;

        public DocumentsDL(string file)
        {
            this.File = file;
            this.DataAccess = new DataAccess(this.File);
        }

        private void Read()
        {
            //Leeo el archivo
            this.FileData = this.DataAccess.Read();
            //Convierto el archivo a una lista de archivos, si es que tiene datos
            this.FileList = this.FileData?.Length > 0 ? JsonConvert.DeserializeObject<List<Documents>>(this.FileData) : new List<Documents>();
        }

        private void Save()
        {
            //Convierto los datos a string 
            this.FileData = JsonConvert.SerializeObject(this.FileList);
            //guardo los datos en el archivo
            this.DataAccess.Save(this.FileData);
        }

        public int Save(Documents file)
        {
            Read();
            int id = 1;
            file.Id = FileList.Count > 0 ? this.FileList.Max(x => x.Id) + 1 : id;
            //Si no existte inserta uno nuevo
            this.FileList.Add(file);
            //Agergo el archivo nueva a la lista de archivos
            Save();
            return id;
        }
   
        public void Delete(int id)
        {
            Read();
            //Busco la persona con el 
            Documents file = new Documents();
            if (FileList.Count > 0)
            {
                file = this.FileList.First(x => x.Id == id);
            }
            //Si la encontro la borro
            if (file.Id > 0)
            {
                this.FileList.Remove(file);
            }
            Save();
        }

        public List<Documents> Get()
        {
            Read();
            return this.FileList;
        }

        public Documents Get(int id)
        {
            Read();
            Documents document = new Documents();
            if (FileList.Count > 0)
            {
                document = this.FileList.FirstOrDefault(x => x.Id == id);
            }
            return document;
        }
    }
}
