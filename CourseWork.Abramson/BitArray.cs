﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CourseWork
{
	[DebuggerDisplay("Length = {Length}, HexValue = {ToString(\"h\")}, BinValue = {ToString(\"b\")}")]
	public class BitArray : IEnumerable<bool>
	{
		private byte[] _array;
		private int _length;

		public bool this[int index]
		{
			get
			{
				if (index < 0 || index >= _length)
					throw new ArgumentOutOfRangeException(nameof(index));

				return GetBit(_array, index);
			}
			set
			{
				if (index < 0 || index >= _length)
					throw new ArgumentOutOfRangeException(nameof(index));

				SetBit(_array, value, index);
			}
		}

		public int Length => _length;

		public BitArray(int lenght)
		{
			_length = lenght;
			_array = new byte[GetBytesLenght(lenght)];
		}

		public BitArray(byte[] array, int length)
			: this(array, new Segment(0, length)) { }

		public BitArray(byte[] array, Segment segment)
		{
			if (array.Length * 8 - segment.Index < segment.Length)
				throw new ArgumentOutOfRangeException(nameof(segment.Length));

			_array = new byte[GetBytesLenght(segment.Length)];
			_length = segment.Length;

			CopyBits(array, segment, _array, 0);
		}

		//public BitArray(int lenght, byte[] source, Segment segment) : this(lenght)
		//{
		//	CopyBits(source, segment, _array, 0);
		//}

		private static bool GetBit(byte[] array, int index)
		{
			return (array[index / 8] & (1 << (index % 8))) != 0;
		}

		private static void SetBit(byte[] array, bool value, int index)
		{
			unchecked
			{
				if (value)
					array[index / 8] |= (byte)(1 << (index % 8));
				else
					array[index / 8] &= (byte)~(1 << (index % 8));
			}
		}

		public void CopyTo(byte[] target, int startBit)
		{
			CopyBits(_array, new Segment(0, _length), target, startBit);
		}

		public void CopyRangeTo(Segment segment, byte[] target, int startBit)
		{
			new BitArray(_array, segment).CopyTo(target, startBit);
		}

		//private static void CopyBits(byte[] source, int length, byte[] target, int startBit)
		//{
		//	int startBitInByte = startBit % 8;
		//	int copyLen = GetBytesLenght(length);
		//	int startByte = startBitInByte == 0 ? GetBytesLenght(startBit)
		//										: GetBytesLenght(startBit) - 1;
		//	int targetBits = target.Length * 8;

		//	if (targetBits - startBit < length)
		//		throw new ArgumentOutOfRangeException(nameof(startBit));

		//	for (int i = 0; i < copyLen; i++)
		//	{
		//		target[startByte + i] &= (byte)(0xFF >> (8 - startBitInByte));
		//		target[startByte + i] ^= (byte)(source[i] << startBitInByte);
		//	}

		//	if (startBitInByte != 0)
		//		for (int i = 0; startBit + (i + 1) * 8 < targetBits; i++)
		//		{
		//			target[startByte + i + 1] &= (byte)(0xFF << startBitInByte);
		//			target[startByte + i + 1] ^= (byte)(source[i] >> (8 - startBitInByte));
		//		}
		//}

		private static void CopyBits(byte[] source, Segment sourceSegment, byte[] target, int startBit)
		{
			if (source.Length * 8 - sourceSegment.Index < sourceSegment.Length)
				throw new ArgumentOutOfRangeException(nameof(sourceSegment.Index));
			if (target.Length * 8 - startBit < target.Length)
				throw new ArgumentOutOfRangeException(nameof(startBit));

			for (int i = 0; i < sourceSegment.Length; i++)
			{
				SetBit(target, GetBit(source, sourceSegment.Index + i), startBit + i);
			}
		}
		public static int GetBytesLenght(int lenghtBit)
		{
			return lenghtBit % 8 == 0 ? lenghtBit / 8
									  : lenghtBit / 8 + 1;
		}

		public IEnumerator<bool> GetEnumerator()
		{
			for (int i = 0; i < _length; i++)
				yield return this[i];
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public override string ToString()
		{
			return ToString("h");
		}

		public string ToString(string param)
		{
			switch (param.ToLower())
			{
				case "h":
				case "":
				case null:
					return new string(BitConverter.ToString(_array).Reverse().ToArray());
				case "b":
					return this.Reverse().Aggregate(new StringBuilder(), (sb, b) => sb.Append(b ? 1 : 0), sb => sb.ToString());
				default:
					throw new ArgumentException(nameof(param));
			}
		}

		public BitArray Clone()
		{
			return new BitArray(_array, _length);
		}

		public BitArray Xor(int firstStart, BitArray second, int secondStart = 0)
		{
			if (this._length < firstStart)
				throw new ArgumentOutOfRangeException(nameof(firstStart));
			if (second._length < secondStart)
				throw new ArgumentOutOfRangeException(nameof(secondStart));

			int lenXoring = second._length - secondStart;

			if (this._length - firstStart < lenXoring)
				throw new ArithmeticException();

			var result = this.Clone();

			for (int i = 0; i < lenXoring; i++)
				result[i + firstStart] = this[i + firstStart] ^ second[i + secondStart];

			return result;
		}

		public struct Segment
		{
			public int Index;
			public int Length;

			public Segment(int index = 0, int length = 0)
			{
				Index = index;
				Length = length;
			}
		}
	}
}

