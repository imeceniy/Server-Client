using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server_Client
{
    class Program
    {
        public string[,] table = new string[3, 300];
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                // Определим нужное максимальное количество потоков
                // Пусть будет по 4 на каждый процессор
                int MaxThreadsCount = Environment.ProcessorCount * 4;
                Console.WriteLine(MaxThreadsCount.ToString());
                // Установим максимальное количество рабочих потоков
                ThreadPool.SetMaxThreads(MaxThreadsCount, MaxThreadsCount);
                // Установим минимальное количество рабочих потоков
                ThreadPool.SetMinThreads(2, 2);


                // Устанавливаем порт для TcpListener = 9595.
                Int32 port = 9595;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                int counter = 0;
                server = new TcpListener(localAddr, port);

                // Запускаем TcpListener и начинаем слушать клиентов.
                server.Start();

                // Принимаем клиентов в бесконечном цикле.
                while (true)
                {

                    Console.Write("\nОжидание соеденения... ");

                    // При появлении клиента добавляем в очередь потоков его обработку.
                    ThreadPool.QueueUserWorkItem(ProcessingRequest, server.AcceptTcpClient());
                    // Выводим информацию о подключении.
                    counter++;
                    Console.Write("\nСоеденение №" + counter.ToString() + "!");

                }
            }
            catch (SocketException e)
            {
                //В случае ошибки, выводим что это за ошибка.
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Останавливаем TcpListener.
                server.Stop();
            }


            Console.WriteLine("\nНажмите Enter чтобы продолжить...");
            Console.Read();
        }
        static void ProcessingRequest(object client_obj)
        {
            // Буфер для принимаемых данных.
            Byte[] bytes = new Byte[256];
            String data = null;
            string str = "";
            string[] commandProp;
            //Можно раскомментировать Thread.Sleep(1000); 
            //Запустить несколько клиентов
            //и наглядно увидеть как они обрабатываются в очереди. 
            Thread.Sleep(1000);

            TcpClient client = client_obj as TcpClient;

            data = null;

            // Получаем информацию от клиента
            NetworkStream stream = client.GetStream();

            int i;
            int kk = 11;
            // Принимаем данные от клиента в цикле пока не дойдём до конца.
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                // Преобразуем данные в ASCII string.
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                // Преобразуем строку к верхнему регистру для проверки есть ли передача в обе стороны.
                // data = data.ToUpper();
                if (data.Contains("add_result"))
                {
                    for (int j = 11; j < data.Length-1; j++)
                    {
                        str += data[j];
                    }
                }

                commandProp = str.Split(new char[] { ',' });
                Console.Write(str);


                // Преобразуем полученную строку в массив Байт.
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                // Отправляем данные обратно клиенту (ответ).
                stream.Write(msg, 0, msg.Length);

            }



            // Закрываем соединение.
            client.Close();


        }
        public void add_result(string game, string name, string balls)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 300; j++)
                {
                    if (table[i, j] == null)
                    {
                        table[i, j] = game;
                        table[i+1, j] = name;
                        table[i+2, j] = balls;
                        break;
                    }
                }
            }
        }
    }
}
