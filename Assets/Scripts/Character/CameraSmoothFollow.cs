using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;

    // The distance in the x-z plane to the target
    [SerializeField] private float _distance = 10.0f;
    // the height we want the camera to be above the target
    [SerializeField] private float _height = 5.0f;
    // How much we 
    [SerializeField] private float _heightDamping = 2.0f;
    [SerializeField] private float _rotationDamping = 3.0f;

    void LateUpdate()
    {
        // Early out if we don't have a target
        if (!this._target) return;

        // Calculate the current rotation angles
        float wantedRotationAngle = this._target.eulerAngles.y;
        float wantedHeight = this._target.position.y + this._height;

        float currentRotationAngle = this.transform.eulerAngles.y;
        float currentHeight = this.transform.position.y;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, this._rotationDamping * Time.deltaTime);

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, this._heightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        this.transform.position = this._target.position;
        this.transform.position -= currentRotation * Vector3.forward * this._distance;

        // Set the height of the camera
        this.transform.position = new Vector3(this.transform.position.x, currentHeight, this.transform.position.z);

        // Always look at the target
        this.transform.LookAt(this._target);
    }
}
