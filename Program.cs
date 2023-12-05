using System.Text;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;

namespace zebra;

class Program
{
    static void Main(string[] args)
    {
        Print("127.0.0.1");
    }

    static void Print(string ip)
    {
        try
        {
            // Aqui utilizei conexão TCP para comunicar com a impressora, mas existem outras possibilidades como por exemplo USB.
            var connection = new TcpConnection(ip, TcpConnection.DEFAULT_ZPL_TCP_PORT);
            
            if (!connection.Connected)
                connection.Open();

            /// Neste exemplo, ^BCN,,N,N é o comando ZPL para um código de barras Code 128 (BCN), 
            /// e >;>8012345000004 é o dado que você está codificando no código de barras. 
            /// Você pode substituir 8012345000004 pelo seu próprio número DUN-14.
            /// Por favor, substitua o número DUN-14 pelo seu próprio número.

            string zplData = "^XA" +
                "^BY2,3,90^FT100,250^BCN,,N,N" +
                "^FD>;>8012345000004^FS" +
                "^XZ";

            connection.Write(Encoding.UTF8.GetBytes(zplData));            
            var printerStatus = GetStatusPrint(ip);
            Console.WriteLine(printerStatus.ToString());
            connection.Close();
        }
        catch (ConnectionException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    // Obtem o status da impressora
    static PrinterStatus GetStatusPrint(string ip)
    {
        try
        {
            Connection connection = new TcpConnection(ip, TcpConnection.DEFAULT_ZPL_TCP_PORT);
            
            if (!connection.Connected)
                connection.Open();

            ZebraPrinter printer = ZebraPrinterFactory.GetInstance(connection);
            var printerStatus = printer.GetCurrentStatus();
            connection.Close();
            return printerStatus;
        }
        catch (ConnectionException e)
        {
            throw new Exception(e.Message);
        }
    }
}