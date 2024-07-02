using Process.Interface;

namespace Test
{
    internal class TestAction(string name) : IAction
    {
        private CancellationTokenSource? _cts;

        public string Name { get; set; } = name;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{Name} StartAsync");
            _cts = new CancellationTokenSource();
            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cts.Token);
            try
            {
                await Task.Delay(5000, cts.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine($"{Name} StartAsync End");
        }

        public async Task StopAsync()
        {
            Console.WriteLine($"{Name} StopAsync");
            _cts?.Cancel();
            await Task.CompletedTask;
        }
    }
}
