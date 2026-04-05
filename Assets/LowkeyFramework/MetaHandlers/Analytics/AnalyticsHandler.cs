using NodeCanvas.Framework;
using UnityEngine;

public class AnalyticsHandler : MonoBehaviour
{
    [SerializeField]
    private bool consentTrueOnStart = false;

    [Header("Signals")]
    [SerializeField]
    private SignalDefinition gameStateBTsInitializedSignal;

    [Header("Optional Signals")]
    [SerializeField]
    private SignalDefinition analyticsInitializedSignal;
    [SerializeField]
    private SignalDefinition consentDecidedSignal;

    private const string CONSENT_KEY = "AnalyticsConsentGiven";

    public IAnalytics Analytics { get; private set; } = new UnityAnalytics();
    public bool IsAnalyticsInitialized { get; private set; } = false;

    public bool IsConsentGiven
    {
        get => PlayerPrefs.GetInt(CONSENT_KEY, 0) == 1;
        private set
        {
            PlayerPrefs.SetInt(CONSENT_KEY, value ? 1 : 0);
            if(IsConsentGiven)
            {
                Analytics.StartDataCollection();
                Debug.Log("Analytics Consent Given");
            }
            else
            {
                Analytics.StopDataCollection();
                Debug.Log("Analytics Consent Revoked");
            }
        }
    }

    private async System.Threading.Tasks.Task InitAnalytics()
    {
        await Analytics.InitAnalyticsAsync();

        IsAnalyticsInitialized = true;
        Debug.Log("Analytics Initialized");

        if(analyticsInitializedSignal)
        {
            analyticsInitializedSignal.Invoke(transform, null, true, this);
        }

        if(consentTrueOnStart)
        {
            IsConsentGiven = true;
        }
        else if(IsConsentGiven)
        {
            Analytics.StartDataCollection();
        }
    }

    private void OnEnable()
    {
        gameStateBTsInitializedSignal.onInvoke += OnGameStateBTsInitialized;
        if(consentDecidedSignal)
        {
            consentDecidedSignal.onInvoke += OnConsentDecided;
        }
    }

    private void OnDisable()
    {
        gameStateBTsInitializedSignal.onInvoke -= OnGameStateBTsInitialized;
        if(consentDecidedSignal)
        {
            consentDecidedSignal.onInvoke -= OnConsentDecided;
        }
    }

    private async void OnGameStateBTsInitialized(Transform sender, Transform receiver, bool isGlobal, object[] args)
    {
        await InitAnalytics();
    }

    private void OnConsentDecided(Transform sender, Transform receiver, bool isGlobal, object[] args)
    {
        IsConsentGiven = (bool)args[0];
    }
}
