using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
#if!UNITY_EDITOR
using Unity.Services.Core.Environments;
#endif

public class UnityAnalytics : IAnalytics
{
    public string PrivacyPolicyUrl => AnalyticsService.Instance.PrivacyUrl;

    private bool IsPrimitiveType(object value) => value is string || value is int || value is long || value is float || value is double || value is bool;

    public async Task InitAnalyticsAsync()
    {
        InitializationOptions options = new InitializationOptions();
#if !UNITY_EDITOR
        options.SetEnvironmentName("production");
#endif
        await UnityServices.InitializeAsync(options);
    }

    public void StartDataCollection()
    {
        AnalyticsService.Instance.StartDataCollection();
    }

    public void SendEvent(string eventName, Dictionary<string, object> parameters = null)
    {
        CustomEvent customEvent = new CustomEvent(eventName);
        Dictionary<string, object> processedParameters = new Dictionary<string, object>();
        if(parameters != null)
        {
            foreach(KeyValuePair<string, object> param in parameters)
            {
                if(param.Value == null)
                {
                    processedParameters[param.Key] = "null";
                }
                else if(IsPrimitiveType(param.Value))
                {
                    processedParameters[param.Key] = param.Value;
                }
                else if(param.Value is GameObject go)
                {
                    processedParameters[param.Key] = AnalyticsFeeder.GetGameObjectHierarchy(go);
                }
                else if(param.Value is Component comp)
                {
                    processedParameters[param.Key] = AnalyticsFeeder.GetComponentHierarchy(comp);
                }
                else
                {
                    processedParameters[param.Key] = param.Value.ToString();
                }

                customEvent.Add(param.Key, processedParameters[param.Key]);
            }
        }
        
        AnalyticsService.Instance.RecordEvent(customEvent);
        Debug.Log($"Analytics Event Sent - Event: '{eventName}' | Data: {string.Join(", ", processedParameters.Select(kvp => $"{kvp.Key}={kvp.Value}"))}\n");

#if UNITY_EDITOR
        FlushEvents();
#endif
    }

    public void FlushEvents()
    {
        AnalyticsService.Instance.Flush();
    }

    public void StopDataCollection()
    {
        AnalyticsService.Instance.StopDataCollection();
    }

    public void RequestDataDeletion()
    {
        AnalyticsService.Instance.RequestDataDeletion();
    }
}
