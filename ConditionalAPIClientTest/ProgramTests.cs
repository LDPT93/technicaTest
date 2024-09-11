namespace ConditionalAPIClientTest
{
    public class ProgramTests
    {
        [Fact]
        public void IsValidURL_ShouldValid()
        {
            //Arrange
            var testUrl = "http://146.190.130.247:5011/donbest";

            //Act
            bool isValid = Program.IsValidURL(testUrl);

            //Assert
            Assert.True(isValid);

        }
        [Fact]
        public void IsValidURL_ShouldNotValid()
        {
            //Arrange
            var testUrl = "asdasdasdasd";

            //Act
            bool isValid = Program.IsValidURL(testUrl);

            //Assert
            Assert.False(isValid);

        }
    }
}