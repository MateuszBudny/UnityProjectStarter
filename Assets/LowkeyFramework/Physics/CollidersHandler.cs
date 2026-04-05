using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollidersHandler : MonoBehaviour
{
    [SerializeField]
    private bool autoCollidersDetection = false;
    public List<Collider> colliders;

    private void Start()
    {
        if(autoCollidersDetection)
        {
            colliders = GetComponentsInChildren<Collider>().ToList();
        }
    }

    public void CollidersSetEnabled(bool enabled)
    {
        colliders.ForEach(collider => collider.enabled = enabled);
    }

    public void CollidersSetActive(bool active)
    {
        colliders.ForEach(collider => collider.gameObject.SetActive(active));
    }
}
