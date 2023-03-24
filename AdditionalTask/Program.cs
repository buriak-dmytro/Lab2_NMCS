using System.Text.RegularExpressions;

namespace AdditionalTask
{
    internal class Program
    {
        static int operationsAmount = 0;

        static int n;

        static double[] alphas;
        static double[] betas;
        static double[] gammas;

        static double[] bs;

        static double[] ls;
        static double[] us;

        static double[] ys;
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

        // fill (l/u) matriсes (calculations)
        static void CalcLU()
        {
            ls = new double[n];
            us = new double[n]; // one element will remain "0" and its ok

            ls[0] = alphas[0]; // =
            us[0] = gammas[0] / ls[0]; // /

            operationsAmount += 2;

            for (int i = 1; i < n - 1; i++)
            {
                ls[i] = alphas[i] - betas[i] * us[i - 1]; // - *
                us[i] = gammas[i] / ls[i]; // /

                operationsAmount += 3;
            }

            ls[n - 1] = alphas[n - 1] - betas[n - 1] * us[n - 2]; // - *

            operationsAmount += 2;
        }

        // fill (y) matrix (calculation)
        static void CalcY()
        {
            ys = new double[n];

            ys[0] = bs[0] / ls[0]; // /

            operationsAmount += 1;

            for (int i = 1; i < n; i++)
            {
                ys[i] = (bs[i] - betas[i] * ys[i - 1]) / ls[i]; // - * /

                operationsAmount += 3;
            }
        }

        // fill (x) matrix (calculation)
        static void CalcX()
        {
            xs = new double[n];

            xs[n - 1] = ys[n - 1]; // =

            operationsAmount += 1;

            for (int i = n - 2; i > -1; i--)
            {
                xs[i] = ys[i] - us[i] * xs[i + 1]; // - *

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

            CalcLU();

            CalcY();

            CalcX();

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
