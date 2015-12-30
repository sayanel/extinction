// @author : florian

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

using Extinction.Skills;

public class HUDSkillButton : MonoBehaviour
{

    [SerializeField]
    private float _updateDeltaTime = 0.2f;

    [SerializeField]
    private string _description = "a skill for herbie";

    private ActiveSkill _skill;

    private Image _coolDownImage;

    private Coroutine _coolDownRoutine;

    private EventTrigger _eventTrigger;

    private GameObject _floatingInfoModel;


    public void setFloatingInfoModel(GameObject model)
    {
        _floatingInfoModel = model;
    }

    public void showDescription(BaseEventData eventData)
    {
        if (_floatingInfoModel == null)
            return;

        _floatingInfoModel.SetActive(true);
        _floatingInfoModel.transform.position = Input.mousePosition;
    }

    public void hideDescription(BaseEventData eventData)
    {
        if (_floatingInfoModel == null)
            return;

        _floatingInfoModel.SetActive(false);
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
