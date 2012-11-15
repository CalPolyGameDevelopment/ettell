using UnityEngine;
using System.Collections;

public class EnergyBar : MonoBehaviour
{
	public Color backgroundColor = Color.black;
	public Color forgroundColor = Color.red;
	public string labelText = "";
	public Rect box = new Rect (10, 10, 400, 20);
	
	private Texture2D background;
	private Texture2D foreground;
	private Rect bgRect;
	private Rect fgRect;
	private Rect labelRect;
	
	private float fullness;
	public float Fullness {
		get {
			return fullness;
		}
		set {
			fullness = Mathf.Min(Mathf.Max(value, 0f), 1f);
			calcWidth();
		}
	}
	
	void Start ()
	{
		background = new Texture2D(1, 1, TextureFormat.RGB24, false);
		foreground = new Texture2D(1, 1, TextureFormat.RGB24, false);

		background.SetPixel(0, 0, backgroundColor);
		foreground.SetPixel(0, 0, forgroundColor);
        
		background.Apply();
		foreground.Apply();
		
		bgRect = new Rect(0f, 0f, box.width, box.height);
		fgRect = new Rect(0f, 0f, 0f, box.height);
		labelRect = new Rect(10, 0, 50, 20);
		
		calcWidth();
	}
	
	private void calcWidth() {
		fgRect.width = box.width * fullness;
	}

	void OnGUI ()
	{
		GUI.BeginGroup(box);
		{
			GUI.DrawTexture(bgRect, background, ScaleMode.StretchToFill);
			GUI.DrawTexture(fgRect, foreground, ScaleMode.StretchToFill);
			GUI.Label(labelRect, labelText);
		}
		GUI.EndGroup();
	}
}