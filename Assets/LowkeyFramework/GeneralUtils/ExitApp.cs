using UnityEngine;
using UnityEngine.Events;

public class ExitApp : MonoBehaviour
{
    [SerializeField]
    private float fadeOutDuration = 3f;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent onFadingStarted;

    public void ExitTheApp()
    {
        onFadingStarted?.Invoke();
        CursorManager.Instance.CurrentCursorLockMode = CursorLockMode.Locked;
        BlackScreen.Instance.FadeIn(fadeOutDuration, onComplete: () => Application.Quit());
    }
}
