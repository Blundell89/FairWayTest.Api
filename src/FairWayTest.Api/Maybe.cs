namespace FairWayTest.Api
{
    public class Maybe<T>
    {
        private Maybe(bool exists, T value = default(T))
        {
            HasValue = exists;
            Value = value;
        }

        public bool HasValue { get; }

        public T Value { get; set; }

        public static Maybe<T> Some(T value) => new Maybe<T>(true, value);

        public static Maybe<T> None() => new Maybe<T>(false);
    }
}