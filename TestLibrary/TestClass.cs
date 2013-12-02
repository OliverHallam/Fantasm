namespace TestLibrary
{
    public class TestClass
    {
        public long Factorial(int x)
        {
            if (x == 1) return 1;

            return x * this.Factorial(x - 1);
        }
    }
}
