using Alchemy.Inspector;
using UnityEngine;

public class MaterialSwapper : MonoBehaviour
{
    [SerializeField]
    private new Renderer renderer;
    [SerializeField]
    private Material defaultMaterial;
    [SerializeField]
    private Material materialToSwap;

    private bool isCurrentMaterialTheDefault = true;

    private void OnValidate()
    {
        if(renderer && !defaultMaterial)
        {
            defaultMaterial = renderer.sharedMaterial;
        }
    }

    [Button]
    public void SwapMaterials() => SwapMaterials(!isCurrentMaterialTheDefault);

    [Button]
    public void SwapMaterials(bool toDefault)
    {
        Material materialToSet = toDefault ? defaultMaterial : materialToSwap;
        isCurrentMaterialTheDefault = toDefault;
        renderer.material = materialToSet;
    }
}
