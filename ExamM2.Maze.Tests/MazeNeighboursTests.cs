namespace ExamM2.Maze.Tests;

public class MazeNeighboursTests
{
    [Fact]
    public void GetNeighbours_CenterCell_ShouldReturnFourNeighbours()
    {
        var maze = new Maze(@"...
...
...");

        var neighbours = maze.GetNeighbours(1, 1);

        Assert.Equal(4, neighbours.Count);
        Assert.Contains((1, 0), neighbours);
        Assert.Contains((1, 2), neighbours);
        Assert.Contains((0, 1), neighbours);
        Assert.Contains((2, 1), neighbours);
    }

    [Fact]
    public void GetNeighbours_TopIsWall_ShouldExcludeTop()
    {
        var maze = new Maze(@".#.
...
...");

        var neighbours = maze.GetNeighbours(1, 1);

        Assert.Equal(3, neighbours.Count);
        Assert.DoesNotContain((1, 0), neighbours);
    }

    [Fact]
    public void GetNeighbours_BottomIsWall_ShouldExcludeBottom()
    {
        var maze = new Maze(@"...
...
.#.");

        var neighbours = maze.GetNeighbours(1, 1);

        Assert.Equal(3, neighbours.Count);
        Assert.DoesNotContain((1, 2), neighbours);
    }

    [Fact]
    public void GetNeighbours_LeftIsWall_ShouldExcludeLeft()
    {
        var maze = new Maze(@"...
#..
...");

        var neighbours = maze.GetNeighbours(1, 1);

        Assert.Equal(3, neighbours.Count);
        Assert.DoesNotContain((0, 1), neighbours);
    }

    [Fact]
    public void GetNeighbours_RightIsWall_ShouldExcludeRight()
    {
        var maze = new Maze(@"...
..#
...");

        var neighbours = maze.GetNeighbours(1, 1);

        Assert.Equal(3, neighbours.Count);
        Assert.DoesNotContain((2, 1), neighbours);
    }

    [Fact]
    public void GetNeighbours_NeighbourIsStart_ShouldExcludeStart()
    {
        var maze = new Maze(@"D..
...
..S");

        var neighbours = maze.GetNeighbours(1, 0);

        Assert.Equal(2, neighbours.Count);
        Assert.DoesNotContain((0, 0), neighbours);
    }

    [Fact]
    public void GetNeighbours_OutOfBounds_ShouldExcludeOutOfBounds()
    {
        var maze = new Maze(@"D..
...
..S");

        var neighbours = maze.GetNeighbours(0, 1);

        Assert.Equal(2, neighbours.Count);
        Assert.Contains((1, 1), neighbours);
        Assert.Contains((0, 2), neighbours);
    }
}
