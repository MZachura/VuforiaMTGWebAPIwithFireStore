using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ObjectDetectionLogger : DefaultObserverEventHandler
{
    public GameObject myModelPrefab;

    //private void Update()
    //{
    //    if ()
    //    {

    //    }
    //}
    ////public GameObject mMyModelObject;

    //protected override void OnTrackingFound()
    //{
    //    Debug.Log("Target Found");

    //    // Instantiate the model prefab only if it hasn't been instantiated yet
    //    if (mMyModelObject == null)
    //    {
    //        InstantiatePrefab();
    //    }
    //    base.OnTrackingFound();
    //}

    //protected void InstantiatePrefab()
    //{
    //    if (myModelPrefab != null)
    //    {
    //        Debug.Log("Target found");
    //        //mMyModelObject = Instantiate(myModelPrefab, mObserverBehaviour.transform);
    //        //mMyModelObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    //        //mMyModelObject.SetActive(true);
    //    }
    //}
}
