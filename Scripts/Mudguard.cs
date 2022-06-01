using System;
using UnityEngine;

namespace Car
{
    
    public class Mudguard : MonoBehaviour
    {
        public AI_CarController carController;

        private Quaternion m_OriginalRotation;


        private void Start()
        {
            m_OriginalRotation = transform.localRotation;
        }


        private void Update()
        {
            transform.localRotation = m_OriginalRotation*Quaternion.Euler(0, carController.CurrentSteerAngle, 0);
        }
    }
}
