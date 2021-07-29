using PA.IO;
using System;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

namespace PA.Network
{
    public class SocketEventData<T>
    {
        public string eventName;
        public T arg;
    }

    public class SocketClient
    {
        private readonly WebSocket ws;
        private readonly Dictionary<string, List<(Action<object>, Type)>> listeners = new Dictionary<string, List<(Action<object>, Type)>>();

        public SocketClient(string url = "ws://localhost:8080")
        {
            this.ws = new WebSocket(url);

            this.ws.OnMessage += (sender, e) =>
            {
                SocketEventData<object> partialData = JsonUtility.FromJson<SocketEventData<object>>(e.Data);

                if (this.listeners.ContainsKey(partialData.eventName))
                    foreach (var (listener, type) in this.listeners[partialData.eventName])
                    {
                        var data = Serialize.DeserializeFromJSON(e.Data, type);
                        var value = type.GetField("arg").GetValue(data);
                        listener(value);
                    }
            };
        }

        public SocketClient Close()
        {
            this.ws.Close();
            return this;
        }

        public SocketClient Connect()
        {
            this.ws.Connect();
            return this;
        }

        public SocketClient On<T>(string eventName, Action<T> listener) where T : class
        {
            if (!this.listeners.ContainsKey(eventName))
                this.listeners.Add(eventName, new List<(Action<object>, Type)>());
            this.listeners[eventName].Add(
                ((obj) => listener((T)obj), typeof(SocketEventData<T>))
            );
            return this;
        }

        public SocketClient Emit<T>(string eventName, T arg)
        {
            this.ws.Send(
                JsonUtility.ToJson(new SocketEventData<T>
                {
                    eventName = eventName,
                    arg = arg
                })
            );

            return this;
        }
    }
}
