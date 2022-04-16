using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Soldier : MonoBehaviour, IAttackable
{
    [SerializeField] bool isME;
    [SerializeField] private int speed;
    [SerializeField] private float maxHP;
    [SerializeField] private Transform HPBarPos;
    [HideInInspector] Vector3 Pos;

    private float currentHP;

    private Vector3 aimPos; 

    private Rigidbody myRB;

    private Transform myT;

    private Animator mAnimator;
    public bool isDeath { get; private set; }

    private int attackLayer, damageLayer, groundLayer;

    private void Awake()
    {
        currentHP = maxHP;

        myT = transform;

        myRB = GetComponent<Rigidbody>();
        myRB.centerOfMass = Vector3.zero; //Devrilmemesi için

        Pos = aimPos = myT.position = myRB.position;
        mAnimator = GetComponent<Animator>();

        attackLayer = LayerMask.GetMask(Layers.AttackLayer);
        damageLayer = LayerMask.GetMask(Layers.DamageCollider);
        groundLayer = LayerMask.GetMask(Layers.Ground);

        ragdollColliders = ragdollParent.GetComponentsInChildren<Collider>();
        ragdollRigidbody = ragdollParent.GetComponentsInChildren<Rigidbody>();
        SetRagdoll(false);
    }

    private void Start()
    {
        EventManager.Fire_OnPlayerHPChanged(currentHP, maxHP);
    }

    private void Update()
    {
        if (!isME) return;

        if (Input.GetMouseButtonDown(0))
        {
            GetAimPos();
        }
    }
    Vector3 lookAt;
    private void FixedUpdate()
    {
        myRB.position = Vector3.MoveTowards(myRB.position, aimPos, Time.fixedDeltaTime * speed);
        Pos = myRB.position;

        if (lookAt != Vector3.zero)
            myRB.rotation = Quaternion.Slerp(myRB.rotation, Quaternion.LookRotation(lookAt), Time.fixedDeltaTime * 15);

        if (!isME) return;

        if (Vector3.SqrMagnitude(aimPos - Pos) > 0.04f * 0.04f)
        {
            mAnimator.SetFloat(AnimParameters.velTotal, 1f, 0.8f, Time.fixedDeltaTime * 2);
        }
        else
        {
            mAnimator.SetFloat(AnimParameters.velTotal, 0f, 0.3f, Time.fixedDeltaTime * 2);
        }

        CameraFollow.AimPos = Pos;
        EventManager.Fire_OnPlayerPosChanged(HPBarPos.position);
    }

    private void GetAimPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;


        if (Physics.Raycast(ray, out raycastHit, 100, damageLayer, QueryTriggerInteraction.Collide))
        {

            Attack(raycastHit.point);
            AttackDefiniton attackDef = new AttackDefiniton();
            attackDef.atacker = this;
            //attackDef.target = raycastHit.transform.GetComponentInParent<IDamagable>();
            attackDef.damagePoint = 20;

            raycastHit.transform.GetComponentInParent<IDamagable>()?.GetDamagable(attackDef);

            return;
        }
        if (Physics.Raycast(ray, out raycastHit, 100, attackLayer, QueryTriggerInteraction.Collide))
        {

            Attack(raycastHit.point);
            return;
        }
        if (Physics.Raycast(ray, out raycastHit, 100, groundLayer, QueryTriggerInteraction.Collide))
        {
            aimPos = raycastHit.point;
            lookAt = aimPos - Pos;
            return;
        }

    }
    private void Attack(Vector3 attackPos)
    {
        aimPos = myRB.position;
        lookAt = attackPos - Pos;
        lookAt.y = 0;
        mAnimator.SetTrigger(AnimParameters.attack);
    }

    private void IncreaseHP()
    {
        if (isME)
        {
            maxHP += 50;
            currentHP += 50;
            EventManager.Fire_OnPlayerHPChanged(currentHP, maxHP);
            Debug.Log("HP arttý");
        }
    }
    private void OnEnable()
    {
        EventManager.OnSpecialZoneTrigger += IncreaseHP;
    }
    private void OnDisable()
    {
        EventManager.OnSpecialZoneTrigger -= IncreaseHP;
    }

    public void GetDamagable(AttackDefiniton attackDefiniton)
    {
        Debug.Log(gameObject.name + " GetDamage");

        currentHP -= attackDefiniton.damagePoint;

        if (currentHP <= 0)
        {
            isDeath = true;

            Debug.Log("Death");
        }
    }

    [SerializeField] Transform ragdollParent;

    Collider[] ragdollColliders;

    Rigidbody[] ragdollRigidbody;

    private void SetRagdoll(bool state)
    {
        for (int i = 0; i < ragdollColliders.Length; i++)
            ragdollColliders[i].isTrigger = !state;

        for (int i = 0; i < ragdollRigidbody.Length; i++)
            ragdollRigidbody[i].isKinematic = !state;
    }



}
