using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiDocuments.Data;
using ApiDocuments.Models;
using System.IO;
using System.Web;
using System.Net.Http;
using System.Net.Mime;
using System.Net.Http.Headers;
using System.Data;





namespace ApiDocuments.Controllers
{
    [Route("api/[controller]")]

    public class DocumentsController : Controller
    {


        private DocumentsDL documentsDL;
        private ApiKeyDL apiKeyDL;
        private String path = "Files\\";
        
        public DocumentsController()
        {
            documentsDL = new DocumentsDL("Documents.dat");
            apiKeyDL = new ApiKeyDL("api_key.dat");

        }
        [HttpGet("{key}")]
        public ActionResult Get()
        {
            try
            {
                return base.Ok(documentsDL.Get());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: DocumentsController/Details/5
        [HttpGet("{id}/{key}")]

        public ActionResult Download(int id,string key)
        {
            if (!apiKeyDL.Exist(key))
            {
                return BadRequest("key not exits");

            }
            if (id > 0)
            {
                var document = documentsDL.Get(id);
                if (document.Id == 0)
                {
                    return BadRequest("id not exits");

                }
                var filePath = path + document.Name + document.type;

                byte[] bytes = System.IO.File.ReadAllBytes(filePath);

                return File(bytes, "application/octet-stream",  document.Name + document.type);
            }
            else
            {

                return BadRequest("id not exits");
            } 
        }
        // POST: DocumentsController/Create
        [HttpPost("{key}")]
        public ActionResult PostDocuments([FromForm] List<IFormFile> files,string key)
        {
            try
            {

                if (!apiKeyDL.Exist(key))
                {
                    return BadRequest("key not exits");

                }
                if (!(files.Count > 0))
                {
                    return BadRequest("no se encontraron documentos en la request");
                }
                foreach (var file in files)
                {

                    using (var stream = System.IO.File.Create(path + file.FileName))
                    {
                        file.CopyToAsync(stream);
                    }
                    double length = file.Length;
                    double size = Math.Round(length / 1000000, 2);
                    Documents document = new Documents();
                    document.type = Path.GetExtension(file.FileName);
                    document.Name = Path.GetFileNameWithoutExtension(file.FileName);
                    document.size = size;
                    documentsDL.Save(document);
                }
                return Ok("almacenados con exito");
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpDelete("{id}/{key}")]
        public ActionResult Delete(int id)
        {
            if (id > 0)
            {
                var document = documentsDL.Get(id);
                if (document.Id == 0)
                {
                    return BadRequest("id not exits");

                }
                documentsDL.Delete(id);

                System.IO.File.Delete(path + document.Name + document.type);
                return Ok("delte File");
                
            }
            else
            {

                return BadRequest("id not exits");
            }
        }

        
    }
}
