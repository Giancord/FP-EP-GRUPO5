using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFV1_PorCorregir
{
    internal class Program
    {
        //Limite máximo
        static int max = 100;

        //Contadores
        static int nprod = 0, ncli = 0, ncom = 0;
        static int nbol = 1; //sirve para dar el ultimo número al Código de comprobante (empezando en 1, ej: N0001-2-3...)

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

        //Respaldo de Clientes
        static string[] dnisRespaldo = new string[max];
        static string[] clientesRespaldo = new string[max];
        static string[] telefonosRespaldo = new string[max]; //Sirven para guardar los datos originales
                                                             //de un cliente asociados a un comprobante en caso se
                                                             //ordenen los datos de los clientes alfabeticamente y luego se muestren los
                                                             //comprobantes por DNI.

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
                op = Validadores.LeerMenu("Ingrese una opción: ");
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
                op = Validadores.LeerMenu("Ingrese una opción: "); ;

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

            string codigo = Validadores.LeerTexto("Código: ");

            if (BuscarProductoPorCodigo(codigo) != -1)
            {
                Console.WriteLine("Codigo ya existente.");
                Console.ReadKey();
                return;
            }

            string nombre = Validadores.LeerTexto("Nombre: ");
            double precio = Validadores.LeerDouble("Precio: ");
            int cantidad = Validadores.LeerEntero("Stock: ");

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
                double precio = Validadores.LeerDouble("Nuevo precio: ");
                int cantidad = Validadores.LeerEntero("Nuevo stock: ");

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
                op = Validadores.LeerMenu("Ingrese una opción: ");

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
            string valpermitidos = "1234567890";
            string dni;
            Console.Clear();
            Console.WriteLine("REGISTRAR CLIENTE");

            if (ncli >= max)
            {
                Console.WriteLine("Limite de clientes alcanzado.");
                Console.ReadKey();
                return;
            }

            while (true)
            {
                bool valido = true;
                Console.Write("DNI: ");
                dni = Console.ReadLine()?.Trim() ?? "";


                foreach (char c in dni)
                {
                    if (!valpermitidos.Contains(c))
                    {
                        valido = false;
                        break;
                    }
                }
                if (dni.Trim() == "" || BuscarClientePorDni(dni) != -1 || dni.Length != 8 || !valido)
                {
                    Console.WriteLine("DNI vacio, ya registrado o ingresó una longitud incorrecta (8 caracteres numéricos).");
                    Console.ReadKey();
                    continue;
                }
                break;
            }

            string nombre = Validadores.LeerTexto("Nombre: ");

            while (true)
            {
                bool valido = true;
                string telefono1 = Validadores.LeerTexto("Telefono (Omita el 9 inicial [solo ingrese los siguientes 8 dígitos]): ");
                string telefono = telefono_inicio + telefono1;

                foreach (char c in telefono)
                {
                    if (!valpermitidos.Contains(c))
                    {
                        valido = false;
                        break;
                    }
                }
                if (telefono.Length == 9 && valido)
                {
                    dnis[ncli] = dni;
                    clientes[ncli] = nombre;
                    telefonos[ncli] = telefono;
                    dnisRespaldo[ncli] = dni;
                    clientesRespaldo[ncli] = nombre;
                    telefonosRespaldo[ncli] = telefono;
                    ncli++;
                    Console.WriteLine("Cliente registrado.");
                    break;
                }
                Console.WriteLine("Ingrese un número telefónico de 9 digitos numéricos [no incluya el 9 inicial].");
            }

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
                op = Validadores.LeerMenu("Ingrese una opción: ");

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
            string codigoComprobante = CodigoComprobInicio + nbol;
            double totalComprobante = 0;
            bool seRegistroProducto = false;

            while (true)
            {
                Console.Write("Codigo del producto: ");
                string codigo = Console.ReadLine() ?? "";
                int pos = BuscarProductoPorCodigo(codigo);

                if (pos == -1)
                {
                    Console.WriteLine("Producto no encontrado.");
                }
                else if (ncom >= max)
                {
                    Console.WriteLine("Se alcanzó el límite permitido de registros.");
                    break;
                }
                else
                {
                    MostrarProducto(pos);
                    int cantidad = Validadores.LeerEntero("Cantidad a vender: ");

                    if (cantidad <= 0 || cantidad > stock[pos])
                    {
                        Console.WriteLine("Cantidad invalida o stock insuficiente.");
                    }
                    else
                    {
                        double subtotal = precios[pos] * cantidad;
                        stock[pos] -= cantidad;



                        ComprobanteCodigos[ncom] = codigoComprobante;
                        indiceProd[ncom] = pos;
                        indiceCli[ncom] = -1;
                        cantidadVendida[ncom] = cantidad;
                        totalVenta[ncom] = subtotal;
                        ncom++;

                        totalComprobante += subtotal;
                        seRegistroProducto = true;
                        Console.WriteLine("Producto agregado al comprobante.");
                        Console.WriteLine($"Subtotal: S/{subtotal}");
                        Console.WriteLine($"Stock restante: {stock[pos]}");
                        if ((stock[pos]) <= (stock_inicial[pos] / 2))
                        {
                            Console.WriteLine($"Alerta!!! El stock restante [{stock[pos]}] es inferior o igual a la mitad del stock inicial " +
                                $"[{stock_inicial[pos]}]," +
                                $" se recomienda reabastecer.");
                        }
                    }
                }
                if (ncom >= max)
                {
                    break;
                }

                string respuesta = "";
                while (true)
                {
                    respuesta = Validadores.LeerTexto("¿Desea agregar otro producto al mismo comprobante[S/N]?: ").ToUpper();
                    if (respuesta == "S" || respuesta == "N")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ingrese solo [S/N].");
                    }
                }
                if (respuesta == "N")
                {
                    if (seRegistroProducto)
                    {
                        nbol++;
                        Console.WriteLine("\nBOLETA DE VENTA");
                        Console.WriteLine($"Código de comprobante: {codigoComprobante}");
                        Console.WriteLine($"Total: S/{totalComprobante}");
                    }
                    else
                    {
                        Console.WriteLine("No se registraron productos (comprobante no emitido).");
                    }
                    break;
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

            Console.Write("Ingrese DNI del cliente: ");
            string dni = Console.ReadLine() ?? "";
            int posdni = BuscarClientePorDni(dni);

            if (posdni == -1)
            {
                Console.WriteLine("Cliente no encontrado.");
                Console.ReadKey();
                return;
            }
            string codigoComprobante = CodigoComprobInicio + nbol;
            double totalComprobante = 0;
            bool seRegistroProducto = false;

            while (true)
            {

                Console.Write("Codigo del producto: ");
                string codigo = Console.ReadLine() ?? "";
                int pos = BuscarProductoPorCodigo(codigo);

                if (pos == -1)
                {
                    Console.WriteLine("Producto no encontrado.");
                }
                else if (ncom >= max)
                {
                    Console.WriteLine("Se alcanzó el límite permitido de registros.");
                    break;
                }
                else
                {
                    MostrarProducto(pos);
                    int cantidad = Validadores.LeerEntero("Cantidad a vender: ");

                    if (cantidad <= 0 || cantidad > stock[pos])
                    {
                        Console.WriteLine("Cantidad invalida o stock insuficiente.");
                    }
                    else
                    {
                        double subtotal = precios[pos] * cantidad;
                        stock[pos] -= cantidad;



                        ComprobanteCodigos[ncom] = codigoComprobante;
                        indiceProd[ncom] = pos;
                        indiceCli[ncom] = posdni;
                        cantidadVendida[ncom] = cantidad;
                        totalVenta[ncom] = subtotal;
                        ncom++;

                        totalComprobante += subtotal;
                        seRegistroProducto = true;
                        Console.WriteLine("Producto agregado al comprobante.");
                        Console.WriteLine($"Subtotal: S/{subtotal}");
                        Console.WriteLine($"Stock restante: {stock[pos]}");

                        if ((stock[pos]) <= (stock_inicial[pos] / 2))
                        {
                            Console.WriteLine($"Alerta!!! El stock restante [{stock[pos]}] es inferior o igual a la mitad del " +
                                $"stock inicial [{stock_inicial[pos]}]," +
                                $" se recomienda reabastecer.");
                        }
                    }
                }
                if (ncom >= max)
                {
                    break;
                }
                string respuesta = "";
                while (true)
                {
                    respuesta = Validadores.LeerTexto("¿Desea agregar otro producto al mismo comprobante[S/N]?: ").ToUpper();
                    if (respuesta == "S" || respuesta == "N")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ingrese solo [S/N].");
                    }
                }
                if (respuesta == "N")
                {
                    if (seRegistroProducto)
                    {
                        nbol++;
                        Console.WriteLine("\nBOLETA DE VENTA CON DNI");
                        Console.WriteLine($"Código de comprobante: {codigoComprobante}");
                        Console.WriteLine($"DNI: {dnis[posdni]}");
                        Console.WriteLine($"Nombre Cliente: {clientes[posdni]}");
                        Console.WriteLine($"Total: S/{totalComprobante}");
                    }
                    else
                    {
                        Console.WriteLine("No se registraron productos (comprobante no emitido).");
                    }
                    break;
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
            {
                Console.WriteLine("No hay comprobantes registrados.");
            }
            else
            {
                for (int i = 0; i < ncom; i++)
                {
                    bool repetido = false;
                    for (int j = 0; j < i; j++)
                    {
                        if (ComprobanteCodigos[i] == ComprobanteCodigos[j])
                        {
                            repetido = true;
                            break;
                        }
                    }
                    if (!repetido)
                    {
                        MostrarComprobante(i);
                    }
                }
            }
            Console.ReadKey();
        }

        //Mostrar información de comprobante
        public static void MostrarComprobante(int i)
        {
            string codigo = ComprobanteCodigos[i];
            double total = 0;

            Console.WriteLine("Codigo de comprobante: " + codigo);


            if (indiceCli[i] != -1)
            {
                Console.WriteLine($"DNI cliente: {dnisRespaldo[indiceCli[i]]}");
                Console.WriteLine($"Nombre cliente: {clientesRespaldo[indiceCli[i]]}");
            }
            Console.WriteLine("===================================DETALLES DE LA COMPRA==================================");
            Console.WriteLine("CODIGO\t| NOMBRE \t\t| PRECIO UNITARIO \t| CANTIDAD VENDIDA \t| SUBTOTAL");
            Console.WriteLine("------------------------------------------------------------------------------------------");
            for (int j = 0; j < ncom; j++)
            {
                if (ComprobanteCodigos[j] == codigo)
                {
                    string nombre = nombres[indiceProd[j]];
                    string tabulacion = nombre.Length < 8 ? "\t" : nombre.Length < 16 ? "\t" : "";
                    Console.WriteLine($"{codigos[indiceProd[j]]} \t| {nombre}{tabulacion} \t| {precios[indiceProd[j]]} \t\t\t| {cantidadVendida[j]} \t\t\t| {totalVenta[j]}");
                    total += totalVenta[j];
                    Console.WriteLine("------------------------------------------------------------------------------------------");
                }
            }
            Console.WriteLine($"TOTAL DE COMPRA: S/{total}");
            Console.WriteLine("==========================");
            Console.WriteLine("\n");
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
                    escribir.WriteLine($"{codigos[i]}|{nombres[i]}|" + $"{precios[i].ToString(CultureInfo.InvariantCulture)}|" +
                    $"{stock[i]}|{stock_inicial[i]}");
                }

                escribir.WriteLine("CLIENTES");
                for (int i = 0; i < ncli; i++)
                {
                    escribir.WriteLine($"{dnis[i]}|{clientes[i]}|{telefonos[i]}");
                }

                escribir.WriteLine("CLIENTES RESPALDO");
                for (int i = 0; i < ncli; i++)
                {
                    escribir.WriteLine($"{dnisRespaldo[i]}|{clientesRespaldo[i]}|{telefonosRespaldo[i]}");
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

                    dnisRespaldo[i] = datos[0];
                    clientesRespaldo[i] = datos[1];
                    telefonosRespaldo[i] = datos[2];
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
