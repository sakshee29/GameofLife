using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Display;

namespace Life
{
    class Program
    {
        static void Main(string[] args)
        {
            Options options = ArgumentProcessor.Process(args);

            //storing the generations
            List<int[,]> stored_gen = new List<int[,]>();     
       
            bool steady_state = false;

            int[,] universe = InitializeUniverse(options);

            stored_gen.Add(universe); 

            Grid grid = new Grid(options.Rows, options.Columns);

            Logging.Message("Press spacebar to begin the game...");
            WaitSpacebar();

            grid.InitializeWindow();

            Stopwatch stopwatch = new Stopwatch();

            int iteration = 0;
            while (iteration <= options.Generations)
            {
                stopwatch.Restart();

                if (iteration != 0)
                {
                    universe = EvolveUniverse(universe, options.Periodic, options.Type, options.Order, options.Survival, options.Birth, options.CentreCount);

                    stored_gen.Add(universe);

                    for (int i = stored_gen.Count-2; i >= 0; i--)
                    {
                        if (GenerationsEqual(universe, stored_gen[i]))
                        {
                            //Steady state!
                            UpdateGrid(grid, universe);

                            grid.SetFootnote($"Generation: {iteration++}");
                            grid.Render();

                            if (options.StepMode)
                            {
                                WaitSpacebar();
                            }
                            else
                            {
                                while (stopwatch.ElapsedMilliseconds < 1000 / options.UpdateRate) ;
                            }

                            steady_state = true;
                            EndProgram(grid, options, universe, steady_state);                            
                        }
                    }

                }

                if (steady_state == false)
                {
                    UpdateGrid(grid, universe);

                    grid.SetFootnote($"Generation: {iteration++}");
                    grid.Render();

                    if (options.StepMode)
                    {
                        WaitSpacebar();
                    }
                    else
                    {
                        while (stopwatch.ElapsedMilliseconds < 1000 / options.UpdateRate) ;
                    }
                }
                else
                {
                    break;
                }
                
            }

            if(steady_state == false)
            {
                EndProgram(grid, options, universe, steady_state);
            }
            
        }

        private static bool GenerationsEqual(int[,] universe, int[,] v)
        {
            for(int i=0; i < universe.GetLength(0); i++) 
            {
                for(int j = 0; j < universe.GetLength(1); j++)
                {
                    if (universe[i,j] != v[i,j])
                    {
                        return false;
                    }
                    
                }
            }
            return true;
            
        }

        private static int[,] EvolveUniverse(int[,] universe, bool periodic, string type, int order, int[] survival, int[] birth, bool CentreCount)
        {
            const int ALIVE = 1;
            const int DEAD = 0;

            int rows = universe.GetLength(0);
            int columns = universe.GetLength(1);

            int[,] buffer = new int[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Neighbours neighbours1 = new Neighbours();
                    int neighbours = neighbours1.CountNeighbours(universe, i, j, type, order, periodic, CentreCount);

                    if (universe[i, j] == ALIVE && survival.Contains(neighbours))
                    {
                        buffer[i, j] = ALIVE;
                    }
                    else if (universe[i, j] == DEAD && birth.Contains(neighbours))
                    {
                        buffer[i, j] = ALIVE;
                    }
                    else
                    { 
                        buffer[i, j] = DEAD;
                    }
                }
            }

            return buffer.Clone() as int[,];
        }

        private static void UpdateGrid(Grid grid, int[,] universe)
        {
            for (int i = 0; i < universe.GetLength(0); i++)
            {
                for (int j = 0; j < universe.GetLength(1); j++)
                {
                    grid.UpdateCell(i, j, (CellState)universe[i, j]);
                }
            }
        }

        private static void WaitSpacebar()
        {
            while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) ;
        }

        private static int[,] InitializeUniverse(Options options)
        {
            int[,] universe;

            if (options.InputFile == null)
            {
                universe = InitializeFromRandom(options.Rows, options.Columns, options.RandomFactor);
            }
            else
            {
                try
                {
                    SeedReader seedReader = new SeedReader();
                    universe = seedReader.InitializeFromFile(options.Rows, options.Columns, options.InputFile);
                }
                
                catch 
                {
                    Logging.Warning($"Error initializing universe using \'{options.InputFile}\'. Reverting to randomised universe...");
                    universe = InitializeFromRandom(options.Rows, options.Columns, options.RandomFactor);
                }
            }
            
            return universe;
        }

        private static int[,] InitializeFromRandom(int rows, int columns, double randomFactor)
        {
            int[,] universe = new int[rows, columns];

            Random random = new Random();
            for (int i = 0; i < universe.GetLength(0); i++)
            {
                for (int j = 0; j < universe.GetLength(1); j++)
                {
                    //Giving each cell in the universe a zero or a one. 
                    universe[i, j] = random.NextDouble() < randomFactor ? 1 : 0;
                }
            }

            return universe;
        }
        
        private static void EndProgram(Grid grid, Options options, int[,] universe, bool steady_state)
        {
            bool all_dead = true; //Check if all values are dead in the array

            SeedWriter seedWriter = new SeedWriter();

            grid.IsComplete = true;
            grid.Render();
            WaitSpacebar();

            grid.RevertWindow();

            if (options.OutputFile != null)
            {
                seedWriter.WriteToFile(options.Rows, options.Columns, universe, options.OutputFile);
                if (seedWriter.Success)
                {
                    Logging.Success($"Final Generation written to file: {Path.GetFileName(options.OutputFile)}");
                }   
            }

            if (steady_state)
            {
                for(int i=0; i < options.Rows; i++)
                {
                    for(int j=0; j<options.Columns; j++)
                    {
                        if (universe[i, j] == 1)
                        {
                            all_dead = false;
                        }
                    }
                }

                if (all_dead)
                {
                    Logging.Message("Steady-state detected... periodicity = {N/A}");
                }
                else
                {
                    Logging.Message("Steady-state detected... periodicity = all cells not dead");
                }
                
            }
            else
            {
                Logging.Message("Steady-state not detected...");
            }
            
            Logging.Message("Press spacebar to exit program...");
            WaitSpacebar();
        }
    }
}
