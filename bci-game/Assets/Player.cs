using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{   
    private GameObject camera;

    public float attackRange = 1f; 
    public int attackDamage = 10; 
    public float attackCooldown = 1f; 
    public Transform frontCheck; 
    private bool canAttack = true;

    private float directionFacing = 1;
    public int weaponChosen = 1;

    public override float[] GetInput() {
        //KEYBOARD INPUT
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        if (hInput != 0) {
            animator.SetFloat("X", hInput);

            //determine the direction the player is facing
            if (hInput < 0)
                directionFacing = -1;
            else if (hInput > 0)
                directionFacing = 1;
        }

        //switching weapons
        if (Input.GetKeyDown(KeyCode.Alpha1))
            weaponChosen = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            weaponChosen = 1;

        if (Input.mouseScrollDelta.y > 0)
            weaponChosen++;
        else if (Input.mouseScrollDelta.y < 0)
            weaponChosen--;

        if (weaponChosen > 1)
            weaponChosen = 0;
        else if (weaponChosen < 0)
            weaponChosen = 1;

        //MOUSE INPUT
        if (Input.GetMouseButton(0)){

            if (canAttack){
                if (weaponChosen == 0)
                {
                    MeleeAttack();
                }
                else if (weaponChosen == 1)
                {
                    longRangeAttack();
                }

            }
        }

        return new float[] {hInput, vInput};
    }

    protected override IEnumerator MoveCharacter(Vector3 movement){
        if (camera == null) camera = GameObject.FindWithTag("MainCamera");
        transform.position += movement;
        camera.transform.position += movement;
        yield return null;
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        yield return null;
    }

    void MeleeAttack()
    {
        if (canAttack)
        {

            StartCoroutine(AttackCooldown()); // prevent further attacks during cooldown
            //flip hitbox if it is facing the wrong direction
            float hitboxLoc = frontCheck.position.x - transform.position.x;
            if ((animator.GetFloat("X") > 0.5) ^ (hitboxLoc > 0)) {//XOR
                frontCheck.position -= new Vector3(hitboxLoc * 2, 0, 0);
            }

            animator.SetTrigger("isMelee");
            Collider[] hitColliders = Physics.OverlapSphere(frontCheck.position, attackRange);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    Enemy enemy = hitCollider.GetComponent<Enemy>();
                    Debug.Log("ENEMY HIT");
                    break; // Exit the loop as soon as a player is found
                }
            }
        }
    }

    void longRangeAttack()
    {
        if (canAttack)
        {
            StartCoroutine(AttackCooldown()); // prevent further attacks during cooldown

            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.right * directionFacing, out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.CompareTag("Enemy"))
                {
                    Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();
                    Debug.Log("ENEMY HIT");
                }
            }

        }

    }


}
