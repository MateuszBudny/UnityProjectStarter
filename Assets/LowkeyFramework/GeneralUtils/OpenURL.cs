using UnityEngine;

public class OpenURL : MonoBehaviour
{
    [SerializeField]
    private string urlToOpen;

    public void OpenTheURL()
    {
        Application.OpenURL(urlToOpen);
    }
}
