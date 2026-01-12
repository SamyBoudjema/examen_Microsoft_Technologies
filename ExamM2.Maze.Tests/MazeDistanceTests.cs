namespace ExamM2.Maze.Tests;

public class MazeDistanceTests
{
    [Fact]
    public void GetDistance_SimpleMaze_ReturnsCorrectDistance()
    {
        var maze = new Maze(@"D..
...
..S");

        int distance = maze.GetDistance();

        Assert.Equal(4, distance);
    }

    [Fact]
    public void GetDistance_MazeWithWalls_ReturnsShortestDistance()
    {
        var maze = new Maze(@"D.#
..#
..S");

        int distance = maze.GetDistance();

        Assert.Equal(4, distance);
    }

    [Fact]
    public void GetDistance_ComplexMaze_ReturnsShortestDistance()
    {
        var maze = new Maze(@"D..#.
##...
.#.#.
...#.
####S");

        int distance = maze.GetDistance();

        Assert.Equal(8, distance);
    }
}
