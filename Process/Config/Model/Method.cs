using LogInterface;
using Process.Interface;

namespace Process.Config.Model
{
    public class Method : IMethod
    {
        private static readonly ILogger _logger = Logs.LogFactory.GetLogger<Method>();
        public string Name { get; set; } = null!;
        public List<IAction> Actions { get; set; } = [];

        private Task? _task;
        private CancellationTokenSource? _cts;
        private readonly ManualResetEvent _pauseEvent = new(true);

        public void Pause()
        {
            _pauseEvent.Reset();
        }

        public void Resume()
        {
            _pauseEvent.Set();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = new CancellationTokenSource();
            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cts.Token);
            _task = Task.Run(async () =>
            {
                foreach (var action in Actions)
                {
                    if (!cts.Token.IsCancellationRequested)
                    {
                        _pauseEvent.WaitOne();
                        try
                        {
                            await action.StartAsync(cts.Token);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Method:{Name},Action:{action.Name} Error\n{ex.Message}");
                            throw;
                        }
                    }
                }
            }, cts.Token);
            await _task;
        }

        public async Task StopAsync()
        {
            _cts?.Cancel();
            if (_task != null) await _task;
            _pauseEvent.Set();
        }
    }
}
