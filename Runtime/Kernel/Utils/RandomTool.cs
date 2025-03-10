﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LibFPS.Kernel.Utils
{
	public static class RandomTool
	{
		static Random random = null;
		static RandomTool()
		{
			random = new Random();
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Init()
		{
			random = new Random();
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Init(int Seed)
		{
			random = new Random(Seed);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int NextInt()
		{
			return random.Next();
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int NextInt(int Upper)
		{
			return random.Next(0, Upper);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]

		public static int NextInt(int Lower, int Upper) => random.Next(Lower, Upper);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool NextInt(int Lower, int Upper, out int result)
		{
			if (Lower < Upper)
			{
				result = random.Next(Lower, Upper);
				return true;

			}
			else
			{
				result = -1;
				return false;
			}
		}
		public static T PickOne<T>(this List<T> __list)
		{
			return __list[NextInt(__list.Count)];
		}
	}
}
