using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_EP
{
    internal class Program
    {
        //Limite máximo
        static int max = 100;
        
        //Contadores
        static int nprod = 0, ncli = 0;

        //Arreglos
        static string[] codigos = new string[max];
        static string[] nombres = new string[max];
        static double[] precios = new double[max];
        static int[] stock = new int[max];

        static string[] dnis = new string[max];
        static string[] clientes = new string[max];
        static string[] telefonos = new string[max];
        static string telefono_inicio = "9";
        public static bool Login()
        {
            for (int i = 1; i <= 3; i++)
            {
                Console.Clear();
                Console.WriteLine("INICIO DE SESION");
                Console.Write("Usuario: ");
                string u = Console.ReadLine() ?? "";
                Console.Write("Clave: ");
                string c = Console.ReadLine() ?? "";

                if (u == "admin" && c == "1234")
                {
                    Console.WriteLine("Acceso correcto.");
                    return true;
                }

                Console.WriteLine("Datos incorrectos. Intento " + i + " de 3.");
                Console.ReadKey();
            }
            return false;
        }
        public static void MenuPrincipal() 
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
        
        public static void RegistrarProducto() 
        {

            Console.Clear();
            Console.WriteLine("REGISTRAR PRODUCTO");

            if (nprod >= max)
            {
                Console.WriteLine("Inventario lleno.");
                Console.ReadKey();
                return;
            }

            Console.Write("Codigo: ");
            string codigo = Console.ReadLine() ?? "";

            if (codigo.Trim() == "" || BuscarProductoPorCodigo(codigo) != -1)
            {
                Console.WriteLine("Codigo vacio o ya existente.");
                Console.ReadKey();
                return;
            }

            Console.Write("Nombre: ");
            string nombre = Console.ReadLine() ?? "";
            double precio = LeerDouble("Precio: ");
            int cantidad = LeerEntero("Stock: ");

            if (nombre.Trim() == "" || precio <= 0 || cantidad < 0)
            {
                Console.WriteLine("Datos invalidos.");
            }
            else
            {
                codigos[nprod] = codigo;
                nombres[nprod] = nombre;
                precios[nprod] = precio;
                stock[nprod] = cantidad;
                nprod++;
                Console.WriteLine("Producto registrado.");
            }
            Console.ReadKey();
        }
        public static void ListarProductos() 
        {
            Console.Clear();
            Console.WriteLine("LISTA DE PRODUCTOS");

            if (nprod == 0)
                Console.WriteLine("No hay productos registrados.");

            for (int i = 0; i < nprod; i++)
                MostrarProducto(i);

            Console.ReadKey();
        }
        public static void BuscarProducto() 
        {
            Console.Clear();
            Console.WriteLine("BUSCAR PRODUCTO");
            Console.Write("Codigo a buscar: ");
            string codigo = Console.ReadLine() ?? "";
            int pos = BuscarProductoPorCodigo(codigo);

            if (pos == -1)
                Console.WriteLine("Producto no encontrado.");
            else
                MostrarProducto(pos);

            Console.ReadKey();
        }
        public static int BuscarProductoPorCodigo(string codigo) 
        {
            for (int i = 0; i < nprod; i++)
            {
                if (codigos[i] == codigo)
                {
                    return i;
                }
            }
            return -1;
        }
        public static void MostrarProducto(int i) 
        {
            Console.WriteLine("Codigo: " + codigos[i]);
            Console.WriteLine("Nombre: " + nombres[i]);
            Console.WriteLine("Precio: S/ " + precios[i]);
            Console.WriteLine("Stock: " + stock[i]);
            Console.WriteLine("----------------------");
        }
        public static void ModificarProducto() 
        {
            Console.Clear();
            Console.WriteLine("MODIFICAR PRODUCTO");
            Console.Write("Codigo: ");
            string codigo = Console.ReadLine() ?? "";
            int pos = BuscarProductoPorCodigo(codigo);

            if (pos == -1)
            {
                Console.WriteLine("Producto no encontrado.");
            }
            else
            {
                MostrarProducto(pos);
                Console.Write("Nuevo nombre: ");
                string nombre = Console.ReadLine() ?? "";
                double precio = LeerDouble("Nuevo precio: ");
                int cantidad = LeerEntero("Nuevo stock: ");

                if (nombre.Trim() == "" || precio <= 0 || cantidad < 0)
                    Console.WriteLine("Datos invalidos.");
                else
                {
                    nombres[pos] = nombre;
                    precios[pos] = precio;
                    stock[pos] = cantidad;
                    Console.WriteLine("Producto modificado.");
                }
            }
            Console.ReadKey();
        }
        public static void EliminarProducto() 
        {
            Console.Clear();
            Console.WriteLine("ELIMINAR PRODUCTO");
            Console.Write("Codigo: ");
            string codigo = Console.ReadLine() ?? "";
            int pos = BuscarProductoPorCodigo(codigo);

            if (pos == -1)
            {
                Console.WriteLine("Producto no encontrado.");
            }
            else
            {
                for (int i = pos; i < nprod - 1; i++)
                {
                    codigos[i] = codigos[i + 1];
                    nombres[i] = nombres[i + 1];
                    precios[i] = precios[i + 1];
                    stock[i] = stock[i + 1];
                }

                nprod--;
                Console.WriteLine("Producto eliminado.");
            }
            Console.ReadKey();
        }

        //PARTE CLIENTES

        public static void MenuClientes() 
        {
            int op = 0;
            while (op != 7)
            {
                Console.Clear();
                Console.WriteLine("CLIENTES");
                Console.WriteLine("[1] Registrar");
                Console.WriteLine("[2] Mostrar");
                Console.WriteLine("[3] Buscar");
                Console.WriteLine("[4] Modificar");
                Console.WriteLine("[5] Eliminar");
                Console.WriteLine("[6] Ordenar alfabeticamente");
                Console.WriteLine("[7] Regresar");
                op = int.Parse(Console.ReadLine());

                switch (op)
                {
                    case 1: 
                        RegistrarCliente(); 
                        break;
                    case 2: 
                        ListarClientes(); 
                        break;
                    case 3: 
                        BuscarCliente(); 
                        break;
                    case 4:
                        ModificarCliente(); 
                        break;
                    case 5: 
                        EliminarCliente(); 
                        break;
                    case 6:
                        OrdenarClientes();
                        break;
                    case 7:
                        break;
                    default:
                        Console.WriteLine("Opcion incorrecta.");
                        Console.ReadKey();
                        break;
                }
            }
        }
        static void RegistrarCliente()
        {
            Console.Clear();
            Console.WriteLine("REGISTRAR CLIENTE");

            if (ncli >= max)
            {
                Console.WriteLine("Limite de clientes alcanzado.");
                Console.ReadKey();
                return;
            }

            Console.Write("DNI: ");
            string dni = Console.ReadLine() ?? "";

            if (dni.Trim() == "" || BuscarClientePorDni(dni) != -1 || dni.Length != 8)
            {
                Console.WriteLine("DNI vacio, ya registrado o ingresó una longitud diferente a 8 caracteres numericos.");
                Console.ReadKey();
                return;
            }

            Console.Write("Nombre: ");
            string nombre = Console.ReadLine() ?? "";
            Console.Write("Telefono (Ingrese los 8 digitos posteriores al 9): ");
            string telefono1 = Console.ReadLine() ?? "";
            string telefono = telefono_inicio + telefono1;

            if (nombre.Trim() == "")
            {
                Console.WriteLine("Nombre invalido.");
            }
            if (telefono.Length != 9) 
            {
                Console.WriteLine("Ingrese un número telefónico de 9 digitos [no incluya el 9 inicial].");
            }
            else
            {
                dnis[ncli] = dni;
                clientes[ncli] = nombre;
                telefonos[ncli] = telefono;
                ncli++;
                Console.WriteLine("Cliente registrado.");
            }
            Console.ReadKey();
        }

        static void ListarClientes()
        {
            Console.Clear();
            Console.WriteLine("LISTA DE CLIENTES");

            if (ncli == 0)
                Console.WriteLine("No hay clientes registrados.");

            for (int i = 0; i < ncli; i++)
                MostrarCliente(i);

            Console.ReadKey();
        }

        static void BuscarCliente()
        {
            Console.Clear();
            Console.WriteLine("BUSCAR CLIENTE");
            Console.Write("DNI: ");
            string dni = Console.ReadLine() ?? "";
            int pos = BuscarClientePorDni(dni);

            if (pos == -1)
                Console.WriteLine("Cliente no encontrado.");
            else
                MostrarCliente(pos);

            Console.ReadKey();
        }

        static int BuscarClientePorDni(string dni)
        {
            for (int i = 0; i < ncli; i++)
                if (dnis[i] == dni)
                    return i;
            return -1;
        }

        static void MostrarCliente(int i)
        {
            Console.WriteLine("DNI: " + dnis[i]);
            Console.WriteLine("Nombre: " + clientes[i]);
            Console.WriteLine("Telefono: " + telefonos[i]);
            Console.WriteLine("----------------------");
        }

        static void ModificarCliente()
        {
            Console.Clear();
            Console.WriteLine("MODIFICAR CLIENTE");
            Console.Write("DNI: ");
            string dni = Console.ReadLine() ?? "";
            int pos = BuscarClientePorDni(dni);

            if (pos == -1)
            {
                Console.WriteLine("Cliente no encontrado.");
            }
            else
            {
                MostrarCliente(pos);
                Console.Write("Nuevo nombre: ");
                string nombre = Console.ReadLine() ?? "";
                Console.Write("Nuevo telefono: ");
                string telefono1 = Console.ReadLine() ?? "";
                string telefono = telefono_inicio + telefono1;

                if (nombre.Trim() == "")
                {
                    Console.WriteLine("Nombre invalido.");
                }
                if (telefono.Length != 9) 
                {
                    Console.WriteLine("Ingrese un número de 9 dígitos, omita el 9 inicial.");
                }
                else
                {
                    clientes[pos] = nombre;
                    telefonos[pos] = telefono;
                    Console.WriteLine("Cliente modificado.");
                }
            }
            Console.ReadKey();
        }

        static void EliminarCliente()
        {
            Console.Clear();
            Console.WriteLine("ELIMINAR CLIENTE");
            Console.Write("DNI: ");
            string dni = Console.ReadLine() ?? "";
            int pos = BuscarClientePorDni(dni);

            if (pos == -1)
            {
                Console.WriteLine("Cliente no encontrado.");
            }
            else
            {
                for (int i = pos; i < ncli - 1; i++)
                {
                    dnis[i] = dnis[i + 1];
                    clientes[i] = clientes[i + 1];
                    telefonos[i] = telefonos[i + 1];
                }

                ncli--;
                Console.WriteLine("Cliente eliminado.");
            }
            Console.ReadKey();
        }

        static void OrdenarClientes() 
        {
            if (ncli <= 1)
            {
                Console.WriteLine("No hay suficientes registros para ordenar.");
                return;
            }
            for (int i = 0; i < ncli - 1; i++)
            {
                for (int j = 0; j < ncli - 1 - i; j++)
                {
                    if (string.Compare(clientes[j], clientes[j + 1]) > 0)
                    {
                        string temp = clientes[j];
                        clientes[j] = clientes[j + 1];
                        clientes[j + 1] = temp;

                        string temp_telefono = telefonos[j];
                        telefonos[j] = telefonos[j + 1];
                        telefonos[j + 1] = temp_telefono;

                        string temp_dni = dnis[j];
                        dnis[j] = dnis[j + 1];
                        dnis[j + 1] = temp_dni;
                    }
                }
            }
        }

        //EMISIÓN DE COMPROBANTES

        public static void EmitirComprobante() 
        {

            Console.Clear();
            Console.WriteLine("EMITIR COMPROBANTE");

            if (nprod == 0)
            {
                Console.WriteLine("No hay productos registrados.");
                Console.ReadKey();
                return;
            }

            Console.Write("Codigo del producto: ");
            string codigo = Console.ReadLine() ?? "";
            int pos = BuscarProductoPorCodigo(codigo);

            if (pos == -1)
            {
                Console.WriteLine("Producto no encontrado.");
            }
            else
            {
                MostrarProducto(pos);
                int cantidad = LeerEntero("Cantidad a vender: ");

                if (cantidad <= 0 || cantidad > stock[pos])
                {
                    Console.WriteLine("Cantidad invalida o stock insuficiente.");
                }
                else
                {
                    double total = precios[pos] * cantidad;
                    stock[pos] -= cantidad;

                    Console.WriteLine("BOLETA DE VENTA");
                    Console.WriteLine("Producto: " + nombres[pos]);
                    Console.WriteLine("Codigo: " + codigos[pos]);
                    Console.WriteLine("Precio unitario: S/ " + precios[pos]);
                    Console.WriteLine("Cantidad: " + cantidad);
                    Console.WriteLine("TOTAL: S/ " + total);
                    Console.WriteLine("Stock restante: " + stock[pos]);
                }
            }
            Console.ReadKey();
        }
        static int LeerEntero(string mensaje)
        {
            int valor;
            Console.Write(mensaje);

            while (!int.TryParse(Console.ReadLine(), out valor))
                Console.Write("Ingrese un numero entero valido: ");

            return valor;
        }

        static double LeerDouble(string mensaje)
        {
            double valor;
            Console.Write(mensaje);

            while (!double.TryParse(Console.ReadLine(), NumberStyles.Number,
                   CultureInfo.CurrentCulture, out valor) ||
                   double.IsNaN(valor) || double.IsInfinity(valor))
            {
                Console.Write("Ingrese un numero valido: ");
            }

            return valor;
        }


        static void Main(string[] args)
        {
            if (Login())
            {
                MenuPrincipal();
            }
            else
            {
                Console.WriteLine("Acceso bloqueado.");
            }
            Console.WriteLine("Cerrando el sistema...");
            Console.ReadKey();
        }
    }
}
