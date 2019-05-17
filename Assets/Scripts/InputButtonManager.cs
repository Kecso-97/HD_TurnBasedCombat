using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputButtonManager : MonoBehaviour
{
    private TurnController controller;

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        controller = TurnController.GetInstance();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetButtonDown($"Skill{i}"))
            {
                SkillKeyPress(i);
                break;
            }
        }
        if(Input.GetMouseButtonDown(0) && MouseToWorld(out RaycastHit hit))
        {
            controller.CurrentCharacterBehavior.SetTarget(hit.point);
        }
    }

    public void SkillKeyPress(int skillIndex)
    {
        controller.CurrentCharacterBehavior.UseSkill(skillIndex);
    }

    protected bool MouseToWorld(out RaycastHit hit)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {

                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }
}
