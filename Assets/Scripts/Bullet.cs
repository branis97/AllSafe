using UnityEngine;

public class Bullet : MonoBehaviour {

	private Enemy enemy;

	public float speed = 70f;

	public int damage = 50;

	public float explosionRadius = 0f;
	public GameObject impactEffect;
	
	public void Seek (Enemy _enemy)
	{
		enemy = _enemy;
	}

	// Update is called once per frame
	void Update () {

		if (enemy == null)
		{
			Destroy(gameObject);
			return;
		}

		Vector3 dir = enemy.transform.position - transform.position;
		float distanceThisFrame = speed * Time.deltaTime;

		if (dir.magnitude <= distanceThisFrame)
		{
			Hitenemy();
			return;
		}

		transform.Translate(dir.normalized * distanceThisFrame, Space.World);
		transform.LookAt(enemy.transform);

	}

	void Hitenemy ()
	{
		GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
		Destroy(effectIns, 5f);

		if (explosionRadius > 0f)
		{
			Explode();
		} else
		{
			Damage(enemy);
		}

		Destroy(gameObject);
	}

	void Explode ()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
		foreach (Collider collider in colliders)
		{
			if (collider.tag == "Enemy")
			{
				Damage(collider.GetComponent<Enemy>());
			}
		}
	}

	void Damage (Enemy enemy)
	{
		enemy.TakeDamage(damage);
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}
}
