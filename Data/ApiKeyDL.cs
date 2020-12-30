using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiDocuments.Models;
using Newtonsoft.Json;



namespace ApiDocuments.Data
{
    partial class ApiKeyDL
    {
        private string File { get; set; }
        //Clase que administra los archivos guardados
        private DataAccess DataAccess;
        private List<ApiKey> FileList;
        private string FileData;

        public ApiKeyDL(string file)
        {
            this.File = file;
            this.DataAccess = new DataAccess(this.File);
        }

        private void Read()
        {
            //Leeo el archivo
            this.FileData = this.DataAccess.Read();
            //Convierto el archivo a una lista de archivos, si es que tiene datos
            this.FileList = this.FileData?.Length > 0 ? JsonConvert.DeserializeObject<List<ApiKey>>(this.FileData) : new List<ApiKey>();
        }

        private void Save()
        {
            //Convierto los datos a string 
            this.FileData = JsonConvert.SerializeObject(this.FileList);
            //guardo los datos en el archivo
            this.DataAccess.Save(this.FileData);
        }

        public int Save(ApiKey file)
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
            Models.ApiKey file = new Models.ApiKey();
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

        public List<Models.ApiKey> Get()
        {
            Read();
            return this.FileList;
        }

        public bool Exist(string code)
        {
            Read();

            ApiKey api = new ApiKey();
            if (FileList.Count > 0)
            {
                api = this.FileList.FirstOrDefault(x => x.code.Equals(code));
            }
            return api?.Id > 0;
        }
        public bool isVoid()
        {
            Read();
            return FileList.Count > 0;
        }
        public Models.ApiKey Get(int id)
        {
            Read();
            Models.ApiKey document = new Models.ApiKey();
            if (FileList.Count > 0)
            {
                document = this.FileList.FirstOrDefault(x => x.Id == id);
            }
            return document;
        }
    }
}
