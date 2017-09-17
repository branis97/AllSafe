using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	private Transform target;
	private Enemy targetEnemy;

	[Header("General")]

	public float range = 15f;

	[Header("Use Bullets (default)")]
	public GameObject bulletPrefab;
	public float fireRate = 1f;
	private float fireCountdown = 0f;

	[Header("Use Second Gun")]
	public bool useSecondGun = false;
	public Transform secondFirePoint;

	[Header("Use Laser")]
	public bool useLaser = false;

	public int damageOverTime = 30;
	public float slowAmount = .5f;

	public LineRenderer lineRenderer;
	public ParticleSystem impactEffect;
	public Light impactLight;

	[Header("Unity Setup Fields")]

	public string enemyTag = "Enemy";

	public Transform partToRotate;
	public float turnSpeed = 10f;

	public Transform firePoint;

	// Use this for initialization
	void Start () {
		InvokeRepeating("UpdateTarget", 0f, 0.5f);
	}
	
	void UpdateTarget ()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		float shortestDistance = Mathf.Infinity;
		Enemy nearestEnemy = null;
		foreach (GameObject enemy in enemies)
		{
			Enemy realEnemy = enemy.GetComponent<Enemy>();
			float distanceToEnemy = Vector3.Distance(transform.position, realEnemy.transform.position);
			if (distanceToEnemy < shortestDistance)
			{
				shortestDistance = distanceToEnemy;
				nearestEnemy = realEnemy;
			}
		}

		if (nearestEnemy != null && shortestDistance <= range)
		{
			target = nearestEnemy.transform;
			targetEnemy = nearestEnemy;
		} else
		{
			target = null;
		}

	}

	// Update is called once per frame
	void Update () {
		if (target == null)
		{
			if (useLaser)
			{
				if (lineRenderer.enabled)
				{
					lineRenderer.enabled = false;
					impactEffect.Stop();
					impactLight.enabled = false;
				}
			}

			return;
		}

		LockOnTarget();

		if (useLaser)
		{
			Laser();
		} else
		{
			if (fireCountdown <= 0f)
			{
				Shoot();
				fireCountdown = 1f / fireRate;
			}

			fireCountdown -= Time.deltaTime;
		}

	}

	void LockOnTarget ()
	{
		Vector3 dir = target.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
		partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
	}

	void Laser ()
	{
		targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
		targetEnemy.Slow(slowAmount);

		if (!lineRenderer.enabled)
		{
			lineRenderer.enabled = true;
			impactEffect.Play();
			impactLight.enabled = true;
		}

		lineRenderer.SetPosition(0, firePoint.position);
		lineRenderer.SetPosition(1, target.position);

		Vector3 dir = firePoint.position - target.position;

		impactEffect.transform.position = target.position + dir.normalized;

		impactEffect.transform.rotation = Quaternion.LookRotation(dir);
	}

	void Shoot ()
	{
		GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		Bullet bullet = bulletGO.GetComponent<Bullet>();

		if (bullet != null)
			bullet.Seek(targetEnemy);

		if (useSecondGun == true) {
			GameObject secondBulletGO = (GameObject)Instantiate(bulletPrefab, secondFirePoint.position, secondFirePoint.rotation);
			Bullet secondBullet = secondBulletGO.GetComponent<Bullet>();

			if (secondBullet != null)
				secondBullet.Seek(targetEnemy);
			}
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}
