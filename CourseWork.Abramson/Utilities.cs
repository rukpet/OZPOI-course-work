using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseWork.Abramson
{
	public static class Utilities
	{
		internal static void CopyBits(byte[] source, Segment sourceSegment, byte[] target, int startBit)
		{
			if (source.Length * 8 - sourceSegment.Index < sourceSegment.Length)
				throw new ArgumentOutOfRangeException(nameof(sourceSegment.Index));
			if (target.Length * 8 - startBit < sourceSegment.Length)
				throw new ArgumentOutOfRangeException(nameof(startBit));

			for (int i = 0; i < sourceSegment.Length; i++)
			{
				SetBit(target, GetBit(source, sourceSegment.Index + i), startBit + i);
			}
		}

		internal static bool GetBit(this byte[] array, int index)
		{
			return (array[index / 8] & (1 << (index % 8))) != 0;
		}

		internal static void SetBit(this byte[] array, bool value, int index)
		{
			unchecked
			{
				if (value)
					array[index / 8] |= (byte)(1 << (index % 8));
				else
					array[index / 8] &= (byte)~(1 << (index % 8));
			}
		}

		internal static int GetBytesLenght(int lenghtBit)
		{
			return GetLenght(lenghtBit, 8);
		}

		internal static int GetLenght(int lenghtBit, int bitsPerUnit)
		{
			return lenghtBit > 0 ? (((lenghtBit - 1) / bitsPerUnit) + 1) : 0;
		}

		internal static IEnumerable<bool> ToBitEnumerable(this byte[] array, int length)
		{
			for (int i = 0; i < length; i++)
				yield return array.GetBit(i);
		}

		public static string BitsToBinaryString(byte[] array, int count)
		{
			var binarySB = array.ToBitEnumerable(count)
								.Aggregate(seed: new StringBuilder(),
										   func: (sb, b) => sb.Append(b ? 1 : 0));

			int numberSpaces = (binarySB.Length - 1) / 8;

			for (int i = numberSpaces; i > 0; i--)
				binarySB.Insert(i * 8, '-');

			return binarySB.ToString();
		}

		public static byte[] BinaryStringToBytes(string s)
		{
			string[] binStrings = s.Split('-');
			byte[] bytes = new byte[binStrings.Length];
			for (int i = 0; i < binStrings.Length; i++)
				bytes[i] = Convert.ToByte(new string(binStrings[i].Reverse().ToArray()), 2);

			return bytes;
		}
	}
}
