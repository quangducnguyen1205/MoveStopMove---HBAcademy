using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : Character
{
    [SerializeField] private GameObject circleTarget;
    private Vector3 destination;
    public NavMeshAgent agent;
    public bool IsDestination => Vector3.Distance(destination, transform.position) < 0.1f;
    public PatrolConfigSO patrolConfig; // Tham chiếu đến PatrolConfig từ Unity Editor
    IState<BotController> currentState;
    protected override void Start() {
        base.Start();
        if (isAttack){
            return;
        }
        ChangeState(new PatrolState(patrolConfig)); // Bắt đầu với trạng thái tuần tra
    }
    protected override void Update() {
        base.Update(); 
        if (currentState != null){
            currentState.OnExcute(this);
        }
    }
    public void SetActiveCircleTarget(bool isActive){
        if (circleTarget != null){
            circleTarget.gameObject.SetActive(isActive);
        }
    }
    public void SetDestination(Vector3 position){
        destination = position;
        agent.SetDestination(position);
    }
    public void ChangeState(IState<BotController> newState){
        if (currentState != null){
            currentState.OnExit(this);
        }
        currentState = newState;
        if (currentState != null){
            currentState.OnEnter(this);
        }
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        SimplePool.Despawn(this);
    }
    public override void OnDeath(Character attacker)
    {
        base.OnDeath(attacker);

        if (attacker != null && attacker.isPlayer) {
            // Tăng số kim cương khi player tiêu diệt bot
            PlayerController player = attacker.GetComponent<PlayerController>();
            player.circleAttack.transform.localScale -= scaleCharacter;
            LevelManager.Instance.IncreaseDiamondsInPlay();
            AudioManager.Instance.PlayKillBotSound();
        }
    }
}
