using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : GameUnit
{
    [SerializeField] private Animator anim;
    [SerializeField] private Transform attackPoint;
    [SerializeField] protected Vector3 scaleCharacter;
    private string currenAnimName;
    protected Character target;
    internal List<Character> targetList = new List<Character>();
    internal bool isAttack = false;
    internal bool isMoving = false;
    public float attackRange;
    public Weapon currentWeapon;
    public bool isPlayer = false;
    
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        OnInit();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
        if (!isAttack) // Chỉ thực hiện tìm kiếm và tấn công nếu không đang tấn công
        {
            FindTargets();
            if (!isMoving)
            {
                if (targetList.Count > 0)
                {
                    Character closetTarget = GetClosestTarget();
                    if (closetTarget != null)
                    {
                        SetTarget(closetTarget);
                        Attack();
                    }
                }
                else
                {
                    ChangeAnim(AnimConstants.idle);
                }
            }
        }
    }

    public virtual void OnInit(){
        if (currentWeapon != null){
            currentWeapon.owner = this;
            // Bỏ qua va chạm giữa vũ khí và chủ sở hữu

            Collider characterCollider = GetComponent<Collider>();
            Physics.IgnoreCollision(characterCollider, currentWeapon.weaponCollider, true);
        }
    }
    public void FindTargets(){
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider hitCollider in hitColliders){
            Character character = hitCollider.GetComponent<Character>();
            if (character != null && character != this && character.gameObject.activeSelf){
                if (!targetList.Contains(character) && IsTargetInRange(character)){
                    targetList.Add(character);
                }
            }
        }
    }
    public void RemoveTarget(Character character)
    {
        if (targetList.Contains(character))
        {
            targetList.Remove(character);
        }
    }
    private bool IsTargetInRange(Character target)
    {
        return Vector3.Distance(target.transform.position, transform.position) <= attackRange;
    }
    public virtual void SetTarget(Character character){
        // Kiểm tra nếu mục tiêu mới
        if (target != character)
        {
            target = character; // Cập nhật mục tiêu mới
        }
    }
    public virtual void Attack(){
        // Nếu mục tiêu không còn trong tầm hoặc không còn mục tiêu, quay lại tìm mục tiêu mới
        if (target == null || !IsTargetInRange(target)){
            RemoveTarget(target);
            target = null;
            FindTargets();
            return;
        }

        ChangeAnim(AnimConstants.attack);
        Invoke(nameof(ResetAttack), 1.1f);
        isAttack = true;

        Vector3 direchtionToTarget = (target.transform.position + new Vector3(0, 0.5f, 0) - attackPoint.position).normalized;
        // Quay nhân vật về phía mục tiêu
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direchtionToTarget.x, 0, direchtionToTarget.z));
        transform.rotation = targetRotation;

        Weapon weapon = WeaponPool.SpawnWeapon(currentWeapon.weaponType, attackPoint.position, Quaternion.LookRotation(direchtionToTarget));
        weapon.SetDirection(direchtionToTarget);
        weapon.owner = this;
        weapon.Shoot();
    }

    private void ResetAttack(){
        ChangeAnim(AnimConstants.idle);
        isAttack = false;

        // Kiểm tra lại mục tiêu sau khi tấn công
        if (target == null || !IsTargetInRange(target)){
            // Nếu mục tiêu không còn trong tầm hoặc không còn mục tiêu, quay lại tìm mục tiêu mới
            RemoveTarget(target);
            target = null;
            FindTargets();
        }
    }
    public Character GetClosestTarget()
    {
        Character closestTarget = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < targetList.Count; i++){
            if (targetList[i] != null && targetList[i].gameObject.activeSelf ){
                float distanceToTarget = Vector3.Distance(transform.position, targetList[i].transform.position);
                if (distanceToTarget < closestDistance) {
                    closestDistance = distanceToTarget;
                    closestTarget = targetList[i];
                }
            }
        }
        return closestTarget;
    }
    protected virtual void Move()
    {

    }
    public void ChangeAnim(string animName){
        if (currenAnimName != animName){
            anim.ResetTrigger(animName);
            currenAnimName = animName;
            anim.SetTrigger(animName);
        }
    }
    public virtual void OnDeath(Character attacker){
        ChangeAnim(AnimConstants.die); 
        if (this.isPlayer){
            this.gameObject.SetActive(false);
            GameManager.Instance.OnLoseGame();
            return;
        }
        if (attacker != null){
            attacker.RemoveTarget(this);
            attacker.transform.localScale += scaleCharacter;
        }
        Invoke(nameof(OnDespawn), 0.5f);
        // Gọi hàm giảm số bot sống trong GameManager
        LevelManager.Instance.DecreaseBotCount();
    }
    public virtual void OnDespawn(){

    }
}
