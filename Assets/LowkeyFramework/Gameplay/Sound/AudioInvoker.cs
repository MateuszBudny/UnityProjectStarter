using UnityEngine;
using static AudioManager;

public class AudioInvoker : MonoBehaviour
{
    [SerializeField]
    private AudioRecord audioRecord;

    public void PlayAudio()
    {
        AudioManager.Instance.Play(audioRecord, transform.position);
    }

    public void PlayAudio(Vector3 position)
    {
        AudioManager.Instance.Play(audioRecord, position);
    }
}
