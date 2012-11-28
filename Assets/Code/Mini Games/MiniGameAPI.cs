using UnityEngine;
using System.Collections;
using System.Linq;
using System.Xml;

public class MiniGameAPI : MonoBehaviour {
	
	protected Ending[] endings;
	public Ending[] Endings {
		get {
			return endings;
		}
	}
	
	public interface IMiniGame {
		XmlNode Data {
			set;
		}
	}
	
	public MonoBehaviour miniGame;

	public XmlNode Data {
		set {
			endings = Ending.findEndings(value).Where(e => e.displayKey).ToArray();
			if (value.getString() == "dialog") {
				endings = new Ending[0];
			}
			(miniGame as IMiniGame).Data = value;
		}
	}
}