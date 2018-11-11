using Prism.Events;

namespace Shared.Events
{
    public class ScanningEvent : PubSubEvent<ScanningEventArgs> { }

    public class ScanningEventArgs
    {
        public string Data { get; set; }
        public string ViewModelName { get; set; }
    }
}
