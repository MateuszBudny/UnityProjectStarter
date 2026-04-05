using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAnalytics
{
    string PrivacyPolicyUrl { get; }

    Task InitAnalyticsAsync();
    void StartDataCollection();
    void SendEvent(string eventName, Dictionary<string, object> parameters = null);
    void FlushEvents();
    void StopDataCollection();
    void RequestDataDeletion();
}
