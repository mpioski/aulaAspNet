using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Data
{
    public class TipoJogo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TipoJogoId { get; set; }
        public string Nome { get; set; }
        public string Url { get; set; }
        public int MinJogadores { get; set; }
        public int MaxJogadores { get; set; }
        public ICollection<Mesa> Mesas { get; set; }
    }

}
