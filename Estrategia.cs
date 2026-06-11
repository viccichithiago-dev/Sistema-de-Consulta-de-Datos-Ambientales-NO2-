using System;
using System.Text;
using System.Collections.Generic;

namespace tpfinal
{

    public static class Estrategia
    {

        // ================================================================
        // CONSULTA 1: Benchmark de tiempos BuscarConHeap vs BuscarConOrden
        // ================================================================
        public static string Consulta1(List<string> datos)
        {
            if (datos == null || datos.Count == 0)
                return "=== CONSULTA 1 ===\nNo hay datos para procesar.\n";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== CONSULTA 1: COMPARATIVA DE TIEMPOS (TOP 5) ===");
            sb.AppendLine("");

            var conteo = ContarOcurrencias(datos);
            sb.AppendLine("Elementos distintos: " + conteo.Count);
            sb.AppendLine("Total datos de entrada: " + datos.Count);
            sb.AppendLine("");

            var sw = new System.Diagnostics.Stopwatch();

            // --- BuscarConHeap ---
            var collectedHeap = new List<Dato>();
            sw.Start();
            BuscarConHeap(datos, 5, collectedHeap);
            sw.Stop();
            long tiempoHeap = sw.ElapsedTicks;

            sb.AppendLine(">> Top 5 con BuscarConHeap:");
            sb.AppendLine("");
            int pos = 1;
            foreach (var d in collectedHeap)
            {
                sb.AppendLine("  #" + pos + " \"" + d.texto + "\"  -> " + d.ocurrencia + " ocurrencias");
                pos++;
            }
            sb.AppendLine("");
            sb.AppendLine("  Tiempo: " + tiempoHeap + " ticks");
            sb.AppendLine("");

            // --- BuscarConOrden ---
            var collectedOrden = new List<Dato>();
            sw.Restart();
            BuscarConOrden(datos, 5, collectedOrden);
            sw.Stop();
            long tiempoOrden = sw.ElapsedTicks;

            sb.AppendLine(">> Top 5 con BuscarConOrden (Bubble Sort):");
            sb.AppendLine("");
            pos = 1;
            foreach (var d in collectedOrden)
            {
                sb.AppendLine("  #" + pos + " \"" + d.texto + "\"  -> " + d.ocurrencia + " ocurrencias");
                pos++;
            }
            sb.AppendLine("");
            sb.AppendLine("  Tiempo: " + tiempoOrden + " ticks");
            sb.AppendLine("");

            // --- Comparativa ---
            sb.AppendLine("--- COMPARATIVA ---");
            if (tiempoHeap > 0 && tiempoOrden > 0)
            {
                double factor = (double)Math.Max(tiempoOrden, tiempoHeap) / (double)Math.Min(tiempoOrden, tiempoHeap);
                string masRapido = (tiempoHeap < tiempoOrden) ? "Heap" : "Bubble Sort";
                sb.AppendLine("  " + masRapido + " fue " + factor.ToString("F2") + "x mas rapido");
            }
            sb.AppendLine("  Diferencia absoluta: " + Math.Abs(tiempoOrden - tiempoHeap) + " ticks");
            sb.AppendLine("");

            return sb.ToString();
        }


        // ================================================================
        // CONSULTA 2: Camino a la hoja mas izquierda del Heap
        // ================================================================
        public static string Consulta2(List<string> datos)
        {
            if (datos == null || datos.Count == 0)
                return "=== CONSULTA 2 ===\nNo hay datos para procesar.\n";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== CONSULTA 2: CAMINO A LA HOJA MAS IZQUIERDA DEL HEAP ===");
            sb.AppendLine("");

            var conteo = ContarOcurrencias(datos);
            var heap = BuildHeap(conteo);

            sb.AppendLine("Total elementos distintos insertados en el heap: " + conteo.Count);
            sb.AppendLine("Tamano del heap: " + heap.Count);
            sb.AppendLine("");

            // Navegar a la hoja mas izquierda
            sb.AppendLine(">> Camino desde la raiz hasta la hoja mas izquierda:");
            sb.AppendLine("");
            int idx = 0;
            int paso = 0;
            while (true)
            {
                int nivel = (int)Math.Floor(Math.Log2(idx + 1));
                string marca = "";
                int hijoIzq = 2 * idx + 1;
                if (hijoIzq >= heap.Count) marca = " [HOJA MAS IZQUIERDA]";

                sb.AppendLine("  Paso #" + (paso + 1) + " | Nivel " + nivel + " | \"" + heap[idx].texto + "\" (" + heap[idx].ocurrencia + " ocurrencias)" + marca);

                if (hijoIzq >= heap.Count) break;
                idx = hijoIzq;
                paso++;
            }

            sb.AppendLine("");
            sb.AppendLine("--- Llego a la hoja mas izquierda del heap ---");

            return sb.ToString();
        }


        // ================================================================
        // CONSULTA 3: Heap completo con niveles
        // ================================================================
        public static string Consulta3(List<string> datos)
        {
            if (datos == null || datos.Count == 0)
                return "=== CONSULTA 3 ===\nNo hay datos para procesar.\n";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== CONSULTA 3: HEAP COMPLETO CON NIVELES ===");
            sb.AppendLine("");

            var conteo = ContarOcurrencias(datos);
            var heap = BuildHeap(conteo);

            sb.AppendLine("Total elementos: " + heap.Count);
            sb.AppendLine("Total datos de entrada: " + datos.Count + " elementos");
            sb.AppendLine("");

            // Mostrar heap con niveles
            sb.AppendLine(">> Estructura interna del heap (array indexado):");
            sb.AppendLine("");

            int nivelActual = -1;
            for (int i = 0; i < heap.Count; i++)
            {
                int nivel = (int)Math.Floor(Math.Log2(i + 1));

                if (nivel > nivelActual)
                {
                    sb.AppendLine("");
                    sb.AppendLine("--- Nivel " + nivel + " ---");
                    nivelActual = nivel;
                }

                string izq = "";
                string der = "";
                int hijoIzq = 2 * i + 1;
                int hijoDer = 2 * i + 2;
                if (hijoIzq < heap.Count) izq = "  [HIJO IZQ: " + (hijoIzq) + "]";
                if (hijoDer < heap.Count) der = "  [HIJO DER: " + (hijoDer) + "]";

                int padreIdx = (i - 1) / 2;
                string padreStr = (i == 0) ? " (RAIZ)" : "  padre=[" + padreIdx + "]";

                sb.AppendLine("  [" + i + "] \"" + heap[i].texto + "\" (" + heap[i].ocurrencia + ")" + padreStr + izq + der);
            }

            sb.AppendLine("");
            sb.AppendLine("--- Fin del heap ---");

            return sb.ToString();
        }


        // ================================================================
        // BuscarConHeap (List<string>)
        // ================================================================
        public static void BuscarConHeap(List<string> datos, int cantidad, List<Dato> collected)
        {
            var conteo = ContarOcurrencias(datos);
            var monticulo = new List<Dato>();
            foreach (var kvp in conteo)
                InsertarEnMonticulo(monticulo, new Dato(kvp.Value, kvp.Key));

            int extraer = cantidad;
            if (extraer > monticulo.Count) extraer = monticulo.Count;

            for (int i = 0; i < extraer; i++)
            {
                if (monticulo.Count > 0)
                    collected.Add(ExtraerMaximo(monticulo));
            }
        }


        // ================================================================
        // BuscarConOrden (List<string>)
        // ================================================================
        public static void BuscarConOrden(List<string> datos, int cantidad, List<Dato> collected)
        {
            var conteo = ContarOcurrencias(datos);
            var lista = new List<Dato>();
            foreach (var kvp in conteo)
                lista.Add(new Dato(kvp.Value, kvp.Key));

            BubbleSortDesc(lista);

            int limite = cantidad;
            if (limite > lista.Count) limite = lista.Count;

            for (int k = 0; k < limite; k++)
                collected.Add(lista[k]);
        }


        // ================================================================
        // BuscarConHeap (original, sobre arbol)
        // ================================================================
        public static void BuscarConHeap(List<Dato> nodos, List<int> padres, int cantidad, List<Dato> collected)
        {
            var g = Backend.grafo;
            List<Dato> monticulo = new List<Dato>();

            g.BFS(Backend.raizIdx, (vertex) =>
            {
                if (g.EsHoja(vertex.Id) && vertex.Data.ocurrencia > 0)
                {
                    Dato nuevo = new Dato(vertex.Data.ocurrencia, vertex.Data.texto, vertex.Data.descripcion);
                    InsertarEnMonticulo(monticulo, nuevo);
                }
            });

            int extraer = cantidad;
            if (extraer > monticulo.Count) extraer = monticulo.Count;

            for (int ext = 0; ext < extraer; ext++)
            {
                if (monticulo.Count > 0)
                {
                    Dato maximo = ExtraerMaximo(monticulo);
                    collected.Add(maximo);
                }
            }
        }


        // ================================================================
        // BuscarConOrden (original, sobre arbol)
        // ================================================================
        public static void BuscarConOrden(List<Dato> nodos, List<int> padres, int cantidad, List<Dato> collected)
        {
            var g = Backend.grafo;
            List<Dato> cands = new List<Dato>();

            g.BFS(Backend.raizIdx, (vertex) =>
            {
                if (vertex.Data.ocurrencia > 0 && g.EsHoja(vertex.Id))
                {
                    Dato copia = new Dato(vertex.Data.ocurrencia, vertex.Data.texto, vertex.Data.descripcion);
                    cands.Add(copia);
                }
            });

            BubbleSortDesc(cands);

            int limite = cantidad;
            if (limite > cands.Count) limite = cands.Count;

            for (int k = 0; k < limite; k++)
                collected.Add(cands[k]);
        }


        // ================================================================
        // Metodos privados - Helpers reutilizables
        // ================================================================

        private static Dictionary<string, int> ContarOcurrencias(List<string> datos)
        {
            var conteo = new Dictionary<string, int>();
            foreach (var s in datos)
            {
                if (conteo.ContainsKey(s)) conteo[s]++;
                else conteo[s] = 1;
            }
            return conteo;
        }

        private static void BubbleSortDesc(List<Dato> lista)
        {
            for (int pas = 0; pas < lista.Count - 1; pas++)
            {
                for (int comp = 0; comp < lista.Count - 1 - pas; comp++)
                {
                    if (lista[comp].ocurrencia < lista[comp + 1].ocurrencia)
                    {
                        Dato temp = lista[comp];
                        lista[comp] = lista[comp + 1];
                        lista[comp + 1] = temp;
                    }
                }
            }
        }

        private static void InsertarEnMonticulo(List<Dato> monticulo, Dato elemento)
        {
            monticulo.Add(elemento);
            int pos = monticulo.Count - 1;

            while (pos > 0)
            {
                int padrePos = (pos - 1) / 2;

                if (monticulo[padrePos].ocurrencia >= monticulo[pos].ocurrencia)
                    break;

                Dato temporal = monticulo[padrePos];
                monticulo[padrePos] = monticulo[pos];
                monticulo[pos] = temporal;

                pos = padrePos;
            }
        }

        private static void HeapifyDown(List<Dato> heap, int idx)
        {
            int tamanio = heap.Count;
            while (true)
            {
                int idxMayor = idx;
                int hijoIzq = 2 * idx + 1;
                int hijoDer = 2 * idx + 2;

                if (hijoIzq < tamanio && heap[hijoIzq].ocurrencia > heap[idxMayor].ocurrencia)
                    idxMayor = hijoIzq;

                if (hijoDer < tamanio && heap[hijoDer].ocurrencia > heap[idxMayor].ocurrencia)
                    idxMayor = hijoDer;

                if (idxMayor == idx)
                    break;

                Dato temp = heap[idx];
                heap[idx] = heap[idxMayor];
                heap[idxMayor] = temp;

                idx = idxMayor;
            }
        }

        private static List<Dato> BuildHeap(Dictionary<string, int> conteo)
        {
            var heap = new List<Dato>(conteo.Count);
            foreach (var kvp in conteo)
                heap.Add(new Dato(kvp.Value, kvp.Key));

            // Bottom-up heap construction: O(N), Floyd's method
            for (int i = (heap.Count / 2) - 1; i >= 0; i--)
                HeapifyDown(heap, i);

            return heap;
        }

        private static Dato ExtraerMaximo(List<Dato> monticulo)
        {
            if (monticulo.Count == 0)
                return new Dato(0, "", "");

            Dato resultado = monticulo[0];
            int ultimoIdx = monticulo.Count - 1;
            monticulo[0] = monticulo[ultimoIdx];
            monticulo.RemoveAt(ultimoIdx);

            int pos = 0;
            int tamanio = monticulo.Count;

            while (true)
            {
                int idxMayor = pos;
                int hijoIzq = 2 * pos + 1;
                int hijoDer = 2 * pos + 2;

                if (hijoIzq < tamanio && monticulo[hijoIzq].ocurrencia > monticulo[idxMayor].ocurrencia)
                    idxMayor = hijoIzq;

                if (hijoDer < tamanio && monticulo[hijoDer].ocurrencia > monticulo[idxMayor].ocurrencia)
                    idxMayor = hijoDer;

                if (idxMayor == pos)
                    break;

                Dato temporal = monticulo[pos];
                monticulo[pos] = monticulo[idxMayor];
                monticulo[idxMayor] = temporal;

                pos = idxMayor;
            }

            return resultado;
        }


        // ================================================================
        // Recorridos del arbol
        // ================================================================
        public static string RecorridoPreorden(List<Dato> nodos, List<int> padres, int raiz)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== RECORRIDO PREORDEN (Raiz -> Hijos) ===");
            sb.AppendLine("");

            var g = Backend.grafo;
            int contador = 0;

            g.Preorden(raiz, (vertex) =>
            {
                contador++;
                int nivel = g.GetDepth(vertex.Id);
                string marca = g.EsHoja(vertex.Id) ? " [HOJA]" : "";
                string sangria = "";
                for (int s = 0; s < nivel; s++) sangria += "  ";
                sb.AppendLine(sangria + contador + ". " + vertex.Data.texto + marca);
            });

            sb.AppendLine("");
            sb.AppendLine("--- Nodos visitados: " + contador + " ---");
            return sb.ToString();
        }

        public static string RecorridoInorden(List<Dato> nodos, List<int> padres, int raiz)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== RECORRIDO INORDEN (Hijo1 -> Raiz -> Resto) ===");
            sb.AppendLine("");

            var g = Backend.grafo;
            int contador = 0;

            g.Inorden(raiz, (vertex) =>
            {
                contador++;
                int nivel = g.GetDepth(vertex.Id);
                string marca = g.EsHoja(vertex.Id) ? " [HOJA]" : "";
                string sangria = "";
                for (int s = 0; s < nivel; s++) sangria += "  ";
                sb.AppendLine(sangria + contador + ". " + vertex.Data.texto + marca);
            });

            sb.AppendLine("");
            sb.AppendLine("--- Nodos visitados: " + contador + " ---");
            return sb.ToString();
        }

        public static string RecorridoPostorden(List<Dato> nodos, List<int> padres, int raiz)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== RECORRIDO POSTORDEN (Hijos -> Raiz) ===");
            sb.AppendLine("");

            var g = Backend.grafo;
            int contador = 0;

            g.Postorden(raiz, (vertex) =>
            {
                contador++;
                int nivel = g.GetDepth(vertex.Id);
                string marca = g.EsHoja(vertex.Id) ? " [HOJA]" : "";
                string sangria = "";
                for (int s = 0; s < nivel; s++) sangria += "  ";
                sb.AppendLine(sangria + contador + ". " + vertex.Data.texto + marca);
            });

            sb.AppendLine("");
            sb.AppendLine("--- Nodos visitados: " + contador + " ---");
            return sb.ToString();
        }

    }
}