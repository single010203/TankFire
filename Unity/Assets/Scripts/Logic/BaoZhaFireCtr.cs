﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirzaBeig.VFX;
using Model;

public class BaoZhaFireCtr : WDXFireCtr
{      
    public ParticleSystems FXChe;
    public GameObject mChe;

    public new void GameOver()
    {
        FireTankComponent.Instance.MyFireTank.EndData();
        if (guidObj == null)
        {
            guidObj = Instantiate(Resources.Load(FilePath.uiPath + "Canvas_Guide")) as GameObject;
        }

        GuideCtr guidCtr = guidObj.GetComponent<GuideCtr>();
        StartCoroutine(Timer(1, guidCtr, 5));
    }
    public new void GameStart()
    {
        if (guidObj != null)
        {
            Destroy(guidObj);
        }
        guidObj = Instantiate(Resources.Load(FilePath.uiPath + "Canvas_Guide")) as GameObject;
        GuideCtr guidCtr = guidObj.GetComponent<GuideCtr>();
        StartCoroutine(Timer(1, guidCtr, 4));
    }

    public new void StepAnim1()
    {
        Debug.Log("1--初始状态---");

    }

    public new void StepAnim2()
    {
        Debug.Log("地面2--火情侦查---");
        OverLookView();
        MarkPoint.SetActive(true);
    }

    public new void StepAnim3()
    {
        Debug.Log("冷却实验3--消防设置---");
        ThirdView();
        EquipBtnClick();
        FXChe.gameObject.SetActive(true);
        FXChe.play();
    }

    public new void StepAnim4()
    {
        Debug.Log("冷却实验4--开始灭火---");
        FXChe.gameObject.SetActive(true);
        FXChe.play();
    }

    public new void StepAnim5()
    {
        Debug.Log("冷却实验5--灭火完成---");
        FXFlow.stop();
        FXChe.stop();
        MarkPoint.SetActive(false);
    }
}