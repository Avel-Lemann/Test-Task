using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.Image;

public class CubeComparer : MonoBehaviour
{
    private int currentIdenticalCubes = 0;
    private bool FunctionBusy;
    public IEnumerator AddIdenticalCubes(bool trueCubeStay)
    {
        if(!FunctionBusy)
        {
            FunctionBusy = true; //Это, честно говоря, затычка от багов, на всякий случай, так как у меня не было времени нормально все потестить.

            if (trueCubeStay)
            {
                currentIdenticalCubes++;
            }
            else if (!trueCubeStay)
            {
                currentIdenticalCubes--;
            }
            Debug.Log(currentIdenticalCubes);
            if (currentIdenticalCubes == 9)
            {
                Debug.Log("Победа");
            }

            yield return new WaitForSeconds(0.3f);

            FunctionBusy = false;
        }
    }
}