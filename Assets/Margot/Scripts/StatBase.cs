using System;
using UnityEngine;

namespace Margot
{
    /// <summary>
    /// Base stats class. Shared between PlayerStats and EnemyStats.
    /// Optional stats can be left null if not applicable.
    /// </summary>
    [Serializable]
    public abstract class StatsBase
    {
        [Header("Core")]
        [Min(0)] public float attackDamage; // damage per hit
        [Min(0)] public float moveSpeed;  // movement speed
        [Min(1)] public float maxHealth; // maximum health
        public float attackSpeedSPS; // shots per second (null if not used)
        public int bulletsPerShot;  // bullets per shot (null if not used)

        /// <summary>
        /// Returns attack interval in seconds.
        /// If attackSpeedSPS is null, returns Infinity.
        /// </summary>
        public float AttackInterval =>
            attackSpeedSPS > 0f ? 1f / Mathf.Max(0.01f, attackSpeedSPS) : float.PositiveInfinity;
    }

    /// <summary>
    /// Player-specific extension of StatsBase.
    /// Add fields like critical chance, stamina, etc.
    /// </summary>
    [Serializable]
    public class PlayerStats : StatsBase
    {
        // Example: public float critChance = 0f;
    }

    /// <summary>
    /// Enemy-specific extension of StatsBase.
    /// Add fields like aggression level, AI difficulty, etc.
    /// </summary>
    [Serializable]
    public class EnemyStats : StatsBase
    {
        // Example: public float aggression = 1f;
    }

}
