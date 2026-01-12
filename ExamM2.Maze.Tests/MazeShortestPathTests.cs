namespace ExamM2.Maze.Tests;

public class MazeShortestPathTests
{
    [Fact]
    public void GetShortestPath_SimpleMaze_ReturnsCorrectPath()
    {
        var maze = new Maze(@"D..
...
..S");

        maze.GetDistance();
        var path = maze.GetShortestPath();

        Assert.Equal((0, 0), path.First());
        Assert.Equal((2, 2), path.Last());
        Assert.Equal(5, path.Count);
    }

    [Fact]
    public void GetShortestPath_MazeWithWalls_ReturnsCorrectPath()
    {
        var maze = new Maze(@"D.#
..#
..S");

        maze.GetDistance();
        var path = maze.GetShortestPath();

        Assert.Equal((0, 0), path.First());
        Assert.Equal((2, 2), path.Last());
        Assert.Equal(5, path.Count);
        
        foreach (var (x, y) in path)
        {
            Assert.True(maze.Grid[y][x]);
        }
    }

    [Fact]
    public void GetShortestPath_PathIsSequential()
    {
        var maze = new Maze(@"D..
...
..S");

        maze.GetDistance();
        var path = maze.GetShortestPath();

        for (int i = 0; i < path.Count - 1; i++)
        {
            var (x1, y1) = path[i];
            var (x2, y2) = path[i + 1];
            
            int distance = Math.Abs(x2 - x1) + Math.Abs(y2 - y1);
            Assert.Equal(1, distance);
        }
    }
}
