// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

namespace DanmakU.Controllers
{

    public class RotateController : IDanmakuController
    {

        [SerializeField, Show]
        public float RotateSpeed
        {
            get;
            set;
        }
        
        #region IDanmakuController implementation

        public void Update(Danmaku danmaku, float dt)
        {
            danmaku.AngularSpeed = 0f;

            float rotationChange = 360f * RotateSpeed * dt;
            danmaku.rotation += rotationChange;
        }

        #endregion

    }


}