using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OM
{
    public interface IState
    {
        public void Enter();
        public void Update();
        public void Exit();
    }
}
