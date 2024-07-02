using DataPairs;
using DataPairs.Interfaces;
using LogInterface;
using Process.Config.Model;

namespace Process.Config.Collection
{
    public class Flows
    {
        public List<Flow> FlowCollection { get; set; } = [];
        private static readonly IDataPair<Flows> _pair = new DataPair<Flows>(nameof(Flows), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PairsDB.dll"));
        private static readonly ILogger _logger = Logs.LogFactory.GetLogger<Flows>();

        public async Task InitAsync()
        {
            var data = await _pair.TryGetValueAsync();
            foreach (var item in data.GetType().GetProperties())
            {
                GetType().GetProperty(item.Name)!.SetValue(this, item.GetValue(data));
            }
        }

        public async Task TrySaveChangeAsync()
        {
            await _pair.TryInitOrUpdateAsync(this);
        }

        public void Pause()
        {
            Parallel.ForEach(FlowCollection, flow => flow.Pause());
        }

        public void Resume()
        {
            Parallel.ForEach(FlowCollection, flow => flow.Resume());
        }

        public async Task StartAsync()
        {
            try
            {
                await Task.WhenAll(FlowCollection.Select(async flow => await flow.StartAsync()));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task StopAsync()
        {
            await Task.WhenAll(FlowCollection.Select(async flow => await flow.StopAsync()));
        }
    }
}
