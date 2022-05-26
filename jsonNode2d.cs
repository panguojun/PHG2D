using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]

[System.Serializable]
class Node
{
    public string name;
	public string parent;
	public int img;
	public string type;
	public Vector3 p;
	public float ang;
	public Vector3 s;
	//public Trigger []trigger;
}
[System.Serializable]
class Nodes
{
    public Node[] nodes;
}
public class jsonNode2d : MonoBehaviour
{
	public GameObject nodetemp;
	public string jsonData;
	public Texture[] texture;

    void Upate()
    {
        Nodes jsn = JsonUtility.FromJson<Nodes>(jsonData);
        foreach (Node node in jsn.nodes)
        {
			GUI.DrawTexture(new Rect(node.p.x, node.p.y, 32, 32), textures[node.img], ScaleMode.ScaleToFit, true, 10.0F)
		}
    }
	void onmsg(string json)
	{
		jsonData = json;
		build3d();
	}
	
    public string ReadData()
    {
        //string类型的数据常量
        string readData;
        //获取到路径
		string fileUrl = Application.dataPath + @"/StreamingAssets/" + "//node1.json";
        //读取文件
        using (StreamReader sr = File.OpenText(fileUrl))
        {
            //数据保存
            readData = sr.ReadToEnd();
            sr.Close();
        }
        //返回数据
        return readData;
    }
}