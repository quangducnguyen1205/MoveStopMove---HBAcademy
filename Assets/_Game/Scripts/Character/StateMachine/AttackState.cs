using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<BotController>
{
    private PatrolConfigSO patrolConfig;
    public AttackState(PatrolConfigSO config){
        patrolConfig = config;
    }
    public void OnEnter(BotController bot){
        // Kiểm tra mục tiêu ngay khi vào trạng thái tấn công
        bot.FindTargets();
        if (bot.targetList.Count > 0){
            Character closeTarget = bot.GetClosestTarget();
            if (closeTarget != null){
                bot.SetTarget(closeTarget);
                bot.ChangeAnim(AnimConstants.attack);
            }
        }
        else{
            bot.ChangeState(new PatrolState(patrolConfig)); 
        }
    }
    public void OnExcute(BotController bot){
        if (!bot.isAttack){
            if (bot.targetList.Count > 0) {
                Character closeTarget = bot.GetClosestTarget();
                if (closeTarget != null){
                    bot.SetTarget(closeTarget);
                    bot.Attack();
                }
            }
            else {
                // Nếu không có mục tiêu, quay lại trạng thái tuần tra
                bot.ChangeState(new PatrolState(patrolConfig));
            }
        }
    }
    public void OnExit(BotController bot){
        // Trạng thái tấn công được đặt lại khi thoát
        bot.isAttack = false;
    }
}
