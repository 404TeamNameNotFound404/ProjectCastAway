using UnityEngine;

[CreateAssetMenu(fileName = "DamageData", menuName = "Custom/DamageData")]
public class DamageData : ScriptableObject
{
    public string SlotName; // used to differentiate dagger from arrow
    
    public int damage;
    public int attackPower;
    public float multiplier;
    
    public float distance; // used just for the arrow damage math
    
    // taken from reddit Damage=(Base Damage+Attack Power×0.8)×Distance Multiplier×Headshot Multiplier
    public float ApplyArrowDamage()
    {
        return (damage + attackPower * multiplier) * distance;
    }
    
    // simplified version of LOL Damage system
    public float ApplyDaggerDamage()
    {
        return (damage + attackPower) * multiplier;
    }
}
