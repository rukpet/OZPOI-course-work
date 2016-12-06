using System;
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

		public BitArray this[Segment segment]
		{
			get
			{
				if (segment.Index < 0 || segment.Index >= _length)
					throw new ArgumentOutOfRangeException(nameof(segment.Index));
				if (_length - segment.Index < segment.Length)
					throw new ArgumentOutOfRangeException(nameof(segment.Length));

				return new BitArray(_array, segment);
			}
		}

		public int Length => _length;

		public int Weight => this.Count(b => b);

		public bool IsPosBitNearby => this.Count(b => b) == this.SkipWhile(b => !b).TakeWhile(b => b).Count();

		public BitArray(int lenght)
		{
			_length = lenght;
			_array = new byte[GetBytesLenght(lenght)];
		}

		public BitArray(byte[] array, int length)
			: this(array, new Segment(0, length)) { }

		public BitArray(byte[] array, Segment segment)
			: this(array, segment, segment.Length) { }

		public BitArray(byte[] array, Segment segment, int length)
		{
			if (array.Length * 8 - segment.Index < segment.Length && segment.Length < length)
				throw new ArgumentOutOfRangeException(nameof(segment.Length));

			_length = length;
			_array = new byte[GetBytesLenght(length)];

			CopyBits(array, segment, _array, 0);
		}

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

		public void CopyTo(int length, byte[] target, int startBit)
		{
			CopyBits(_array, new Segment(0, length), target, startBit);
		}

		public void CopyRangeTo(Segment segment, byte[] target, int startBit)
		{
			CopyBits(_array, segment, target, startBit);
		}

		public BitArray CyclicShift(int shift)
		{
			var result = new BitArray(this._length);
			int relativeShift = shift % this._length;

			if (relativeShift < 0)
				relativeShift = this._length + relativeShift;

			int lenFirstCopy = this._length - relativeShift;

			CopyBits(this._array, new Segment(relativeShift, lenFirstCopy), result._array, 0);
			CopyBits(this._array, new Segment(0, relativeShift), result._array, lenFirstCopy);

			return result;
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
			if (target.Length * 8 - startBit < sourceSegment.Length)
				throw new ArgumentOutOfRangeException(nameof(startBit));

			for (int i = 0; i < sourceSegment.Length; i++)
			{
				SetBit(target, GetBit(source, sourceSegment.Index + i), startBit + i);
			}
		}
		private static int GetBytesLenght(int lenghtBit)
		{
			return GetLenght(lenghtBit, 8);
		}

		public static int GetLenght(int lenghtBit, int bitsPerUnit)
		{
			return lenghtBit > 0 ? (((lenghtBit - 1) / bitsPerUnit) + 1) : 0;
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
					return new string(BitConverter.ToString(_array).ToArray());
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

