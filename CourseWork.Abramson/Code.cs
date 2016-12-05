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

		static public byte[] Encode(byte[] source)
		{

			int numberCombinatons = BitArray.GetLenght(source.Length * 8, lenInfBits);
			int padding = numberCombinatons * lenInfBits;
			byte[] paddedSource = PadBits(source, padding);

			int countBytes = BitArray.GetLenght(numberCombinatons * lenCombinaton, 8);
			byte[] result = new byte[countBytes];

			for (int i = 0; i < numberCombinatons; i++)
			{
				var infBits = new BitArray(paddedSource, new BitArray.Segment(i * lenInfBits, lenInfBits));
				infBits.CopyTo(result, i * lenCombinaton);

				var remnant = GetRemnant(infBits);
				remnant.CopyTo(result, (i + 1) * lenCombinaton - lenRemnant);
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

		private static BitArray GetRemnant(BitArray infBits)
		{
			const int countXor = lenInfBits - lenRemnant;
			const int indexRemnant = lenInfBits - lenRemnant;

			for (int j = 0; j < countXor; j++)
				if (infBits[j])
					infBits = infBits.Xor(j, Polinom);

			return infBits[new BitArray.Segment(indexRemnant, lenRemnant)];
		}

		static public byte[] Decode(byte[] source)
		{
			int numberCombinatons = source.Length * 8 / 15;
			//int totalInfBits = numberCombinaton * 10;
			int countBytes = BitArray.GetLenght(numberCombinatons * lenInfBits, 8);

			byte[] result = new byte[countBytes];

			//for (int i = 0; i < numberCombinatons; i++)
			//{
			//	var combination = new BitArray(source, new BitArray.Segment(i * lenInfBits, lenInfBits));

			//	infBits.CopyTo(result, i * lenCombinaton);

			//	for (int j = 0; j < countXor; j++)
			//		if (infBits[j])
			//			infBits = infBits.Xor(j, Polinom);

			//	infBits.CopyRangeTo(new BitArray.Segment(indexRemainder, lenRemnant), result, (i + 1) * lenCombinaton - lenRemnant);
			//}

			return new byte[0];
		}
	}
}
