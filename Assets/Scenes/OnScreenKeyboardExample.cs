using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 这是虚拟键盘插件脚本
/// Tips:June于2022.3.13修改
/// </summary>

[RequireComponent(typeof(OnScreenKeyboard))]    //必须添加必要的脚本
public class OnScreenKeyboardExample : MonoBehaviour
{
	/// <summary>
	/// 虚拟键盘脚本
	/// </summary>
	private OnScreenKeyboard osk;
	/// <summary>
	/// 输入文字
	/// </summary>
	private string inputString;
	/// <summary>
	/// 输入文本框
	/// </summary>
	public InputField inputField;
	private string keyPressed;

	//每次激活清空文本框内容	(lambda表达式)
	private void OnEnable() => inputString = "";

	private void Start() => osk = GetComponent<OnScreenKeyboard>();     //获取组件

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
				// 完成输入，这里可以写自己的逻辑-->隐藏等操作...

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
			//将文字赋值给文本框中的文本属性
			inputField.text = inputString;
		}
	}
}
