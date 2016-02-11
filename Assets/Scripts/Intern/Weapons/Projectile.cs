// @author : Pascale

using UnityEngine;
using System.Collections;
using Extinction.Characters;

namespace Extinction
{
    namespace Weapons
    {

        public class Projectile : MonoBehaviour, IHitTarget
        {
            // ----------------------------------------------------------------------------
            // -------------------------------- ATTRIBUTES --------------------------------
            // ----------------------------------------------------------------------------
            protected float _lifeTime = 10f;
            protected float _damage = 1;
            public float Damage
            {
                get { return _damage; }
                set { _damage = value; }
            }

            protected string[] _targetTag;
            public string[] TargetTag
            {
                get { return _targetTag; }
                set { _targetTag = value; }
            }
            protected bool _isAlive = true;
            protected Renderer _renderer;
            protected Collider _collider;

            [SerializeField]
            float _speed = 1;

            public float Speed{
                get { return _speed; }
                set { _speed = value; }
            }

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------
            public void Start()
            {
                _renderer = GetComponent<Renderer>();
                _collider = GetComponent<Collider>();
            }

            //public void OnTriggerEnter(Collider other)
            //{
            //    foreach (string tag in _targetTag)
            //    {
            //        if (other.CompareTag(tag))
            //        {
            //            GameObject target = other.gameObject;
            //            onHit(target);
            //            _lifeTime = 0;
            //        }
            //    }
            //}

            public void onHit(GameObject obj)
            {
                Character o = obj.GetComponent<Character>();

                //TODO : deal with float for character life
                o.getDamage((int)_damage);
            }

            /// <summary>
            /// Decrements the lifeTime, and hide the projectile after the lifeTime reach 0.
            /// </summary>
            public void updateLifeTime()
            {
                _lifeTime -= Time.deltaTime;
                if( _lifeTime < 0 ) destroy();
            }

            /// <summary>
            /// Make the projectile fly toward it's destination
            /// </summary>
            public void fly()
            {
                transform.position += _speed * transform.forward;
            }

            public void Update()
            {
                updateLifeTime();
                fly();
            }

            public bool isAlive()
            {
                return _isAlive;
            }

            public void destroy()
            {
                _isAlive = false;
                _collider.enabled = false;
                _renderer.enabled = false;
                Destroy(this.gameObject, 1);
            }

        }
    }
}