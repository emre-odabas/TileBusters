using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace GameCore.Core
{
    public class CollisionEmitter : MonoBehaviour
    {
        public UnityAction<Collision, GameObject> onCollisionEnter;
        public UnityAction<Collider, GameObject> onTriggerEnter;
        public UnityAction<Collision, GameObject> onCollisionExit;
        public UnityAction<Collider, GameObject> onTriggerExit;
        public UnityAction<Collision, GameObject> onCollisionStay;
        public UnityAction<Collider, GameObject> onTriggerStay;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            
            onCollisionEnter?.Invoke(collision, gameObject);
        }
        private void OnCollisionExit(Collision collision)
        {
            onCollisionExit?.Invoke(collision, gameObject);
        }
        private void OnCollisionStay(Collision collision)
        {
            onCollisionStay?.Invoke(collision, gameObject);
        }
        private void OnTriggerEnter(Collider other)
        {
            onTriggerEnter?.Invoke(other, gameObject);
        }
        private void OnTriggerExit(Collider other)
        {
            onTriggerExit?.Invoke(other, gameObject);
        }
        private void OnTriggerStay(Collider other)
        {
            onTriggerStay?.Invoke(other, gameObject);
        }
    } 
}
