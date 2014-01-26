using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maximo
{
    public class Maximo
    {
        public static int GetMax(int[] lista)
        {
            int indice, max = Int32.MinValue;

            for (indice = 0; indice < lista.Length; indice++)
            {
                if (lista[indice] > max)
                {
                    max = lista[indice];
                }
            }
            return max;
        }

        static void Main(string[] args)
        {



        }
    }

}
