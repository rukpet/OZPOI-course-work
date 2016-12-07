using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static CourseWork.Abramson.Utilities;

namespace CourseWork.Abramson
{
	[DebuggerDisplay("Length = {Length}, HexValue = {ToString(\"h\")}, BinValue = {ToString(\"b\")}")]
	internal class BitArray : IEnumerable<bool>
	{
		private byte[] _array;
		private int _length;

		public bool this[int index]
		{
			get
			{
				if (index < 0 || index >= _length)
					throw new ArgumentOutOfRangeException(nameof(index));

				return _array.GetBit(index);
			}
			set
			{
				if (index < 0 || index >= _length)
					throw new ArgumentOutOfRangeException(nameof(index));

				_array.SetBit(value, index);
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
			: this(array, Segment.During(length)) { }

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

		public void CopyTo(byte[] target, int startBit)
		{
			CopyBits(_array, Segment.During(_length), target, startBit);
		}

		public void CopyTo(int length, byte[] target, int startBit)
		{
			CopyBits(_array, Segment.During(length), target, startBit);
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

			CopyBits(this._array, Segment.From(relativeShift).For(lenFirstCopy), result._array, 0);
			CopyBits(this._array, Segment.During(relativeShift), result._array, lenFirstCopy);

			return result;
		}

		public IEnumerator<bool> GetEnumerator()
		{
			return _array.ToBitEnumerable(_length).GetEnumerator();
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
					return BitConverter.ToString(_array);
				case "b":
					return BitsToBinaryString(_array, _length);
				default:
					throw new ArgumentException(nameof(param));
			}
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

			var result = new BitArray(_array, _length);

			for (int i = 0; i < lenXoring; i++)
				result[i + firstStart] = this[i + firstStart] ^ second[i + secondStart];

			return result;
		}
	}
}

