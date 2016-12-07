using static CourseWork.Abramson.Utilities;

namespace CourseWork.Abramson
{
	public static class Code
	{
		// утворюючий поліном для
		// неприводимого полінома x^4 + x^2 + 1
		// ( 1 + x ) • ( x^4 + x + 1 ) =
		// = x^4 + x + 1 + x^5 + x^2 + x =
		// = x^5 + x^4 + x^2 + 1
		// шуканий поліном -- 101011
		const int lenPolinom = 6;
		public const string Polinom = "101011";

		static private readonly BitArray _polinom = new BitArray(BinaryStringToBytes(Polinom), lenPolinom);

		// довжина кодової комбінації Абрамсона 2^h - 1 = 15,
		// де h -- степінь неприводимого полінома
		const int lenCombinaton = 15;
		const int lenInfBits = 10;	// inf -- information
		const int lenRemnant = lenCombinaton - lenInfBits;
		const int numberXors = lenCombinaton - lenRemnant;

		static public byte[] Encode(byte[] source, out int numberBits)
		{
			int numberCombinatons = GetLenght(source.Length * 8, lenInfBits);
			int padding = numberCombinatons * lenInfBits;
			byte[] paddedSource = PadBits(source, padding);

			numberBits = numberCombinatons * lenCombinaton;
			int numberBytes = GetBytesLenght(numberBits);
			byte[] result = new byte[numberBytes];

			for (int i = 0; i < numberCombinatons; i++)
			{
				var combination = new BitArray(paddedSource, Segment.From(i * lenInfBits).For(lenInfBits), lenCombinaton);

				var remnant = GetRemnant(combination, numberXors, lenInfBits);
				combination = combination.Xor(lenInfBits, remnant);

				combination.CopyTo(result, i * lenCombinaton);
			}

			return result;
		}

		static public byte[] Decode(byte[] source)
		{
			const int maxWeight = 2;

			int numberCombinatons = source.Length * 8 / lenCombinaton;
			int countBytes = GetBytesLenght(numberCombinatons * lenInfBits);

			byte[] result = new byte[countBytes];

			for (int i = 0; i < numberCombinatons; i++)
			{
				var fixedCombination = new BitArray(source, Segment.From(i * lenCombinaton).For(lenCombinaton));

				for (int shift = 0; shift < lenCombinaton; shift++)
				{
					var erroredCombination = fixedCombination.CyclicShift(shift);
					var remnant = GetRemnant(erroredCombination, numberXors, lenInfBits);
					if (remnant.Weight <= maxWeight && remnant.IsPosBitNearby)
					{
						fixedCombination = erroredCombination.Xor(lenInfBits, remnant).CyclicShift(-shift);
						break;
					}
				}

				fixedCombination.CopyTo(lenInfBits, result, i * lenInfBits);
			}

			return result;
		}

		private static byte[] PadBits(byte[] source, int padding)
		{
			if (source.Length * 8 >= padding)
				return source;

			byte[] paddedSource = new byte[GetBytesLenght(padding)];
			source.CopyTo(paddedSource, 0);
			return paddedSource;
		}

		private static BitArray GetRemnant(BitArray bits, int numberXors, int indexRemnant)
		{
			for (int j = 0; j < numberXors; j++)
				if (bits[j])
					bits = bits.Xor(j, _polinom);

			return bits[Segment.From(indexRemnant).For(lenRemnant)];
		}
	}
}
