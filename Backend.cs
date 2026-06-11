using System;
using tp1;

namespace tpfinal
{ 
    public class Backend
    {
        public static List<string> datos = new List<string>();
        public static List<Dato> nodosArbol = new List<Dato>();
        public static List<int> padres = new List<int>();
        public static int raizIdx = -1;

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
                string registro = datos[fila];
                string[] partes = registro.Split(new char[] { '-' }, 3);
                if (partes.Length < 3) continue;
				
                string geoTipo = partes[0].Trim();
                string lugar = partes[1].Trim();
                string periodo = partes[2].Trim();
				
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
                    Dato nPer = new Dato(0, periodo, "Medicion en " + lugar);
                    nodosArbol.Add(nPer);
                    padres.Add(idxLugar);
                    idxPeriodo = cont;
                    indices.Add(cPer, cont);
                    cont++;
                }
				
                if (idxPeriodo >= 0)
                {
                    nodosArbol[idxPeriodo].ocurrencia++;
                }
            }
        }

        public static String aProfundidad()
        {
            return (new Estrategia()).Consulta3(nodosArbol, padres, raizIdx);
        }

        public static String caminoAPrediccion()
        {
            return (new Estrategia()).Consulta2(nodosArbol, padres, raizIdx);
        }

        public static String todasLasPredicciones()
        {
            return (new Estrategia()).Consulta1(nodosArbol, padres, raizIdx);
        }

        public static void buscar(bool heapOP, int cantidad, List<Dato> collected)
        {
			
            if (heapOP)
            {
                (new Estrategia()).BuscarConHeap(nodosArbol, padres, cantidad, collected);
            }
            else
            {
                (new Estrategia()).BuscarConOtro(nodosArbol, padres, cantidad, collected);
            }
			
        }
    }

}
