using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class TcpServer
{
    static void Main()
    {
        int port = 5000;
        TcpListener server = new TcpListener(IPAddress.Any, port); // Tillad forbindelser fra enhver IP
        server.Start();
        Console.WriteLine($"Server started on port {port} and listening on all network interfaces...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Client connected...");
            Thread clientThread = new Thread(() => HandleClient(client));
            clientThread.Start();
        }
    }

    static void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;

        try
        {
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string request = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received: {request}");

                string[] parts = request.Split(' ');

                string response = parts[0] switch
                {
                    "Random" => "2: Input numbers",
                    "Add" => "2: Input numbers",
                    "Subtract" => "2: Input numbers",
                    _ => "Error: Invalid command"
                };

                if (parts.Length == 1)
                {
                    SendResponse(stream, response);
                }
                else if (parts.Length == 3 && int.TryParse(parts[1], out int num1) && int.TryParse(parts[2], out int num2))
                {
                    response = parts[0] switch
                    {
                        "Random" => $"4: {new Random().Next(num1, num2 + 1)}",
                        "Add" => $"4: {num1 + num2}",
                        "Subtract" => $"4: {num1 - num2}",
                        _ => "Error: Invalid operation"
                    };

                    SendResponse(stream, response);
                }
                else
                {
                    SendResponse(stream, "Error: Invalid input format");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Client disconnected: {ex.Message}");
        }
        finally
        {
            client.Close();
        }
    }

    static void SendResponse(NetworkStream stream, string response)
    {
        byte[] data = Encoding.ASCII.GetBytes(response);
        stream.Write(data, 0, data.Length);
        Console.WriteLine($"Sent: {response}");
    }
}

