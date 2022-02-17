using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Life
{
    class SeedReader
    {
        protected int start_row = 0, end_row = 0, start_column = 0, end_column = 0;

        protected int ellipse_width = 0, ellipse_height = 0;

        public int[,] InitializeFromFile(int rows, int columns, string inputFile)
        {
            int[,] universe = new int[rows, columns];

            using (StreamReader reader = new StreamReader(inputFile))
            {
                string line = reader.ReadLine();

                if (line == "#version=1.0")
                {
                    line = reader.ReadLine();
                    string[] elements = line.Split(" ");

                    universe = ReadFile(line, reader, universe, elements);
                    
                }
                else if (line == "#version=2.0")
                {
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();

                        string[] dimensions = line.Split(": ");
                        string[] coordinates = dimensions[1].Split(", ");

                        if (line.Contains("cell"))
                        {
                            Cell cell = new Cell(); 
                            universe = cell.ReadFile(line, reader, universe , coordinates);                         
                        }
                        else if (line.Contains("rectangle"))
                        {
                            Rectangle rectangle = new Rectangle();
                            universe = rectangle.ReadFile(line, reader, universe, coordinates);

                        }
                        else if (line.Contains("ellipse"))
                        {
                            Ellipse ellipse = new Ellipse();
                            universe = ellipse.ReadFile(line, reader, universe, coordinates);

                        }
                    }
                }
            }
            return universe;
        }

        public virtual int[,] ReadFile(string line, StreamReader reader, int[,] universe, string[] elements)
        {
            while (!reader.EndOfStream)
            {
                int row = int.Parse(elements[0]);
                int column = int.Parse(elements[1]);

                universe[row, column] = 1;
            }
            return universe;
        }
    }

    class Cell : SeedReader
    {
        public override int[,] ReadFile(string line, StreamReader reader, int[,] universe, string[] coordinates)
        {
            int row = int.Parse(coordinates[0]);
            int column = int.Parse(coordinates[1]);

            universe[row, column] = 1;

            return universe;
        }
    }

    class Rectangle : SeedReader
    {

        public override int[,] ReadFile(string line, StreamReader reader, int[,] universe, string[] coordinates)
        {
            start_row = int.Parse(coordinates[0]);
            start_column = int.Parse(coordinates[1]);
            end_row = int.Parse(coordinates[2]);
            end_column = int.Parse(coordinates[3]);

            for (int r = start_row; r <= end_row; r++)
            {
                for (int c = start_column; c <= end_column; c++)
                {
                    if (line.Contains("(o)"))
                    {
                        universe[r, c] = 1;
                    }
                    else
                    {
                        universe[r, c] = 0;
                    }

                }
            }
            return universe;
        }
    }

    class Ellipse : SeedReader
    {
        public override int[,] ReadFile(string line, StreamReader reader, int[,] universe, string[] coordinates)
        {
            start_row = int.Parse(coordinates[0]);
            start_column = int.Parse(coordinates[1]);
            end_row = int.Parse(coordinates[2]);
            end_column = int.Parse(coordinates[3]);

            ellipse_width = end_row - start_row + 1;
            ellipse_height = end_column - start_column + 1;

            double centre_x = ((end_row + start_row) / 2.0);
            double centre_y = ((end_column + start_column) / 2.0);

            for (int r = start_row; r <= end_row; r++)
            {
                for (int c = start_column; c <= end_column; c++)
                {
                    if (formula(r, centre_x, c, centre_y) <= 1)
                    {
                        if (line.Contains("(o)"))
                        {
                            universe[r, c] = 1;
                        }
                        else
                        {
                            universe[r, c] = 0;
                        }
                    }

                }
            }

            return universe;
        }

        private double formula(int r, double centre_x, int c, double centre_y)
        {
            double formula = ((4 * Math.Pow((r - centre_x), 2)) / (Math.Pow(ellipse_width, 2))) + ((4 * Math.Pow((c - centre_y), 2)) / (Math.Pow(ellipse_height, 2)));

            return formula;
        }
    }
    


}
