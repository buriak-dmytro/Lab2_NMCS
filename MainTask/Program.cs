using System.Text.RegularExpressions;

namespace MainTask
{
    internal class Program
    {
        static int operationsAmount = 0;

        static int n;

        static double[] alphas;
        static double[] betas;
        static double[] gammas;

        static double[] bs;

        static double[] ws;
        static double[] vs;

        static double[] xs;

        // get "a" and "b" matrices and fill (alpha/beta/gamma) matriсes
        static void ProcessInput()
        {
            Console.WriteLine("Enter \"a\" matrix:");

            string[] rowStr = Regex.Split(Console.ReadLine(), @"\s+");
            n = rowStr.Length;

            alphas = new double[n];
            betas = new double[n]; // one element will remain "0" and its ok
            gammas = new double[n]; // one element will remain "0" and its ok

            alphas[0] = double.Parse(rowStr[0].Trim());
            gammas[0] = double.Parse(rowStr[1].Trim());

            for (int i = 1; i < n - 1; i++)
            {
                rowStr = Regex.Split(Console.ReadLine(), @"\s+");

                gammas[i] = double.Parse(rowStr[i + 1].Trim());
                betas[i] = double.Parse(rowStr[i - 1].Trim());
                alphas[i] = double.Parse(rowStr[i].Trim());
            }

            rowStr = Regex.Split(Console.ReadLine(), @"\s+");

            betas[n - 1] = double.Parse(rowStr[n - 2].Trim());
            alphas[n - 1] = double.Parse(rowStr[n - 1].Trim());

            Console.WriteLine();

            Console.WriteLine("Enter \"b\" matrix:");

            bs = new double[n];

            for (int i = 0; i < n; i++)
            {
                bs[i] = double.Parse(Console.ReadLine().Trim());
            }
        }

        // fill (w/v) matriсes (calculations)
        static void StraightCourse()
        {
            ws = new double[n]; // one element will remain "0" and its ok
            vs = new double[n];

            ws[0] = - (gammas[0] / alphas[0]); // - /
            vs[0] = bs[0] / alphas[0]; // /

            operationsAmount += 3;

            for (int i = 1; i < n - 1; i++)
            {
                ws[i] = - (gammas[i] / (betas[i] * ws[i - 1] + alphas[i])); // - / * +
                vs[i] = (bs[i] - betas[i] * vs[i - 1]) / (betas[i] * ws[i - 1] + alphas[i]); // - * / * +

                operationsAmount += 9;
            }

            vs[n - 1] = (bs[n - 1] - betas[n - 1] * vs[n - 2]) / (betas[n - 1] * ws[n - 2] + alphas[n - 1]); // - * / * +

            operationsAmount += 5;
        }

        // fill (x) matrix (calculation)
        static void ReverseCourse()
        {
            xs = new double[n];

            xs[n - 1] = vs[n - 1];

            for (int i = n - 2; i > -1; i--)
            {
                xs[i] = ws[i] * xs[i + 1] + vs[i]; // * +

                operationsAmount += 2;
            }
        }

        // output roots
        static void OutputRoots()
        {
            Console.WriteLine("Roots:");

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"{xs[i]:0.#####}");
            }
        }

        // check roots
        static void CheckRoots()
        {
            Console.WriteLine("Checking roots:");

            Console.WriteLine(
                $"b[{0}] - x[{0}]*alpha[{0}] - x[{1}]*gamma[{0}] = " +
                $"{(bs[0] - xs[0] * alphas[0] - xs[1] * gammas[0]):0.#####}");

            for (int i = 1; i < n - 1; i++)
            {
                Console.WriteLine(
                    $"b[{i}] - x[{i - 1}]*beta[{i}] - x[{i}]*alpha[{i}] - x[{i + 1}]*gamma[{i}] = " +
                    $"{(bs[i] - xs[i - 1] * betas[i] - xs[i] * alphas[i] - xs[i + 1] * gammas[i]):0.#####}");
            }

            Console.WriteLine(
                $"b[{n - 1}] - x[{n - 2}]*beta[{n - 1}] - x[{n - 1}]*alpha[{n - 1}] = " +
                $"{(bs[n - 1] - xs[n - 2] * betas[n - 1] - xs[n - 1] * alphas[n - 1]):0.#####}");
        }

        static void Main(string[] args)
        {
            ProcessInput();
            Console.WriteLine();

            StraightCourse();

            ReverseCourse();

            OutputRoots();
            Console.WriteLine();

            CheckRoots();
            Console.WriteLine();

            Console.WriteLine($"Operations executed: {operationsAmount}");
            Console.WriteLine();

            Console.ReadLine();
        }
    }
}
