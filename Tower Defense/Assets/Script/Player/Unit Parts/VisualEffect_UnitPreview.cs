using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class VisualEffect_UnitPreview : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;
    private VisualEffect_AttackRadius attackRadiusEffect;
    private VisualEffect_AttackLinear attackLinearEffect;

    private float attackRange;

    private void Awake()
    {
        Player_TowerBase unitToBuild = GetComponent<Player_TowerBase>();
        attackRange = unitToBuild.GetAttackRange();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        attackLinearEffect = GetComponent<VisualEffect_AttackLinear>();

        if (attackLinearEffect == null)
        {
            attackRadiusEffect = gameObject.AddComponent<VisualEffect_AttackRadius>();
        }

        DestroyExtraComponents();
    }

    public void Initialize(Material previewMaterial, Material attackRadiusMaterial)
    {
        MakeAllMeshTransperent(previewMaterial);

        if (attackRadiusEffect != null)
        {
            attackRadiusEffect.SetMaterial(attackRadiusMaterial);
        }
    }

    public void ShowPreview(bool showPreview, Vector3 previewPosition)
    {
        transform.position = previewPosition;

        if (attackLinearEffect != null)
        {
            attackLinearEffect.CreateLines(showPreview, attackRange);
        }
        else
        {
            attackRadiusEffect.CreateCircle(showPreview, attackRange);
        }
    }

    private void DestroyExtraComponents()
    {
        System.Type[] componentToKepp =
        {
            typeof(Transform),
            typeof(VisualEffect_UnitPreview),
            typeof(VisualEffect_AttackRadius),
            typeof(VisualEffect_AttackLinear),
            typeof(LineRenderer),
        };

        Component[] components = GetComponents<Component>();

        for (int i = components.Length - 1; i >= 0; i--)
        {
            if (System.Array.IndexOf(componentToKepp, components[i].GetType()) < 0)
            {
                Destroy(components[i]);
            }
        }
    }

    private void MakeAllMeshTransperent(Material previewMaterial)
    {
        foreach (var mesh in meshRenderers)
        {
            mesh.material = previewMaterial;
        }
    }

    public void Rotate(float angle)
    {
        transform.Rotate(0f, angle, 0f, Space.World);

        if (attackLinearEffect != null)
        {
            attackLinearEffect.UpdateLines();
        }
    }
}
