using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.StatControllers
{
    public class StatController
    {
        public float GetAttackRate(Tower tower, List<Tower> towers)
        {
            return Mathf.Max(tower.attackRate + towers.Sum(t => t.GetAttackRateModifier()), 0.5f);
        }

        public float GetCastTime(Tower tower, List<Tower> towers)
        {
            return Mathf.Max(tower.castTime - towers.Sum(t => t.GetCastTimeModifier()), 0.5f);
        }

        public float GetDamage(Tower tower, List<Tower> towers)
        {
            return Mathf.Max(tower.damage + towers.Sum(t => t.GetDamageModifier()), 0.5f);
        }

        public float GetMovementSpeed(Tower tower, List<Tower> towers)
        {
            return Mathf.Max(tower.speed + towers.Sum(t => t.GetMovementSpeedModifier()), 0.5f);
        }

        public int GetMaxHealth(Tower tower, List<Tower> towers)
        {
            return Mathf.Max(tower.maxHealth + towers.Sum(t => t.GetHealthModifier()), 1);
        }

        public float GetAbilityCooldown(Tower tower, List<Tower> towers)
        {
            return Mathf.Max(tower.abilityCooldown + towers.Sum(t => t.GetAbilityCooldownModifier()), 0.5f);
        }

        public float GetTpRate(Tower tower, List<Tower> towers)
        {
            return Mathf.Max(tower.tpRate + towers.Sum(t => t.GetTpRateModifier()), 0.5f);
        }
    }
}
