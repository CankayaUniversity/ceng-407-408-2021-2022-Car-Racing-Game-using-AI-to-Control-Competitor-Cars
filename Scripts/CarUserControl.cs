using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; 


        private void Awake()
        {
        
            m_Car = GetComponent<CarController>();
        }

        
        private void FixedUpdate()
        {
            
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
#if !MOBILE_INPUT
            float handbrake = Input.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
        

}
