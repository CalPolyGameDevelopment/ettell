using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ProjectileLauncher))]
public class ProjectileLauncherController : MonoBehaviour {

    private ProjectileLauncher launcher;

    void Start() {
        launcher = gameObject.GetComponent<ProjectileLauncher>();
        launcher.Disable();
    }


    void Update() {


        if (Input.GetKeyDown(KeyCode.Space))
            launcher.Enable();
        if (Input.GetKeyUp(KeyCode.Space))
            launcher.Disable();


    }

}
