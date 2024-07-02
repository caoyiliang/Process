using LogInterface;
using Process.Interface;

namespace Process.Config.Model
{
    public class Flow : IFow
    {
        public string Name { get; set; } = null!;
        public List<Method> Methods { get; set; } = [];

        private static readonly ILogger _logger = Logs.LogFactory.GetLogger<Flow>();
        private Task? _task;
        private CancellationTokenSource? _cts;
        private readonly ManualResetEvent _pauseEvent = new(true);

        public void Pause()
        {
            _pauseEvent.Reset();
            Parallel.ForEach(Methods, method => method.Pause());
        }

        public void Resume()
        {
            _pauseEvent.Set();
            Parallel.ForEach(Methods, method => method.Resume());
        }

        public async Task StartAsync()
        {
            _cts = new CancellationTokenSource();
            _task = Task.Run(async () =>
            {
                foreach (var method in Methods)
                {
                    if (!_cts.Token.IsCancellationRequested)
                    {
                        _pauseEvent.WaitOne();
                        try
                        {
                            await method.StartAsync(_cts.Token);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Flow:{Name},Method:{method.Name} Error\n{ex.Message}");
                            throw;
                        }
                    }
                }
            }, _cts.Token);
            await Task.CompletedTask;
        }

        public async Task StopAsync()
        {
            _cts?.Cancel();
            if (_task != null) await _task;
            _pauseEvent.Set();
        }
    }
}
