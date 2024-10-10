using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    static void Main()
    {
        // Khởi tạo địa chỉ IP của server là 127.0.0.1 (localhost)
        IPAddress ip = IPAddress.Parse("127.0.0.1");

        // Tạo một TcpListener lắng nghe các kết nối từ client trên cổng 8080
        TcpListener server = new TcpListener(ip, 8080);

        // Bắt đầu lắng nghe kết nối từ client
        server.Start();
        Console.WriteLine("Server started...");

        // Vòng lặp vô hạn để liên tục lắng nghe kết nối từ nhiều client
        while (true)
        {
            Socket client = null;
            try
            {
                // Chấp nhận kết nối từ client và trả về đối tượng Socket để giao tiếp
                client = server.AcceptSocket();
                Console.WriteLine("Client connected!");

                // Nhận dữ liệu từ client
                byte[] data = new byte[1024]; // Tạo một mảng byte để lưu dữ liệu nhận được
                int size = client.Receive(data); // Đọc dữ liệu từ client, trả về kích thước dữ liệu nhận được
                string expression = Encoding.ASCII.GetString(data, 0, size); // Chuyển đổi dữ liệu từ byte sang chuỗi
                Console.WriteLine("Received expression: " + expression); // In ra biểu thức nhận được

                // Tính toán kết quả từ biểu thức và gửi kết quả về cho client
                string result = Calculate(expression); // Gọi hàm Calculate để tính kết quả
                byte[] resultData = Encoding.ASCII.GetBytes(result); // Chuyển đổi kết quả thành mảng byte để gửi
                client.Send(resultData); // Gửi dữ liệu kết quả về cho client
            }
            catch (Exception ex)
            {
                // In ra thông báo lỗi nếu có ngoại lệ xảy ra trong quá trình xử lý
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // Đảm bảo đóng kết nối với client sau khi hoàn tất giao tiếp
                if (client != null)
                {
                    client.Close(); // Đóng kết nối socket
                    Console.WriteLine("Client disconnected");
                }
            }
        }
    }

    // Hàm Calculate nhận vào một biểu thức dưới dạng chuỗi và trả về kết quả tính toán
    static string Calculate(string expression)
    {
        try
        {
            // Tách biểu thức thành các phần tử dựa trên dấu cách (mỗi toán hạng và toán tử cách nhau bởi dấu cách)
            string[] parts = expression.Split(' ');

            // Giả sử biểu thức đầu vào có dạng: "operand1 operator operand2 operator operand3 ..."
            double result = Convert.ToDouble(parts[0]); // Bắt đầu với toán hạng đầu tiên (operand1)

            // Xử lý từng cặp (toán tử, toán hạng) trong biểu thức
            for (int i = 1; i < parts.Length; i += 2)
            {
                string operation = parts[i]; // Toán tử (+, -, *, /)
                double operand = Convert.ToDouble(parts[i + 1]); // Toán hạng tiếp theo (operand2, operand3,...)

                // Thực hiện phép toán tương ứng với toán tử
                switch (operation)
                {
                    case "+":
                        result += operand; // Cộng thêm operand vào result
                        break;
                    case "-":
                        result -= operand; // Trừ operand từ result
                        break;
                    case "*":
                        result *= operand; // Nhân operand với result
                        break;
                    case "/":
                        if (operand != 0)
                            result /= operand; // Chia result cho operand nếu operand khác 0
                        else
                            return "Error: Division by zero"; // Báo lỗi nếu chia cho 0
                        break;
                    default:
                        return "Error: Invalid operation"; // Báo lỗi nếu gặp toán tử không hợp lệ
                }
            }

            // Trả về kết quả tính toán dưới dạng chuỗi
            return "Result: " + result;
        }
        catch
        {
            // Nếu xảy ra lỗi trong quá trình tính toán, trả về thông báo lỗi
            return "Error: Invalid expression format";
        }
    }
}
