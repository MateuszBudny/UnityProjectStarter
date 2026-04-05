using UnityEngine;
using UnityEngine.Events;

public class RecordingSetup : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onRecording;
    [SerializeField]
    private UnityEvent onNotRecording;

    private void Awake()
    {
#if RECORDING
        onRecording.Invoke();
#else
        onNotRecording.Invoke();
#endif
    }
}
