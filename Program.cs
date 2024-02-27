using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
namespace lab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; //українська мова в консолі
            int row = 900;
            int col1 = 900;
            int col2 = 500;
            int t = 2;
            int[][] matrix1 = GenerateRandomMatrix(row, col1);
            int[][] matrix2 = GenerateRandomMatrix(row,col2);
            int[][] matrixRes = new int[matrix1.Length][];
            int[][] matrixResThread = new int[matrix1.Length][];
            for (int i = 0; i < matrix1.Length; i++)
            {
                matrixRes[i] = new int[matrix2[0].Length];
                matrixResThread[i] = new int[matrix2[0].Length];
            }
            //просто множення
            var timer = Stopwatch.StartNew();
            for(int i = 0; i < matrix1.Length; i++)
            {
                for(int j = 0; j < matrix2[0].Length; j++)
                {
                    for (int k = 0; k < matrix2.Length; k++)
                    {
                        matrixRes[i][j] += matrix1[i][k]*matrix2[k][j];
                    }
                }
            }
            timer.Stop();
            double timerRes = timer.Elapsed.TotalSeconds;
            //множення в потоках
            var timerThread = Stopwatch.StartNew();
            Thread[] threads = new Thread[t];
            int rowPerThread = row / t;
            for(int i = 0; i < t; i++)
            {
                int indexStart = i * rowPerThread;
                int indexEnd = (i==t-1)?row:indexStart+rowPerThread;
                threads[i] = new Thread (() =>
                {
                    for( int r = indexStart;r< indexEnd;r++)
                    {
                        for(int j = 0;j<col2;j++)
                        {
                            int sum = 0;
                            for(int k = 0;k<col1;k++)
                            {
                                sum+= matrix1[r][k]*matrix2[k][j];
                            }
                            matrixResThread[r][j] = sum;
                        }
                            
                    }
                });
                threads[i].Start();
            }
            foreach(var thread in threads)
            {
                thread.Join();
            }
            timerThread.Stop();
            double timerThreadRes = timerThread.Elapsed.TotalSeconds;
            Console.WriteLine($"час виконання звичайного множення матриць: {timerRes}");
            Console.WriteLine($"час виконання  множення матриць з потоками: {timerThreadRes}");
            bool EqualMatrix = true;
            for (int i = 0; i < matrixRes.Length; i++)
            {
                for(int j = 0; j< matrixRes[0].Length; j++)
                {
                    if (matrixRes[i][j] != matrixResThread[i][j])
                    {
                        EqualMatrix = false;
                        break;
                    }
                }
            }
            if (EqualMatrix)
            {
                Console.WriteLine("обидві матриці рівні, отже операція виконана правильно");
            }

        }
        static int[][] GenerateRandomMatrix(int rows, int cols)
        {
            Random random = new Random();   
            int[][] matrix = new int[rows][];
            for (int i = 0; i < rows; i++)
            {
                matrix[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                {
                    matrix[i][j] = random.Next(500, 1000);
                }
            }
            return matrix;
        }

    }

}