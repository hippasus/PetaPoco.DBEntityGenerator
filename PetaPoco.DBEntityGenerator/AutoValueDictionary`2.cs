namespace PetaPoco.DBEntityGenerator
{
    using System.Collections.Generic;

    public class AutoValueDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : new()
    {
        public AutoValueDictionary()
        {
        }

        public AutoValueDictionary(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
        }

        public new TValue this[TKey key]
        {
            get
            {
                TValue v;
                if (!TryGetValue(key, out v))
                {
                    v = new TValue();
                    Add(key, v);
                }

                return v;
            }

            set
            {
                base[key] = value;
            }
        }
    }
}
