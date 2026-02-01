using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityAsync;
using System;
using System.Linq;


public class OptionMovement : MonoBehaviour
{

    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject buttons;
    private Transform option;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move("LEFT");
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move("RIGHT");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<DialogueSystem>().type = option.gameObject.GetComponent<UIHelper>().type;
            GetComponent<DialogueSystem>().optionSelected = true;
        }
    }

    void Move(string action)
    {
        int index = option.parent.GetSiblingIndex();

        switch (action.ToUpper())
        {
            case "LEFT":
                index--;
                if (index < 0) index = buttons.transform.childCount - 1;
                break;
            case "RIGHT":
                index++;
                if (index > buttons.transform.childCount - 1) index = 0;
                break;
        }
        StartCoroutine(Draw(index));
    }

    IEnumerator Draw(int index)
    {
        yield return new UnityEngine.WaitForSeconds(Time.deltaTime);
        option = buttons.transform.GetChild(index);
        UI.transform.position = option.Find("PivotSelector").position;
    }

    IEnumerator InitPosition()
    {
        yield return new UnityEngine.WaitForSeconds(Time.deltaTime);
        option = buttons.transform.GetChild(0);
        UI.transform.position = option.Find("PivotSelector").position;
        if (!UI.activeSelf) UI.SetActive(true);
    }

    private void OnEnable()
    {
        StartCoroutine(InitPosition());
    }
}
