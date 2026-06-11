using System;
using System.Collections.Generic;

namespace tpfinal
{
    public struct DatoCSV
    {
        public string geoTipo;
        public string lugar;
        public string periodo;
    }

    public class Backend
    {
        public static List<DatoCSV> datos = new List<DatoCSV>();
        public static List<Dato> nodosArbol = new List<Dato>();
        public static List<int> padres = new List<int>();
        public static int raizIdx = -1;
        public static Graph<Dato> grafo = new Graph<Dato>();

        public static void armarArbolDesdeDatos()
        {
            nodosArbol.Clear();
            padres.Clear();
			
            if (datos.Count == 0) return;
			
            // indices para no repetir nodos
            Dictionary<string, int> indices = new Dictionary<string, int>();
            raizIdx = 0;
			
            Dato raiz = new Dato(0, "RAIZ", "Todos los registros");
            nodosArbol.Add(raiz);
            padres.Add(-1);
            indices.Add("RAIZ", 0);
			
            int cont = 1;
			
            for (int fila = 0; fila < datos.Count; fila++)
            {
                var registro = datos[fila];
                string geoTipo = registro.geoTipo;
                string lugar = registro.lugar;
                string periodo = registro.periodo;
				
                string cGeo = "GT_" + geoTipo;
                string cLug = "LP_" + geoTipo + "_" + lugar;
                string cPer = "PR_" + lugar + "_" + periodo;
				
                int idxGeo = -1;
                if (!indices.ContainsKey(cGeo))
                {
                    Dato nGeo = new Dato(0, geoTipo, "Tipo geografico");
                    nodosArbol.Add(nGeo);
                    padres.Add(raizIdx);
                    idxGeo = cont;
                    indices.Add(cGeo, cont);
                    cont++;
                }
                else
                {
                    idxGeo = indices[cGeo];
                }
				
                int idxLugar = -1;
                if (!indices.ContainsKey(cLug))
                {
                    Dato nLug = new Dato(0, lugar, "Lugar: " + geoTipo);
                    nodosArbol.Add(nLug);
                    padres.Add(idxGeo);
                    idxLugar = cont;
                    indices.Add(cLug, cont);
                    cont++;
                }
                else
                {
                    idxLugar = indices[cLug];
                }
				
                int idxPeriodo = -1;
                if (!indices.ContainsKey(cPer))
                {
                    Dato nPer = new Dato(0, geoTipo + " - " + periodo + " - " + lugar, "Medicion en " + lugar);
                    nodosArbol.Add(nPer);
                    padres.Add(idxLugar);
                    idxPeriodo = cont;
                    indices.Add(cPer, cont);
                    cont++;
                }
                else
                {
                    idxPeriodo = indices[cPer];
                }
				
                if (idxPeriodo >= 0)
                {
                    nodosArbol[idxPeriodo].ocurrencia++;
                }
            }

            // Construir grafo con lista de adyacencia a partir de las listas paralelas
            grafo = new Graph<Dato>();
            foreach (var nodo in nodosArbol)
                grafo.AddVertex(nodo);

            for (int i = 0; i < padres.Count; i++)
            {
                if (padres[i] != -1)
                    grafo.AddEdge(padres[i], i);
            }
        }

        public static List<string> ObtenerPredicciones()
        {
            var resultado = new List<string>();
            var g = grafo;
            for (int i = 0; i < g.Count; i++)
            {
                if (g.EsHoja(i) && nodosArbol[i].ocurrencia > 0)
                {
                    for (int r = 0; r < nodosArbol[i].ocurrencia; r++)
                        resultado.Add(nodosArbol[i].texto);
                }
            }
            return resultado;
        }

        public static String Consulta1()
        {
            var preds = ObtenerPredicciones();
            return Estrategia.Consulta1(preds);
        }

        public static String Consulta2()
        {
            var preds = ObtenerPredicciones();
            return Estrategia.Consulta2(preds);
        }

        public static String Consulta3()
        {
            var preds = ObtenerPredicciones();
            return Estrategia.Consulta3(preds);
        }

        public static String preorden()
        {
            return Estrategia.RecorridoPreorden(nodosArbol, padres, raizIdx);
        }

        public static String inorden()
        {
            return Estrategia.RecorridoInorden(nodosArbol, padres, raizIdx);
        }

        public static String postorden()
        {
            return Estrategia.RecorridoPostorden(nodosArbol, padres, raizIdx);
        }

        public static void buscar(bool heapOP, int cantidad, List<Dato> collected)
        {
			
            if (heapOP)
            {
                Estrategia.BuscarConHeap(nodosArbol, padres, cantidad, collected);
            }
            else
            {
                Estrategia.BuscarConOrden(nodosArbol, padres, cantidad, collected);
            }
			
        }
    }

}
