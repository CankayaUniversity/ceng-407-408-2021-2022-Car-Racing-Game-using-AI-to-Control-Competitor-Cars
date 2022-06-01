using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Car
{
    [RequireComponent(typeof (AI_CarController))]
    public class CarAIControl : MonoBehaviour
    {
        public enum BrakeCondition
        {
            NeverBrake,                
            TargetDirectionDifference, 
            TargetDistance,            
                                      
        }

        

        [SerializeField] [Range(0, 1)] private float m_CautiousSpeedFactor = 0.05f;               // tedbirli olacaðý yüzde
        [SerializeField] [Range(0, 180)] private float m_CautiousMaxAngle = 50f;                  // köþeleri dönerken alacaðý risk-denge
        [SerializeField] private float m_CautiousMaxDistance = 100f;                              // tedbir aldýðý mesafe
        [SerializeField] private float m_CautiousAngularVelocityFactor = 30f;                     // mevcut açýsýný ve hýzýný hesaplarken aldýðý tedbir(savrulmamasý için)
        [SerializeField] private float m_SteerSensitivity = 0.05f;                                // dönüþ hassasiyeti
        [SerializeField] private float m_AccelSensitivity = 0.04f;                                // istenen mevcut hýza ulaþmak için hýzlanmayý ne kadar hassas kullanacaðý
        [SerializeField] private float m_BrakeSensitivity = 1f;                                   // fren hassasiyeti
        [SerializeField] private float m_LateralWanderDistance = 3f;                              // bir objenin yanýndan geçme mesafesi
        [SerializeField] private float m_LateralWanderSpeed = 0.1f;                               // yanýndan geçerkenki hýz
        [SerializeField] [Range(0, 1)] private float m_AccelWanderAmount = 0.1f;                  // hýzlanma-ivme durumu
        [SerializeField] private float m_AccelWanderSpeed = 0.1f;                                 // ^^
        [SerializeField] private BrakeCondition m_BrakeCondition = BrakeCondition.TargetDistance; // fren sistemi 
        public bool m_Driving;                                                  // sürüþ modu açýk-kapalý
        [SerializeField] private Transform m_Target;                                              // arabanýn waypoint dikkati
        [SerializeField] private bool m_StopWhenTargetReached;                                    // hedefe ulaþýldýðýnda aracý sürmeyi býrakýp býrakmama
        [SerializeField] private float m_ReachTargetThreshold = 2;                               



        
        private float m_RandomPerlin;              //arabalarýn ip gibi gitmemesi için random deðerler
        private AI_CarController m_CarController;    // Reference to actual car controller we are controlling
        private float m_AvoidOtherCarTime;        // baþka bir araç ile çarpýþtýðýnda kaçýnma süresi
        private float m_AvoidOtherCarSlowdown;    // kaçýnýrken yavaþlama durumu
        private float m_AvoidPathOffset;          //kaçýnýrken yolun kaçacaðý yön
        private Rigidbody m_Rigidbody;


        private void Awake()
        {
         
            m_CarController = GetComponent<AI_CarController>();

        
            m_RandomPerlin = Random.value*100;

            m_Rigidbody = GetComponent<Rigidbody>();
        }
        /*
        IEnumerator goBack()
        {
           
            yield return new WaitForSeconds(0.2f);
            m_CarController.Move(0, -10, -10f, 1f);
            yield return new WaitForSeconds(2f);
          
        }*/

        IEnumerator TurnTheCar()
        {
            m_Driving = false;
            yield return new WaitForSeconds(1f);
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            yield return new WaitForSeconds(0.3f);
            m_Driving = true;



        }

        private void OnTriggerEnter(Collider other)
        {   /*
            if (other.CompareTag("terrain"))
            {

                StartCoroutine(goBack());

            }
            */

            if (other.CompareTag("roads"))
            {

                StartCoroutine(TurnTheCar());

            }
            if (other.CompareTag("Finish"))
            {
                m_CarController.Move(0, 0, -1f, 1f);
                gameObject.GetComponentInParent<CarAIControl>().enabled = false;


            }


        }


        private void FixedUpdate()
        {
            if (m_Target == null || !m_Driving)
            {
                
                m_CarController.Move(0, 0, -1f, 1f);
            }
            else
            {
                Vector3 fwd = transform.forward;
                if (m_Rigidbody.velocity.magnitude > m_CarController.MaxSpeed*0.1f)
                {
                    fwd = m_Rigidbody.velocity;
                }

                float desiredSpeed = m_CarController.MaxSpeed;

               
                switch (m_BrakeCondition)
                {
                    case BrakeCondition.TargetDirectionDifference:
                        {
                          
                            float approachingCornerAngle = Vector3.Angle(m_Target.forward, fwd);

                           
                            float spinningAngle = m_Rigidbody.angularVelocity.magnitude*m_CautiousAngularVelocityFactor;

                          
                            float cautiousnessRequired = Mathf.InverseLerp(0, m_CautiousMaxAngle,
                                                                           Mathf.Max(spinningAngle,
                                                                                     approachingCornerAngle));
                            desiredSpeed = Mathf.Lerp(m_CarController.MaxSpeed, m_CarController.MaxSpeed*m_CautiousSpeedFactor,
                                                      cautiousnessRequired);
                            break;
                        }

                    case BrakeCondition.TargetDistance:
                        {
                          
                            Vector3 delta = m_Target.position - transform.position;
                            float distanceCautiousFactor = Mathf.InverseLerp(m_CautiousMaxDistance, 0, delta.magnitude);

                           
                            float spinningAngle = m_Rigidbody.angularVelocity.magnitude*m_CautiousAngularVelocityFactor;

                           
                            float cautiousnessRequired = Mathf.Max(
                                Mathf.InverseLerp(0, m_CautiousMaxAngle, spinningAngle), distanceCautiousFactor);
                            desiredSpeed = Mathf.Lerp(m_CarController.MaxSpeed, m_CarController.MaxSpeed*m_CautiousSpeedFactor,
                                                      cautiousnessRequired);
                            break;
                        }

                    case BrakeCondition.NeverBrake:
                        break;
                }

                
                Vector3 offsetTargetPos = m_Target.position;

               
                if (Time.time < m_AvoidOtherCarTime)
                {
                   
                    desiredSpeed *= m_AvoidOtherCarSlowdown;

                   
                    offsetTargetPos += m_Target.right*m_AvoidPathOffset;
                }
                else
                {
                    
                    offsetTargetPos += m_Target.right*
                                       (Mathf.PerlinNoise(Time.time*m_LateralWanderSpeed, m_RandomPerlin)*2 - 1)*
                                       m_LateralWanderDistance;
                }

                
                float accelBrakeSensitivity = (desiredSpeed < m_CarController.CurrentSpeed)
                                                  ? m_BrakeSensitivity
                                                  : m_AccelSensitivity;

               
                float accel = Mathf.Clamp((desiredSpeed - m_CarController.CurrentSpeed)*accelBrakeSensitivity, -1, 1);

               
                accel *= (1 - m_AccelWanderAmount) +
                         (Mathf.PerlinNoise(Time.time*m_AccelWanderSpeed, m_RandomPerlin)*m_AccelWanderAmount);

               
                Vector3 localTarget = transform.InverseTransformPoint(offsetTargetPos);

               
                float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z)*Mathf.Rad2Deg;

                
                float steer = Mathf.Clamp(targetAngle*m_SteerSensitivity, -1, 1)*Mathf.Sign(m_CarController.CurrentSpeed);

             
                m_CarController.Move(steer, accel, accel, 0f);

              
                if (m_StopWhenTargetReached && localTarget.magnitude < m_ReachTargetThreshold)
                {
                    m_Driving = false;
                }
            }
        }


        private void OnCollisionStay(Collision col)
        {
            
            if (col.rigidbody != null)
            {
                var otherAI = col.rigidbody.GetComponent<CarAIControl>();
                if (otherAI != null)
                {
                    
                    m_AvoidOtherCarTime = Time.time + 1;

                   
                    if (Vector3.Angle(transform.forward, otherAI.transform.position - transform.position) < 90)
                    {
                       
                        m_AvoidOtherCarSlowdown = 0.5f;
                    }
                    else
                    {
                        
                        m_AvoidOtherCarSlowdown = 1;
                    }

                  
                    var otherCarLocalDelta = transform.InverseTransformPoint(otherAI.transform.position);
                    float otherCarAngle = Mathf.Atan2(otherCarLocalDelta.x, otherCarLocalDelta.z);
                    m_AvoidPathOffset = m_LateralWanderDistance*-Mathf.Sign(otherCarAngle);
                }
            }
        }


        public void SetTarget(Transform target)
        {
            m_Target = target;
            m_Driving = true;
        }
    }
}
