using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PatrolConfigSO", menuName = "Configs/PatrolConfig", order = 1)]
public class PatrolConfigSO : ScriptableObject
{
    public float minPatrolTime = 3f;  // Thời gian tuần tra tối thiểu
    public float maxPatrolTime = 6f;  // Thời gian tuần tra tối đa
    public float patrolRadius = 10f;  // Bán kính tuần tra
}
