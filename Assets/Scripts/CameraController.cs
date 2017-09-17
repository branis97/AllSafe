using UnityEngine;

public class CameraController : MonoBehaviour {
	public float panSpeed = 30f;
	public float panBorderThickness = 10f;

	public float scrollSpeed = 5f;
	public float minY = 10f;
	public float maxY = 80f;

	private float perspectiveZoomSpeed;
	private float touchSpeed;
	private float currentZoomLevel;

	void Start () {
		#if UNITY_IOS
			perspectiveZoomSpeed = 0.1f;
			touchSpeed = 0.03f;
			currentZoomLevel = 1.4f;
 		#endif
 		#if UNITY_ANDROID
   			perspectiveZoomSpeed = 0.5f;
			touchSpeed = 0.1f;
			currentZoomLevel = 1.4f;
 		#endif
	}

	void Update () {
		if (GameManager.GameIsOver)
		{
			this.enabled = false;
			return;
		}

		#if UNITY_IOS || UNITY_ANDROID
			// Mobile
			// If there are two touches on the device...
			if (Input.touchCount == 2)
			{
				// Store both touches.
				Touch touchZero = Input.GetTouch(0);
				Touch touchOne = Input.GetTouch(1);

				// Find the position in the previous frame of each touch.
				Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
				Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

				// Find the magnitude of the vector (the distance) between the touches in each frame.
				float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
				float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

				// Find the difference in the distances between each frame.
				float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

				// Change the field of view based on the change in distance between the touches.
				Camera.main.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

				// Clamp the field of view to make sure it's between 15 and 60.
				Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 15f, 60f);
				currentZoomLevel = 0.4f + (Camera.main.fieldOfView - 15f) / (45f);
			} 
			else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
				transform.Translate(-touchDeltaPosition.x * touchSpeed * currentZoomLevel, -touchDeltaPosition.y * touchSpeed * currentZoomLevel, 0f);
				
				// Check MAX up-down
				if (transform.position.z > -9.4f) {
					Vector3 temp = transform.position;
 					temp.z = -9.4f;
 					transform.position = temp;
				} else if (transform.position.z < -97f) {
					Vector3 temp = transform.position;
 					temp.z = -97f;
 					transform.position = temp;
				}

				// Check MAX left-right
				if (transform.position.x > 79) {
					Vector3 temp = transform.position;
 					temp.x = 79;
 					transform.position = temp;
				} else if (transform.position.x < 0) {
					Vector3 temp = transform.position;
 					temp.x = 0;
 					transform.position = temp;
				}
			}
		#endif

		#if UNITY_STANDALONE || UNITY_EDITOR
			// Desktop
			if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
			{
				transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
				if (transform.position.z > -9.4f) {
					Vector3 temp = transform.position;
 					temp.z = -9.4f;
 					transform.position = temp;
				}
			}
			if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
			{
				transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
				if (transform.position.z < -97f) {
					Vector3 temp = transform.position;
 					temp.z = -97f;
 					transform.position = temp;
				}
			}
			if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
			{
				transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
				if (transform.position.x > 79) {
					Vector3 temp = transform.position;
 					temp.x = 79;
 					transform.position = temp;
				}
			}
			if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
			{
				transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
				if (transform.position.x < 0) {
					Vector3 temp = transform.position;
 					temp.x = 0;
 					transform.position = temp;
				}
			}

			float scroll = Input.GetAxis("Mouse ScrollWheel");

			Vector3 pos = transform.position;

			pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
			pos.y = Mathf.Clamp(pos.y, minY, maxY);

			transform.position = pos;
 		#endif
	}
}
