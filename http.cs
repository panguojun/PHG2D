using System.Collections; 
using UnityEngine; 
using UnityEngine.Networking;  
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.UI;

public class http : MonoBehaviour 
{  
	public GameObject jsonModel;
	public Text iptext;
	public Text nodetxt;
	public Text cmdtxt;
	public void sendmsg() 
	{     
		//sendphg("{{md:gear}{md:gear}}setup('entity');tojson();");
		sendphg("{" + nodetxt.text + "}" + cmdtxt.text);
	}

	void sendphg(string phg)
	{
		ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
		var url = "http://" + iptext.text + ":8080/cmd";//"http://192.168.1.122:8080/cmd";
		//var url = "http://localhost:8080/cmd";
		StartCoroutine(PostUrl(url,phg));
	}

	IEnumerator PostUrl(string url, string postData)
	{
		using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
		{
			byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(postData);
			webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
			webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
			webRequest.SetRequestHeader("Content-Type", "text/plain");

			yield return webRequest.Send();
			if (webRequest.isNetworkError)
			{
				Debug.Log(webRequest.error);
			}
			else
			{
				Debug.Log(webRequest.downloadHandler.text);
				jsonModel.SendMessage("onmsg",webRequest.downloadHandler.text);
			}
		}
	}

	public bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
	{
		bool isOk = true;
		// If there are errors in the certificate chain, look at each error to determine the cause.
		if (sslPolicyErrors != SslPolicyErrors.None)
		{
			for (int i = 0; i < chain.ChainStatus.Length; i++)
			{
				if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
				{
					chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
					chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
					chain.ChainPolicy.UrlRetrievalTimeout = new System.TimeSpan(0, 1, 0);
					chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
					bool chainIsValid = chain.Build((X509Certificate2)certificate);
					if (!chainIsValid)
					{
						isOk = false;
					}
				}
			}
		}
		return isOk;
	}
}