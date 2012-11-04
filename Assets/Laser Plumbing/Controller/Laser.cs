using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {
    public Color c = Color.red;
	public float width = 2f;
	
	private LineRenderer lineRenderer;
	
	public GameObject target;
	public Vector3 origin;
	public float length;
	public float speed;
	private bool ready;
	public bool Ready {
		set {
			if (value) {
				pathLength = (target.transform.position - origin).magnitude;
				travelTime = pathLength / speed;
				lengthTime = length / speed;
				frontT = 0f;
				ready = true;
			}
		}
	}
	
	private float frontT;
	private float backT;
	private float pathLength;
	private float travelTime;
	private float lengthTime;
	
    void Start() {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(c, c);
		lineRenderer.SetWidth(width, width);
		lineRenderer.SetVertexCount(2);
    }
	
	private Vector3 lerp(float time) {
		return Vector3.Lerp(origin, target.transform.position, Mathf.Min(Mathf.Max(0f, time), travelTime) / travelTime);
	}
	
    void Update() {
		if (!ready) {
			return;
		}
		frontT += Time.deltaTime;
		backT = frontT - lengthTime;
		lineRenderer.SetPosition(0, lerp(backT));
		lineRenderer.SetPosition(1, lerp(frontT));
		if (frontT >= travelTime) {
			target.SendMessageUpwards("hitByLaser");
		}
		if (backT >= travelTime) {
			Destroy(gameObject);
		}
    }
}