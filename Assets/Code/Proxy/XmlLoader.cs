using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

public abstract class XmlLoader : MonoBehaviour {
	
	public string resourceName;
	
	internal XmlDocument xmlDoc;
	
	internal bool xmlAvailable = false;
	
	protected delegate void xmlMutator();
	
	public virtual void Start () {
		if (Application.isPlaying) {
			load();
			xmlAvailable = true;
		}
	}
	
	protected IEnumerator withLock(xmlMutator f) {
		while (!xmlAvailable) {
			yield return 0;
		}
		xmlAvailable = false;
		f();
		xmlAvailable = true;
	}
	
	protected void assertReady() {
		if (!xmlAvailable) {
			throw new MissingReferenceException("xml file " + resourceName + " is not ready");
		}
	}
	
	private void load() {
		xmlDoc = new XmlDocument();
		MemoryStream ms = new MemoryStream(docBytes());
		xmlDoc.Load(ms);
		ms.Close();
	}
	
	protected virtual byte[] docBytes() {
		TextAsset textDoc = (TextAsset)Resources.Load(resourceName);
		return textDoc.bytes;
	}
	
    
}