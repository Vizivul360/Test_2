using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour, ICameraController
{
    public Camera _camera;
    public Transform root, anchor, roaming;

    [Space]
    public bool gizmos;

    //Runtime
    private Quaternion mainRot, roamRot;
    private Vector3 lookAt;

    public void SetConfig(CameraModel config)
    {
        //main position
        anchor.localPosition = new Vector3(config.roundRadius, config.height);
        anchor.localRotation = Quaternion.identity;
        
        //main rotation
        var mainAngle = 360.0f / config.roundDuration * Time.deltaTime;
        mainRot = Quaternion.Euler(0, mainAngle, 0);

        //roaming radius
        roaming.localPosition = Vector3.right * config.roamingRadius;
        roaming.localRotation = Quaternion.identity;

        //roaming rotation
        var roamAngle = 360.0f / config.roamingDuration * Time.deltaTime;
        roamRot = Quaternion.Euler(0, roamAngle, 0);

        //look point
        lookAt = root.position + Vector3.up * config.lookAtHeight;
        _camera.transform.localPosition = Vector3.zero;

        //fov control
        StartCoroutine(FovCoro(config));
    }

    void Update()
    {
        root.localRotation *= mainRot;
        anchor.localRotation *= roamRot;

        setCameraLook();
    }

    private void setCameraLook()
    {
        _camera.transform.rotation = Quaternion.LookRotation(lookAt - anchor.position, Vector3.up);
    }

    private void OnDrawGizmos()
    {
        if (gizmos)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(lookAt, root.position);
            Gizmos.DrawLine(root.position, anchor.position);
            Gizmos.DrawLine(anchor.position, roaming.position);
        }
    }

    IEnumerator FovCoro(CameraModel config)
    {
        var min = config.fovMin;
        var max = config.fovMax;

        var iterations = config.fovDuration / Time.deltaTime;
        var delay = new WaitForSeconds(config.fovDelay);

        while(true)
        {
            yield return delay;

            var newFov = Random.Range(min, max);
            var delta = (newFov - _camera.fieldOfView) / iterations;

            for (var i = 0; i < iterations; i++)
            {
                _camera.fieldOfView += delta;
                yield return null;
            }   
        }
    }
}

public interface ICameraController
{
    void SetConfig(CameraModel config);
}
