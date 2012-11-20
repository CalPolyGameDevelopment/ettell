using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public abstract class WritableXml : XmlLoader {
	
	protected static Encoding enc = Encoding.Unicode;
	
	protected override byte[] docBytes() {
		return enc.GetBytes(PlayerPrefs.GetString(resourceName));
	}
	
	protected void save() {
		StartCoroutine(withLock(saveInner));
	}
	
	private void saveInner() {
		MemoryStream ms = new MemoryStream();
		xmlDoc.Save(ms);
		PlayerPrefs.SetString(resourceName, enc.GetString(ms.GetBuffer()));
		ms.Close();
	}
}