using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Business;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuscaController : ControllerBase
    {
        IJogoService _jogoService;
        public BuscaController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }
    }
    public class EntrarMesaReq
    {
        public int MesaId { get; set; }
    }
    public class EntrarMesaResp
    {
        public string Mensagem { get; set; }
        public int Status { get; set; }
        public string Url { get; set; }
    }
}