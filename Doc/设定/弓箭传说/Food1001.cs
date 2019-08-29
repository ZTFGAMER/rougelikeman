using System;

public class Food1001 : FoodBase
{
    protected override void OnEnables()
    {
        base.bAbsorbImme = true;
    }
}

