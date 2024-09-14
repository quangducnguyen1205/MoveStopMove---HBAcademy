using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IState<BotController>
{
    private float patrolTime;
    private float patrolTimer;
    private PatrolConfigSO patrolConfig;
    public PatrolState(PatrolConfigSO config){
        patrolConfig = config;
    } 
    public void OnEnter(BotController bot){
        // Random thời gian tuần tra
        patrolTime = Random.Range(patrolConfig.minPatrolTime, patrolConfig.maxPatrolTime);
        patrolTimer = 0f;

        // Random vị trí tuần tra
        Vector3 randomDestination = Random.insideUnitSphere * patrolConfig.patrolRadius;
        randomDestination += bot.transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDestination, out hit, 10f, 1);
        Vector3 finishPosition = hit.position;

        bot.SetDestination(finishPosition);
        bot.isMoving = true;
        bot.ChangeAnim(AnimConstants.run);
    }
    public void OnExcute(BotController bot){
        patrolTimer += Time.deltaTime;

        // Nếu đến vị trí đích hoặc hết thời gian tuần tra, bot dừng lại
        if (bot.IsDestination || patrolTimer > patrolTime){
            bot.isMoving = false;
            bot.ChangeState(new AttackState(patrolConfig)); // Chuyển sang trạng thái tấn công
        }
    }
    public void OnExit(BotController bot){
        // Dừng animation khi thoát khỏi trạng thái tuần tra
        bot.isMoving = false;
        bot.ChangeAnim(AnimConstants.idle);
    }
}
