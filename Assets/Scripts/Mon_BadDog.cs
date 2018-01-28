using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon_BadDog : MonsterScript {

    protected override void Start() {
        base.Start();

        id = MonsterContainerScript.MonsterID.BadDog;
    }
}
