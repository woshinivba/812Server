using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ����������̲���ű�
/// Tips:June��2022.3.13�޸�
/// </summary>

[RequireComponent(typeof(OnScreenKeyboard))]    //������ӱ�Ҫ�Ľű�
public class OnScreenKeyboardExample : MonoBehaviour
{
	/// <summary>
	/// ������̽ű�
	/// </summary>
	private OnScreenKeyboard osk;
	/// <summary>
	/// ��������
	/// </summary>
	private string inputString;
	/// <summary>
	/// �����ı���
	/// </summary>
	public InputField inputField;
	private string keyPressed;

	//ÿ�μ�������ı�������	(lambda���ʽ)
	private void OnEnable() => inputString = "";

	private void Start() => osk = GetComponent<OnScreenKeyboard>();     //��ȡ���

	private void Update()
	{
		keyPressed = osk.GetKeyPressed();
		if (keyPressed != "")
		{
			if (keyPressed == "Backspace" || keyPressed == "<<")
			{
				if (inputString.Length > 0)
					inputString = inputString.Substring(0, inputString.Length - 1);
			}
			else if (keyPressed == "Space")
			{
				inputString += " ";
			}
			else if (keyPressed == "Enter" || keyPressed == "Done")
			{
				// ������룬�������д�Լ����߼�-->���صȲ���...

			}
			else if (keyPressed == "Caps")
			{
				// Toggle the capslock state yourself
				osk.SetShiftState(osk.GetShiftState() == ShiftState.CapsLock ? ShiftState.Off : ShiftState.CapsLock);
			}
			else if (keyPressed == "Shift")
			{
				// Toggle shift state ourselves
				osk.SetShiftState(osk.GetShiftState() == ShiftState.Shift ? ShiftState.Off : ShiftState.Shift);
			}
			else inputString += keyPressed;
			//�����ָ�ֵ���ı����е��ı�����
			inputField.text = inputString;
		}
	}
}
