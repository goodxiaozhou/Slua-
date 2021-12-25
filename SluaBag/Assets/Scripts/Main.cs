using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SLua;
/// <summary>
/// 启动脚本
/// lua没有办法直接访问C#需要先从c#调用lua脚本后才把
/// 核心逻辑交给lua编写
/// </summary>
public class Main : MonoBehaviour {

	private LuaSvr lua_svr;
	void Start () {
		LuaMgr.GetInstance().Init();
		LuaMgr.GetInstance().DoLuaFile("Main");
    }
}
