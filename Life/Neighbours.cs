using System;
using System.Collections.Generic;
using System.Text;

namespace Life
{
    class Neighbours
    {
    
        private int neighbours = 0;

        public int CountNeighbours(int[,] universe, int i, int j, string type, int order, bool periodic, bool CentreCount)
        {
            int rows = universe.GetLength(0);
            int columns = universe.GetLength(1);

            if(type == "moore")
            {
                neighbours = MooreNeighbours(periodic, universe, i, j, order, CentreCount, rows, columns);
            }
            else
            {
                neighbours = VonNeighbours(periodic, universe, i, j, order, CentreCount, rows, columns);
            }

            return neighbours;
        }

        private int MooreNeighbours(bool periodic, int[,] universe, int i, int j, int order, bool CentreCount, int rows, int columns)
        {
            if (!periodic)
            {
                for (int r = i - order; r <= i + order; r++)
                {
                    for (int c = j - order; c <= j + order; c++)
                    {
                        if (CentreCount)
                        {
                            if (r >= 0 && r < rows && c >= 0 && c < columns)
                            {
                                neighbours += universe[r, c];
                            }
                        }
                        else
                        {
                            if ((r != i || c != j) && r >= 0 && r < rows && c >= 0 && c < columns)
                            {
                                neighbours += universe[r, c];
                            }
                        }
                    }
                }
            }
            else
            {
                for (int r = i - order; r <= i + order; r++)
                {
                    for (int c = j - order; c <= j + order; c++)
                    {
                        if (CentreCount)
                        {
                            neighbours += universe[Modulus(r, rows), Modulus(c, columns)];
                        }
                        else
                        {
                            if (r != i || c != j)
                            {
                                neighbours += universe[Modulus(r, rows), Modulus(c, columns)];
                            }
                        }

                    }
                }
            }

            return neighbours;
        }

        private int VonNeighbours(bool periodic, int[,] universe, int i, int j, int order, bool CentreCount, int rows, int columns)
        {
            int vonNeumann_formula;

            if (!periodic)
            {
                for (int r = i - order; r <= i + order; r++)
                {
                    for (int c = j - order; c <= j + order; c++)
                    {
                        vonNeumann_formula = vonFormula(r,i,c,j);

                        if (vonNeumann_formula <= order)
                        {
                            if (CentreCount)
                            {
                                if (r >= 0 && r < rows && c >= 0 && c < columns)
                                {
                                    neighbours += universe[r, c];
                                }
                            }
                            else
                            {
                                if ((r != i || c != j) && r >= 0 && r < rows && c >= 0 && c < columns)
                                {
                                    neighbours += universe[r, c];
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int r = i - order; r <= i + order; r++)
                {
                    for (int c = j - order; c <= j + order; c++)
                    {
                        vonNeumann_formula = vonFormula(r, i, c, j);

                        if (vonNeumann_formula <= order)
                        {
                            if (CentreCount)
                            {
                                neighbours += universe[Modulus(r, rows), Modulus(c, columns)];
                            }
                            else
                            {
                                if (r != i || c != j)
                                {
                                    neighbours += universe[Modulus(r, rows), Modulus(c, columns)];
                                }
                            }
                        }
                    }
                }
            }
            return neighbours;
        }

        // "Borrowed" from: https://stackoverflow.com/questions/1082917/mod-of-negative-number-is-melting-my-brain
        private static int Modulus(int x, int m)
        {
            return (x % m + m) % m;
        }

        private static int vonFormula(int r, int i, int c, int j)
        {
            return Math.Abs(r - i) + Math.Abs(c - j);
        }
    }
}
