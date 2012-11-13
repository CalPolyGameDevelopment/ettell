using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class LightningArcs : MonoBehaviour {
	
	private class Arc {
		
		private const float wiggleSpeed = 10f;
		
		private class ArcPoint {
			//requires tuning, roughly half the height of a capsule collider
			private const float farRadius = 5f;
			private const float farRadiusSquared = farRadius * farRadius;
			
			public Vector3 curPos;
			
			private Vector3 genPos; //format is rhow, phi, radius
			private Vector3 genVel;
			
			private Vector3 farPoint;
			
			public ArcPoint(Vector3 genPos, Vector3 genVel) {
				this.genPos = genPos;
				this.genVel = genVel;
				curPos = new Vector3();
				farPoint = new Vector3();
			}
			
			public void Update(Collider innerShell) {
				genPos += genVel * Time.deltaTime;
				farPoint.x = Mathf.Cos(genPos.y) * farRadius * Mathf.Cos(genPos.x);
				farPoint.y = Mathf.Sin(genPos.y) * farRadius * Mathf.Cos(genPos.x);
				farPoint.z = Mathf.Sin(genPos.x) * farRadius;
				curPos = innerShell.ClosestPointOnBounds(farPoint) * genPos.z;
			}
			
		}
		private ArcPoint[] points;
		private ParticleSystem.Particle[] particles;
		
		private int start;
		private int end;
		private float len;
		
		public Arc(ParticleSystem.Particle[] particles, int start, int count) {
			this.start = start;
			this.end = start + count;
			len = (float)count;
			this.particles = particles;
			points = new ArcPoint[2];
			points[0] = new ArcPoint(new Vector3(0f, 0f, 1f), Vector3.zero);
			points[1] = new ArcPoint(new Vector3(0f, 0f, 2f), new Vector3(0f, 0f, 0f));
		}
		
		public void Update(Collider innerShell) {
			foreach (ArcPoint p in points) {
				p.Update(innerShell);
			}
			for (int i = start; i < end; i++) {
				Vector3 position = Vector3.Lerp(points[0].curPos, points[1].curPos, ((float)i - start) / len);
				position.x +=  noise.Noise(Time.time * wiggleSpeed + position.x, position.y, position.z);
				position.y +=  noise.Noise(position.x, Time.time * wiggleSpeed + position.y, position.z);
				position.z +=  noise.Noise(position.x, position.y, Time.time * wiggleSpeed + position.z);
				particles[i].position = position;
			}
		}
	}
	
	public int particlesPerArc;
	public int maxConcurrentArcs;
	public float outerShellScale;
	
	private int pCount {
		get {
			return particlesPerArc * maxConcurrentArcs;
		}
	}
	
	private Arc[] arcs;
	ParticleSystem.Particle[] particles;
	
	protected static Perlin noise;
	
	//TODO
	private IEnumerable<Arc> activeArcs {
		get {
			return arcs;
		}
	}
	
	void Start () {
		if (noise == null) {
			noise = new Perlin();
		}
		
		particles = new ParticleSystem.Particle[pCount];
		particleSystem.Emit(pCount);
		if (particleSystem.GetParticles(particles) < pCount) {
			Debug.Log("Hey!");
		}
		arcs = new Arc[maxConcurrentArcs];
		for (int i = 0; i < maxConcurrentArcs; i++) {
			arcs[i] = new Arc(particles, particlesPerArc * i, particlesPerArc);
		}
	}
	
	void Update () {
		foreach (Arc a in activeArcs) {
			a.Update(collider);
		}
		particleSystem.SetParticles(particles, pCount);
	}
}