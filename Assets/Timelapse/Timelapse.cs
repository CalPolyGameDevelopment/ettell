using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
    

public class Timelapse : MonoBehaviour, MiniGameAPI.IMiniGame {
	
	private const string TIME_FORMAT = "timeFormat";
	private const string BLANK_SECONDS = "blankSeconds";
	private const string START_TIME = "startTime";
	private const string END_TIME = "endTime";
	private const string LERP_SECONDS = "lerpSeconds";
	
	public GUIStyle promptStyle;
	private string promptText;
	private Ending ending;
	private DateTime start;
	private DateTime end;
	private float blankSeconds;
	private float lerpSeconds;
	private float t;
	
	private Vector2 calcPromptSize() {
		return promptStyle.CalcSize(new GUIContent(promptText));
	}
	
	private XmlNode data;
    
	public XmlNode Data {
		set {
			data = value;
			
			promptText = data.childNode(TIME_FORMAT).getString();
			ending = Ending.findEndings(data).ToArray()[0];
			start = data.childNode(START_TIME).GetDate();
			end = data.childNode(END_TIME).GetDate();
			blankSeconds = data.childNode(BLANK_SECONDS).GetFloat();
			lerpSeconds = data.childNode(LERP_SECONDS).GetFloat();
			
			// CODE DUPLICATION WARNING: @Dialog.cs
			int maxFontSize = -1;
			int minFontSize = -1;
			int curSize = promptStyle.fontSize;
			Vector2 promptSize = Vector2.zero;
			while (maxFontSize > minFontSize + 1 || maxFontSize == -1) {
				if (minFontSize > 0 && maxFontSize > 0) {
					curSize = (minFontSize + maxFontSize) / 2;
				}
				else if (maxFontSize > 0) {
					curSize /= 2;
				}
				else {
					curSize *= 2;
				}
				
				promptStyle.fontSize = curSize;
				promptSize = calcPromptSize();
				
				if (promptSize.x > Screen.width || promptSize.y > Screen.height) {
					maxFontSize = curSize;
				}
				else {
					minFontSize = curSize;
				}
			}
			
			promptStyle.fontSize = minFontSize;
			promptSize = calcPromptSize();
			promptText = "";
			t = 0f;
		}
	}
	
	void OnGUI() {
		if (data == null) {
			return;
		}
		GUILayout.BeginArea(new Rect(0f, 0f, Screen.width, Screen.height));
		GUILayout.Label(promptText, promptStyle, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
		GUILayout.EndArea();
	}
	
	//http://www.wolframalpha.com/input/?i=%28erf%28%28x+-+0.5%29+*+4%29+%2B+1%29+%2F+2
	private float smoothLerp {
		get {
			//(erf((x - 0.5) * 4) + 1) / 2
			float x = ((float)(t - blankSeconds)) / lerpSeconds;
			return (((float)Erf((x - 0.5f) * 7f)) + 1f) / 2f;
		}
	}
	
	void Update() {
		t += Time.deltaTime;
		if (t > blankSeconds + lerpSeconds) {
			MiniGameController.endMiniGame(ending.edgeId);
		}
		if (t > blankSeconds) {
			promptText = start.Lerp(end, smoothLerp).ToString("MM/dd/yy HH:mm:ss");
		}
	}
	
	//http://www.johndcook.com/csharp_erf.html
    static double Erf(double x)
    {
        // constants
        double a1 = 0.254829592;
        double a2 = -0.284496736;
        double a3 = 1.421413741;
        double a4 = -1.453152027;
        double a5 = 1.061405429;
        double p = 0.3275911;

        // Save the sign of x
        int sign = 1;
        if (x < 0)
            sign = -1;
        x = Math.Abs(x);

        // A&S formula 7.1.26
        double t = 1.0 / (1.0 + p*x);
        double y = 1.0 - (((((a5*t + a4)*t) + a3)*t + a2)*t + a1)*t*Math.Exp(-x*x);

        return sign*y;
    }
}