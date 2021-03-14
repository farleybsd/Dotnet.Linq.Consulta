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
                                    select new { f, g };

                Console.WriteLine();
                faixaEGeneros = faixaEGeneros.Take(10);
                Console.WriteLine();
                //contexto.Database.Log = Console.WriteLine; // Depurar Sql

                foreach (var faixaEGenero in faixaEGeneros)
                {
                    Console.WriteLine("{0}\t{1}", faixaEGenero.f.Nome, faixaEGenero.g.Nome);
                }


                Console.WriteLine();
                //1 - Linq to entities join

                Console.WriteLine("Linq to entities join");
                var textoBusca = "Led";

                var queryBuscar = from a in contexto.Artistas
                                  join alb in contexto.Albums
                                  on a.ArtistaId equals alb.ArtistaId
                                  where a.Nome.Contains(textoBusca)
                                  select new
                                  {
                                      NomeArtista = a.Nome,
                                      NomeAlbum = alb.Titulo
                                  };

                foreach (var item in queryBuscar)
                {
                    Console.WriteLine("{0}\t{1}", item.NomeArtista, item.NomeAlbum);
                }

                Console.WriteLine();
                //2 - Linq to entities sem join

                Console.WriteLine("2 - Linq to entities sem join");

                var queryLinqtoentitiessemjoin = from alb in contexto.Albums
                                                 where alb.Artista.Nome.Contains(textoBusca)
                                                 select new
                                                 {
                                                     NomeArtista = alb.Artista.Nome,
                                                     NomeAlbum = alb.Titulo

                                                 };

                foreach (var item in queryLinqtoentitiessemjoin)
                {
                    Console.WriteLine("{0}\t{1}", item.NomeArtista, item.NomeAlbum);
                }


                Console.WriteLine();
                //3 - Linq to entities refinando consultas
                Console.WriteLine("3 - Linq to entities refinando consultas");

                var buscarArtista = "Led Zeppelin";
                var buscaAlbum = "Graffiti";

                GetFaixas(contexto, buscarArtista, buscarArtista);

                Console.ReadKey();
            }


        }

        private static void GetFaixas(AluraTunesEntities contexto, string buscarArtista,string buscaAlbum)
        {
            var queryLinqtoentitiesrefinandoconsultas = from f in contexto.Faixas
                                                        where f.Album.Artista.Nome.Contains(buscarArtista)
                                                        select f;

            if (!string.IsNullOrEmpty(buscaAlbum))
            {
                queryLinqtoentitiesrefinandoconsultas = queryLinqtoentitiesrefinandoconsultas.Where(q => q.Album.Artista.Nome.Contains(buscaAlbum));
            }

            foreach (var faixa in queryLinqtoentitiesrefinandoconsultas)
            {
                Console.WriteLine("{0}\t{1}", faixa.Album.Titulo.PadRight(40), faixa.Nome);
            }
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
