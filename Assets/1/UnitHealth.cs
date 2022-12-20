using UnityEngine;

public abstract class UnitHealth : MonoBehaviour
{
    public abstract void Damage(int damage);
    public abstract int GetCurrentHealth();
}