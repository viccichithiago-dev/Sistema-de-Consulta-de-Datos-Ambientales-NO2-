
using System;
using System.Text;
using System.Collections.Generic;
using tp1;

namespace tpfinal
{

	public class Estrategia
	{
	
		public String Consulta1(List<Dato> nodos, List<int> padres, int raiz)
		{
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== TODAS LAS PREDICCIONES ===");
            sb.AppendLine("");
			
            int totalHojas = 0;
            int totalNodos = nodos.Count;
			
            for (int i = 0; i < totalNodos; i++)
            {
                bool tieneHijos = nodos[i].hijos.Count > 0;
				
                if (!tieneHijos && nodos[i].ocurrencia > 0)
                {
                    totalHojas++;
                    int nivel = 0;
                    int actual = i;
                    while (padres[actual] != -1)
                    {
                        nivel++;
                        actual = padres[actual];
                    }
					
                    string sangria = "";
                    for (int s = 0; s < nivel; s++)
                    {
                        sangria += "  ";
                    }
					
                    sb.AppendLine(sangria + "[" + totalHojas + "] " + nodos[i].texto + " (repeticiones: " + nodos[i].ocurrencia + ")");
                    sb.AppendLine(sangria + "     -> " + nodos[i].descripcion);
                    sb.AppendLine("");
                }
            }
			
            sb.AppendLine("--- Resumen ---");
            sb.AppendLine("Total predicciones encontradas: " + totalHojas);
            sb.AppendLine("Total nodos en estructura: " + totalNodos);
			
            return sb.ToString();
		}


		public String Consulta2(List<Dato> nodos, List<int> padres, int raiz)
		{
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== CAMINO A PREDICCION ===");
            sb.AppendLine("");
			
            int totalNodos = nodos.Count;
            int contadorCaminos = 0;
			
            for (int i = 0; i < totalNodos; i++)
            {
                bool tieneHijos = nodos[i].hijos.Count > 0;
				
                if (!tieneHijos && nodos[i].ocurrencia > 0)
                {
                    contadorCaminos++;
                    sb.AppendLine(">> Camino #" + contadorCaminos + ":");
					
                    List<int> rutaInv = new List<int>();
                    int actual = i;
                    while (actual != -1)
                    {
                        rutaInv.Add(actual);
                        actual = padres[actual];
                    }
					
                    for (int paso = rutaInv.Count - 1; paso >= 0; paso--)
                    {
                        int idxNodo = rutaInv[paso];
                        int profundidad = rutaInv.Count - 1 - paso;
                        string prefijo = "";
                        for (int p = 0; p < profundidad; p++)
                        {
                            prefijo += "  |";
                        }
                        if (profundidad > 0)
                        {
                            prefijo += "--> ";
                        }
						
                        sb.AppendLine(prefijo + "[" + profundidad + "] " + nodos[idxNodo].texto);
                    }
					
                    sb.AppendLine("     [HOJA] Ocurrencias: " + nodos[i].ocurrencia);
                    sb.AppendLine("");
                }
            }
			
            sb.AppendLine("--- Total de caminos: " + contadorCaminos + " ---");
			
            return sb.ToString();
        }

		
		public String Consulta3(List<Dato> nodos, List<int> padres, int raiz)
		{
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== RECORRIDO A PROFUNDIDAD ===");
            sb.AppendLine("");
			
            Cola<int> pilaRecorrido = new Cola<int>();
            pilaRecorrido.encolar(raiz);
			
            List<int> visitados = new List<int>();
            int contadorVisitas = 0;
			
            while (pilaRecorrido.cantidadElementos() > 0)
            {
                int actual = pilaRecorrido.desencolar();
				
                if (visitados.Contains(actual)) continue;
                visitados.Add(actual);
				
                contadorVisitas++;
				
                int nivel = 0;
                int temp = actual;
                while (padres[temp] != -1)
                {
                    nivel++;
                    temp = padres[temp];
                }
				
                string marca = "";
                bool tieneHijos = nodos[actual].hijos.Count > 0;
                if (!tieneHijos) marca = " [HOJA]";
				
                string sangria = "";
                for (int s = 0; s < nivel; s++)
                {
                    sangria += "  ";
                }
				
                sb.AppendLine(sangria + contadorVisitas + ". " + nodos[actual].texto + marca);
				
                // encolar hijos
                List<int> hijos = nodos[actual].hijos;
                for (int h = hijos.Count - 1; h >= 0; h--)
                {
                    pilaRecorrido.encolar(hijos[h]);
                }
            }
			
            sb.AppendLine("");
            sb.AppendLine("--- Nodos visitados: " + contadorVisitas + " ---");
            sb.AppendLine("--- Total nodos: " + nodos.Count + " ---");
			
            return sb.ToString();
		}


        public void BuscarConOtro(List<Dato> nodos, List<int> padres, int cantidad, List<Dato> collected)
        {
            Cola<int> cola = new Cola<int>();
            cola.encolar(Backend.raizIdx);
			
            List<int> procesados = new List<int>();
            List<Dato> cands = new List<Dato>();
			
            while (cola.cantidadElementos() > 0)
            {
                int idx = cola.desencolar();
				
                if (procesados.Contains(idx)) continue;
                procesados.Add(idx);
				
                if (nodos[idx].ocurrencia > 0)
                {
                    bool tieneHijos = nodos[idx].hijos.Count > 0;
					
                    if (!tieneHijos)
                    {
                        Dato copia = new Dato(nodos[idx].ocurrencia, nodos[idx].texto, nodos[idx].descripcion);
                        cands.Add(copia);
                    }
                }
				
                List<int> hijos = nodos[idx].hijos;
                for (int j = 0; j < hijos.Count; j++)
                {
                    cola.encolar(hijos[j]);
                }
            }
			
            // bubble sort descendente
            for (int pas = 0; pas < cands.Count - 1; pas++)
            {
                for (int comp = 0; comp < cands.Count - 1 - pas; comp++)
                {
                    if (cands[comp].ocurrencia < cands[comp + 1].ocurrencia)
                    {
                        Dato temp = cands[comp];
                        cands[comp] = cands[comp + 1];
                        cands[comp + 1] = temp;
                    }
                }
            }
			
            int limite = cantidad;
            if (limite > cands.Count) limite = cands.Count;
			
            for (int k = 0; k < limite; k++)
            {
                collected.Add(cands[k]);
            }
        }


        public void BuscarConHeap(List<Dato> nodos, List<int> padres, int cantidad, List<Dato> collected)
        {
            List<Dato> monticulo = new List<Dato>();
			
            Cola<int> colaRecorrido = new Cola<int>();
            colaRecorrido.encolar(Backend.raizIdx);
			
            List<int> procesados = new List<int>();
			
            while (colaRecorrido.cantidadElementos() > 0)
            {
                int idx = colaRecorrido.desencolar();
				
                if (procesados.Contains(idx)) continue;
                procesados.Add(idx);
				
                bool esHoja = nodos[idx].hijos.Count == 0;
                List<int> hijos = nodos[idx].hijos;
                for (int j = 0; j < hijos.Count; j++)
                {
                    colaRecorrido.encolar(hijos[j]);
                }
				
                if (esHoja && nodos[idx].ocurrencia > 0)
                {
                    Dato nuevo = new Dato(nodos[idx].ocurrencia, nodos[idx].texto, nodos[idx].descripcion);
                    InsertarEnMonticulo(monticulo, nuevo);
                }
            }
			
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
		
        private void InsertarEnMonticulo(List<Dato> monticulo, Dato elemento)
        {
            monticulo.Add(elemento);
            int pos = monticulo.Count - 1;
			
            while (pos > 0)
            {
                int padrePos = (pos - 1) / 2;
				
                if (monticulo[padrePos].ocurrencia >= monticulo[pos].ocurrencia)
                {
                    break;
                }
				
                Dato temporal = monticulo[padrePos];
                monticulo[padrePos] = monticulo[pos];
                monticulo[pos] = temporal;
				
                pos = padrePos;
            }
        }
		
        private Dato ExtraerMaximo(List<Dato> monticulo)
        {
            if (monticulo.Count == 0)
            {
                return new Dato(0, "", "");
            }
			
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
                {
                    idxMayor = hijoIzq;
                }
				
                if (hijoDer < tamanio && monticulo[hijoDer].ocurrencia > monticulo[idxMayor].ocurrencia)
                {
                    idxMayor = hijoDer;
                }
				
                if (idxMayor == pos)
                {
                    break;
                }
				
                Dato temporal = monticulo[pos];
                monticulo[pos] = monticulo[idxMayor];
                monticulo[idxMayor] = temporal;
				
                pos = idxMayor;
            }
			
            return resultado;
        }




    }
}
