using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_EP
{
    internal class Validadores
    {

        //Validar que lo ingresado sea entero int
        public static int LeerEntero(string mensaje)
        {
            int valor;
            while (true)
            {
                Console.Write(mensaje);
                if (int.TryParse(Console.ReadLine(), out valor))
                {
                    if (valor <= 0)
                    {
                        Console.WriteLine("Ingrese un numero entero valido: ");
                        continue;
                    }
                    return valor;
                }
                Console.WriteLine("Error: Ingrese un valor numérico.");
            }
        }

        //Validador para Menus (evitar nulos)
        public static int LeerMenu(string mensaje)
        {
            int valor;
            while (true)
            {
                Console.Write(mensaje);
                if (int.TryParse(Console.ReadLine(), out valor))
                {
                    return valor;
                }
                Console.WriteLine("Error: Ingrese un valor numérico.");
            }
        }

        //Validar que el valor ingresado sea double
        public static double LeerDouble(string mensaje)
        {
            double valor;

            while (true)
            {
                Console.Write(mensaje);
                if (double.TryParse(Console.ReadLine(), out valor))
                {
                    if (valor <= 0)
                    {
                        Console.WriteLine("Ingrese un numero valido: ");
                        continue;
                    }
                    return valor;
                }
                Console.WriteLine("Error: Ingrese un valor númerico.");
            }
            
        }

        //Validar que el texto no esté vacío
        public static string LeerTexto(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string valor = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(valor))
                {
                    return valor;
                }
                Console.WriteLine("Error: Este campo no puede estar vacio.");
            }
        }

    }
}
