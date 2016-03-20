using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HUDLifeBar : MonoBehaviour 
{

    [SerializeField]
    private Transform _viewerCamera;

    public Transform ViewerCamera {
        get{ return _viewerCamera; }
        set{ _viewerCamera = value; }
    }

    [SerializeField]
    private RectTransform _panel;
    private Image _panelImage;
    [SerializeField]
    private RectTransform _background;

    void Awake()
    {
        //try to automatically fill _panel and _background parameters.
        if(_panel == null )
        {
            Transform child = transform.GetChild( 0 );
            if( child != null )
            {
                RectTransform rectTransform = child.GetComponent<RectTransform>();
                if( rectTransform != null )
                    _background = rectTransform;

                child = child.GetChild( 0 );
                if(child != null)
                {
                    rectTransform = child.GetComponent<RectTransform>();
                    if( rectTransform != null )
                        _panel = rectTransform;
                }

            }

            if( _panel != null )
                _panelImage = _panel.GetComponent<Image>();
        }
    }
	
	void Update() 
	{
        transform.rotation = Camera.main.transform.rotation;
	}

    public void changeHealth(float currentHealth, float maxHealth )
    {
        float clampedHealth = currentHealth / maxHealth;

        if( clampedHealth < 0 )
            clampedHealth = 0;

        if(_panel != null)
            _panel.transform.localScale = new Vector3(clampedHealth * _background.transform.localScale.x, _background.transform.localScale.y, _background.transform.localScale.z );

        if(_panelImage != null)
            _panelImage.color = Color.Lerp( Color.red, Color.green, clampedHealth );
    }
}
