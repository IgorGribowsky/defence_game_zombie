using UnityEditor.SearchService;
using UnityEngine;

public class HealthPoints : MonoBehaviour
{
    public float CurrentHP = 100;

    public float MaximumHP = 100;

    public void GetDamage(float damage, GameObject source)
    {
        //Debug.Log("(Get damage) Object: " + gameObject + ". Damage value: " + damage);
        CurrentHP -= damage;
        OnDamaged(new DamagedEventArgs { Damage = damage, Source = source });
    }

    public delegate void DamagedHandler(object sender, DamagedEventArgs e);

    public event DamagedHandler Damaged;

    protected void OnDamaged(DamagedEventArgs e)
    {
        Damaged?.Invoke(this, e);
    }

    public class DamagedEventArgs
    {
        public float Damage { get; set; }

        public GameObject Source { get; set; }
    }
}