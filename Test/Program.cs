// See https://aka.ms/new-console-template for more information
using Process.Config;
using Process.Config.Model;
using Test;

Console.WriteLine("Hello, World!");
var config = await ConfigManager.Instance.InitAsync();
var flows = config.Flows;
//for (int x = 0; x < 2; x++)
//{
//    var flow = new Flow() { Name = $"Flow_{x}" };
//    flows.FlowCollection.Add(flow);
//    for (int i = 0; i < 3; i++)
//    {
//        var method = new Method() { Name = $"Method_{x}_{i}" };
//        flow.Methods.Add(method);
//        for (int j = 0; j < 10; j++)
//        {
//            method.Actions.Add(new TestAction($"Action_{x}_{i}_{j}"));
//        }
//    }
//}
//await flows.TrySaveChangeAsync();
string? cmd;
do
{
    cmd = Console.ReadLine();
    switch (cmd)
    {
        case "start":
            {
                await flows.StartAsync();
                Console.WriteLine("Start");
            }
            break;
        case "p":
            {
                flows.Pause();
                Console.WriteLine("Pause");
            }
            break;
        case "r":
            {
                flows.Resume();
                Console.WriteLine("Resume");
            }
            break;
        case "stop":
            {
                await flows.StopAsync();
                Console.WriteLine("Stop");
            }
            break;
    }
} while (cmd != "exit");