﻿
// =================================	
// Namespaces.
// =================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace VFX
    {

        // =================================	
        // Classes.
        // =================================

        //[ExecuteInEditMode]
        [System.Serializable]

        public class ParticleSystems : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================



            // =================================	
            // Variables.
            // =================================

            public ParticleSystem[] particleSystems { get; set; }

            // Event delegates.

            public delegate void onParticleSystemsDeadEventHandler();
            public event onParticleSystemsDeadEventHandler onParticleSystemsDeadEvent;

            // =================================	
            // Functions.
            // =================================

            // ...

            protected virtual void Awake()
            {
                particleSystems = GetComponentsInChildren<ParticleSystem>();
            }

            // ...

            protected virtual void Start()
            {

            }

            // ...

            protected virtual void Update()
            {

            }

            // ...

            protected virtual void LateUpdate()
            {
                if (!isAlive())
                {
                    if (onParticleSystemsDeadEvent != null)
                    {
                        onParticleSystemsDeadEvent();
                    }
                }
            }

            // ...

            //public void play()
            //{
            //    for (int i = 0; i < particleSystems.Length; i++)
            //    {
            //        particleSystems[i].Play();
            //    }
            //}

            public void play()
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                   
                    ParticleSystem tempParticleSystem = particleSystems[i];
                    StartCoroutine(Timer(3f, delegate () { tempParticleSystem.Play(); }));
                }
            }

            // ...

            public void pause()
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    particleSystems[i].Pause();
                }
            }

            // ...

            public void stop()
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    ParticleSystem tempParticleSystem = particleSystems[i];
                    StartCoroutine(Timer(3f, delegate() { tempParticleSystem.Stop(); }));
                }
            }

            public void stopImediate()
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    particleSystems[i].Stop();
                }
            }

            IEnumerator Timer(float time, UnityAction action)
            {
                while (time > 0)
                {
                    yield return new WaitForSeconds(1);
                    //btn.GetComponentInChildren<Text>().text = string.Format("等" + "{0}" + "秒", time);
                    time--;
                }
                action();
            }

            // ...

            public void clear()
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    particleSystems[i].Clear();
                }
            }

            // ...

            public void setLoop(bool loop)
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    particleSystems[i].loop = loop;
                }
            }

            // ...

            public void setPlaybackSpeed(float speed)
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    particleSystems[i].playbackSpeed = speed;
                }
            }

            // ...

            public bool isAlive()
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    if (particleSystems[i])
                    {
                        if (particleSystems[i].IsAlive())
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            // ...

            public bool isPlaying(bool checkAll = false)
            {
                if (particleSystems.Length == 0)
                {
                    return false;
                }
                else if (!checkAll)
                {
                    return particleSystems[0].isPlaying;
                }
                else
                {
                    for (int i = 0; i < 0; i++)
                    {
                        if (!particleSystems[i].isPlaying)
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            // ...

            public int getParticleCount()
            {
                int pcount = 0;

                for (int i = 0; i < particleSystems.Length; i++)
                {
                    if (particleSystems[i])
                    {
                        pcount += particleSystems[i].particleCount;
                    }
                }

                return pcount;
            }

            // =================================	
            // End functions.
            // =================================

        }

        // =================================	
        // End namespace.
        // =================================

    }

}

// =================================	
// --END-- //
// =================================
