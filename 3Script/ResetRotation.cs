using UnityEngine;

public class ResetRotation : MonoBehaviour
{
    public float resetSpeed;

    private Quaternion startAngleVector;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = this.transform.position;
        startAngleVector = this.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = startPosition;
        /*        float x = Mathf.Clamp(transform.transform.localEulerAngles.x, -20f, 20f);
                float z = Mathf.Clamp(transform.transform.localEulerAngles.z, -20f , 20f);*/
        //this.transform.localRotation = Quaternion.Euler(new Vector3(x, this.transform.localEulerAngles.y, z));

        this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, startAngleVector, resetSpeed * Time.deltaTime);
    }


}
