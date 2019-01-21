namespace Experiments
{
    public class Evaluated<T>
    {
        public T Individual { get; private set; }
        public double Fitness { get; private set; }

        public static Evaluated<T> Create(T individual, double fitness)
            => new Evaluated<T> { Individual = individual, Fitness = fitness };
    }
}