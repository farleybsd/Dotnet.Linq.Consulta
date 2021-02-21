using Linq_To_Entities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq_To_Entities
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var contexto = new AluraTunesEntities())
            {
                // Definição de uma consulta
                var query = from g in contexto.Generos
                            select g;

                // imprimir no console
                ImprimirGenero(query);

                var faixaEGeneros = from g in contexto.Generos
                                   join f in contexto.Faixas
                                   on g.GeneroId equals f.GeneroId
                                   select new{f,g};

                Console.WriteLine();
                faixaEGeneros = faixaEGeneros.Take(10);
                Console.WriteLine();
                contexto.Database.Log = Console.WriteLine; // Depurar Sql

                foreach (var faixaEGenero in faixaEGeneros)
                {
                    Console.WriteLine("{0}\t{1}",faixaEGenero.f.Nome, faixaEGenero.g.Nome);
                }
            }


            Console.ReadKey();
        }

        private static void ImprimirGenero(IQueryable<Genero> query)
        {
            foreach (var genero in query)
            {
                Console.WriteLine("{0}\t{1}", genero.GeneroId, genero.Nome);
            }
        }
    }
}
