using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System.Net.WebSockets;
using System;
using System.Text;

public class WebSocketBridge : MonoBehaviour
{
    private World world;
    private string webSocketURL = "ws://localhost:1337";

    private ClientWebSocket webSocket;
    public delegate void ReceiveAction(string message);
    public event ReceiveAction OnReceived;

    private CancellationTokenSource cancellationTokenSource;
    private CancellationToken cancellationToken;

    private void Awake()
    {
        world = GetComponent<World>();

        webSocket = new ClientWebSocket();
        cancellationTokenSource = new CancellationTokenSource();
        cancellationToken = cancellationTokenSource.Token;
    }

    // Start is called before the first frame update
    async void Start()
    {
       // Debug.Log("<color=cyan>WebSocket connecting to "+ webSocketURL + "</color>");

        try
        {
            await webSocket.ConnectAsync(new System.Uri(webSocketURL), cancellationToken);

           // Debug.Log("<color=cyan>WebSocket receiving.</color>");
            await Receive();
        }
        catch (OperationCanceledException)
        {
            //Debug.Log("<color=cyan>WebSocket shutting down.</color>");
        }
        catch (WebSocketException)
        {
            //Debug.Log("<color=cyan>WebSocket connection lost.</color>");
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            throw;
        }
    }

    private async Task Receive()
    {
        var arraySegment = new ArraySegment<byte>(new byte[8192]);
        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(arraySegment, cancellationToken);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = Encoding.UTF8.GetString(arraySegment.Array, 0, result.Count);
                if (OnReceived != null) OnReceived(message);
                //Debug.Log("<color=red>WebSocket message: </color> " + message); // Todo: Remove debug & use event
            }
        }
    }

    public void Send(string message)
    {
        if (webSocket != null && webSocket.State == WebSocketState.Open)
        {
            var encoded = Encoding.UTF8.GetBytes(message);
            var buffer = new ArraySegment<Byte>(encoded, 0, encoded.Length);

            webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, cancellationToken);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Send(world.enemies[0].transform.position.ToString());
/*        if(world.targets[0])
        {
            Debug.Log(world.targets[0].transform.position);
        }
*/
    }
}
