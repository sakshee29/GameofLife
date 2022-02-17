using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Life
{
    class SeedWriter
    {
        private bool success = true;
        public bool Success
        {
            get => success;
            set
            {
                success = value;
            }
        }

        public void WriteToFile(int rows, int columns, int[,] universe, string outputFile)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(outputFile))
                {
                    writer.WriteLine("#version=2.0");

                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < columns; j++)
                        {
                            if (universe[i, j] == 1)
                            {
                                writer.WriteLine($"(o) cell: {i}, {j}");
                            }
                        }
                    }
                }
            }
            catch
            {
                Success = false;
                Logging.Error($"File cannot be written. File Directory is invalid.");
                
            }

        }
    }

}
