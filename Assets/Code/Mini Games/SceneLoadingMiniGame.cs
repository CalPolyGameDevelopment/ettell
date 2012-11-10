using UnityEngine;
using System.Collections;
using System.Xml;

public abstract class SceneLoadingMiniGame : MonoBehaviour, MiniGameAPI.IMiniGame {

    protected XmlNode data;
    public XmlNode Data {
        set {
            data = value;
            Application.LoadLevel(XmlUtilities.getData(data.SelectSingleNode(XmlUtilities.sceneName)));
        }
    }

    void OnDestroy() {
        Application.LoadLevel("Empty");
    }

    void OnLevelWasLoaded(int level) {
        if (Application.loadedLevelName == XmlUtilities.getData(data.SelectSingleNode(XmlUtilities.sceneName))) {
            onMyLevelLoaded();
        }
    }

    protected virtual void onMyLevelLoaded() {
    }
}