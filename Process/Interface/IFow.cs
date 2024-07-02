using Process.Config.Model;

namespace Process.Interface
{
    public interface IFow
    {
        string Name { get; set; }
        List<Method> Methods { get; set; }
        void Pause();
        void Resume();
        Task StartAsync();
        Task StopAsync();
    }
}
