using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.PyroParticles;

public class Firewall : MonoBehaviour {
	private Transform target;
    private List<Enemy> enemies;
    private float detectionRange = 12f;

	[Header("Use Fire")]
	public int damageOverTime = 30;
	public float slowAmount = .5f;
    public float range = 3f;

	[Header("Unity Setup Fields")]
	public string enemyTag = "Enemy";
	public Transform partToRotate;
    public float turnSpeed = 5f;
    public Transform colisionPoint;
    public FireBaseScript flame;

    private float shortestDistance;
    private bool isFireOn = false;

	// Use this for initialization
	void Start () {
		InvokeRepeating("UpdateTargets", 0f, 0.5f);
	}
	
	void UpdateTargets ()
	{
        enemies = new List<Enemy>();
        Collider[] colliders = Physics.OverlapSphere(partToRotate.position, detectionRange);
        shortestDistance = Mathf.Infinity;
		Enemy nearestEnemy = null;
		foreach (Collider collider in colliders)
		{
			if (collider.tag == "Enemy")
			{
                if (!isFireOn) {
                    isFireOn = true;
                    flame.StartFire();
                }

                enemies.Add(collider.GetComponent<Enemy>());

                // Get nearest enemy to lock on
                float distanceToEnemy = Vector3.Distance(colisionPoint.position, collider.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    Enemy realEnemy = collider.GetComponent<Enemy>();
                    nearestEnemy = realEnemy;
                }
			}
		}
        // Get nearest enemy
        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        } else
        {
            target = null;
        }

        if (isFireOn && nearestEnemy == null) {
            isFireOn = false;
            flame.StopFire(); 
        }
	}

    void LockOnTarget ()
	{
		Vector3 dir = target.position - partToRotate.position;
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
		partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
	}

	// Update is called once per frame
	void Update () {
		if (target == null)
		{
			return;
		}
        LockOnTarget();
		Fire();
	}

	void Fire ()
	{
        if (enemies.Count > 0 && shortestDistance <= range)
        {
            foreach (var enemy in enemies) {
                float distanceToEnemy = Vector3.Distance(colisionPoint.position, enemy.transform.position);
                if (distanceToEnemy <= range) {
                    enemy.TakeDamage(damageOverTime * Time.deltaTime);
                    enemy.Slow(slowAmount);
                }
            }
        }
	}
}
