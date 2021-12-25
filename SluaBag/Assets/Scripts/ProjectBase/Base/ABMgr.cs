using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;
using SLua;
/// <summary>
/// 知识点
/// 1.AB包相关的API
/// 2.单例模式
/// 3.委托
/// 4.协成
/// 5.字典
/// </summary>
[CustomLuaClassAttribute]
public class ABMgr : SingletonAutoMono<ABMgr>
{
    /// <summary>
    /// 主包
    /// </summary>
    private AssetBundle mainAB = null;

    

    /// <summary>
    /// 依赖包获取用的配置文件
    /// </summary>
    private AssetBundleManifest manifest = null;
    /// <summary>
    /// 字典用于存储加载过的AB包
    /// </summary>
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();
    /// <summary>
    /// AB包存放路径方便修改
    /// </summary>
    private string PathUrl
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }
    /// <summary>
    /// 主包名，方便修改
    /// </summary>
    private string MainABName
    {
        get
        {
#if UNITY_IOS
            return "IOS";
#elif UNITY_ANDROID
            return "Android";
#else
            return "PC";
#endif
        }
    }
   
    
    /// <summary>
    /// 加载AB包方法
    /// </summary>
    /// <param name="abName">资源包名字</param>
    public void LoadAB(string abName)
    {
        //加载AB包
        if (mainAB==null)
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainABName);
            manifest=mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //获取依赖信息
        AssetBundle ab = null;
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            //判断是否加载
            if (!abDic.ContainsKey(strs[i]))
            {
                ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                abDic.Add(strs[i], ab);
            }
        }
        //加载资源来源包
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(PathUrl + abName);
            abDic.Add(abName, ab);
        }
        
    }

   /// <summary>
   /// 同步加载，不指定类型
   /// </summary>
   /// <param name="abName"></param>
   /// <param name="resName"></param>
    public Object LoadRes(string abName, string resName)
    {
        //加载AB包
        LoadAB(abName);
        //判断是否为Gameobject,是就实例化在返回给外部
        Object obj = abDic[abName].LoadAsset(resName);
        if (obj is GameObject)
            return Instantiate(obj);
        else
            return obj;
    }
    /// <summary>
    /// 同步加载 根据type指定类型
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="type"></param>
    /// <returns></returns>

    public  Object LoadRes(string abName,string resName,System.Type type)
    {
        LoadAB(abName);
        Object obj = abDic[abName].LoadAsset(resName, type);
        if (obj is GameObject)
            return Instantiate(obj);
        else
            return obj;
    }
    /// <summary>
    /// 同步加载 泛型指定类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <returns></returns>
    public T LoadRes<T>(string abName,string resName) where T:Object
    {
        LoadAB(abName);
        T obj = abDic[abName].LoadAsset<T>(resName);
        if (obj is GameObject)
            return Instantiate(obj);
        else
            return obj;
    }
    /// <summary>
    /// 异步加载不指定类型
    /// </summary>
    public void LoadResAsync(string abName,string resName,UnityAction<UnityEngine.Object> callBack)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, callBack));   
    }
    /// <summary>
    /// 异步加载Type指定类型
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="type"></param>
    /// <param name="callBack"></param>
    public void LoadResAsync(string abName, string resName,System.Type type, UnityAction<Object> callBack)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, type,callBack));
    }
    /// <summary>
    /// 异步加载泛型指定类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callBack"></param>
    public void LoadResAsync<T>(string abName, string resName,UnityAction<T> callBack)where T:Object
    {
        StartCoroutine(ReallyLoadResAsync<T>(abName, resName, callBack));
    }


    private IEnumerator ReallyLoadResAsync(string abName,string resName,UnityAction<Object> callBack)
    {
        LoadAB(abName);
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);
        yield return abr;
        if (abr.asset is GameObject)
            callBack(Instantiate(abr.asset));
        else
            callBack(abr.asset);
    }

    private IEnumerator ReallyLoadResAsync(string abName,string resName,System.Type type,UnityAction<Object> callBack)
    {
        LoadAB(abName);
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName, type);
        yield return abr;
        if (abr.asset is GameObject)
            callBack(Instantiate(abr.asset));
        else
            callBack(abr.asset);
    }
    private IEnumerator ReallyLoadResAsync<T>(string abName, string resName,UnityAction<T> callBack)where T : Object
    {
        LoadAB(abName);
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
        yield return abr;
        if (abr.asset is GameObject)
            callBack(Instantiate(abr.asset) as T);
        else
            callBack(abr.asset as T);
    }

    /// <summary>
    /// 单个包的卸载
    /// </summary>
    /// <param name="abName"></param>
    public void UnLoad(string abName)
    {
        if (abDic.ContainsKey(abName))
        {
            abDic[abName].Unload(false);
            abDic.Remove(abName);
        }
    }
    /// <summary>
    /// 所有包卸载
    /// </summary>
    public void ClearAB()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        mainAB = null;
        manifest = null;
    }

}
