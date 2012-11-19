using UnityEngine;
using System.Collections;
using System.Xml;

public abstract class SceneLoadingMiniGame : MonoBehaviour, MiniGameAPI.IMiniGame {
	
	private const string SCENE_NAME = "sceneName";

    protected XmlNode data;
    public XmlNode Data {
        set {
            data = value;
            Application.LoadLevel(data.childNode(SCENE_NAME).getString());
        }
    }

    void OnDestroy() {
        Application.LoadLevel("Empty");
    }

    void OnLevelWasLoaded(int level) {
        if (Application.loadedLevelName == data.childNode(SCENE_NAME).getString()) {
            onMyLevelLoaded();
        }
    }

    protected virtual void onMyLevelLoaded() {
    }
}