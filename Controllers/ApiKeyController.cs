using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using ApiDocuments.Data;
using ApiDocuments.Models;

namespace ApiDocuments.Controllers
{
    [Route("api/[controller]")]

    public class ApiKeyController : Controller
    {
        private ApiKeyDL apiKeyDL;
        private static Random random = new Random();

        public ApiKeyController()
        {
            apiKeyDL = new ApiKeyDL("api_key.dat");
            if(!apiKeyDL.isVoid())
            {
                ApiKey apikey = new ApiKey();
                apikey.code = "123456";
                apiKeyDL.Save(apikey);
            }
        }


        [HttpPost("recovery/{code}")]

        public ActionResult Post(double code)
        {
            try
            {
                if(code == 2750636519913847)
                { 
                return Ok(apiKeyDL.Get());
                }
                return BadRequest("fail code");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: DocumentsController/Create
        [HttpPost("{key}")]
        public ActionResult Post(string key)
        {
            try
            {
                if (!apiKeyDL.Exist(key)) return BadRequest("fail key");
                apiKeyDL.Delete(1);
                ApiKey apikey = new ApiKey();
                apikey.code = RandomString(30);
                apiKeyDL.Save(apikey);
                return Ok(apikey);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

   

    }
}
