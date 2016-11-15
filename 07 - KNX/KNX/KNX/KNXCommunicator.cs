using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KNXDemo
    {
    public class KNXCommunicator
        {
        public event EventHandler<KNXMessageEventArgs> KNXMessage;
        //private TcpClient client;
        private string _ipadres;
        private int _port;
        private UIElement _uieParent;   //nodig om terug te keren naar de ui thread
        private StreamWriter writer;
        private NetworkStream output;
        private StreamReader reader;
        private Boolean _gesloten = true;

        // ' thread prevents client from blocking data transfer
        private Thread readThread;

        public KNXCommunicator(string IPAdres, int port, UIElement uieParent)
            {
            _ipadres = IPAdres;
            _port = port;
            _uieParent = uieParent;
            InitClient();
            }

        private void InitClient()
            {
            readThread = new Thread(RunClient);
            readThread.Start();
            }
        public void ZendMSG(string msg)
            {
            //eventueel gesloten door een aanpassing in de knx omgeving
            if (IsGesloten) InitClient();

            writer.WriteLine(msg);
            writer.Flush();
            }

        public  bool IsGesloten { get { return _gesloten; } }


        protected virtual void OnKNXMessage(string msg)
            {
            if (KNXMessage != null)
                {
                KNXMessage(this, new KNXMessageEventArgs(msg));
                }
            }

        public delegate void OnKNXMessageDelegate(string msg);
        public void RunClient()
            {
            TcpClient client = new TcpClient(); ;
            // ' instantiate TcpClient for sending data to server
            try
                {
                //' Step 1: create TcpClient and connect to server
                client.Connect(_ipadres, _port);

                //' Step 2: get NetworkStream associated with TcpClient
                output = client.GetStream();

                //' create objects for writing and reading across stream
                writer = new StreamWriter(output);
                reader = new StreamReader(output);

                string message = "";
                try
                    {
                    //' loop until server signals termination
                    while (!message.Trim().ToLower().Equals("server>>>terminate"))
                        //' read message from server
                        try
                            {
                            message = reader.ReadLine();
                            Console.WriteLine(message);
                            OnKNXMessageDelegate del = new OnKNXMessageDelegate(OnKNXMessage);
                            //OnKNXMessage(message);
                            _uieParent.Dispatcher.BeginInvoke(del, new object[] { message });
                            }
                        catch (Exception ex)
                            {
                            }
                    }

       //' handle exception if error in reading server data
                catch (IOException inputOutputException)
                    {
                    _gesloten = true;
                    MessageBox.Show("Client application closing: " + inputOutputException.Message);     //reconnect triggeren
                    }

      // ' Step 4: close connection
                finally
                    {
                    _gesloten = true;
                    Console.WriteLine("closing connection");
                    writer.Close();
                    reader.Close();
                    output.Close();
                    client.Close();
                    }
                }

    // ' handle exception if error in establishing connection
            catch (Exception inputOutputException)
                {
                _gesloten = true;
                MessageBox.Show("Client application closing");
                }
            }
        }
    public class KNXMessageEventArgs : EventArgs
        {
        private string _msg;
        public KNXMessageEventArgs(string msg) { _msg = msg; }
        public String Message { get { return _msg; } }
        }
    }
