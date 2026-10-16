using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_EP
{
    internal class Program
    {
        public static void MenuProductos() 
        {
            int op = 0;
            while (op != 6)
            {
                Console.Clear();
                Console.WriteLine("PRODUCTOS");
                Console.WriteLine("[1] Registrar");
                Console.WriteLine("[2] Listar");
                Console.WriteLine("[3] Buscar");
                Console.WriteLine("[4] Modificar");
                Console.WriteLine("[5] Eliminar");
                Console.WriteLine("[6] Regresar");
                op = int.Parse(Console.ReadLine());

                switch (op)
                {
                    case 1: 
                        RegistrarProducto();
                        break;
                    case 2: 
                        ListarProductos(); 
                        break;
                    case 3: 
                        BuscarProducto(); 
                        break;
                    case 4: 
                        ModificarProducto(); 
                        break;
                    case 5: 
                        EliminarProducto(); 
                        break;
                    case 6: break;
                    default:
                        Console.WriteLine("Opcion incorrecta.");
                        Console.ReadKey();
                        break;
                }
            }
        }
        }
        public static void RegistrarProducto() 
        {

        }
        public static void ListarProductos() 
        {

        }
        public static void BuscarProducto() 
        {

        }
        public static int BuscarProductoPorCodigo(string codigo) { }
        public static void MostrarProducto(int i) 
        {

        }
        public static void ModificarProducto() 
        {

        }
        public static void EliminarProducto() 
        {
        }
        public static void MenuClientes() { }
        public static void RegistrarCliente() { }
        public static void ListarClientes() { }
        public static void BuscarCliente() { }
        public static int BuscarClientePorDni(string dni) { }
        public static void MostrarCliente(int i) { }
        public static void ModificarCliente() { }
        public static void EliminarCliente() { }
        public static void EmitirComprobante() { }



        static void Main(string[] args)
        {
            int op = 0;
            while (op != 4)
            {
                Console.Clear();
                Console.WriteLine("SISTEMA DE VENTAS");
                Console.WriteLine("[1] Productos");
                Console.WriteLine("[2] Clientes");
                Console.WriteLine("[3] Emitir comprobante");
                Console.WriteLine("[4] Salir");
                op = int.Parse(Console.ReadLine());

                switch (op)
                {
                    case 1: 
                        MenuProductos(); 
                        break;
                    case 2: 
                        MenuClientes(); 
                        break;
                    case 3: 
                        EmitirComprobante(); 
                        break;
                    case 4: 
                        break;
                    default:
                        Console.WriteLine("Opcion incorrecta.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
