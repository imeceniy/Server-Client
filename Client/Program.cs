using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    
    class Program
    {

        public static string result;
        static void Main(string[] args)
        {
            result=Console.ReadLine();
            Console.Write(result);
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine("\n Позиция № " + i.ToString() + "\n");
                Connect("127.0.0.1", "Hello World! № " + i.ToString());
            }
            Console.WriteLine("\n Нажмите Enter чтобы продолжить...");
            Console.Read();

        }

        static void Connect(String server, String message)
        {
            try
            {
                // Создаём TcpClient.
                // Для созданного в предыдущем проекте TcpListener 
                // Настраиваем его на IP нашего сервера и тот же порт.

                Int32 port = 9595;
                TcpClient client = new TcpClient(server, port);

                // Переводим наше сообщение в ASCII, а затем в массив Byte.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(result);

                // Получаем поток для чтения и записи данных.
                NetworkStream stream = client.GetStream();

                // Отправляем сообщение нашему серверу. 
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent: {0}", result);

                // Получаем ответ от сервера.

                // Буфер для хранения принятого массива bytes.
                data = new Byte[256];

                // Строка для хранения полученных ASCII данных.
                String responseData = String.Empty;

                // Читаем первый пакет ответа сервера. 
                // Можно читать всё сообщение.
                // Для этого надо организовать чтение в цикле как на сервере.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Закрываем всё.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }


        }
    }
}
