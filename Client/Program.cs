using System;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main()
    {
        TcpClient client = new TcpClient("127.0.0.1", 8080);
        NetworkStream stream = client.GetStream();

        // Gửi phép toán đến server
        Console.WriteLine("Enter expression (e.g., 5 + 3): ");
        string expression = Console.ReadLine();
        byte[] data = Encoding.ASCII.GetBytes(expression);
        stream.Write(data, 0, data.Length);

        // Nhận kết quả từ server
        byte[] resultData = new byte[1024];
        int size = stream.Read(resultData, 0, resultData.Length);
        string result = Encoding.ASCII.GetString(resultData, 0, size);
        Console.WriteLine(result);

        stream.Close();
        client.Close();
    }
}
