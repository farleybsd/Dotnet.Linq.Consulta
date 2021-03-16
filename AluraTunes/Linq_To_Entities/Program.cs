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

                Console.WriteLine();

                Console.WriteLine("GroupBy");

                var queryGroupBy = from inf in contexto.ItemNotaFiscals
                                   where inf.Faixa.Album.Artista.Nome == "Led Zeppelin"
                                   group inf by inf.Faixa.Album into agrupado
                                   let vendasPorAlbum = agrupado.Sum(a => a.Quantidade * a.PrecoUnitario)
                                   orderby vendasPorAlbum
                                   descending
                                   select new
                                   {
                                      TituloDoAlbum = agrupado.Key.Titulo,
                                      TotalPorAlbum = vendasPorAlbum
                                   };
                                  

                foreach (var agrupado in queryGroupBy)
                {
                    Console.WriteLine(
                        "{0}\t{1}",
                        agrupado.TituloDoAlbum.PadRight(40),
                        agrupado.TotalPorAlbum
                        );
                }

                Console.WriteLine();

                Console.WriteLine("Max");
                var contextoMax = new AluraTunesEntities();

                var maiorVenda = contextoMax.NotaFiscals.Max(nf => nf.Total);
                var menorVenda = contextoMax.NotaFiscals.Max(nf => nf.Total);
                var vendaMedia = contextoMax.NotaFiscals.Average(nf => nf.Total);

                Console.WriteLine("A menor venda é de R$ {0}", menorVenda);
                Console.WriteLine("A venda média é de R$ {0}", vendaMedia);

                Console.WriteLine("A maior venda é de R$ {0}", maiorVenda);


                Console.WriteLine("Mediana:");

                var mediana = from nf in contextoMax.NotaFiscals
                              select nf.Total;

                var contagem = mediana.Count();

                var queryordenada = mediana.OrderBy(Total => Total);

               var elementoCentral = queryordenada.Skip(contagem / 2).First();

                var medianaa = elementoCentral;

                Console.WriteLine("Mediana:{0}",medianaa);

                Console.ReadKey();
            }


        }

        private static void GetFaixas(AluraTunesEntities contexto, string buscarArtista,string buscaAlbum)
        {
            var queryLinqtoentitiesrefinandoconsultas = from f in contexto.Faixas
                                                        where f.Album.Artista.Nome.Contains(buscarArtista)
                                                        && (!string.IsNullOrEmpty(buscaAlbum) 
                                                        ?  f.Album.Artista.Nome.Contains(buscaAlbum) 
                                                        : true)
                                                        orderby f.Album.Titulo,f.Nome
                                                        select f;

            //if (!string.IsNullOrEmpty(buscaAlbum))
            //{
            //    queryLinqtoentitiesrefinandoconsultas = queryLinqtoentitiesrefinandoconsultas.Where(q => q.Album.Artista.Nome.Contains(buscaAlbum));
            //}

            //queryLinqtoentitiesrefinandoconsultas = queryLinqtoentitiesrefinandoconsultas.OrderBy(q => q.Album.Titulo).ThenBy(q => q.Nome);

            foreach (var faixa in queryLinqtoentitiesrefinandoconsultas)
            {
                Console.WriteLine("{0}\t{1}", faixa.Album.Titulo.PadRight(40), faixa.Nome);
            }

            Console.WriteLine();
            //1 - Linq to entities count
            Console.WriteLine("1 - Linq to entities count");

            var queryLinqtoentitiescount = from c in contexto.Faixas
                                           where c.Album.Artista.Nome == "Led Zeppelin"
                                           select c;

            // var quantidade = queryLinqtoentitiescount.Count();
            var quantidade = contexto.Faixas
                            .Count(c => c.Album.Artista.Nome == "Led Zeppelin");
            Console.WriteLine($"Led Zeppelin tem {quantidade} musicas na databse");

            Console.WriteLine();
            Console.WriteLine("Notas Fiscais");
            var queryItensNotaFiscal = from inf in contexto.ItemNotaFiscals
                                       where inf.Faixa.Album.Artista.Nome == "Led Zeppelin"
                                       select new { totaldoItem = inf.Quantidade * inf.PrecoUnitario};

            var totoalDoArtista = queryItensNotaFiscal.Sum(inf => inf.totaldoItem);

            Console.WriteLine($"Total do artista em R$ {totoalDoArtista}");




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
