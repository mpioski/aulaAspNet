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
    public class MinhasMesasController : ControllerBase
    {
        IJogoService _jogoService;
        public MinhasMesasController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }
    }
    public class CriarMesaReq
    {
        public string Nome { get; set; }
    }
}