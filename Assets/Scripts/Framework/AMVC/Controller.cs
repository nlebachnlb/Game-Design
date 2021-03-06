using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.AMVC
{
    public class Controller : Entity
    {

    }

    public class Controller<T> : Controller where T : App
    {
        new public T app 
        {
            get
            {
                return (T) base.app;
            }
        }
    }
}
