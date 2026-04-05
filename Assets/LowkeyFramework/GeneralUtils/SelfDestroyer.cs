using System.Collections;
using UnityEngine;

public class SelfDestroyer : MonoBehaviour
{
    [SerializeField]
    private bool playOnAwake = true;
    [SerializeField]
    private float selfDestroyTimer = 5f;

    private bool isActive;

    private void Awake()
    {
        if(!playOnAwake)
            return;
        DestroySelfOnTimer();
    }

    public void DestroySelfOnTimer() => StartCoroutine(SelfDestroyTimerRoutine());

    private IEnumerator SelfDestroyTimerRoutine()
    {
        if(isActive)
        {
            Debug.LogError("SelfDestroy routine is already active");
            yield break;
        }
        isActive = true;
        yield return new WaitForSeconds(selfDestroyTimer);

        Destroy(gameObject);
    }
}
