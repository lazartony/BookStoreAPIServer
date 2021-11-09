using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BookStoreAPIServer.Controllers
{
    public class ImageController : ApiController
    {
        [Route("api/Image/Upload")]
        [HttpPost]
        public HttpResponseMessage Upload()
        {
            string path = HttpContext.Current.Server.MapPath("~/Uploads/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //Fetch the File.
            HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];

            //Fetch the File Name.
            string fileName = Path.GetFileName(postedFile.FileName);
            string newFileName = $@"{DateTime.Now.Ticks}.{fileName.Split('.').Last()}";

            //Save the File.
            postedFile.SaveAs(path + newFileName);
            return Request.CreateResponse(HttpStatusCode.Created, $@"https://{Request.RequestUri.Host}:{Request.RequestUri.Port}/Uploads/{newFileName}");
        }
    }
}
