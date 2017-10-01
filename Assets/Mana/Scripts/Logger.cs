using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mana
{
	/// <summary>
	/// Substitue Unity logs with these logs. 
	/// Useful having first line verbose because call stack can get jumbled on some platforms or when using coroutines (I hope usage of StackFrame doesn't get jumbled)
	/// TODO: Extend with categories / channels.
	/// TODO: Wrap this with a preprocessor directive to disable at runtime.
	/// TODO: Profile whether the usage of reflection for logging has noticeable impact in dev environment.
	/// TODO: Ensure usage of reflection here does not reach a production environment (should get compiled out though).
	/// </summary>
	public static class Logger
	{
		/// <summary>
		/// Transient class that nicely wraps StackFrame
		/// </summary>
		private class MethodCallerInfo
		{
			public string ClassName;
			public string MethodName;

			public MethodCallerInfo(int skipFrames)
			{
				// Add frame because of the call into here 
				++skipFrames;

				System.Diagnostics.StackFrame stackFrame = new System.Diagnostics.StackFrame(skipFrames);
				System.Reflection.MethodBase methodBase = stackFrame.GetMethod();

				MethodName = methodBase.Name;
				ClassName = methodBase.ReflectedType.Name;
			}
		}

		private static StringBuilder s_stringBuilder = new StringBuilder();	

		private static string ConstructLog(string log, params object[] parameters)
		{
			const int SKIP_FRAMES = 2;
			MethodCallerInfo callerInfo = new MethodCallerInfo(SKIP_FRAMES);

			s_stringBuilder.Length = 0; // Apparently this will empty out the builder (keeps memory allocation though)

			// TODO: Decide whether frame count is actually useful to log
			s_stringBuilder.AppendFormat("[{0}] {1}::{2} | ", UnityEngine.Time.frameCount, callerInfo.ClassName, callerInfo.MethodName);
			s_stringBuilder.AppendFormat(log, parameters);
			return s_stringBuilder.ToString();
		}

		public static void Log(string log, params object[] parameters)
		{
			UnityEngine.Debug.Log(ConstructLog(log, parameters));
		}

		public static void Warning(string log, params object[] parameters)
		{
			UnityEngine.Debug.LogWarning(ConstructLog(log, parameters));
		}

		public static void Error(string log, params object[] parameters)
		{
			UnityEngine.Debug.LogError(ConstructLog(log, parameters));
		}

		public static void Exception(System.Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}
}