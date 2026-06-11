using System;
using System.Collections.Generic;

namespace tpfinal
{

	[Serializable]
	public class Dato
	{
		public int ocurrencia { get; set; }
		public string texto { get; set; }
		public string descripcion { get; set; }
		public List<int> hijos;


        public Dato(int ocurrencia, string texto)
        {
            this.ocurrencia = ocurrencia;
            this.texto = texto;
            this.hijos = new List<int>();
        }

        public Dato(int ocurrencia, string texto, string descripcion)
		{
			this.ocurrencia = ocurrencia;
			this.texto = texto;
			this.descripcion = descripcion;
			this.hijos = new List<int>();
		}



		public override string ToString()
		{
			if (texto != null)
			{

				return "(" + ocurrencia + ") " + texto;

			}
			else
			{

				return "";
			}
		}

	}
}