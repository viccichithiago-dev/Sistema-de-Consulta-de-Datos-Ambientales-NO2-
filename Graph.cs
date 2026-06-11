using System;
using System.Collections.Generic;

namespace tpfinal
{
    public class Vertex<T>
    {
        public int Id { get; set; }
        public bool Visitado { get; set; }
        public T Data { get; set; }
    }

    public class Edge
    {
        public int Origen { get; set; }
        public int Destino { get; set; }
        public double Peso { get; set; }
    }

    public class Graph<T>
    {
        public List<Vertex<T>> Vertices { get; private set; }
        public List<List<Edge>> AdjList { get; private set; }
        public List<int> ParentIndex { get; private set; }

        public Graph()
        {
            Vertices = new List<Vertex<T>>();
            AdjList = new List<List<Edge>>();
            ParentIndex = new List<int>();
        }

        public int AddVertex(T data)
        {
            int id = Vertices.Count;
            Vertices.Add(new Vertex<T> { Id = id, Data = data });
            AdjList.Add(new List<Edge>());
            ParentIndex.Add(-1);
            return id;
        }

        public void AddEdge(int from, int to, double peso = 0)
        {
            AdjList[from].Add(new Edge { Origen = from, Destino = to, Peso = peso });
            ParentIndex[to] = from;
        }

        public int GetParent(int idx)
        {
            return ParentIndex[idx];
        }

        public int GetDepth(int idx)
        {
            int nivel = 0;
            int actual = idx;
            while (ParentIndex[actual] != -1)
            {
                nivel++;
                actual = ParentIndex[actual];
            }
            return nivel;
        }

        public List<int> GetChildren(int idx)
        {
            var hijos = new List<int>(AdjList[idx].Count);
            foreach (var e in AdjList[idx])
                hijos.Add(e.Destino);
            return hijos;
        }

        public bool EsHoja(int idx)
        {
            return AdjList[idx].Count == 0;
        }

        public int Count
        {
            get { return Vertices.Count; }
        }

        public void ResetVisitados()
        {
            foreach (var v in Vertices)
                v.Visitado = false;
        }

        public void BFS(int start, Action<Vertex<T>> visitor)
        {
            ResetVisitados();
            Cola<int> cola = new Cola<int>();
            cola.encolar(start);
            Vertices[start].Visitado = true;

            while (cola.cantidadElementos() > 0)
            {
                int actual = cola.desencolar();
                visitor(Vertices[actual]);

                foreach (var edge in AdjList[actual])
                {
                    if (!Vertices[edge.Destino].Visitado)
                    {
                        Vertices[edge.Destino].Visitado = true;
                        cola.encolar(edge.Destino);
                    }
                }
            }
        }

        public void DFS(int start, Action<Vertex<T>> visitor)
        {
            ResetVisitados();
            DFS_Recursivo(start, visitor);
        }

        private void DFS_Recursivo(int idx, Action<Vertex<T>> visitor)
        {
            Vertices[idx].Visitado = true;
            visitor(Vertices[idx]);

            foreach (var edge in AdjList[idx])
            {
                if (!Vertices[edge.Destino].Visitado)
                {
                    DFS_Recursivo(edge.Destino, visitor);
                }
            }
        }

        public void Preorden(int idx, Action<Vertex<T>> visitor)
        {
            visitor(Vertices[idx]);
            foreach (var edge in AdjList[idx])
                Preorden(edge.Destino, visitor);
        }

        public void Inorden(int idx, Action<Vertex<T>> visitor)
        {
            var hijos = AdjList[idx];
            if (hijos.Count > 0)
                Inorden(hijos[0].Destino, visitor);
            visitor(Vertices[idx]);
            for (int i = 1; i < hijos.Count; i++)
                Inorden(hijos[i].Destino, visitor);
        }

        public void Postorden(int idx, Action<Vertex<T>> visitor)
        {
            foreach (var edge in AdjList[idx])
                Postorden(edge.Destino, visitor);
            visitor(Vertices[idx]);
        }

        public void BFS_All(Action<Vertex<T>> visitor)
        {
            ResetVisitados();
            for (int i = 0; i < Vertices.Count; i++)
            {
                if (!Vertices[i].Visitado)
                {
                    BFS(i, visitor);
                }
            }
        }

        public void DFS_All(Action<Vertex<T>> visitor)
        {
            ResetVisitados();
            for (int i = 0; i < Vertices.Count; i++)
            {
                if (!Vertices[i].Visitado)
                {
                    DFS_Recursivo(i, visitor);
                }
            }
        }
    }
}
