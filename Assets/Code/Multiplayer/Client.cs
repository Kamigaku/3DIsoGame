using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace GameClient {

    public class Client : MonoBehaviour {

        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent disconnectMessageDone = new ManualResetEvent(false);

        private Socket _clientSocket = null;
        private const int BufferSize = 1024;
        private byte[] buffer = new byte[BufferSize];
        private StringBuilder sb = new StringBuilder();
        private Guid clientId;

        public string ipAddress;
        public int port;

        void Start() {
            connectionToServer(IPAddress.Loopback, 25565);
        }

        void Update() {

        }

        void OnApplicationQuit() {
            Debug.Log("Disconnecting...");
            disconnectToServer();
            Debug.Log("Disconnected");
        }

        public void connectionToServer(IPAddress ipAddress, int port) {
            try {
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Loopback, 25565);
                this._clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this._clientSocket.BeginConnect(remoteEP, new AsyncCallback(cb_ConnectionToServer), null);
                connectDone.WaitOne();
                Debug.Log("Connected");
                this._clientSocket.BeginReceive(this.buffer, 0, Client.BufferSize, 0, new AsyncCallback(cb_receiveMessage), null);
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
                disconnectToServer();
            }
        }

        private void cb_ConnectionToServer(IAsyncResult ar) {
            try {
                this._clientSocket.EndConnect(ar);
                Console.WriteLine("Socket connected to {0}", this._clientSocket.RemoteEndPoint.ToString());
                connectDone.Set();
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        public void disconnectToServer() {
            sendMessageToServer("disconnected");
            this._clientSocket.Shutdown(SocketShutdown.Both);
            this._clientSocket.Close();
        }

        public void sendMessageToServer(String data) {
            byte[] byteData = Encoding.ASCII.GetBytes(data + "<EOF>");
            this._clientSocket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(cb_sendCallback), null);
        }

        private void cb_sendCallback(IAsyncResult ar) {
            try {
                int bytesSent = this._clientSocket.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        private void cb_receiveMessage(IAsyncResult ar) {
            try {
                String content = String.Empty;
                int bytesRead = this._clientSocket.EndReceive(ar);
                if (bytesRead > 0) {
                    this.sb.Append(Encoding.ASCII.GetString(this.buffer, 0, bytesRead));
                    content = this.sb.ToString();
                    if (content.IndexOf("<EOF>") > -1) {
                        content = content.Remove(content.Length - 5, 5);
                        Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content);
                        this.clientId = new Guid(content);
                        this.sb.Remove(0, this.sb.Length);
                    }
                    else {
                        Console.WriteLine("I receive something but it wasn't complete");
                    }
                    this._clientSocket.BeginReceive(this.buffer, 0, Client.BufferSize, 0, new AsyncCallback(cb_receiveMessage), null);
                }
            } catch (SocketException socket) {
                Console.WriteLine(" >> Le serveur distant à du se fermer, déconnexion (erreur : " + socket.ToString());
                disconnectToServer();
            }
        }
    }

}
