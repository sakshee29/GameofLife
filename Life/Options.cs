using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Life
{
    class Options
    {
        //Constant Values (Upper and Lower Bounds)
        private const int MIN_DIMENSION = 4;
        private const int MAX_DIMENSION = 48;
        private const int MIN_GENERATION = 4;
        private const double MIN_UPDATE = 1.0;
        private const double MAX_UPDATE = 30.0;
        private const double MIN_RANDOM = 0.0;
        private const double MAX_RANDOM = 1.0;
        //Progress
        private const int MIN_ORDER = 1;
        private const int MAX_ORDER = 10;
        private const int MIN_MEMORY = 4;
        private const int MAX_MEMORY = 512;



        private int rows = 16;
        private int columns = 16;
        private int generations = 50;
        private double updateRate = 5.0;
        private double randomFactor = 0.5;
        private string inputFile = null;
        private string outputFile = null;
        //Progress
        private string type = "moore";
        private int order = 1;
        private int memory = 16;
        private int[] survival = { 2, 3 };
        private int[] birth = { 3 };
        private string birth_args = "3";
        private string survival_args = "2...3";
       

        public int Rows 
        {
            get => rows;
            set 
            {
                if (value < MIN_DIMENSION || value > MAX_DIMENSION)
                {
                    throw new ArgumentException($"Row dimension \'{value}\' is outside of the acceptable " +
                        $"range of values ({MIN_DIMENSION} - {MAX_DIMENSION})");
                }
                rows = value;
            } 
        }

        public int Columns
        {
            get => columns;
            set
            {
                if (value < MIN_DIMENSION || value > MAX_DIMENSION)
                {
                    throw new ArgumentException($"Column dimension \'{value}\' is outside of the acceptable " +
                        $"range of values ({MIN_DIMENSION} - {MAX_DIMENSION})");
                }
                columns = value;
            }
        }

        public int Generations
        {
            get => generations;
            set
            {
                if (value < MIN_GENERATION)
                {
                    throw new ArgumentException($"Generation count \'{value}\' is outside of the acceptable " +
                        $"range of values ({MIN_GENERATION} and above)");
                }
                generations = value;
            }
        }

        public double UpdateRate
        {
            get => updateRate;
            set
            {
                if (value < MIN_UPDATE || value > MAX_UPDATE)
                {
                    throw new ArgumentException($"Update rate \'{value:F2}\' is outside of the acceptable " +
                        $"range of values ({MIN_UPDATE} - {MAX_UPDATE})");
                }
                updateRate = value;
            }
        }

        public double RandomFactor
        {
            get => randomFactor;
            set
            {
                if (value < MIN_RANDOM || value > MAX_RANDOM)
                {
                    throw new ArgumentException($"Random factor \'{value:F2}\' is outside of the acceptable " +
                        $"range of values ({MIN_RANDOM} - {MAX_RANDOM})");
                }
                randomFactor = value;
            }
        }

        public string InputFile
        {
            get =>  inputFile;
            set
            {
                if (!File.Exists(value))
                {
                    throw new ArgumentException($"File \'{value}\' does not exist.");
                }
                if (!Path.GetExtension(value).Equals(".seed"))
                {
                    throw new ArgumentException($"Incompatible file extension \'{Path.GetExtension(value)}\'");
                }
                inputFile = value;
            }
        }

        //Progress
        public string Type
        {
            get => type;
            set
            {
                if (!(value == "moore" || value == "vonneumann"))
                {
                    throw new ArgumentException($"Neighbourhood type \'{value}\' is not valid");
                }

                type = value;
            }

        }

        public int Order
        {
            get => order;
            set
            {
                if (value < MIN_ORDER || value > MAX_ORDER)
                {
                    throw new ArgumentException($"Neighborhood Order \'{value}\' is outside of the acceptable " +
                        $"range of values ({MIN_ORDER} - {MIN_ORDER})");
                }
                else if (value > rows/2 || value > columns/2)
                {
                    throw new ArgumentException($"Neighborhood Order \'{value}\' is too large for the neighbourhood");
                }
                order = value;
            }

        }
        public int Neighbours()
        {
            int neighbours = 0;

            if (Type == "Moore")
            {
                neighbours = (Order) * 8;
            }
            else if (Type == "vonNeumann")
            {
                neighbours = 2 * Order * (Order + 1);
            }

            if (CentreCount)
            {
                neighbours = neighbours + 1;
            }

            return neighbours;
        }
        public int[] Survival
        {
            get => survival;
            set
            {
                
                foreach (int num in value)
                {
                    if (num < 0)
                    {
                        throw new ArgumentException($"The Value \'{num}\' in Survival Set cannot be negative");
                    }
                    //else if (num > Neighbours())
                    //{
                    //    throw new ArgumentException($"The Value \'{num}\' in Survival Set must not be greater than number of neighbouring cells");
                    //}

                }
                survival = value;
            }
        }

        public string Survival_Args
        {
            get => survival_args;
            set
            {
                survival_args = value;
            }
        }

        public int[] Birth
        {
            get => birth;
            set
            {
                foreach(int num in value)
                {
                    if (num < 0)
                    {
                        throw new ArgumentException($"The Value \'{num}\' in Birth Set cannot be negative");
                    }
                    //else if (num > Neighbours())
                    //{
                    //    throw new ArgumentException($"The Value \'{num}\' in Birth Set must not be greater than number of neighbouring cells");
                    //}
                }
                birth = value;
            }
        }

        public string Birth_Args
        {
            get => birth_args;
            set
            {
                birth_args = value;
            }
        }
  
        public int Memory
        {
            get => memory;
            set
            {
                if (value < MIN_MEMORY || value > MAX_MEMORY)
                {
                    throw new ArgumentException($"Generational Memory \'{value}\' is outside of the acceptable " +
                        $"range of values ({MIN_MEMORY} and {MAX_MEMORY})");
                }
                memory = value;
            }
        }

        public string OutputFile
        {
            get => outputFile;
            set
            {
              
                if (!Path.GetExtension(value).Equals(".seed"))
                {
                    throw new ArgumentException($"Incompatible file extension \'{Path.GetExtension(value)}\'");
                }
                outputFile = value;
            }
        }
        //
        public bool Periodic { get; set; } = false;

        public bool StepMode { get; set; } = false;

        //Progress
        public bool CentreCount { get; set; } = false;

        public bool GhostMode { get; set; } = false;
        

        //
        public override string ToString()
        {
            const int padding = 30;

            string output = "\n";

            //output += "Input File: ".PadLeft(padding) + (InputFile != null ? InputFile : "N/A") + "\n";
            //output += "Output File: ".PadLeft(padding) + (OutputFile != null ? OutputFile : "N/A") + "\n";
            output += "Input File: ".PadLeft(padding) + (InputFile != null ? Path.GetFileName(InputFile) : "N/A") + "\n";
            output += "Output File: ".PadLeft(padding) + (OutputFile != null ? Path.GetFileName(OutputFile) : "N/A") + "\n";
            output += "Generations: ".PadLeft(padding) + $"{Generations}\n";
            output += "Memory: ".PadLeft(padding) + $"{Memory}\n";
            output += "Refresh Rate: ".PadLeft(padding) + $"{UpdateRate} updates/s\n";
            output += "Rules: ".PadLeft(padding) + $"S ( {Survival_Args} ) B ( {Birth_Args} )\n";
            output += "Neighborhood: ".PadLeft(padding) + $"{Type} ({Order}) {CentreCount}\n";
            output += "Periodic: ".PadLeft(padding) + (Periodic ? "Yes" : "No") + "\n";
            output += "Rows: ".PadLeft(padding) + Rows + "\n";
            output += "Columns: ".PadLeft(padding) + Columns + "\n";
            output += "Random Factor: ".PadLeft(padding) + $"{100 * RandomFactor:F2}%\n";
            output += "Step Mode: ".PadLeft(padding) + (StepMode ? "Yes" : "No") + "\n";
            output += "Ghost Mode: ".PadLeft(padding) + "Not implemented\n";

            return output;
        }
    }
}
