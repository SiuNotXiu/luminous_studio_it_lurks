using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShapeShifterBaseState 
{
    public abstract void EnterState(ShapeShifterManager shapeShifter);

    public abstract void UpdateState(ShapeShifterManager shapeShifter);

    public abstract void ExitState(ShapeShifterManager shapeShifter);



}
