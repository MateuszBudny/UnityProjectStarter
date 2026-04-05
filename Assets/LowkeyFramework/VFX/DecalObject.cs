using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalObject : MonoBehaviour
{
    [SerializeField]
    private DecalProjector decalProjector;

    public DecalProjector DecalProjector => decalProjector;
    public Material Material { get; private set; }

    private void Awake()
    {
        Material = decalProjector.material;
    }
}
