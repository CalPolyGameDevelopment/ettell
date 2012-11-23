using UnityEngine;
using System.Collections;

public static class GuiUtilities{
	
	private static Rect ReferenceResolution = new Rect(0f, 0f, 1360f, 768f); 
	
	/// <summary>
	/// Scale the Gui.
	/// </summary>
	/// <param name='originalMatrix'>
	/// The gui matrix before scaling.
	/// </param>
	public static Matrix4x4 Scale(out Matrix4x4 originalMatrix){

		originalMatrix = GUI.matrix;
		
		// Create the new matrix that is scaled from the reference resolution.
		// So now the GUI elements get bigger and smaller based on screen size.
		Matrix4x4 scaledMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
				new Vector3(
				Screen.width / ReferenceResolution.width,
				Screen.height / ReferenceResolution.height,
				1.0f));
			

		// Overwrite the current matrix to be the scaled matrix.
		GUI.matrix = scaledMatrix;

		return scaledMatrix;	
	}
	
	
}

