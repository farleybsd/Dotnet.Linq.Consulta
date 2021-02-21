using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Linq_To_Xml
{
    class Program
    {
        static void Main(string[] args)
        {
            XElement root = XElement.Load(@"Data\AluraTunes.xml");

            var queryXML = from g in root.Element("Generos").Elements("Genero")
                           select g;
            Imprimir(queryXML);

            var query = from g in root.Element("Generos").Elements("Genero")
                        join m in root.Element("Musicas").Elements("Musica")
                             on g.Element("GeneroId").Value equals m.Element("GeneroId").Value
                        select new { 
                            Musica = m.Element("Nome").Value,
                            Genero = m.Element("Nome").Value,
                        };


            Console.WriteLine();

            foreach (var musicaEGenero in query)
            {
                Console.WriteLine("{0}\t{1}", musicaEGenero.Musica, musicaEGenero.Genero);
            }

            Console.ReadKey();
        }

        private static void Imprimir(IEnumerable<XElement> queryXML)
        {
            foreach (var genero in queryXML)
            {
                Console.WriteLine("{0}\t{1}",genero.Element("GeneroId").Value, genero.Element("Nome").Value);
            }
        }
    }
}
