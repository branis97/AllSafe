using UnityEngine;

public class Waypoints : MonoBehaviour {

	public static Transform[] points;
	public static Waypoint[] waypoints;

	void Awake ()
	{
		waypoints = new Waypoint[transform.childCount];
		points = new Transform[transform.childCount];
		for (int i = 0; i < points.Length; i++)
		{
			points[i] = transform.GetChild(i);
			waypoints[i] = transform.GetChild(i).GetComponent<Waypoint>();
		}
	}

}
