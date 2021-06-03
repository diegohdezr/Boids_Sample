using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* also attached to the main camera, this script causes the camera to turn and look
 * at the attractor each frame
 */

public class LookAtAttractor : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Attractor.POS);
    }
}
