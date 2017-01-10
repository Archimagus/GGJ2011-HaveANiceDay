using UnityEngine;
using System.Collections;

public class StartTargetButton : MyButton 
{
	public GameObject chooseraceobject;
	public GameObject buttonpinkobject;
	public GameObject buttonblueobject;
	public GameObject player1object;

	protected override void Click()
	{
        base.Click();

        buttonpinkobject.transform.position = new Vector3(-3.2f, 1f, 0f);
        buttonblueobject.transform.position = new Vector3(1.0f, 1f, 0f);
        chooseraceobject.transform.position= new Vector3(-1.0f,0.9f,-2.246367f);
		
        player1object.transform.position= new Vector3(-20.0f,0.9f,-2.246367f);
				
	}
}
