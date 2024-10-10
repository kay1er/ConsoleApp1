using System;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main()
    {
        // Tạo một kết nối TCP đến server tại địa chỉ 127.0.0.1 (localhost) và cổng 8080
        TcpClient client = new TcpClient("127.0.0.1", 8080);

        // Lấy luồng dữ liệu (stream) để gửi và nhận dữ liệu từ server
        NetworkStream stream = client.GetStream();

        // Nhập phép toán từ người dùng để gửi lên server
        Console.WriteLine("Enter expression (e.g., 5 + 3): ");
        string expression = Console.ReadLine(); // Đọc phép toán từ bàn phím
        byte[] data = Encoding.ASCII.GetBytes(expression); // Chuyển chuỗi biểu thức sang dạng byte
        stream.Write(data, 0, data.Length); // Gửi dữ liệu (phép toán) lên server

        // Tạo một mảng byte để nhận kết quả từ server
        byte[] resultData = new byte[1024];
        int size = stream.Read(resultData, 0, resultData.Length); // Đọc dữ liệu trả về từ server, trả về kích thước dữ liệu nhận được
        string result = Encoding.ASCII.GetString(resultData, 0, size); // Chuyển đổi dữ liệu từ byte sang chuỗi
        Console.WriteLine(result); // Hiển thị kết quả nhận được từ server

        // Đóng stream và kết nối
        stream.Close();
        client.Close();
    }
}
