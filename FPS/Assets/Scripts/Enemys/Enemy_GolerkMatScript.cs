using UnityEngine;

public class Enemy_GolerkMatScript : MonoBehaviour
{
    public SkinnedMeshRenderer sRenderer;
    public Target target;
    Material[] materials;
    private void Start()
    {
        materials = new Material[2];
        materials[0] = new Material(sRenderer.materials[1]);
        materials[1] = new Material(sRenderer.materials[2]);
        sRenderer.materials[1] = materials[0];
        sRenderer.materials[2] = materials[1];

    }
    private void FixedUpdate()
    {
        foreach (Material item in materials)
        {
            item.SetFloat("_life", target.health);
        }
        sRenderer.materials[1] = materials[0];
        sRenderer.materials[2] = materials[1];
    }
}
