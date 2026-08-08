using Unity.VisualScripting;
using UnityEngine;

public class VisualEffect_UnitPreview : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;
    private VisualEffect_AttackRadius attackRadiusEffect;
    private Player_TowerBase unitToBuild;

    private float attackRange;

    private void Awake()
    {
        attackRadiusEffect = transform.AddComponent<VisualEffect_AttackRadius>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();

        unitToBuild = GetComponent<Player_TowerBase>();
        attackRange = unitToBuild.GetAttackRange();

        MakeAllMeshTransperent();
        DestroyExtraComponents();
    }

    public void ShowPreview(bool showPreview, Vector3 previewPosition)
    {
        transform.position = previewPosition;
        attackRadiusEffect.CreateCircle(showPreview, attackRange);
    }

    private void DestroyExtraComponents()
    {
        if (unitToBuild != null)
        {
            VisualEffect_CrossbowTower crossbow_Visuals = GetComponent<VisualEffect_CrossbowTower>();

            Destroy(crossbow_Visuals);
            Destroy(unitToBuild);
        }
    }

    private void MakeAllMeshTransperent()
    {
        Material previewMat = BuildManager.Instance.GetBuildPreviewMaterial();

        foreach (var mesh in meshRenderers)
        {
            mesh.material = previewMat;
        }
    }
}
