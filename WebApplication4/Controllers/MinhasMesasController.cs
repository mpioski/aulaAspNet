using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Business;

namespace WebApplication4.Controllers
{
    [Route("api/MinhasMesas")]
    [ApiController]
    public class MinhasMesasController : ControllerBase
    {
        IJogoService _jogoService;
        public MinhasMesasController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }
        // http://kkkk:123/api/MinhasMesas
        // {Nome : "mesa 2"}
        [HttpPost]
        public ActionResult CriarMesa(CriarMesaReq req)
        {
            int? usuarioId = HttpContext.Session.GetInt32("usuarioId");
            if (!usuarioId.HasValue)
            {
                return Unauthorized(); // erro HTTP 401
            }
            _jogoService.MontarMesa(req.Nome, usuarioId??0);
            return Ok(new { resp = "Ok" });
        }
        // POST http://localhost:123/api/MinhasMesas/sair
        // mesaId=123
        [HttpPost("sair")]
        public IActionResult SairMesa([FromForm] int mesaId)
        {
            int? usuarioId = HttpContext.Session.GetInt32("usuarioId");
            if (!usuarioId.HasValue)
            {
                return Unauthorized(); // erro HTTP 401
            }
            if (mesaId == 0)
            {
                return BadRequest("Id da mesa é inválido");
            }
            _jogoService.SairMesa(usuarioId ?? 0, mesaId);
            // {resp:"Ok"}
            return Ok(new { resp = "Ok" });
        }
    }
    public class CriarMesaReq
    {
        public string Nome { get; set; }
    }
}