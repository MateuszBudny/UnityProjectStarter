using NodeCanvas.Framework;
using UnityEngine;

public class AnalyticsConsentChanger : MonoBehaviour
{
    [Header("Signals")]
    [SerializeField]
    private SignalDefinition analyticsInitializedSignal;
    [SerializeField]
    private SignalDefinition consentDecidedSignal;

    private AnalyticsHandler analyticsHandler;

    private void OnEnable()
    {
        analyticsInitializedSignal.onInvoke += OnAnalyticsInitialized;
    }

    private void OnDisable()
    {
        analyticsInitializedSignal.onInvoke -= OnAnalyticsInitialized;
    }

    public void ChangeConsent(bool consent)
    {
        consentDecidedSignal.Invoke(transform, null, true, consent);
    }

    public void OpenPrivacyPolicyUrl()
    {
        Application.OpenURL(analyticsHandler.Analytics.PrivacyPolicyUrl);
    }

    private void OnAnalyticsInitialized(Transform sender, Transform receiver, bool isGlobal, object[] args)
    {
        analyticsHandler = args[0] as AnalyticsHandler;
    }
}
