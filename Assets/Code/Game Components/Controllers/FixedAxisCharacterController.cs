using UnityEngine;
using System.Collections;

/// <summary>
/// Simplistic CharacterController that fixes the character to one axis.
/// 
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class FixedAxisCharacterController : MonoBehaviour {

    public enum AxisChoice {
        X,
        Y,
        Z
    };

    public float maxSpeed;
    public AxisChoice axis;

    public float axisPositionMin = float.NegativeInfinity;
    public float axisPositionMax = float.PositiveInfinity;
    private CharacterController controller;
    private Vector3 velocity = Vector3.zero;


    float GetAxisPosition() {
        switch (axis) {
            case AxisChoice.X:
                return controller.transform.position.x;
            case AxisChoice.Y:
                return controller.transform.position.y;
            case AxisChoice.Z:
                return controller.transform.position.z;
        }
        return 0;
    }
    void SetPositionToMax() {
        Vector3 pos = controller.transform.position;
        switch (axis) {
            case AxisChoice.X:
                controller.transform.position = new Vector3(axisPositionMax, pos.y, pos.z);
                break;
            case AxisChoice.Y:
                controller.transform.position = new Vector3(pos.x, axisPositionMax, pos.z);
                break;
            case AxisChoice.Z:
                controller.transform.position = new Vector3(pos.x, pos.y, axisPositionMax);
                break;
        }

    }
    void SetPositionToMin() {
        Vector3 pos = controller.transform.position;
        switch (axis) {
            case AxisChoice.X:
                controller.transform.position = new Vector3(axisPositionMin, pos.y, pos.z);
                break;
            case AxisChoice.Y:
                controller.transform.position = new Vector3(pos.x, axisPositionMin, pos.z);
                break;
            case AxisChoice.Z:
                controller.transform.position = new Vector3(pos.x, pos.y, axisPositionMin);
                break;
        }

    }
    // Update is called once per frame
    void Update() {

        velocity = Vector3.zero;

        controller = GetComponent(
            typeof(CharacterController)) as CharacterController;

        // Determine if movement is desired along the 
        // selected axis by the user. If set, "velocity" will be an
        // axis specific unit vector.
        if (axis == AxisChoice.X) {
            velocity.x = Input.GetAxis("Horizontal");
        }
        else if (axis == AxisChoice.Y) {
            velocity.y = Input.GetAxis("Vertical");
        }
        else if (axis == AxisChoice.Z) {
            // NOTE: Jump doesn't have a negative button
            velocity.z = Input.GetAxis("Jump");
        }

        // Scale the unit vector and transform to worldspace
        velocity = transform.TransformDirection(
            velocity * maxSpeed * Time.deltaTime);

        float pos = GetAxisPosition();
        if (pos > axisPositionMax) {
            SetPositionToMax();
        }
        else if (pos < axisPositionMin) {
            SetPositionToMin();
        }
        else {
            controller.Move(velocity);
        }



    }


}
