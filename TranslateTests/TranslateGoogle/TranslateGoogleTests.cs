using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Translate.Google.Tests
{
    [TestClass]
    public class TranslateGoogleTests
    {
        [TestMethod]
        public async Task TranslateTextAsyncShouldReturnTranslatedTextWhenGivenValidArguments()
        {
            // Arrange
            var texto = "olá mundo!";
            var idiomaOrigem = "pt";
            var idiomaDestino = "en";
            var expected = "Hello World!";

            var getTranslateGoogleMock = new Mock<IGetTranslateGoogle>();
            var translateGoogle = new TranslateGoogle(getTranslateGoogleMock.Object);

            // Setup GetTranslateAsync to return the expected translated text
            getTranslateGoogleMock
                .Setup(x => x.GetTranslateAsync(texto, idiomaOrigem, idiomaDestino))
                .ReturnsAsync("[\"Hello World!\"]");

            // Act
            var result = await translateGoogle.TranslateTextAsync(texto, idiomaOrigem, idiomaDestino);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TranslateTextAsync_ShouldReturnNull_WhenGetTranslateAsyncReturnsNull()
        {
            // Arrange
            var texto = "olá mundo!";
            var idiomaOrigem = "pt";
            var idiomaDestino = "en";
            var expected = "Hello World!";

            var getTranslateGoogleMock = new Mock<IGetTranslateGoogle>();
            var translateGoogle = new TranslateGoogle(getTranslateGoogleMock.Object);

            // Setup GetTranslateAsync to return the expected translated text
            getTranslateGoogleMock
                .Setup(x => x.GetTranslateAsync(texto, idiomaOrigem, idiomaDestino))
                .ReturnsAsync((string)null);

            // Act
            var result = await translateGoogle.TranslateTextAsync(texto, idiomaOrigem, idiomaDestino);

            // Assert
            Assert.IsNull(result);
        }

    }
}
