namespace Process.Interface
{
    public interface IAction
    {
        string Name { get; set; }
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync();
    }
}
