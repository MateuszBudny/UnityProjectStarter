using UnityEngine;

public class ForcedFaceCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        if(Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
