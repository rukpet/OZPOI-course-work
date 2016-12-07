namespace CourseWork.Abramson
{
	internal struct Segment
	{
		public int Index;
		public int Length;

		public Segment(int index = 0, int length = 0)
		{
			Index = index;
			Length = length;
		}

		public static Segment From(int index) => new Segment(index);

		public static Segment During(int length) => new Segment(length: length);

		public Segment For(int length)
		{
			Length = length;
			return this;
		}

	}
}
