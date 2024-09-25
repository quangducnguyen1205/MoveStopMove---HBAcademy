using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask layerGround;
    public DynamicJoystick joystick;
    public Transform instantiateWeapon;
    public Transform circleAttack;
    public override void OnInit()
    {
        if (this.currentWeapon == null){
            ChangeWeapon(WeaponType.Stone);
        }
        // Gán vũ khí mặc định khi bắt đầu game
        base.OnInit();
        isPlayer = true;
    }
    protected override void Move(){
        if (Input.GetMouseButton(0))
        {
            // Kiểm tra giá trị joystick trước khi tạo direction
        
            Vector3 direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

            // Thay vì kiểm tra magnitude, kiểm tra sqrMagnitude để tránh tính toán căn bậc hai
            if (direction.sqrMagnitude >= 0.01f) // 0.1f * 0.1f = 0.01f
            {
                isMoving = true;

                // Không cần normalized trừ khi cần độ dài bằng 1, dùng sqrMagnitude đã xác định vector đủ lớn để di chuyển,
                // nên chỉ cần sử dụng .Normalize để xác định hướng 
                direction.Normalize();
                // Calculate the target position
                Vector3 nextPoint = transform.position + direction * moveSpeed * Time.deltaTime;
                
                // Rotate the player to face the movement direction
                transform.rotation = Quaternion.LookRotation(direction);
                
                ChangeAnim(AnimConstants.run);
                // Move the player to the new position
                transform.position = CheckGround(nextPoint);
            }
        }
        else {
            isMoving = false;
        }
    }
    public override void SetTarget(Character character)
    {
        // If the old target is a bot, disable the old bot's circleTarget
        if (target != character){
            BotController oldBotTarget = target as BotController;
            if (oldBotTarget != null){
                oldBotTarget.SetActiveCircleTarget(false);
            }
        }
        // Update new target
        target = character;
        // If the new target is a bot, enable the new bot's circleTarget
        BotController newBotTarget = target as BotController;
        if (newBotTarget != null){
            newBotTarget.SetActiveCircleTarget(true);
        }
    }

    public override void Attack(){
        base.Attack();
        if (target != null){
            BotController botTarget = target as BotController;
            if (botTarget != null){
                botTarget.SetActiveCircleTarget(true);
            }
        }
        AudioManager.Instance.PlayAttackSound();
    }
    public void ChangeWeapon(WeaponType weaponType){
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }
        // Spawn vũ khí mới và gán vào currentWeapon
        currentWeapon = WeaponManager.Instance.SpawnWeapon(weaponType);
    }
    // Check if the nextPoint is movable to go to that point
    private Vector3 CheckGround(Vector3 nextPoint)
    {
        RaycastHit hit;
        if (Physics.Raycast(nextPoint, Vector3.down, out hit, 5f, layerGround))
        {
            return hit.point + Vector3.up * 0.6f;
        }
        return transform.position;
    }
}
