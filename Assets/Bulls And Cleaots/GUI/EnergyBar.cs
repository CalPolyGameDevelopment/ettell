using UnityEngine;
using System.Collections;

namespace BullsAndCleots.Gui{

public class EnergyBar : MonoBehaviour
{
	public Color backgroundColor = Color.black;
	public Color forgroundColor = Color.red;
	public string labelText = "";
	public Rect box = new Rect (10, 10, 200, 20);
	
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
			fgRect.width = calcWidth();
		}
	}
	
	public void SetFullness(int val, int max){
		Fullness = (float)val / max;
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
		
		fgRect.width = calcWidth();
	}
	
	private float calcWidth() {
		return box.width * fullness;
	}

	public void OnGUI()
	{
		Matrix4x4 originalMatrix;
		GuiUtilities.Scale(out originalMatrix);
		
		GUI.BeginGroup(box);
		{
			GUI.DrawTexture(bgRect, background, ScaleMode.StretchToFill);
			GUI.DrawTexture(fgRect, foreground, ScaleMode.StretchToFill);
			GUI.Label(labelRect, labelText);
		}
		GUI.EndGroup();
		
		GUI.matrix = originalMatrix;
	}
	
	
}
}