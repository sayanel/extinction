// @author : florian

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

using Extinction.Skills;

public class HUDSkillButton : MonoBehaviour
{

    [SerializeField]
    private float _delayBeforeDisplayingInfo = 1;

    [SerializeField]
    private float _updateDeltaTime = 0.2f;

    [SerializeField]
    private string _description = "a skill for herbie";

    private ActiveSkill _skill;

    private Image _coolDownImage;

    private Coroutine _coolDownRoutine;

    private EventTrigger _eventTrigger;

    private GameObject _floatingInfoHUD;

    private Coroutine _showFloatingInfoRoutine;

    public void setDelayBeforeDisplayingInfo(float time)
    {
        _delayBeforeDisplayingInfo = time;
    }

    public void setFloatingInfoModel(GameObject model)
    {
        _floatingInfoHUD = model;
    }

    public void showDescription(BaseEventData eventData)
    {
        if (_floatingInfoHUD == null)
            return;

        _showFloatingInfoRoutine = StartCoroutine(showFloatingInfoCoroutine());
    }

    public void hideDescription(BaseEventData eventData)
    {
        if (_showFloatingInfoRoutine != null)
            StopCoroutine(_showFloatingInfoRoutine);

        if (_floatingInfoHUD == null)
            return;

        _floatingInfoHUD.SetActive(false);
    }

    IEnumerator showFloatingInfoCoroutine()
    {
        yield return new WaitForSeconds(_delayBeforeDisplayingInfo);

        //change the description on floating info hud
        _floatingInfoHUD.transform.GetChild(0).GetComponent<Text>().text = _description;

        //active the floating info hud
        _floatingInfoHUD.SetActive(true);
        _floatingInfoHUD.transform.position = Input.mousePosition;

    }

    public void setSkill(ActiveSkill skill)
    {
        _skill = skill;
    }

    public void setCoolDownImage(Image coolDownImage)
    {
        _coolDownImage = coolDownImage;
    }

    public void setDescription(string description)
    {
        _description = description;
    }

    /// <summary>
    /// The function we call when we launch an active skill.
    /// </summary>
    public void OnActiveSkill()
    {
        //visual cooldown : 
        if (_coolDownRoutine != null)
            StopCoroutine(_coolDownRoutine);

        _coolDownRoutine = StartCoroutine(coolDownHandling());
    }

    IEnumerator coolDownHandling()
    {
        if (_skill == null)
            yield return null;

        while(_skill.CurrentCoolDown > 0)
        {
            float fillRatio = _skill.CurrentCoolDown / _skill.CoolDown;

            _coolDownImage.fillAmount = fillRatio;

            yield return new WaitForSeconds(_updateDeltaTime);

            if (_skill.CurrentCoolDown <= 0)
                _coolDownImage.fillAmount = 0;
        }
    }

}
