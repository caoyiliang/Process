namespace Process.Interface
{
    public interface IMethod
    {
        string Name { get; set; }
        List<IAction> Actions { get; set; }
        void Pause();
        void Resume();
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync();
    }
}
