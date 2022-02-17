using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Life
{
    //Borrowed from Part 1 Solution
    static class ArgumentProcessor
    {
        public static Options Process(string[] args)
        {
            //Creating new object options of Options class
            Options options = new Options();

            //Handling error if the flag is incorrect
            try
            {
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        //Processing flags 
                        case "--dimensions":
                            ProcessDimensions(args, i, options);
                            break;
                        case "--generations":
                            ProcessGenerations(args, i, options);
                            break;
                        case "--max-update":
                            ProcessUpdateRate(args, i, options);
                            break;
                        case "--random":
                            ProcessRandomFactor(args, i, options);
                            break;
                        case "--seed":
                            ProcessInputFile(args, i, options);
                            break;
                        case "--periodic":
                            options.Periodic = true;
                            break;
                        case "--step":
                            options.StepMode = true;
                            break;
                        case "--neighbour":
                            ProcessNeighbour(args, i, options);
                            break;
                        case "--survival":
                            ProcessSurvival(args, i, options);
                            break;
                        case "--birth":
                            ProcessBirth(args, i, options);
                            break;
                        case "--memory":
                            ProcessMemory(args, i, options);
                            break;
                        case "--output":
                            ProcessOutputFile(args, i, options);
                            break;
                        case "--ghost":
                            options.GhostMode = true;
                            break;
                    }
                }
                Logging.Success("Command line arguments processed without issue!");
            }
            catch (Exception exception)
            {
                Logging.Warning(exception.Message);
                Logging.Message("Reverting to defaults for unprocessed arguments...");
            }
            finally
            {
                Logging.Message("The following options will be used:");
                Console.WriteLine(options);
            }

            return options;
        }

        private static void ProcessDimensions(string[] args, int i, Options options)
        {
            //Validating whether dimensions flag has 2 parameters
            ValidateParameterCount(args, i, "dimensions", 2);

            //Checking if the two paramters are integers and assiging the first parameter 
            //as row and second as column 
            if (!int.TryParse(args[i + 1], out int rows))
            {
                throw new ArgumentException($"Row dimension \'{args[i + 1]}\' is not a valid integer.");
            }

            if (!int.TryParse(args[i + 2], out int columns))
            {
                throw new ArgumentException($"Column dimension \'{args[i + 2]}\' is not a valid integer.");
            }

            //Assigning the public variables the values if no exception thrown.
            options.Rows = rows;
            options.Columns = columns;
        }

        private static void ProcessGenerations(string[] args, int i, Options options)
        {
            //Validating whether dimensions flag has 1 parameter
            ValidateParameterCount(args, i, "generations", 1);

            //Checking the first param for int and storing it in generations
            if (!int.TryParse(args[i + 1], out int generations))
            {
                throw new ArgumentException($"Generation count \'{args[i + 1]}\' is not a valid integer.");
            }

            //Assigning the public variable the value if no exception thrown.
            options.Generations = generations;
        }

        //Similarly processing all other args. 
        private static void ProcessUpdateRate(string[] args, int i, Options options)
        {
 
            ValidateParameterCount(args, i, "max-update", 1);

            if (!double.TryParse(args[i + 1], out double updateRate))
            {
                throw new ArgumentException($"Update rate \'{args[i + 1]}\' is not a valid double.");
            }

            options.UpdateRate = updateRate;
        }

        private static void ProcessRandomFactor(string[] args, int i, Options options)
        {
            ValidateParameterCount(args, i, "random", 1);

            if (!double.TryParse(args[i + 1], out double randomFactor))
            {
                throw new ArgumentException($"Random factor \'{args[i + 1]}\' is not a valid double.");
            }

            options.RandomFactor = randomFactor;
        }

        private static void ProcessInputFile(string[] args, int i, Options options)
        {
            ValidateParameterCount(args, i, "seed", 1);

            options.InputFile = args[i + 1];
        }
   
        private static void ProcessNeighbour(string[] args, int i, Options options)
        {
            ValidateParameterCount(args, i, "neighbour", 3);

            if (!int.TryParse(args[i + 2], out int order))
            {
                throw new ArgumentException($"Order \'{args[i + 2]}\' is not a valid integer.");
            }

            if (!bool.TryParse(args[i + 3], out bool centreCount))
            {
                throw new ArgumentException($"Center Count \'{args[i + 3]}\' is not a valid boolean.");
            }
  
            options.Type = args[i+1].ToLower();
            options.Order = order;
            options.CentreCount = centreCount;
            
        }

        private static void ProcessSurvival(string[] args, int i, Options options)
        {
            ValidateParameterCount(args, i, "survival", 1);

            int values_s = 1;
            string survival_args = "";

            //Creating the survival list to store rules
            List<int> survival = new List<int>();

            while ((i+values_s < args.Length) && int.TryParse(args[i + values_s], out int survivalNum))
            {
                survival_args += args[i + values_s] + " ";
                survival.Add(survivalNum);
                values_s++;
            }

            // Spiltting the args with ranges
            if ((i + values_s < args.Length) && args[i + values_s].Contains("..."))
            {
                survival_args += args[i + values_s] + " ";

                string[] extremeValues = args[i + values_s].Split("...");

                int survival_rangeA = Convert.ToInt32(extremeValues[0]);
                int survival_rangeB = Convert.ToInt32(extremeValues[1]);

                for (int j = survival_rangeA; j <= survival_rangeB; j++)
                {
                    survival.Add(j);
                }
            }
            
            options.Survival = survival.ToArray();
            options.Survival_Args = survival_args;
        }

        //Similar as survival
        private static void ProcessBirth(string[] args, int i, Options options)
        {
            ValidateParameterCount(args, i, "birth", 1);

            int values_b = 1;
            string birth_args = "";

            List<int> birth = new List<int>();

            while ((i + values_b < args.Length) && int.TryParse(args[i + values_b], out int birthNum))
            {
                birth_args += args[i + values_b] + " ";
                birth.Add(birthNum);
                values_b++;
            }

            if ((i + values_b < args.Length) && args[i + values_b].Contains("..."))
            {
                birth_args += args[i + values_b] + " ";
                string[] extremeValues = args[i + values_b].Split("...");

                int birth_rangeA = Convert.ToInt32(extremeValues[0]);
                int birth_rangeB = Convert.ToInt32(extremeValues[1]);

                for (int x = birth_rangeA; x <= birth_rangeB; x++)
                {
                    birth.Add(x);
                }

            }

            options.Birth = birth.ToArray();
            options.Birth_Args = birth_args;

        }

        private static void ProcessMemory(string[] args, int i, Options options)
        {
            ValidateParameterCount(args, i, "memory", 1);

            if (!int.TryParse(args[i + 1], out int memory))
            {
                throw new ArgumentException($"Generational Memory \'{args[i + 1]}\' is not a valid integer.");
            }

            options.Memory = memory;
        }

        private static void ProcessOutputFile(string[] args, int i, Options options)
        {
            ValidateParameterCount(args, i, "output", 1);

            options.OutputFile = args[i + 1];
        }

        private static void ValidateParameterCount(string[] args, int i, string option, int numParameters)
        {
            if (i >= args.Length - numParameters)
            {
                throw new ArgumentException($"Insufficient parameters for \'--{option}\' option " +
                    $"(provided {args.Length - i - 1}, expected {numParameters})");
            }
        }
    }
}
