namespace CourseWork.Abramson
{
	public static class Code
	{
		// утворюючий поліном для
		// неприводимого полінома x^4 + x^2 + 1
		// ( 1 + x ) • ( x^4 + x^2 + 1 ) =
		// = x^4 + x^2 + 1 + x^5 + x^3 + x =
		// = x^5 + x^4 + x^3 + x^2 + x^1 + 1
		// шуканий поліном -- 111111
		static public readonly BitArray Polinom = new BitArray(new byte[] { 63 }, 6);

		// довжина кодової комбінації Абрамсона 2^h - 1 = 15,
		// де h -- степінь неприводимого полінома
		const int lenCombinaton = 15;
		const int lenInfBits = 10; // inf -- information
		const int lenRemnant = lenCombinaton - lenInfBits;
		const int numberXors = lenCombinaton - lenRemnant;

		static public byte[] Encode(byte[] source)
		{
			//const int numberXors = lenInfBits - lenRemnant;
			//const int indexRemnant = lenInfBits - lenRemnant;

			int numberCombinatons = BitArray.GetLenght(source.Length * 8, lenInfBits);
			int padding = numberCombinatons * lenInfBits;
			byte[] paddedSource = PadBits(source, padding);

			int numberBytes = BitArray.GetLenght(numberCombinatons * lenCombinaton, 8);
			byte[] result = new byte[numberBytes];

			for (int i = 0; i < numberCombinatons; i++)
			{
				var infBits = new BitArray(paddedSource, new BitArray.Segment(i * lenInfBits, lenInfBits), lenCombinaton);

				var remnant = GetRemnant(infBits, numberXors, lenInfBits);
				infBits = infBits.Xor(lenInfBits, remnant);

				infBits.CopyTo(result, i * lenCombinaton);
			}

			return result;
		}

		private static byte[] PadBits(byte[] source, int padding)
		{
			if (source.Length * 8 >= padding)
				return source;

			byte[] paddedSource = new byte[BitArray.GetLenght(padding, 8)];
			source.CopyTo(paddedSource, 0);
			return paddedSource;
		}

		private static BitArray GetRemnant(BitArray bits, int numberXors, int indexRemnant)
		{
			for (int j = 0; j < numberXors; j++)
				if (bits[j])
					bits = bits.Xor(j, Polinom);

			return bits[new BitArray.Segment(indexRemnant, lenRemnant)];
		}

		static public byte[] Decode(byte[] source)
		{
			const int maxWeight = 2;

			int numberCombinatons = source.Length * 8 / lenCombinaton;
			int countBytes = BitArray.GetLenght(numberCombinatons * lenInfBits, 8);

			byte[] result = new byte[countBytes];

			for (int i = 0; i < numberCombinatons; i++)
			{
				var fixedCombination = new BitArray(source, new BitArray.Segment(i * lenCombinaton, lenCombinaton));

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
	}
}
