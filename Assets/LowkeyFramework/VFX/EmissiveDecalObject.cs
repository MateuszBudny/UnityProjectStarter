using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// based on HandDecal shadergraph
public class EmissiveDecalObject : DecalObject
{
    private const string EMISSIVE_COLOR_PROPERTY = "_EmmisiveColor";
    private const string EMISSIVE_STRENGTH_PROPERTY = "_Emmisive_Strength";

    public Color EmissiveColor
    {
        get => Material.GetColor(EMISSIVE_COLOR_PROPERTY);
        set => Material.SetColor(EMISSIVE_COLOR_PROPERTY, value);
    }

    public float EmissiveStrength
    {
        get => Material.GetFloat(EMISSIVE_STRENGTH_PROPERTY);
        set => Material.SetFloat(EMISSIVE_STRENGTH_PROPERTY, value);
    }
}
