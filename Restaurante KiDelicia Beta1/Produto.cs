using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurante_KiDelicia_Beta1
{
    internal class Produto{
        public string Nome { get; set; }
        public double Valor { get; set; }
        public String ParaString(string quantidade, string nome, double preço)
        {
            return (quantidade + "x - " + nome + " - R$" + preço);
        }
    }

}
