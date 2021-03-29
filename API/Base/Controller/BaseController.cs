using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using API.Repository.Interface;
using API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Base.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseController<Entity, Repository, Key> : ControllerBase
        where Entity : class
        where Repository : IRepository<Entity, Key>
    {
        private readonly Repository repository;
        public BaseController(Repository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public virtual ActionResult<Entity> Get()
        {
            var result = repository.Get();

            if (result != null)
            {
                return Ok(new { status = HttpStatusCode.OK, data = result, message = "Data Ditemukan" });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, data = result, message = "Terjadi Kesalahan" }); ;
            }
        }

        [HttpGet("{key}")]
        public virtual ActionResult<Entity> Get(Key key)
        {
            var result = repository.Get(key);
            if (result != null)
            {
                return Ok(new { status = HttpStatusCode.OK, data = result, message = "Data Ditemukan" });
            }
            else
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data Tidak Ditemukan", data = "" });
            }
        }

        [HttpPost]
        public ActionResult Post(Entity entity)
        {
            var result = repository.Insert(entity);
            if (result > 0)
            {
                return Ok(new { status = HttpStatusCode.OK, data = "", message = "Berhasil Membuat Data Baru" });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Gagal Membuat Data Baru", data = "" });
            }
        }

        [HttpDelete("{key}")]
        public ActionResult Delete(Key key)
        {
            var result = repository.Delete(key);
            if (result > 0)
            {
                return Ok(new { status = HttpStatusCode.OK, data = "", message = "Berhasil Menghapus Data Baru" });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Gagal Menghapus Data Baru", data = "" });
            }
        }

        [HttpPut]
        public virtual ActionResult Put(Entity entity)
        {
            if (entity == null)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Data Yang Di-Input Salah / Tidak Lengkap", data = "" });
            }

            var result = repository.Update(entity);

            if (result > 0)
            {
                return Ok(new { status = HttpStatusCode.OK, data = "", message = "Berhasil Update Data" });
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Gagal Update Data", data = "" });
            }
        }

        [HttpPost("SendEmail")]
        [AllowAnonymous]
        public IActionResult SendEmail(SendEmailVM sendEmailVM)
        {
            string to = sendEmailVM.Email; //To address    
            string from = "xianlanzou@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);

            message.Subject = sendEmailVM.MessageSubject;
            message.Body = sendEmailVM.MessageBody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential(from, "akiyam4mio");

            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = basicCredential1;

            try
            {
                client.Send(message);
                return Ok(new { status = HttpStatusCode.OK, Message = "Message send Successfully", data = "" });
            }
            catch
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "Message failed to send", data = "" });
            }
        }
    }
}