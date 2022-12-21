using System.Collections;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    enum DamageBehaviour { Estandar, Reducido, Inmortal };
    enum DeadBehaviour { Inmediato, Escalado, Vfx };

    [SerializeField] DamageBehaviour damageBehaviour;
    [SerializeField] DeadBehaviour deadBehaviour;
    [SerializeField] private float health = 100;
    [SerializeField] private GameObject vfxPrefab;
    
    public void Damage(int damage)
    {
        if (health > 0)
        {
            switch (damageBehaviour)
            {
                case DamageBehaviour.Estandar:
                    health -= damage;
                    if (health <= 0) { Dead(); }
                    break;
                case DamageBehaviour.Reducido:
                    float damageReduced = damage * 60 / 100f;
                    health -= damageReduced;
                    if (health <= 0) { Dead(); }
                    break;
                case DamageBehaviour.Inmortal:
                    health -= damage;
                    if (health < 1) { health = 1; }
                    break;
            }
        }
    }

    public float GetCurrentHealth() { return health; }
    

    void Dead()
    {
        switch (deadBehaviour)
        {
            case DeadBehaviour.Inmediato:
                Destroy(gameObject);
                break;
            case DeadBehaviour.Escalado:
                StartCoroutine(ScalingDead());
                break;
            case DeadBehaviour.Vfx:
                StartCoroutine(VfXDead());
                break;
        }
    }
    private IEnumerator ScalingDead()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        while (gameObject)
        {
            yield return wait;
            gameObject.transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);

            if (gameObject.transform.localScale.x <= 0.05f)
            {
                Destroy(gameObject);
            }
        }
    }
    private IEnumerator VfXDead()
    {
        WaitForSeconds wait = new WaitForSeconds(1.5f);
        GameObject vfxObject = Instantiate(vfxPrefab);
        Destroy(vfxObject, 5);
        vfxObject.transform.position = transform.position;
        yield return wait;
        Destroy(gameObject);
        
    }
    
}