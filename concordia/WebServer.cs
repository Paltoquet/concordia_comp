using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Collections.Generic;
using Newtonsoft.Json;

using concordia;

// State object for reading client data asynchronously

public class StateObject {
    // Client  socket.
    public Socket workSocket = null;
    // Size of receive buffer.
    public const int BufferSize = 1024;
    // Receive buffer.
    public byte[] buffer = new byte[BufferSize];
// Received data string.
    public StringBuilder sb = new StringBuilder();  
}

public class AsynchronousSocketListener {
    // Thread signal.
    public static ManualResetEvent allDone = new ManualResetEvent(false);
    public static float temperature = -1;
    public static float ph = -1;
    public static float level = -1;
    public static bool is_pump_on = false;
    public static bool is_heater__on = false;
    

    public AsynchronousSocketListener() {
    }

    public static void StartListening() {
        // Data buffer for incoming data.
        byte[] bytes = new Byte[1024];

        // Establish the local endpoint for the socket.
        // The DNS name of the computer
        // running the listener is "host.contoso.com".
        IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        Console.WriteLine(ipAddress.ToString());
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        // Create a TCP/IP socket.
        Socket listener = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp );

        // Bind the socket to the local endpoint and listen for incoming connections.
        try {
            listener.Bind(localEndPoint);
            listener.Listen(100);

            while (true) {
                // Set the event to nonsignaled state.
                allDone.Reset();

                // Start an asynchronous socket to listen for connections.
                Console.WriteLine("Waiting for a connection...");
                listener.BeginAccept( 
                    new AsyncCallback(AcceptCallback),
                    listener );

                // Wait until a connection is made before continuing.
                allDone.WaitOne();
            }

        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nPress ENTER to continue...");
        Console.Read();

    }

    public static void AcceptCallback(IAsyncResult ar) {
        // Signal the main thread to continue.
        allDone.Set();

        // Get the socket that handles the client request.
        Socket listener = (Socket) ar.AsyncState;
        Socket handler = listener.EndAccept(ar);

        // Create the state object.
        StateObject state = new StateObject();
        state.workSocket = handler;
        handler.BeginReceive( state.buffer, 0, StateObject.BufferSize, 0,
            new AsyncCallback(ReadCallback), state);
    }

    public static void ReadCallback(IAsyncResult ar) {
        String content = String.Empty;

        // Retrieve the state object and the handler socket
        // from the asynchronous state object.
        StateObject state = (StateObject) ar.AsyncState;
        Socket handler = state.workSocket;
        Send(handler, buildMessage());
		/*
        // Read data from the client socket. 
        int bytesRead = handler.EndReceive(ar);

        if (bytesRead > 0) {
            // There  might be more data, so store the data received so far.
            state.sb.Append(Encoding.ASCII.GetString(
                state.buffer,0,bytesRead));

            // Check for end-of-file tag. If it is not there, read 
            // more data.
            content = state.sb.ToString();
            if (content.IndexOf("<EOF>") > -1) {
                // All the data has been read from the 
                // client. Display it on the console.
                Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                    content.Length, content );
                // Echo the data back to the client.
                Send(handler, "coucou beinvenue au concordia");
            } else {
                // Not all data received. Get more.
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
            }
        }
        */
    }

    private static void Send(Socket handler, String data) {
        // Convert the string data to byte data using ASCII encoding.
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        // Begin sending the data to the remote device.
        handler.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), handler);
    }

    private static void SendCallback(IAsyncResult ar) {
        try {
            // Retrieve the socket from the state object.
            Socket handler = (Socket) ar.AsyncState;

            // Complete sending the data to the remote device.
            int bytesSent = handler.EndSend(ar);
            Console.WriteLine("Sent {0} bytes to client.", bytesSent);

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();

        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }

    public static string buildMessage(){
    	var capteurList = new List<Captor>();
    	Captor captor_temp = new Captor();
    	captor_temp.value= temperature;
    	captor_temp.name = "temperature";
    	
    	Captor captor_ph = new Captor();
    	captor_ph.value = ph;
    	captor_ph.name = "ph";
    	
    	Captor pump = new Captor();
    	pump.value = is_pump_on ? 1 : 0;
    	pump.name = "Pump is on";
    	
    	Captor heater = new Captor();
    	heater.value = is_heater__on ? 1 : 0;
    	heater.name = "Heater is on";
    	
    	Captor captor_level = new Captor();
    	captor_level.value = level;
    	captor_level.name = "Level of water";
    	
    	
    	
    	capteurList.Add(captor_temp);
    	capteurList.Add(captor_ph);
    	capteurList.Add(captor_level);
    	capteurList.Add(pump);
    	capteurList.Add(heater);
    	string json = JsonConvert.SerializeObject(capteurList);
    	Console.WriteLine(json);
    	return json;
    	
    }
	/*
    public static int Main(String[] args) {
        StartListening();
        return 0;
    }
    */
}