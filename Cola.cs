using System;
using System.Collections.Generic;

namespace tp1
{
    public class Cola<T>
    {
        private T[] buffer;
        private int head;
        private int tail;
        private int count;
        private int capacidad;

        public Cola()
        {
            capacidad = 16;
            buffer = new T[capacidad];
            head = 0;
            tail = 0;
            count = 0;
        }

        public Cola(int capacidadInicial)
        {
            capacidad = capacidadInicial;
            buffer = new T[capacidad];
            head = 0;
            tail = 0;
            count = 0;
        }

        public void encolar(T elem)
        {
            if (count == capacidad)
            {
                int nuevaCapacidad = capacidad * 2;
                T[] nuevo = new T[nuevaCapacidad];
                for (int i = 0; i < count; i++)
                {
                    nuevo[i] = buffer[(head + i) % capacidad];
                }
                buffer = nuevo;
                head = 0;
                tail = count;
                capacidad = nuevaCapacidad;
            }

            buffer[tail] = elem;
            tail = (tail + 1) % capacidad;
            count++;
        }

        public T desencolar()
        {
            if (count == 0)
            {
                throw new InvalidOperationException("Cola vacia");
            }

            T valor = buffer[head];
            head = (head + 1) % capacidad;
            count--;
            return valor;
        }

        public T tope()
        {
            if (count == 0)
            {
                throw new InvalidOperationException("Cola vacia");
            }

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
    }
}
