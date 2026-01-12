namespace ExamM2.Maze.Tests;

public class MazeDistanceTests
{
    [Fact]
    public void GetDistance_SimpleMaze_ReturnsCorrectDistance()
    {
        // Maze 3x3 : D->S (chemin direct = 4 cases)
        // D..
        // ...
        // ..S
        var maze = new Maze(@"D..
...
..S");

        int distance = maze.GetDistance();

        Assert.Equal(4, distance); // D(0,0) -> (1,0) -> (2,0) -> (2,1) -> (2,2)S
    }

    [Fact]
    public void GetDistance_MazeWithWalls_ReturnsShortestDistance()
    {
        // Maze avec murs : 
        // D.#
        // ..#
        // ..S
        // Distance = 4
        var maze = new Maze(@"D.#
..#
..S");

        int distance = maze.GetDistance();

        Assert.Equal(4, distance); // D(0,0) -> (0,1) -> (0,2) -> (1,2) -> (2,2)S
    }

    [Fact]
    public void GetDistance_ComplexMaze_ReturnsShortestDistance()
    {
        // Maze complexe 5x5 (de l'énoncé)
        var maze = new Maze(@"D..#.
##...
.#.#.
...#.
####S");

        int distance = maze.GetDistance();

        // Calculons manuellement le plus court chemin
        // D(0,0) -> (1,0) -> (2,0) -> (3,0) impossible (mur)
        // D(0,0) -> (0,1) impossible (mur)
        // Donc D(0,0) -> (1,0) -> (2,0) -> (2,1) -> (3,1) -> (4,1) -> (4,2) -> (4,3) -> (4,4)S
        // Distance = 8
        Assert.Equal(8, distance);
    }
}
