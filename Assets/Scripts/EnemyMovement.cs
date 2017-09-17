using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour {

	private Transform target;
	private int wavepointIndex = 0;

	private Enemy enemy;

	private float nextRotation;
	private GameObject endRotation; 

	void Start()
	{
		enemy = GetComponent<Enemy>();
		endRotation = new GameObject();
		Waypoint waypoint = Waypoints.waypoints[0];
		target = waypoint.transform;
		nextRotation = waypoint.rotation;
	}

	void Update()
	{
		Vector3 dir = target.position - enemy.transform.position;
		enemy.transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

		if (Vector3.Distance(enemy.transform.position, target.position) <= 0.4f)
		{
			GetNextWaypoint();
		}

		enemy.speed = enemy.startSpeed;
		enemy.partToRotate.rotation = Quaternion.Lerp(enemy.partToRotate.rotation, endRotation.transform.rotation, Time.deltaTime * 10f);
	}

	void GetNextWaypoint()
	{
		if (wavepointIndex >= Waypoints.points.Length - 1)
		{
			EndPath();
			return;
		}

		wavepointIndex++;
		Waypoint waypoint = Waypoints.waypoints[wavepointIndex];
		target = waypoint.transform;
		endRotation.transform.Rotate(Vector3.up, nextRotation, Space.World);
		nextRotation = waypoint.rotation;
	}

	void EndPath()
	{
		PlayerStats.Lives--;
		WaveSpawner.EnemiesAlive--;
		Destroy(gameObject);
	}

}
