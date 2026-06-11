using System;

namespace tpfinal
{
    public class Cola<T>
    {
        private T[] buffer;
        private int head;
        private int tail;
        private int count;
        private const int capacidadInicial = 256;

        public Cola()
        {
            buffer = new T[capacidadInicial];
        }

        public void encolar(T elem)
        {
            if (count == buffer.Length)
            {
                Redimensionar(buffer.Length * 2);
            }
            buffer[tail] = elem;
            tail = (tail + 1) % buffer.Length;
            count++;
        }

        public T desencolar()
        {
            T resultado = buffer[head];
            head = (head + 1) % buffer.Length;
            count--;
            return resultado;
        }

        public T tope()
        {
            return buffer[head];
        }

        public bool esVacia()
        {
            return count == 0;
        }

        public int cantidadElementos()
        {
            return count;
        }

        private void Redimensionar(int nuevaCapacidad)
        {
            T[] nuevo = new T[nuevaCapacidad];
            for (int i = 0; i < count; i++)
            {
                nuevo[i] = buffer[(head + i) % buffer.Length];
            }
            buffer = nuevo;
            head = 0;
            tail = count;
        }
    }
}
