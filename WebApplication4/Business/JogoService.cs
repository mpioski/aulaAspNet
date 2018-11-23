using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using WebApplication4.Data;
using WebApplication4.Models;

namespace WebApplication4.Business
{
    public class JogoService : IJogoService
    {
        WebApplication4Context _context;
        public JogoService(WebApplication4Context context)
        {
            _context = context;
        }
        public List<Mesa> ListarMesasDoUsuario(string cpf)
        {
            Usuario usuario = _context.Usuario
                .Include(u => u.MesasUsuarios)
                .ThenInclude(mu => mu.Mesa)
                .ThenInclude(m => m.MesasUsuarios)
                .ThenInclude(mu2 => mu2.Usuario)
                .Where(u=>u.Cpf == cpf).FirstOrDefault();
            List<Mesa> mesas = new List<Mesa>();
            if (usuario != null && usuario.MesasUsuarios != null)
            {
                foreach (MesaUsuario mu in usuario.MesasUsuarios)
                {
                    mesas.Add(mu.Mesa);
                }
            }
            return mesas;
        }
        public List<Mesa> ListarMesasDoUsuarioPeloNome(string nome)
        {
            Usuario usuario = _context.Usuario
                .Include(u => u.MesasUsuarios)
                .ThenInclude(mu => mu.Mesa)
                .ThenInclude(m => m.MesasUsuarios)
                .ThenInclude(mu2 => mu2.Usuario)
                .Where(u => u.Nome.Contains(nome)).FirstOrDefault();
            List<Mesa> mesas = new List<Mesa>();
            if (usuario != null && usuario.MesasUsuarios != null)
            {
                foreach (MesaUsuario mu in usuario.MesasUsuarios)
                {
                    mesas.Add(mu.Mesa);
                }
            }
            return mesas;
        }
        public List<Mesa> ListarMesa(int usuarioId)
        {
            List<MesaUsuario> mesasUsuarios =
                _context.MesasUsuarios
                .Include(mu => mu.Mesa) // mesas do usuarioId
                .ThenInclude(m => m.MesasUsuarios) // os usuários na mesa
                .ThenInclude(mu2 => mu2.Usuario) // nomes dos usuários
                .Where(mu => mu.UsuarioId == usuarioId).ToList();
            List<Mesa> mesas = new List<Mesa>();
            foreach(MesaUsuario mu in mesasUsuarios)
            {
                mesas.Add(mu.Mesa);
            }
            return mesas;
        }
        public void MontarMesa(string nome, int usuarioId)
        {
            Mesa mesa = new Mesa { Nome = nome };
            MesaUsuario mesaUsuario = new MesaUsuario
            {
                UsuarioId = usuarioId,
                Mesa = mesa
            };
            _context.MesasUsuarios.Add(mesaUsuario);
            _context.SaveChanges();
        }
        public void EntrarMesa(int usuarioId, int mesaId)
        {
            int count = _context.MesasUsuarios
                .Where(m1 => m1.UsuarioId == usuarioId &&
                        m1.MesaId == mesaId).Count();
            if (count > 0)
            {
                throw new JaEstaNaMesaException();
            }
            count = _context.MesasUsuarios
                .Where(m1 => m1.MesaId == mesaId).Count();
            if (count >= 2)
            {
                throw new MesaCheiaException();
            }
            MesaUsuario mu = new MesaUsuario {
                MesaId = mesaId,
                UsuarioId = usuarioId
            };
            _context.MesasUsuarios.Add(mu);
            _context.SaveChanges();
        }
        public void SalvarEstadoMesa(int mesaId,
            string estado, string historico)
        {
            Mesa mesa = new Mesa
            {
                MesaId = mesaId,
                Estado = estado,
                Historico = historico
            };
            _context.Attach(mesa);
            _context.Entry(mesa).Property(m => m.Historico).IsModified = true;
            _context.Entry(mesa).Property(m => m.Estado).IsModified = true;
            _context.SaveChanges();
        }
        public void SalvarEstadoMesa(int mesaId,
            string config,
            string estado, string historico)
        {
            Mesa mesa = new Mesa
            {
                MesaId = mesaId,
                Estado = estado,
                Historico = historico,
                Configuracao = config
            };
            _context.Attach(mesa);
            _context.Entry(mesa).Property(m => m.Historico).IsModified = true;
            _context.Entry(mesa).Property(m => m.Estado).IsModified = true;
            _context.Entry(mesa).Property(m => m.Configuracao).IsModified = true;
            _context.SaveChanges();
        }
        public void SalvarEstadoMesa()
        {
            _context.SaveChanges();
        }
        public Mesa DadosMesa(int mesaId)
        {
            return _context.Mesas
                .Include(m => m.MesasUsuarios)
                .ThenInclude(mu => mu.Usuario)
                .Where(m => m.MesaId == mesaId).FirstOrDefault();
        }
    }

    [Serializable]
    internal class MesaCheiaException : Exception
    {
        public MesaCheiaException()
        {
        }

        public MesaCheiaException(string message) : base(message)
        {
        }

        public MesaCheiaException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MesaCheiaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    internal class JaEstaNaMesaException : Exception
    {
        public JaEstaNaMesaException()
        {
        }

        public JaEstaNaMesaException(string message) : base(message)
        {
        }

        public JaEstaNaMesaException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected JaEstaNaMesaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public interface IJogoService
    {
        List<Mesa> ListarMesa(int usuarioId);
        List<Mesa> ListarMesasDoUsuario(string cpf);
        List<Mesa> ListarMesasDoUsuarioPeloNome(string nome);
        void EntrarMesa(int usuarioId, int mesaId);
        void MontarMesa(string nome, int usuarioId);
        Mesa DadosMesa(int mesaId);
        void SalvarEstadoMesa(int mesaId,
            string config,
            string estado, string historico);
        void SalvarEstadoMesa();
    }
}
