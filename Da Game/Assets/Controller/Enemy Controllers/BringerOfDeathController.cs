using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BringerOfDeathController : Enemy
{
    private Animator animator;
    Rigidbody2D rb;

    private float MaxHP;

    [SerializeField] private Transform spell;
    private int spellAmountPerCast = 15;
    private int castAmount = 2, currentCast=0;
    private float spellOffsetY = 3f;
    [SerializeField] private Vector2 attackRange;    
    public bool spellCD = true;
    public bool teleportCD = true;
    public bool cloneCD = true;

    public float spellCDTime =5f, teleportCDTime =7f, cloneCDTime = 25f;
    public bool isClone = false;
    [SerializeField] private GameObject clonePrefab;

    public bool[] phase = new bool[2]{true,false};

    bool FacingRight = true;


    public Transform target;
    Path path;
    Seeker seeker;
    int currentWaypoint = 0;
    private Vector2 direction;
    bool reachedEndofPath = false;
    public float nextWaypointDistance = 5f;
    private CapsuleCollider2D collider2D;


    public State state;


    public enum State
    {
        Attack,
        Spell,
        Teleport,
        Run,
        Idle,
        Roam,
        Die
    }

    private void Start() 
    {
        target = PlayerController.instance.transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        if (animator == null)
            animator = GetComponent<Animator>();
        spellOffsetY = spell.transform.localScale.y*2.3f;
        currentCast = castAmount;
        spellCDTime = castAmount * spellAmountPerCast*0.75f + 3.5f; // arbitrarily + 3.5 for the cooldown
        attackRange = new Vector2(2f,1f);
        attackRange.x *= GetComponent<SpriteRenderer>().bounds.size.x;
        attackRange.y *= GetComponent<SpriteRenderer>().bounds.size.y/2.25f;
        collider2D = GetComponent<CapsuleCollider2D>();
        MaxHP = Hp;

        if (!isClone)
        {
            // CloneSkill();
            // StartCoroutine(CoolDown("SpellCD",spellCDTime));
            // StartCoroutine(CoolDown("TeleportCD",20f));
            StartCoroutine(CoolDown("SpellCD",spellCDTime));
            animator.SetFloat("AttackSpeed",AttackSpeed);
            clonePrefab = Resources.Load("Prefabs/Bringer Of Death") as GameObject;
            AudioManager.instance.PlaySoundTrack(AudioManager.SoundTrack.BossST2);
        }
       

        state = State.Run;
        InvokeRepeating("UpdatePath", 0f, .5f);

        
    }
    void UpdatePath() // being call in InvokeRepeating above
    {
        if (seeker.IsDone())
            if (state == State.Run)
            {
                seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
            }
            else seeker.StartPath(rb.position, rb.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate() 
    {
        CheckLife();
        UpdateAction();
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndofPath = true;
            return;
        }
        else
        {
            reachedEndofPath = false;
        }

        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 Velocity = new Vector2(direction.x * MovementSpeed, direction.y * MovementSpeed);
        if (!animator.GetBool("Run"))
            Velocity = Vector2.zero;
        if ((direction.x >= 0.01f && FacingRight) || (direction.x <= -0.01f && !FacingRight))
            flip();

        switch (state)
        {
            case State.Run:
                animator.SetBool("Run",true);
                animator.SetBool("Idle",false);
                animator.SetBool("Attack",false);
                animator.SetBool("Spell",false);
                break;
            case State.Attack:
                animator.SetBool("Attack",true);
                animator.SetBool("Spell",false);
                animator.SetBool("Run",false);
                animator.SetBool("Idle",false);
                break;
            case State.Spell:
                animator.SetBool("Spell",true);
                animator.SetBool("Attack",false);
                animator.SetBool("Run",false);
                animator.SetBool("Idle",false);
                break;
            case State.Teleport:
                animator.SetBool("Teleport",true);
                animator.SetBool("Run",false);
                animator.SetBool("Idle",false);
                animator.SetBool("Attack",false);
                animator.SetBool("Spell",false);
                break;
            case State.Die:
                animator.SetBool("Dying",true);
                break;
            default:
                Debug.Log("State = null in Bringer Of Death Controller Script!");
                break;
        }


        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        rb.velocity = Velocity;
    }
    
    void flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void UpdateAction()
    {
        if (state != State.Die)
        {
            if (Mathf.Abs(target.position.x - transform.position.x) <= attackRange.x 
                && Mathf.Abs(target.position.y - transform.position.y) <= attackRange.y
                && teleportCD 
                && spellCD)
            {
                state = State.Attack;
            } 
            if (!spellCD && teleportCD)
                state = State.Spell;
            if (!teleportCD)
                state = State.Teleport;
            if (!cloneCD && !isClone && teleportCD)
                CloneSkill();
        }
      
    }
    public void CheckAttackRange()
    {
        // && !Utilities.IsAnimationPlaying(animator,"attack")
        if ((target.position.x - transform.position.x > 0 && FacingRight == true) || (target.position.x - transform.position.x < 0 && FacingRight == false))
        {
            flip();
        }
        if (Mathf.Abs(target.position.x - transform.position.x) > attackRange.x 
            || Mathf.Abs(target.position.y - transform.position.y) > attackRange.y)
        {
            state = State.Run;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("BoD hit player with attack");
            // if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            // {
            //     Debug.Log("Hit player layer");
            // }
            if (animator.GetBool("Attack"))
            {
                Vector2 knockBack = (PlayerController.instance.transform.position - transform.position).normalized;
                if (!isClone)
                {
                    knockBack *=0.5f;
                    PlayerController.instance.takeDamage(Damage*1.5f,DamageType.Physical,knockBack);
                }
                else 
                {
                    knockBack*=0.25f;
                    PlayerController.instance.takeDamage(Damage,DamageType.Physical,knockBack);
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player"))
            Debug.Log("Player is still in trigger");    
    }
    private void OnCollisionEnter2D(Collision2D other) {

        if (other.gameObject.layer ==LayerMask.NameToLayer("Monster"))
        {
            // Debug.Log("monster touch monster");
            Physics2D.IgnoreCollision(other.collider, collider2D);
        }
        if (other.collider.CompareTag("Player"))
        {
            Vector2 knockBack = 0.3f*(PlayerController.instance.transform.position - transform.position).normalized;
            PlayerController.instance.takeDamage(Damage/10f,DamageType.Physical,knockBack);
        }
        
    }

    private IEnumerator CastSpell()
    {
        // Debug.Log("Bringer of Death spell casted");
        currentCast --;
        if (currentCast == 0)
        {
            animator.SetBool("Spell",false);
            animator.SetBool("Idle",true);
            state = State.Run;
            currentCast = castAmount;
            spellCD = true;
            StartCoroutine(CoolDown("SpellCD",spellCDTime));
        }
        for (int i = 0; i < spellAmountPerCast; i++)
        {
            Vector3 spawnPos = PlayerController.instance.transform.position;
            spawnPos.y+=spellOffsetY;
            spawnPos.x+= Random.Range(-0.075f,0.075f);
            Transform Spell = Instantiate(spell,spawnPos,Quaternion.identity);
            Spell.GetComponent<BODSpellController>().SetUp(0.25f*Damage,DamageType.Lightning); // change this later
            yield return new WaitForSeconds(0.75f);
        }
    }

    public IEnumerator CoolDown(string CDFunctionName, float CDTime)
    {
        yield return new WaitForSeconds(CDTime);
        Invoke(CDFunctionName,0f);
    }
    private void SpellCD()
    {
        spellCD = false;
    }
    private void TeleportCD()
    {
        teleportCD = false;
    }
    private void CloneCD()
    {
        cloneCD = false;
    }
    private IEnumerator Teleport()
    {
        yield return new WaitForSeconds(3f);
        animator.SetBool("Teleport",false);
        animator.SetBool("Run",true);
        Vector3 teleportPos = PlayerController.instance.transform.position;
        teleportPos.x += Random.Range(-3f,3f);
        transform.position = teleportPos;
        teleportCD = true;
        StartCoroutine(CoolDown("TeleportCD",teleportCDTime));
    }
    private void CloneSkill()
    {
        float cloneSpawnDistance = Random.Range(5,8);
        float [] pos_x =  new float[4] {-cloneSpawnDistance,cloneSpawnDistance,0,0}; 
        float [] pos_y = new float[4] {0,0,cloneSpawnDistance,-cloneSpawnDistance};
        for (int i = 0; i < 4; i++)
        {
            Vector3 clonePos = target.position;
            // clonePos.x += 5f*(Random.Range(0,2) * 2 - 1f);
            // clonePos.y += 5f*(Random.Range(0,2) * 2 - 1f);
            clonePos.x += pos_x[i];
            clonePos.y += pos_y[i];
            GameObject clone = Instantiate(clonePrefab,clonePos,Quaternion.identity);
            clone.GetComponent<BringerOfDeathController>().SetUpClone();
            clone.name = "BoD Clone";

        }
        cloneCD = true;
        StartCoroutine(CoolDown("CloneCD",cloneCDTime));
    }
    private IEnumerator CloneDieAfter(float DieAfter)
    {
        yield return new WaitForSeconds(DieAfter);
        state = State.Die;
    }
    public void CheckLife()
    {
        if (Hp<=0.6f*MaxHP && !phase[1] && !isClone)
        {
            StartCoroutine(AudioManager.instance.FadeOutST(fadeDuration:0.75f,targetVolumne:0.05f,NextST:AudioManager.SoundTrack.BossST2_Phase2));
            phase[1]=true; // phase 2
            AttackSpeed *=1.25f;
            Damage *=1.5f;
            animator.SetFloat("AttackSpeed",AttackSpeed);
            CloneSkill();
            teleportCD = false;
        }

        if (Hp<=0 && state != State.Die)
        {
            state = State.Die;
           
            
            if (!isClone)
            {
                BringerOfDeathController [] bossClone = GameObject.FindObjectsOfType<BringerOfDeathController>();
                foreach(var clone in bossClone)
                    clone.state = State.Die;
                Drop();
            }
        }
    }
    public void SetUpClone(float hp = 100, float damage = 1, DamageType damagetype = DamageType.Physical) // all these stat are abitrary
    {
        isClone = true;
        spellCD = true;
        teleportCD = true;
        cloneCD = true;
        Hp = hp;
        Damage = damage;
        DamageType_ = damagetype;
        GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0.7843f);
        GetComponent<Animator>().SetFloat("AppearSpeed",0.45f);
        GetComponent<Animator>().SetFloat("AttackSpeed",0.75f);
        collider2D = GetComponent<CapsuleCollider2D>();
        collider2D.enabled = true; // this to try to fix some weird bug
        StartCoroutine(CloneDieAfter(cloneCDTime-10f));

    }
}
