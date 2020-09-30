using System.Collections;
using UnityEngine;
using System;
using UniRx;

public class TestCao : MonoBehaviour
{
    IObservable<Func<int, string>> getIslandType0;
    Func<int, string> getIslandType1;
    TestMediator testMediator;

    Hashtable table = new Hashtable();

    // Start is called before the first frame update
    private void Awake()
    {
        testMediator = GetComponent<TestMediator>();
    }
    void Start()
    {
        #region//Plan0
        testMediator.getIslandType0 = Observable.Start(() =>
        {
            Func<int, string> g = (n) =>
            {
                if (n == 1)
                {
                    return "Core Island";
                }
                else if (n == 2)
                {
                    return "Food Island";
                }
                else if (n == 3)
                {
                    return "Normal Island";
                }
                return "Normal Island";
            };
            return g;
        });

        //testMediator.getIslandType0 = Observable.Start<string>()
        #endregion

        #region//Plan1
        getIslandType0 = testMediator.getIslandType0;
        testMediator.getIslandType1 = (_n) =>
        {
            if (_n == 1)
            {
                return "Core Island";
            }
            else if (_n == 2)
            {
                return "Food Island";
            }
            else if (_n == 3)
            {
                return "Normal Island";
            }
            return "Normal Island";
        };
        #endregion

        table.Add("a", 1);
        table.Add("b", 4.3f);

        Action a = () => { print("666"); };

        table.Add("c", a);

        print(table["a"]);
        print(table["b"]);

        var b = table["c"] as Action;
        b();
    }
}
