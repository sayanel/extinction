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
            protected int _dammage = 1;
            public int Dammage
            {
                get { return _dammage; }
                set { _dammage = value; }
            }

            protected string[] _targetTag;
            public string[] TargetTag
            {
                get { return _targetTag; }
                set { _targetTag = value; }
            }
            private bool _isAlive = true;
            private Renderer _renderer;
            private Collider _collider;

            // ----------------------------------------------------------------------------
            // --------------------------------- METHODS ----------------------------------
            // ----------------------------------------------------------------------------
            public void Start()
            {
                _renderer = GetComponent<Renderer>();
                _collider = GetComponent<Collider>();
            }

            public void OnTriggerEnter(Collider other)
            {
                foreach (string tag in _targetTag)
                {
                    if (other.CompareTag(tag))
                    {
                        GameObject target = other.gameObject;
                        onHit(target);
                        _lifeTime = 0;
                    }
                }
            }

            public void onHit(GameObject obj)
            {
                Character o = obj.GetComponent<Character>();
                o.getDamage(_dammage);
            }

            public void Update()
            {
                _lifeTime -= Time.deltaTime;
                if (_lifeTime < 0) destroy();
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