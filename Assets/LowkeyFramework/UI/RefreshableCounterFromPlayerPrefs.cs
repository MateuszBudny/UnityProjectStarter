using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class RefreshableCounterFromPlayerPrefs : MonoBehaviour
{
    [SerializeField]
    private string playerPrefsKey;
    [SerializeField]
    private TextMeshProUGUI counterTMP;
    [SerializeField]
    private LocalizeStringEvent counterLocalizeEvent;
    [SerializeField]
    private int multiplier = 1;

    public int CounterNum => PlayerPrefs.GetInt(playerPrefsKey, 0) * multiplier;

    private int previousCounterNum;

    private void Update()
    {
        if(CounterNum > 0)
        {
            counterTMP.enabled = true;
            if(previousCounterNum != CounterNum)
            {
                counterLocalizeEvent.RefreshString();
                previousCounterNum = CounterNum;
            }
        }
        else
        {
            counterTMP.enabled = false;
        }
    }
}
