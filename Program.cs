using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FP_EP
{
    internal class Program
    {
        //Limite máximo
        static int max = 100;
        
        //Contadores
        static int nprod = 0, ncli = 0, ncom=0;

        //Arreglos:
        //Productos
        static string[] codigos = new string[max];
        static string[] nombres = new string[max];
        static double[] precios = new double[max];
        static int[] stock = new int[max];
        static int[] stock_inicial = new int[max];

        //Clientes
        static string[] dnis = new string[max];
        static string[] clientes = new string[max];
        static string[] telefonos = new string[max];
        static string telefono_inicio = "9";

        //Comprobantes
        static string[] ComprobanteCodigos = new string[max];
        static int[] indiceProd = new int[max];
        static int[] indiceCli = new int[max];
        static double[] totalVenta = new double[max];
        static int[] cantidadVendida = new int[max];

        static string CodigoComprobInicio = "N00";

        //Credenciales de acceso
        static string usuario = "admin";
        static string contra = "1234";

        //Archivo de guardado
        static string rutaArchivo = "Minimarket_datos.txt";

        //Método Login
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

                if (u == usuario && c == contra)
                {
                    Console.WriteLine("Acceso correcto.");
                    return true;
                }

                Console.WriteLine("Datos incorrectos. Intento " + i + " de 3.");
                Console.ReadKey();
            }
            return false;
        }

        //Método menú principal

        public static void MenuPrincipal() 
        {
            int op = 0;
            while (op != 6)
            {
                Console.Clear();
                Console.WriteLine("SISTEMA DE VENTAS");
                Console.WriteLine("[1] Productos");
                Console.WriteLine("[2] Clientes");
                Console.WriteLine("[3] Comprobantes de pago");
                Console.WriteLine("[4] Guardar datos");
                Console.WriteLine("[5] Cargar datos");
                Console.WriteLine("[6] Salir");
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
                        MenuComprobante();
                        break;
                    case 4:
                        Guardar_datos();
                        break;
                    case 5:
                        Cargar_datos();
                        break;
                    case 6:
                        break;
                    default:
                        Console.WriteLine("Opcion incorrecta.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        //Método Menu productos
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
        
        //Método Registro de productos
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
                stock_inicial[nprod] = cantidad;
                nprod++;
                Console.WriteLine("Producto registrado.");
            }
            Console.ReadKey();
        }

        //Método mostrar todos los productos
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

        //Buscar y mostrar información de 1 producto
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

        //Buscar índice de producto por código (auxiliar para otros métodos)
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

        //Mostrar información de un producto (auxiliar a otros métodos)

        public static void MostrarProducto(int i) 
        {
            Console.WriteLine("Codigo: " + codigos[i]);
            Console.WriteLine("Nombre: " + nombres[i]);
            Console.WriteLine("Precio: S/ " + precios[i]);
            Console.WriteLine("Stock: " + stock[i]);
            Console.WriteLine("----------------------");
        }

        //Modificar datos de un producto (menos código)

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

        //Eliminar datos de un producto y su código + reordenamiento de arreglos

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

        //Registro de clientes
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

        //Mostrar todos los clientes
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

        //Buscar cliente por dni y mostrar sus datos
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

        //Obtener índice de cliente por dni (auxiliar a otros métodos)
        
        static int BuscarClientePorDni(string dni)
        {
            for (int i = 0; i < ncli; i++)
                if (dnis[i] == dni)
                    return i;
            return -1;
        }

        //Mostrar info de cliente (auxiliar a otros métodos)
        static void MostrarCliente(int i)
        {
            Console.WriteLine("DNI: " + dnis[i]);
            Console.WriteLine("Nombre: " + clientes[i]);
            Console.WriteLine("Telefono: " + telefonos[i]);
            Console.WriteLine("----------------------");
        }

        //Modificar datos de clientes (menos DNI)
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

        //Eliminar datos de clientes y reordenamiento de arreglos
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

        //Ordenar clientes por nombre alfabeticamente
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

        //COMPROBANTES

        public static void MenuComprobante() 
        {
            int op = 0;
            while (op != 5)
            {
                Console.Clear();
                Console.WriteLine("MENU COMPROBANTES");
                Console.WriteLine("[1] Emitir comprobante");
                Console.WriteLine("[2] Emitir comprobante con DNI");
                Console.WriteLine("[3] Mostrar comprobantes emitidos");
                Console.WriteLine("[4] Buscar comprobante por código");
                Console.WriteLine("[5] Salir");
                op = int.Parse(Console.ReadLine());

                switch (op)
                {
                    case 1:
                        EmitirComprobante();
                        break;
                    case 2:
                        EmitirComprobanteDni();
                        break;
                    case 3:
                        ListarComprobantes();
                        break;
                    case 4:
                        BuscarComprobante();
                        break;
                    case 5:
                        break;
                    default:
                        Console.WriteLine("Opcion incorrecta.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        //Emitir comprobante
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

                    ComprobanteCodigos[ncom] = CodigoComprobInicio + ncom;
                    indiceProd[ncom] = pos;
                    indiceCli[ncom] = -1;
                    cantidadVendida[ncom] = cantidad;
                    totalVenta[ncom] = total;
                    ncom++;
                    if ((stock[pos]) <= (stock_inicial[pos] / 2))
                    {
                        Console.WriteLine($"Alerta!!! El stock restante [{stock[pos]}] es inferior o igual a la mitad del stock inicial [{stock_inicial[pos]}]," +
                            $" se recomienda reabastecer.");
                    }
                }
            }
            Console.ReadKey();
        }

        //Emitir comprobante con DNI de cliente
        public static void EmitirComprobanteDni() 
        {
            Console.Clear();
            Console.WriteLine("EMITIR COMPROBANTE CON DNI");

            if (nprod == 0)
            {
                Console.WriteLine("No hay productos registrados.");
                Console.ReadKey();
                return;
            }
            if (ncli == 0) 
            {
                Console.WriteLine("No hay clientes registrados.");
                Console.ReadKey();
                return;
            }

            Console.Write("Codigo del producto: ");
            string codigo = Console.ReadLine() ?? "";
            int pos = BuscarProductoPorCodigo(codigo);

            Console.Write("Ingrese DNI del cliente: ");
            string dni = Console.ReadLine() ?? "";
            int posdni = BuscarClientePorDni(dni);

            if (pos == -1 ||posdni==-1)
            {
                Console.WriteLine("Producto y/o cliente no encontrado.");
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
                    Console.WriteLine("DNI: " + dnis[posdni]);
                    Console.WriteLine("Nombre cliente: " + clientes[posdni]);
                    Console.WriteLine("Producto: " + nombres[pos]);
                    Console.WriteLine("Codigo: " + codigos[pos]);
                    Console.WriteLine("Precio unitario: S/ " + precios[pos]);
                    Console.WriteLine("Cantidad: " + cantidad);
                    Console.WriteLine("TOTAL: S/ " + total);
                    Console.WriteLine("Stock restante: " + stock[pos]);

                    ComprobanteCodigos[ncom] = CodigoComprobInicio + ncom;
                    indiceProd[ncom] = pos;
                    indiceCli[ncom] = posdni;
                    cantidadVendida[ncom] = cantidad;
                    totalVenta[ncom] = total;
                    ncom++;
                    if ((stock[pos]) <= (stock_inicial[pos] / 2)) 
                    {
                        Console.WriteLine($"Alerta!!! El stock restante [{stock[pos]}] es inferior o igual a la mitad del stock inicial [{stock_inicial[pos]}]," +
                            $" se recomienda reabastecer.");
                    }
                }
            }
            Console.ReadKey();
        }

        //Mostrar info de todos los comprobantes emitidos
        public static void ListarComprobantes() 
        {
            Console.Clear();
            Console.WriteLine("LISTA DE COMPROBANTES");

            if (ncom == 0)
                Console.WriteLine("No hay comprobantes registrados.");

            for (int i = 0; i < ncom; i++)
                MostrarComprobante(i);

            Console.ReadKey();
        }

        //Mostrar información de comprobante
        public static void MostrarComprobante(int i) 
        {
            Console.WriteLine("Codigo comprobante: " + ComprobanteCodigos[i]);

            if (indiceCli[i]!=-1)
            {
                Console.WriteLine($"DNI cliente: {dnis[indiceCli[i]]}");
                Console.WriteLine($"Nombre cliente: {clientes[indiceCli[i]]}");
            }

            Console.WriteLine("Codigo producto: " + codigos[indiceProd[i]]);
            Console.WriteLine("Nombre: " + nombres[indiceProd[i]]);
            Console.WriteLine("Precio unitario: S/ " + precios[indiceProd[i]]);
            Console.WriteLine("Cantidad vendida: " + cantidadVendida[i]);
            Console.WriteLine("----------------------");
        }

        //Buscar comprobante por código
        public static void BuscarComprobante() 
        {
            Console.Clear();
            Console.WriteLine("BUSCAR COMPROBANTE");
            Console.Write("Código: ");
            string comprobante = Console.ReadLine() ?? "";
            int pos = BuscarComprobantePorCodigo(comprobante);

            if (pos == -1)
                Console.WriteLine("Comprobante no encontrado.");
            else
                MostrarComprobante(pos);

            Console.ReadKey();
        }

        //Obtener índice del código del comprobante
        static int BuscarComprobantePorCodigo(string codigo) 
        {
            for (int i = 0; i < ncom; i++)
                if (ComprobanteCodigos[i] == codigo)
                    return i;
            return -1;
        }

        //
        //MÉTODOS AUXILIARES GENERALES
        //

        //Validar que lo ingresado sea entero int
        static int LeerEntero(string mensaje)
        {
            int valor;
            Console.Write(mensaje);

            while (!int.TryParse(Console.ReadLine(), out valor))
                Console.Write("Ingrese un numero entero valido: ");

            return valor;
        }

        //Validar que el valor ingresado sea double
        static double LeerDouble(string mensaje)
        {
            double valor;
            Console.Write(mensaje);

            while (!double.TryParse(Console.ReadLine(), out valor) || double.IsNaN(valor) || double.IsInfinity(valor))
            {
                Console.Write("Ingrese un numero valido: ");
            }

            return valor;
        }

        //MÉTODOS PARA GUARDAR Y CARGAR DATOS REGISTRADOS EN NOTEPAD
        static public void Guardar_datos()
        {
            Console.WriteLine("*****GUARDAR DATOS EN ARCHIVO*****");
            if (nprod == 0)
            {
                Console.WriteLine("No hay registros de productos para guardar.");
                return;
            }
            using (StreamWriter escribir = new StreamWriter(rutaArchivo))
            {
                escribir.WriteLine("CONTADORES");
                escribir.WriteLine(nprod);
                escribir.WriteLine(ncli);
                escribir.WriteLine(ncom);

                escribir.WriteLine("PRODUCTOS");

                for (int i = 0; i < nprod; i++)
                {
                    escribir.WriteLine($"{codigos[i]}|{nombres[i]}|" +$"{precios[i].ToString(CultureInfo.InvariantCulture)}|" +
                    $"{stock[i]}|{stock_inicial[i]}");
                }

                escribir.WriteLine("CLIENTES");
                for (int i = 0; i < ncli; i++)
                {
                    escribir.WriteLine($"{dnis[i]}|{clientes[i]}|{telefonos[i]}");
                }

                escribir.WriteLine("COMPROBANTES");
                for (int i = 0; i < ncom; i++) 
                {
                    escribir.WriteLine($"{ComprobanteCodigos[i]}|" + $"{indiceProd[i]}|" + $"{indiceCli[i]}|" +
                    $"{totalVenta[i].ToString(CultureInfo.InvariantCulture)}|" + $"{cantidadVendida[i]}");
                }
            }
            Console.WriteLine($"Datos guardados correctamente.");
        }
        static public void Cargar_datos() 
        {
            Console.WriteLine("*****CARGAR DATOS DE ARCHIVO*****");
            if (!File.Exists(rutaArchivo))
            {
                Console.WriteLine("No existe el archivo.");
                return;
            }
            using (StreamReader archivo = new StreamReader(rutaArchivo)) 
            {
                archivo.ReadLine();

                nprod = int.Parse(archivo.ReadLine() ?? "0");
                ncli = int.Parse(archivo.ReadLine() ?? "0");
                ncom = int.Parse(archivo.ReadLine() ?? "0");

                archivo.ReadLine();

                for (int i = 0; i < nprod; i++)
                {
                    string linea = archivo.ReadLine() ?? "";
                    string[] datos = linea.Split('|');

                    codigos[i] = datos[0];
                    nombres[i] = datos[1];
                    precios[i] = double.Parse(
                        datos[2], CultureInfo.InvariantCulture);
                    stock[i] = int.Parse(datos[3]);
                    stock_inicial[i] = int.Parse(datos[4]);
                }
                archivo.ReadLine();

                for (int i = 0; i < ncli; i++)
                {
                    string linea = archivo.ReadLine() ?? "";
                    string[] datos = linea.Split('|');

                    dnis[i] = datos[0];
                    clientes[i] = datos[1];
                    telefonos[i] = datos[2];
                }
                archivo.ReadLine();

                for (int i = 0; i < ncom; i++)
                {
                    string linea = archivo.ReadLine() ?? "";
                    string[] datos = linea.Split('|');

                    ComprobanteCodigos[i] = datos[0];
                    indiceProd[i] = int.Parse(datos[1]);
                    indiceCli[i] = int.Parse(datos[2]);
                    totalVenta[i] = double.Parse(
                        datos[3], CultureInfo.InvariantCulture);
                    cantidadVendida[i] = int.Parse(datos[4]);
                }
                Console.WriteLine("Datos cargados correctamente.");
            }
        }


        //Método Principal Main:
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
