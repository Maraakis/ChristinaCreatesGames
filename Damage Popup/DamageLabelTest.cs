using UnityEngine;

namespace Christina.GameSystems
{
    public class DamageLabelTest : MonoBehaviour
    {
        [SerializeField] private int critChance = 10;
        [SerializeField] private int critMultiplier = 2;

        [SerializeField] private int minDamage = 10;
        [SerializeField] private int maxDamage = 30;

        [SerializeField] private Vector3 exampleLocationOfTarget = Vector3.zero;
        

        public void DoDamage()
        {
            var damage = Random.Range(minDamage, maxDamage);
            var isCrit = Random.Range(0, 100) < critChance;
            if (isCrit)
                damage *= critMultiplier;
            
            SpawnsDamagePopups.Instance.DamageDone(damage, exampleLocationOfTarget, isCrit);
        }
        
    }
}