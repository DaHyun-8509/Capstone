using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroEnergy : MonoBehaviour
{
    [SerializeField] GameObject sleepUI;
    [SerializeField] Transform spawnPos;

    int sleepStartTime;

    void Update()
    {
        if(Managers.Energy.GetEnergy() <= 0.0001)
        {
            Managers.Energy.IncreaseEnergy(100);
            sleepUI.SetActive(true);
            transform.position = spawnPos.position;
            sleepStartTime = Managers.Time.GetHour();
            Managers.Time.NowSpeed = 30f;
            Inventory.instance.ClearInven();
            StartCoroutine(TimeRecover());
        }
    }

    IEnumerator TimeRecover()
    {
        int sleepEndTime = sleepStartTime + 10;
        if (sleepEndTime >= 24)
            sleepEndTime -= 24;
        yield return new WaitUntil(() => Managers.Time.GetHour() == sleepEndTime );
        Managers.Time.NowSpeed = 1;
        sleepUI.SetActive(false);

    }
}
