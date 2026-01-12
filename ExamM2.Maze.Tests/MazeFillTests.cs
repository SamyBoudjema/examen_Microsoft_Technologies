namespace ExamM2.Maze.Tests;

public class MazeFillTests
{
    [Fact]
    public void Constructor_ShouldInitializeQueueWithStart()
    {
        var maze = new Maze(@"D..
...
..S");

        Assert.Single(maze.ToVisit);
        var (x, y, distance) = maze.ToVisit.Peek();
        Assert.Equal(0, x);
        Assert.Equal(0, y);
        Assert.Equal(0, distance);
    }

    [Fact]
    public void Fill_WhenNotAtExit_ShouldReturnFalse()
    {
        var maze = new Maze(@"D..
...
..S");

        var result = maze.Fill();

        Assert.False(result);
    }

    [Fact]
    public void Fill_WhenAtExit_ShouldReturnTrue()
    {
        var maze = new Maze("DS");

        maze.Fill();
        var result = maze.Fill();

        Assert.True(result);
    }

    [Fact]
    public void Fill_ShouldUpdateDistancesArray()
    {
        var maze = new Maze(@"D..
...
..S");

        maze.Fill(); // Traite le Start (0,0)

        Assert.Equal(0, maze.Distances[0][0]); // Start a distance 0
    }

    [Fact]
    public void Fill_ShouldAddNeighboursToQueue()
    {
        var maze = new Maze(@"D..
...
..S");

        maze.Fill(); // Traite le Start (0,0)

        // Après avoir traité Start, ses voisins (0,1) et (1,0) sont dans la queue
        Assert.True(maze.ToVisit.Count >= 2);
    }
}
