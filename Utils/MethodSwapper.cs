using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace ServerSideCharacter2.Utils
{
	public class MethodSwapper
	{

		public static BindingFlags methodFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		public static BindingFlags methodFlags2 = BindingFlags.Instance | BindingFlags.Public;

		

		//public static void M_Draw(UIElement uIElement, SpriteBatch sb)
		//{
		//	// 傻逼原版程序员不好好写剪裁效果，连矩形相交都不判
		//	bool overflowHidden = uIElement.OverflowHidden;
		//	bool useImmediateMode = (bool)typeof(UIElement).GetField("_useImmediateMode",
		//		BindingFlags.NonPublic | BindingFlags.Instance).GetValue(uIElement);
		//	RasterizerState rasterizerState = sb.GraphicsDevice.RasterizerState;
		//	Rectangle scissorRectangle = sb.GraphicsDevice.ScissorRectangle;
		//	SamplerState anisotropicClamp = SamplerState.AnisotropicClamp;

		//	var mystate = new RasterizerState
		//	{
		//		CullMode = CullMode.None,
		//		ScissorTestEnable = true
		//	};
		//	if (useImmediateMode)
		//	{
		//		sb.End();
		//		sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, mystate, null, Main.UIScaleMatrix);
		//		typeof(UIElement).GetMethod("DrawSelf", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(uIElement, new object[] { sb });
		//		// uIElement.DrawSelf(sb);
		//		sb.End();
		//		sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, mystate, null, Main.UIScaleMatrix);
		//	}
		//	else
		//	{
		//		typeof(UIElement).GetMethod("DrawSelf", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(uIElement, new object[] { sb });
		//		// this.DrawSelf(sb);
		//	}
		//	if (overflowHidden)
		//	{
		//		sb.End();
		//		Rectangle clippingRectangle = uIElement.GetClippingRectangle(sb);
		//		sb.GraphicsDevice.ScissorRectangle = GetRectIntersections(scissorRectangle, clippingRectangle);
		//		sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, mystate, null, Main.UIScaleMatrix);
		//	}
		//	typeof(UIElement).GetMethod("DrawChildren", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(uIElement, new object[] { sb });
		//	// this.DrawChildren(sb);
		//	if (overflowHidden)
		//	{
		//		sb.End();
		//		sb.GraphicsDevice.ScissorRectangle = scissorRectangle;
		//		sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, rasterizerState, null, Main.UIScaleMatrix);
		//	}
		//}

		public static void SwapMethods()
		{
			try
			{
				MethodSwap(typeof(UIElement), "Draw", "M_Draw", methodFlags, true, new Type[] { typeof(SpriteBatch) });
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		public static MethodInfo GetMethod(Type type, string methodName, Type[] parameters, BindingFlags flags)
		{
			var flag = parameters != null;
			MethodInfo method;
			if (flag)
			{
				method = type.GetMethod(methodName, flags, null, parameters, null);
			}
			else
			{
				method = type.GetMethod(methodName, flags);
			}
			return method;
		}

		public static void MethodSwap(Type type, string original, string injection, BindingFlags flags, bool nonStatic = false, Type[] parameters = null)
		{
			try
			{
				var type2 = typeof(MethodSwapper);
				var repMethod = GetMethod(type, original, parameters, flags);
				var flag = parameters != null && nonStatic;
				if (flag)
				{
					var prevParam = parameters;
					parameters = new Type[parameters.Length + 1];
					parameters[0] = type;
					prevParam.CopyTo(parameters, 1);
				}
				var injMethod = GetMethod(type2, injection, parameters, flags);
				var repHandle = repMethod.MethodHandle;
				var injHandle = injMethod.MethodHandle;
				RuntimeHelpers.PrepareMethod(repHandle);
				RuntimeHelpers.PrepareMethod(injHandle);
				var repPtr = repHandle.Value + IntPtr.Size * 2;
				var injPtr = injHandle.Value + IntPtr.Size * 2;
				Marshal.WriteInt32(repPtr, Marshal.ReadInt32(injPtr));

			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
			}
		}
	}
}
