using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HUDProgressBar : MonoBehaviour 
{
    /// <summary>
    /// A reference to an Image which is the representation of the life bar in the HUD.
    /// </summary>
    [SerializeField]
    Image _lifeBarVisual;


    void Awake()
    {
        //try to fill the _lifeBarVisual parameter searching an Image on this gameObject.
        if( _lifeBarVisual == null )
            _lifeBarVisual = GetComponent<Image>();
    }

    /// <summary>
    /// fill the life bar with a value bettween 0 and 1.
    /// example, for life : ratio = life/maxLife;
    /// </summary>
    /// <param name="ratio"></param>
    public void setProgression(float ratio)
    {
        _lifeBarVisual.fillAmount = ratio;
    }

    public float getProgression()
    {
        return _lifeBarVisual.fillAmount;
    }

}
