using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleServer
{
    public partial class Form1 : Form
    {
        private TcpListener _listener;
        private List<TcpClient> _clients = new List<TcpClient>();
        private int _port = 8888;
        private bool _isRunning = false;
        private CancellationTokenSource _cancelSource;

        public Form1()
        {
            InitializeComponent();
            StartServer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void StartServer()
        {
            _cancelSource = new CancellationTokenSource();
            try
            {
                _listener = new TcpListener(IPAddress.Any, _port);
                _listener.Start();

                AppendToChat("Сервер запущен. Ожидание подключений...");
                _isRunning = true;
                Task.Run(() => AcceptClientsAsync(_cancelSource.Token));

            }
            catch (Exception ex)
            {
                _isRunning = false;
                AppendToChat($"Ошибка запуска сервера: {ex.Message}");
            }
        }
        private async Task AcceptClientsAsync(CancellationToken cancellationToken)
        {
            while (_isRunning && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    TcpClient client = await _listener.AcceptTcpClientAsync();
                    _clients.Add(client);
                    AppendToChat($"Новый клиент подключился {client.Client.RemoteEndPoint}");
                    Task.Run(() => HandleClientAsync(client, cancellationToken));
                }
                catch (Exception ex)
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        AppendToChat($"Ошибка приема клиента {ex.Message}");
                    }

                }
            }

        }
        private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
        {
            try
            {
                using (var stream = client.GetStream())
                using (var reader = new StreamReader(stream))
                using (var writer = new StreamWriter(stream))
                {
                    while (_isRunning && !cancellationToken.IsCancellationRequested)
                    {
                        string message = await reader.ReadLineAsync();
                        if (string.IsNullOrWhiteSpace(message)) continue;
                        AppendToChat($"Клиент {client.Client.RemoteEndPoint}: {message}");
                        await BroadcastMessageAsync(message, client, writer);
                    }
                }
            }
            catch (Exception ex)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    AppendToChat($"Ошибка клиента {client.Client.RemoteEndPoint}: {ex.Message}");
                }

            }
            finally
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    _clients.Remove(client);
                    AppendToChat($"Клиент {client.Client.RemoteEndPoint} отключился.");
                    client.Close();

                }


            }
        }

        private async Task BroadcastMessageAsync(string message, TcpClient sender, StreamWriter senderWriter)
        {
            foreach (TcpClient client in _clients.ToList())
            {
                if (client != sender)
                {
                    try
                    {
                        using (var stream = client.GetStream())
                        using (var writer = new StreamWriter(stream))
                        {
                            await writer.WriteLineAsync(message);
                            await writer.FlushAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendToChat($"Ошибка при отправке клиенту {client.Client.RemoteEndPoint} сообщения {ex.Message}");
                    }

                }
            }

        }


        private void AppendToChat(string message)
        {
            if (chatTextBox.InvokeRequired)
            {
                chatTextBox.Invoke(new Action(() => AppendToChat(message)));
            }
            else
            {
                chatTextBox.AppendText(message + Environment.NewLine);
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _isRunning = false;
            _cancelSource?.Cancel();
            _listener?.Stop();
            foreach (var client in _clients)
            {
                client?.Close();
            }
            _clients.Clear();

        }

        private void sendMessageButton_Click(object sender, EventArgs e)
        {
            string message = inputTextBox.Text;
            inputTextBox.Text = "";
            AppendToChat($"Сервер: {message}");
            _ = BroadcastMessageAsync(message, null, null);
        }
    }
}
