using System.Collections;
using UnityEngine;

public class Script1 : MonoBehaviour
{
    // ** This class should not be modified, unless you're proposing your own system design. **
    
    private UnitHealth[] targets;

    private void Start()
    {
        targets = FindObjectsOfType<UnitHealth>();
        if (targets.Length == 0)
            Debug.LogWarning($"No units were found in the scene.");
        
        StartCoroutine(DamageRoutine());
    }

    private IEnumerator DamageRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(1);
        while (true)
        {
            yield return wait;
            foreach (UnitHealth unitHealth in targets)
            {
                if (unitHealth != null)
                {
                    unitHealth.Damage(10);
                    Debug.Log($"Damaged {unitHealth}. Remaining: {unitHealth.GetCurrentHealth()}");
                }
            }
        }
    }
}