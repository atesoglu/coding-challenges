// using System;
// using System.Collections.Generic;
// using System.Globalization;
// using System.IO;
// using System.Linq;
// using System.Text.RegularExpressions;
// using MathNet.Numerics.LinearAlgebra;
//
// class Program
// {
//     // Helper: extract submatrix by arbitrary row and column index lists
//     static Matrix<double> SubMatrixByIndices(Matrix<double> M, int[] rowIndices, int[] colIndices)
//     {
//         var S = Matrix<double>.Build.Dense(rowIndices.Length, colIndices.Length);
//         for (int i = 0; i < rowIndices.Length; i++)
//             for (int j = 0; j < colIndices.Length; j++)
//                 S[i, j] = M[rowIndices[i], colIndices[j]];
//         return S;
//     }
//
//     // ---------------------------------------------------------------------
//     // Parse input
//     // ---------------------------------------------------------------------
//     static (List<int> pattern, List<List<int>> indexLists, List<int> b) ParseInput(string text)
//     {
//         // 1) pattern: [.#...]
//         var pattern = new List<int>();
//         var m = Regex.Match(text, @"\[(.*?)\]");
//         if (m.Success)
//         {
//             foreach (char ch in m.Groups[1].Value)
//             {
//                 if (ch == '.') pattern.Add(0);
//                 else if (ch == '#') pattern.Add(1);
//             }
//         }
//
//         // 2) index lists in (... )
//         var indexLists = new List<List<int>>();
//         foreach (Match g in Regex.Matches(text, @"\((.*?)\)"))
//         {
//             var inner = g.Groups[1].Value.Trim();
//             if (inner == "")
//             {
//                 indexLists.Add(new List<int>());
//             }
//             else
//             {
//                 indexLists.Add(
//                     inner.Split(',')
//                          .Select(x => int.Parse(x.Trim()))
//                          .ToList());
//             }
//         }
//
//         // 3) b vector { ... }
//         var bList = new List<int>();
//         m = Regex.Match(text, @"\{(.*?)\}");
//         if (m.Success)
//         {
//             var inner = m.Groups[1].Value;
//             foreach (var x in inner.Split(','))
//             {
//                 var t = x.Trim();
//                 if (t != "") bList.Add(int.Parse(t));
//             }
//         }
//
//         return (pattern, indexLists, bList);
//     }
//
//     // ---------------------------------------------------------------------
//     // Create M from index lists (each list is a column vector)
//     // ---------------------------------------------------------------------
//     static Matrix<double> IndicesToBinaryColumnMatrix(List<List<int>> indexLists, int dim)
//     {
//         int cols = indexLists.Count;
//         var M = Matrix<double>.Build.Dense(dim, cols, 0.0);
//
//         for (int c = 0; c < cols; c++)
//         {
//             foreach (int r in indexLists[c])
//                 M[r, c] = 1.0;
//         }
//
//         return M;
//     }
//
//     // ---------------------------------------------------------------------
//     // Rank-increasing greedy column selector
//     // ---------------------------------------------------------------------
//     static List<int> SelectIndependentColumns(Matrix<double> M, double tol = 1e-10)
//     {
//         int rows = M.RowCount;
//         int cols = M.ColumnCount;
//
//         var independent = new List<int>();
//
//         for (int j = 0; j < cols; j++)
//         {
//             var candidateIndices = independent.Concat(new[] { j }).ToArray();
//             var allRows = Enumerable.Range(0, rows).ToArray();
//             var sub = SubMatrixByIndices(M, allRows, candidateIndices);
//
//             var svd = sub.Svd(true);
//             int r = svd.Rank; // effective numerical rank
//
//             if (r > independent.Count)
//             {
//                 independent.Add(j);
//                 if (independent.Count == rows)
//                     break;
//             }
//         }
//         return independent;
//     }
//
//     // ---------------------------------------------------------------------
//     // Rank-increasing greedy row selector
//     // ---------------------------------------------------------------------
//     static List<int> SelectIndependentRows(Matrix<double> M, double tol = 1e-10)
//     {
//         int rows = M.RowCount;
//         int cols = M.ColumnCount;
//
//         var independent = new List<int>();
//
//         for (int i = 0; i < rows; i++)
//         {
//             var candidateIndices = independent.Concat(new[] { i }).ToArray();
//             var allCols = Enumerable.Range(0, cols).ToArray();
//             var sub = SubMatrixByIndices(M, candidateIndices, allCols);
//
//             var svd = sub.Svd(true);
//             int r = svd.Rank;
//
//             if (r > independent.Count)
//             {
//                 independent.Add(i);
//                 if (independent.Count == cols)
//                     break;
//             }
//         }
//
//         return independent;
//     }
//
//     // ---------------------------------------------------------------------
//     // Make square A and reduced b plus remaining columns Y
//     // ---------------------------------------------------------------------
//     static (Matrix<double> A, Vector<double> bNew, List<Vector<double>> Y)
//         MakeSquareAandY(Matrix<double> M, Vector<double> b, double tol = 1e-10)
//     {
//         int rows = M.RowCount;
//         int cols = M.ColumnCount;
//
//         var indepCols = SelectIndependentColumns(M, tol);
//         int r = indepCols.Count;
//
//         if (r == 0)
//             throw new Exception("Matrix rank is 0: no independent columns.");
//
//         var allRows = Enumerable.Range(0, rows).ToArray();
//         var indepColsArr = indepCols.ToArray();
//         var Mcol = SubMatrixByIndices(M, allRows, indepColsArr);
//
//         var indepRows = SelectIndependentRows(Mcol, tol);
//         int r2 = indepRows.Count;
//         if (r2 < r)
//         {
//             r = r2;
//             indepCols = indepCols.Take(r).ToList();
//             indepColsArr = indepCols.ToArray();
//             Mcol = SubMatrixByIndices(M, allRows, indepColsArr);
//             indepRows = SelectIndependentRows(Mcol, tol);
//         }
//
//         var A = SubMatrixByIndices(M, indepRows.ToArray(), indepCols.ToArray());
//         var bNew = Vector<double>.Build.Dense(indepRows.Count, i => b[indepRows[i]]);
//
//         var Y = new List<Vector<double>>();
//         var indepSet = new HashSet<int>(indepCols);
//
//         for (int j = 0; j < cols; j++)
//         {
//             if (!indepSet.Contains(j))
//             {
//                 // Build a vector of the entries at indepRows for column j
//                 var col = Vector<double>.Build.Dense(indepRows.Count, i => M[indepRows[i], j]);
//                 Y.Add(col);
//             }
//         }
//
//         return (A, bNew, Y);
//     }
//
//     // ---------------------------------------------------------------------
//     // Generate all n-dimensional nonnegative integer vectors summing to k
//     // ---------------------------------------------------------------------
//     static IEnumerable<int[]> NonnegIntVectorsSumTo(int n, int k)
//     {
//         if (n == 1)
//         {
//             yield return new[] { k };
//             yield break;
//         }
//
//         for (int i = 0; i <= k; i++)
//         {
//             foreach (var rest in NonnegIntVectorsSumTo(n - 1, k - i))
//             {
//                 var v = new int[n];
//                 v[0] = i;
//                 Array.Copy(rest, 0, v, 1, n - 1);
//                 yield return v;
//             }
//         }
//     }
//
//     // ---------------------------------------------------------------------
//     // The infinite generator: all nonneg integer vectors of any total sum
//     // ---------------------------------------------------------------------
//     static IEnumerable<int[]> Combinations(int n)
//     {
//         if (n == 0)
//         {
//             yield return null;
//             yield break;
//         }
//
//         int k = 0;
//         while (true)
//         {
//             foreach (var v in NonnegIntVectorsSumTo(n, k))
//                 yield return v;
//             k++;
//         }
//     }
//
//     // ---------------------------------------------------------------------
//     // MAIN
//     // ---------------------------------------------------------------------
//     static void Main()
//     {
//         double res = 0;
//         string path = "input.txt";
//
//         int lineNo = 0;
//         foreach (var rawLine in File.ReadLines(path))
//         {
//             lineNo++;
//             string line = rawLine.Trim();
//             if (line == "") continue;
//
//             Console.WriteLine($"\n===== Line {lineNo} =====");
//
//             var (pattern, indexLists, bList) = ParseInput(line);
//
//             if (pattern == null || bList == null)
//             {
//                 Console.WriteLine("ERROR: Missing pattern or b vector");
//                 continue;
//             }
//
//             int dim = pattern.Count;
//             var M = IndicesToBinaryColumnMatrix(indexLists, dim);
//             var b = Vector<double>.Build.DenseOfEnumerable(bList.Select(x => (double)x));
//
//             var (A, bSquare, Y) = MakeSquareAandY(M, b);
//
//             Console.WriteLine($"  M shape: {M.RowCount} x {M.ColumnCount}");
//             Console.WriteLine($"  A shape: {A.RowCount} x {A.ColumnCount}");
//             Console.WriteLine($"  b_square shape: {bSquare.Count}");
//             Console.WriteLine($"  Y count: {Y.Count}");
//
//             double best = double.PositiveInfinity;
//             double tol = 1e-9;
//
//             int index = 0;
//             foreach (var iVec in Combinations(Y.Count))
//             {
//                 index++;
//
//                 Vector<double> q;
//
//                 if (iVec == null)
//                 {
//                     q = bSquare.Clone();
//                 }
//                 else
//                 {
//                     var iVector = Vector<double>.Build.DenseOfEnumerable(iVec.Select(x => (double)x));
//                     // Build Y matrix (each Y[j] is a column)
//                     Matrix<double> Ymat = Matrix<double>.Build.Dense(Y[0].Count, Y.Count);
//                     for (int jc = 0; jc < Y.Count; jc++)
//                         for (int ri = 0; ri < Y[jc].Count; ri++)
//                             Ymat[ri, jc] = Y[jc][ri];
//
//                     var i_y = Ymat * iVector;
//                     q = bSquare - i_y;
//                 }
//
//                 Vector<double> x;
//                 try
//                 {
//                     x = A.Solve(q); // solves A*x = q
//                 }
//                 catch
//                 {
//                     // singular / cannot solve, skip
//                     continue;
//                 }
//
//                 bool nonNegative = x.Enumerate().All(v => v >= -tol);
//                 bool integers = x.Enumerate().All(v => Math.Abs(v - Math.Round(v)) < tol);
//
//                 if (nonNegative && integers)
//                 {
//                     double s = x.Sum();
//                     if (iVec != null) s += iVec.Sum();
//
//                     if (s < best)
//                     {
//                         best = s;
//                         Console.WriteLine($"  x = [{string.Join(",", x)}], i = {FormatIntArray(iVec)}, sum = {s}, res = {res}");
//                     }
//                 }
//
//                 if (iVec == null || (iVec.Sum() >= best))
//                     break;
//             }
//
//             res += best;
//         }
//
//         Console.WriteLine((int)res);
//     }
//
//     static string FormatIntArray(int[] arr)
//     {
//         if (arr == null) return "null";
//         return "[" + string.Join(",", arr) + "]";
//     }
// }
