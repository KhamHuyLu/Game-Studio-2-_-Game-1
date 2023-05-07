using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Gteem
{
    public class cameraPlayer : MonoBehaviour
    {

        public Transform Target;
        public float Dis = -15f;
        // Update is called once per frame
        void Update()
        {
            gameObject.transform.position = new Vector3(Target.position.x, Target.position.y, Dis);
        }
    }
}