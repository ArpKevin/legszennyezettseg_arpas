namespace legszennyezettseg_arpas
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var legszennyezesek = new Dictionary<int, List<int>>();

            //foreach (var sor in File.ReadAllLines(@"..\..\..\src\SO2.txt"))
            //{
            //    legszennyezesek.Add(sor.Select((e, i) => i, sor.Split(";").ToList()));
            //}

            var lines = File.ReadAllLines(@"..\..\..\src\SO2.txt");

            for (int i = 0; i < lines.Length; i++)
            {
                var values = lines[i].Split(';').Select(int.Parse).ToList();
                legszennyezesek.Add(i + 1, values);
            }


            Console.ReadKey();
        }
    }
}
