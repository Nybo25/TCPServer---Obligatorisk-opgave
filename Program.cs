using System;
using System.Net.Sockets;
using System.Text;

class TcpClientApp
{
    static void Main()
    {
        string serverIp = "127.0.0.1"; 
        int port = 5000;

        try
        {
            using (TcpClient client = new TcpClient(serverIp, port))
            {
                NetworkStream stream = client.GetStream();
                Console.WriteLine("Connected to server!");

                while (true)
                {
                    Console.WriteLine("Enter command (Random, Add, Subtract): ");
                    string command = Console.ReadLine();

                    SendData(stream, command);
                    string serverResponse = ReceiveData(stream);
                    Console.WriteLine($"Server: {serverResponse}");

                    if (serverResponse.Contains("Input numbers"))
                    {
                        Console.WriteLine("Enter two numbers separated by space: ");
                        string numbers = Console.ReadLine();

                        SendData(stream, command + " " + numbers);
                        string result = ReceiveData(stream);
                        Console.WriteLine($"Server result: {result}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void SendData(NetworkStream stream, string data)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(data);
        stream.Write(bytes, 0, bytes.Length);
    }

    static string ReceiveData(NetworkStream stream)
    {
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        return Encoding.ASCII.GetString(buffer, 0, bytesRead);
    }
}