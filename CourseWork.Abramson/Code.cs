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
		static public byte[] Encode(byte[] source)
		{

			return new byte[0];
		}

		static public byte[] Decode(byte[] source)
		{
			// довжина кодової комбінації Абрамсона 2^h - 1 = 15,
			// де h -- степінь неприводимого полінома
			const int countXor = 5;
			const int lenRemnant = 5;
			const int lenInfBits = 10; // inf -- information
			const int indexRemainder = lenInfBits - lenRemnant;
			const int lenCombinaton = lenInfBits + lenRemnant;

			int countBits = source.Length * 8;
			int count = countBits % lenInfBits != 0 ? countBits / lenInfBits + 1 : countBits / lenInfBits;

			int padding = count * lenInfBits;
			byte[] paddedSource = PadBits(source, padding);

			int countBytes = source.Length * 2 - source.Length / 8;
			byte[] result = new byte[countBytes];

			for (int i = 0; i < count; i++)
			{
				var infBits = new BitArray(paddedSource, new BitArray.Segment(i * lenInfBits, lenInfBits));

				infBits.CopyTo(result, i * lenCombinaton);

				for (int j = 0; j < countXor; j++)
					if (infBits[j])
						infBits = infBits.Xor(j, Polinom);

				infBits.CopyRangeTo(new BitArray.Segment(indexRemainder, lenRemnant), result, (i + 1) * lenCombinaton - lenRemnant);
			}

			return result;
		}

		private static byte[] PadBits(byte[] source, int padding)
		{
			if (source.Length >= padding)
				return source;

			byte[] paddedSource = new byte[BitArray.GetBytesLenght(padding)];
			source.CopyTo(paddedSource, 0);
			return paddedSource;
		}
	}
}
