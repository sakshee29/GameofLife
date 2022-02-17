# Game of Life Simulation

## About The Project (Conway's Game of Life)

The Game of Life, also known simply as Life, is a cellular automaton devised by the British mathematician John Horton Conway in 1970. It is a zero-player game, meaning that its evolution is determined by its initial state, requiring no further input. 
[Wikipedia](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life).

**Given by Uni**: Display Frame/Dimensions were setup.

**My Task**: The goal was to create a command line application that can simulate Life. The application is designed to be invoked from the command line, where game states and program settings can be provided. Additionally,
the application displays a running animation of Life in a graphical display window. Life progresses (or evolves) through a series of generations, with each generation depending on the one that
proceeded it. The rules that determine the next generation are rather simple and can be summarized by the following:
  - [ ] Any live cell with exactly 2 or 3 live neighbours survive (stay alive) in the next generation.
  - [ ] Any dead cell with exactly 3 live neighbours resurrect (become alive) in the next generation.
  - [ ] All other cells are dead in the next generation

### Built With

* [C#](https://docs.microsoft.com/en-us/dotnet/csharp/)
* [Visual Studio](https://visualstudio.microsoft.com/)

## Build Instructions
1. Download the Zip file of this repository.
2. Navigate to folder Solution in zip file.
3. Open Life.Sln in Visual Studio
4. open **program.cs** from Solution explorer which contains the code and the main function.
5. Enter Command line args in the properties of Visual studio if you want. 


## Usage 
1. Open Windows>Command Promt
2. Change directory to the **Life** directory that stores the project
3. Use code <dotnet life.dll //whatever args> and run the code.
4. The program runs with showing either an error or success for processing the command line arguments.

## Example Command Line Arguments 

#### Default
    dotnet life.dll
<img src="https://github.com/sakshee29/GameofLife/blob/main/default.png" alt="Image1"/> 

#### Kaleidoscope 1
    dotnet life.dll --survival 12...20 --birth 7 8 --neighbour vonNeumann 4 true --seed Seeds\kaleidoscope1_47x47.seed --dimensions 47 47 --generations 400 --max-update 30
<img src="https://github.com/sakshee29/GameofLife/blob/main/Kaleidoscope.png" alt="Image2" width="250" height="300"/> 

#### Target 2
    dotnet life.dll --seed Seeds\target2_16x16.seed
<img src="https://github.com/sakshee29/GameofLife/blob/main/target.png" alt="Image3"/> 

## Contact

Sakshee Tosniwal - [@LinkedIn_url](https://www.linkedin.com/in/sakshee-tosniwal-32a0a8188/) - toshniwalsakshee2002@gmail.com

Project Link: [https://github.com/sakshee29/GameofLife](https://github.com/sakshee29/GameofLife)

## Acknowledgments
CAB201 (Programming Principles) Teaching Team
