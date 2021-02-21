using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AluraTunes
{
    class Program
    {
        static void Main(string[] args)
        {
            // Listar os gêneros com a palavra rock
            var generos = new List<Genero>
            {
            new Genero { Id = 1, Nome = "Rock"},
            new Genero { Id = 2, Nome =  "Reggae"},
            new Genero { Id = 3, Nome =  "Rock Progressivo"},
            new Genero { Id = 4, Nome =  "Punk"},
            new Genero { Id = 5, Nome =  "Clássica"}
            };

            // Sem Linq
            Imprimir(generos);

            var query = from g in generos
                        where g.Nome.Contains("Rock")
                        select g;

            Console.WriteLine();

            // Com Linq
            ImprimirGeneroLinq(query);

            // Linq = Linguagem Integrated Query = Consulta integrada a linguagem

            Console.ReadKey();
        }

        private static void ImprimirGeneroLinq(IEnumerable<Genero> query)
        {
            foreach (var genero in query)
            {
                Console.WriteLine("{0}\t{1}", genero.Id, genero.Nome);
            }
        }

        private static void Imprimir(List<Genero> generos)
        {
            foreach (var genero in generos)
            {
                if(genero.Nome.Contains("Rock"))
                Console.WriteLine("{0}\t{1}",genero.Id,genero.Nome);
            }
        }
    }

    class Genero
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
