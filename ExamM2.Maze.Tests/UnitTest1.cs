namespace ExamM2.Maze.Tests;

public class MazeParserTests
{
    private const string SimpleMaze = @"D..#.
##...
.#.#.
...#.
####S";

    [Fact]
    public void Constructor_ShouldParseStartAndExitCorrectly()
    {
        var maze = new Maze(SimpleMaze);

        Assert.Equal((0, 0), maze.Start);
        Assert.Equal((4, 4), maze.Exit);
    }

    [Fact]
    public void Constructor_ShouldParseWallsCorrectly()
    {
        var maze = new Maze(SimpleMaze);

        Assert.False(maze.Grid[0][3]);
        Assert.False(maze.Grid[1][0]);
        Assert.False(maze.Grid[1][1]);
        Assert.False(maze.Grid[2][1]);
    }

    [Fact]
    public void Constructor_ShouldParseEmptyCellsCorrectly()
    {
        var maze = new Maze(SimpleMaze);

        Assert.True(maze.Grid[0][0]);
        Assert.True(maze.Grid[0][1]);
        Assert.True(maze.Grid[0][2]);
        Assert.True(maze.Grid[1][2]);
    }

    [Fact]
    public void Constructor_ShouldInitializeDistancesWithCorrectSize()
    {
        var maze = new Maze(SimpleMaze);

        Assert.Equal(5, maze.Distances.Length);
        Assert.Equal(5, maze.Distances[0].Length);
    }

    [Fact]
    public void Constructor_ShouldInitializeAllDistancesToZero()
    {
        var maze = new Maze(SimpleMaze);

        for (int y = 0; y < maze.Distances.Length; y++)
        {
            for (int x = 0; x < maze.Distances[y].Length; x++)
            {
                Assert.Equal(0, maze.Distances[y][x]);
            }
        }
    }
}
