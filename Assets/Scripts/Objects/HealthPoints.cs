using UnityEngine;

public class HealthPoints : MonoBehaviour
{
    public float CurrentHP = 100;
    public float MaximumHP = 100;

    public void GetDamage(float damage)
    {
        Debug.Log("(Get damage) Object: " + gameObject + ". Damage value: " + damage);
        CurrentHP -= damage;
    }
}
