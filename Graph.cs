using System;
using System.Collections.Generic;
using tp1;

namespace tpfinal
{
    public class Vertice
    {
        public int Id;
        public bool Visitado;
        public Object Data;
    }

    public class Arista
    {
        public int Origen;
        public int Destino;
        public double Peso;
    }

    public class Grafo
    {
        public List<Vertice> vertices;
        public List<List<Arista>> listaAdyacencia;
        public List<int> indicePadre;

        public Grafo()
        {
            vertices = new List<Vertice>();
            listaAdyacencia = new List<List<Arista>>();
            indicePadre = new List<int>();
        }

        public int agregarVertice(Object data)
        {
            int id = vertices.Count;
            Vertice v = new Vertice();
            v.Id = id;
            v.Data = data;
            vertices.Add(v);
            listaAdyacencia.Add(new List<Arista>());
            indicePadre.Add(-1);
            return id;
        }

        public void agregarArista(int desde, int hasta, double peso)
        {
            Arista a = new Arista();
            a.Origen = desde;
            a.Destino = hasta;
            a.Peso = peso;
            listaAdyacencia[desde].Add(a);
            indicePadre[hasta] = desde;
        }

        public int obtenerPadre(int idx)
        {
            return indicePadre[idx];
        }

        public int obtenerProfundidad(int idx)
        {
            int nivel = 0;
            int actual = idx;
            while (indicePadre[actual] != -1)
            {
                nivel++;
                actual = indicePadre[actual];
            }
            return nivel;
        }

        public List<int> obtenerHijos(int idx)
        {
            List<int> hijos = new List<int>();
            for (int i = 0; i < listaAdyacencia[idx].Count; i++)
            {
                hijos.Add(listaAdyacencia[idx][i].Destino);
            }
            return hijos;
        }

        public bool esHoja(int idx)
        {
            return listaAdyacencia[idx].Count == 0;
        }

        public int cantidadVertices()
        {
            return vertices.Count;
        }

        public void resetearVisitados()
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i].Visitado = false;
            }
        }

        public void BFS(int inicio, List<int> recolectados)
        {
            resetearVisitados();
            Cola<int> cola = new Cola<int>();
            cola.encolar(inicio);
            vertices[inicio].Visitado = true;

            while (cola.cantidadElementos() > 0)
            {
                int actual = cola.desencolar();
                recolectados.Add(actual);

                List<Arista> aristas = listaAdyacencia[actual];
                for (int i = 0; i < aristas.Count; i++)
                {
                    int destino = aristas[i].Destino;
                    if (!vertices[destino].Visitado)
                    {
                        vertices[destino].Visitado = true;
                        cola.encolar(destino);
                    }
                }
            }
        }

        public void DFS(int inicio, List<int> recolectados)
        {
            resetearVisitados();
            dfsRecursivo(inicio, recolectados);
        }

        private void dfsRecursivo(int idx, List<int> recolectados)
        {
            vertices[idx].Visitado = true;
            recolectados.Add(idx);

            List<Arista> aristas = listaAdyacencia[idx];
            for (int i = 0; i < aristas.Count; i++)
            {
                int destino = aristas[i].Destino;
                if (!vertices[destino].Visitado)
                {
                    dfsRecursivo(destino, recolectados);
                }
            }
        }

        public void preorden(int idx, List<int> recolectados)
        {
            recolectados.Add(idx);
            List<Arista> aristas = listaAdyacencia[idx];
            for (int i = 0; i < aristas.Count; i++)
            {
                preorden(aristas[i].Destino, recolectados);
            }
        }

        public void inorden(int idx, List<int> recolectados)
        {
            List<Arista> aristas = listaAdyacencia[idx];
            if (aristas.Count > 0)
            {
                inorden(aristas[0].Destino, recolectados);
            }
            recolectados.Add(idx);
            for (int i = 1; i < aristas.Count; i++)
            {
                inorden(aristas[i].Destino, recolectados);
            }
        }

        public void postorden(int idx, List<int> recolectados)
        {
            List<Arista> aristas = listaAdyacencia[idx];
            for (int i = 0; i < aristas.Count; i++)
            {
                postorden(aristas[i].Destino, recolectados);
            }
            recolectados.Add(idx);
        }

        public void BFS_completo(List<int> recolectados)
        {
            resetearVisitados();
            for (int i = 0; i < vertices.Count; i++)
            {
                if (!vertices[i].Visitado)
                {
                    Cola<int> cola = new Cola<int>();
                    cola.encolar(i);
                    vertices[i].Visitado = true;

                    while (cola.cantidadElementos() > 0)
                    {
                        int actual = cola.desencolar();
                        recolectados.Add(actual);

                        List<Arista> aristas = listaAdyacencia[actual];
                        for (int j = 0; j < aristas.Count; j++)
                        {
                            int destino = aristas[j].Destino;
                            if (!vertices[destino].Visitado)
                            {
                                vertices[destino].Visitado = true;
                                cola.encolar(destino);
                            }
                        }
                    }
                }
            }
        }

        public void DFS_completo(List<int> recolectados)
        {
            resetearVisitados();
            for (int i = 0; i < vertices.Count; i++)
            {
                if (!vertices[i].Visitado)
                {
                    dfsRecursivo(i, recolectados);
                }
            }
        }
    }
}
