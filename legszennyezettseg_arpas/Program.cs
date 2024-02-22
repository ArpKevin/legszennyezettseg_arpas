namespace legszennyezettseg_arpas
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var legszennyezesek = File.ReadAllLines(@"..\..\..\src\SO2.txt")
                .Select((val, index) => new { Index = index + 1, Value = val.Split(";").Select(int.Parse).ToList() })
                .ToDictionary(k => k.Index, v => v.Value);

            Console.WriteLine("\n2. feladat:");

            legszennyezesek.ToList().ForEach(e => Console.WriteLine($"{e.Key} - {string.Join(", ", e.Value)}"));

            Console.WriteLine("\n3. feladat:");
            EgeszsegugyiHatarertekFeletti(legszennyezesek);
            Console.WriteLine("A fájlbeírás megtörtént.");

            Console.WriteLine("\n4. feladat:");
            LegmagasabbSO2Koncentracio(legszennyezesek);

            Console.WriteLine("\n5. feladat:");
            Console.WriteLine($"{SzazAlattiErtekek(legszennyezesek)} óra volt összesen, amikor 100 µg/m3 alá süllyedt az SO2-koncentráció.");

            Console.WriteLine("\n6. feladat:");
            Console.WriteLine($"Az átlagos koncentrációérték {AtlagosKoncentracioErtek(legszennyezesek)} volt.");

            Console.WriteLine("\n7. feladat:");
            var nemMent60Fole = NemMent60Fole(legszennyezesek);

            Console.WriteLine(nemMent60Fole.Key == 0 && nemMent60Fole.Value == null ?
                "Nem volt olyan nap, amikor nem ment 60 fölé az SO2-koncentráció." :
                $"A(z) {nemMent60Fole.Key}. nap volt az első nap, amikor nem ment 60 fölé az SO2-koncentráció.");

            Console.WriteLine("\n8. feladat:");
            KeyValuePair<int, int> marcius1MaxErteke = Marcius1Max(legszennyezesek);

            Console.WriteLine($"Március 1 legmagasabb SO2-koncentráció értéke {marcius1MaxErteke.Value}, ezt a(z) {marcius1MaxErteke.Key}. órában mérték.");

            Console.WriteLine("\n9. feladat:");
            Console.WriteLine($"A déli órákban mért értékek átlaga {DeliAtlag(legszennyezesek)}.");

            Console.WriteLine("\n10. feladat:");
            int melyikFel = MelyikFelebenNagyobb(legszennyezesek);

            Console.WriteLine(melyikFel == 0 ?
                "Egyenlő volt az SO2-koncentráció." :
                $"A hónap {melyikFel}. felében volt nagyobb az SO2-koncentráció.");

            Console.WriteLine("\n11. feladat:");
            Console.WriteLine($"A következő napok voltak szelesek: {string.Join(", ", OtSzelesNap(legszennyezesek))}");


            Console.ReadKey();
        }

        static void EgeszsegugyiHatarertekFeletti(Dictionary<int, List<int>> d) => File.WriteAllLines(@"..\..\..\src\egeszsegtelen.txt", d.Where(e => e.Value.Any(e => e > 250)).Select(e => $"Március {e.Key}").ToList());
        static void LegmagasabbSO2Koncentracio(Dictionary<int, List<int>> d)
        {
            var maxVal = d.Max(e => e.Value.Max());
            var legmagasabbKoncentraciosNap = d.Where(e => e.Value.Contains(maxVal)).Select(e => new { Nap = e.Key, Ora = e.Value.IndexOf(maxVal) + 1 }).First();
            Console.WriteLine($"A(z) {legmagasabbKoncentraciosNap.Nap}. nap {legmagasabbKoncentraciosNap.Ora}. órájában volt a legmagasabb az SO2-koncentráció, ez az érték {maxVal} volt.");
        }
        static int SzazAlattiErtekek(Dictionary<int, List<int>> d) => d.Sum(e => e.Value.Count(e => e < 100));
        static double AtlagosKoncentracioErtek(Dictionary<int, List<int>> d) => d.Average(e => e.Value.Average());
        static KeyValuePair<int, List<int>> NemMent60Fole(Dictionary<int, List<int>> d) => d.FirstOrDefault(e => e.Value.Max() <= 60);
        static KeyValuePair<int,int> Marcius1Max(Dictionary<int, List<int>> d) => d.Where(e => e.Key == 1).Select(e => new KeyValuePair<int,int>(e.Value.IndexOf(e.Value.Max()) + 1, e.Value.Max())).First();

        static double DeliAtlag(Dictionary<int, List<int>> d) => d.Average(e => e.Value[11]);
        static int MelyikFelebenNagyobb(Dictionary<int, List<int>> d)
        {
            int elsoFel = d.Take(15).Sum(e => e.Value.Sum());
            int masodikFel = d.TakeLast(15).Sum(e => e.Value.Sum());

            return elsoFel > masodikFel ? 1 : (elsoFel < masodikFel ? 2 : 0);
        }
        static List<int> OtSzelesNap(Dictionary<int, List<int>> d) => d.OrderBy(e => e.Value.Sum()).Take(5).Select(e => e.Key).OrderBy(e => e).ToList();
    }
}