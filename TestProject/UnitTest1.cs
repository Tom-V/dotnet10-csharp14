using Xunit;

namespace TestProject;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Assert.True(true);
    }
    //write a unit test that always fails
    [Fact]
    public void Test2()
    {
        Assert.Equal(5, 5);
    }
}
