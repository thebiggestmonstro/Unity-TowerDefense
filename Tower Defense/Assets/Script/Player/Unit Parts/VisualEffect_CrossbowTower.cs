using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player_TowerCrossbow))]
public class VisualEffect_CrossbowTower : MonoBehaviour
{
    [SerializeField]
    private LineRenderer visualEffect;
    [SerializeField]
    private float visualEffectDuration = 0.1f;

    private Player_TowerCrossbow crossbowTower;

    private void Awake()
    {
        crossbowTower = GetComponent<Player_TowerCrossbow>();
    }

    public void EnableVisualEffect(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(CoDisableVisualEffect(startPoint, endPoint));
    }

    private IEnumerator CoDisableVisualEffect(Vector3 startPoint, Vector3 endPoint)
    {
        crossbowTower.EnableRotation(false);
        visualEffect.enabled = true;

        visualEffect.SetPosition(0, startPoint);
        visualEffect.SetPosition(1, endPoint);

        yield return new WaitForSeconds(visualEffectDuration);

        visualEffect.enabled = false;
        crossbowTower.EnableRotation(true);
    }
}
