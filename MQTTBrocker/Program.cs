
using MQTTnet.Protocol;
using MQTTnet.Server;
using System.Net;
using System.Text;

var mqttServerFactory = new MqttServerFactory();
var mqttServerOptions = new MqttServerOptionsBuilder()
    .WithDefaultEndpoint()
    .WithDefaultEndpointPort(1883)
    .WithDefaultEndpointBoundIPAddress(IPAddress.Any)
    .Build();


using var mqttServer = mqttServerFactory.CreateMqttServer(mqttServerOptions);

mqttServer.ValidatingConnectionAsync += e =>
{
    e.ReasonCode = MqttConnectReasonCode.Success;
    return Task.CompletedTask;
};

mqttServer.InterceptingPublishAsync += e =>
{
    Console.WriteLine(e.ApplicationMessage);
    Console.WriteLine(e.ApplicationMessage.Payload);
    var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
    Console.WriteLine(payload);
    return Task.CompletedTask;
};
mqttServer.ClientConnectedAsync += e =>
{
    Console.WriteLine(e.ClientId);
    return Task.CompletedTask;
};

await mqttServer.StartAsync();
Console.WriteLine("MQTT Broker started. Press Enter to exit.");
await Task.Delay(Timeout.Infinite);