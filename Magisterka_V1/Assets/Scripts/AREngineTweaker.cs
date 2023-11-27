using UnityEngine;
using Vuforia;

public class AREngineTweaker : MonoBehaviour
{
    public ModelTargetBehaviour target;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(target.TargetStatus.Status.ToString());
        if (target.TargetStatus.Status.ToString().Equals("TRACKED_EXTENDED") && target.TargetStatus.StatusInfo.ToString().Equals("WRONG_SCALE"))
        {
            Debug.Log("reseting");
            target.Reset();
        }


    }
}
