﻿using System;
using System.Net.Sockets;
using System.Text;

namespace Unknown_World_of_Mystery_server
{
    public class Server
    {
        public TcpClient client;

        public Server(TcpClient client)
        {
            this.client = client;
        }

        public void Process()
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                byte[] data = new byte[64]; // буфер для получаемых данных
                while (true)
                {
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();

                    Console.WriteLine(message);
                    // отправляем сообщение
                    message = message.Substring(message.IndexOf(':') + 1).Trim();
                    IBroker broker = new Broker();
                    data = Encoding.Unicode.GetBytes(broker.FormResponse(message));
                    stream.Write(data, 0, data.Length);
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }
    }
}
