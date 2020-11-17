namespace CoreServices.ServiceModels
{
	public class KeyVal<TKey, TVal>
	{
		public TKey Key { get; set; }
		public TVal Value { get; set; }

		public KeyVal()
		{
		}

		public KeyVal(TKey key, TVal val)
		{
			Key = key;
			Value = val;
		}
	}
}