namespace HelloFormatter
{
    public static class SayHello
    {
        public static string To(string name)
        {
            return string.Format("Hello {0}!", name);
        }
    }
}
