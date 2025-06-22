using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using project.fil;

namespace MSTest_Test_Project
{
    [TestClass]
    public class MainWindowTests
    {
        private MainWindow _mainWindow;

        [TestInitialize]
        public void Setup()
        {
            // Инициализируем окно перед каждым тестом
            _mainWindow = new MainWindow();
        }

        [TestMethod]
        public void TestGetRandomWord_ReturnsWordFromCategory()
        {
            // Arrange
            string category = "Природа";

            // Act
            string result = _mainWindow.GetRandomWord(category);

            // Assert
            Assert.IsTrue(_mainWindow._wordCategories[category].Contains(result));
        }

        [TestMethod]
        public void TestGetRandomWord_ReturnsNullForInvalidCategory()
        {
            // Arrange
            string category = "НекорректнаяКатегория";

            // Act and Assert
            Assert.ThrowsException<KeyNotFoundException>(() => _mainWindow.GetRandomWord(category));
        }

        [TestMethod]
        public void TestUpdateDisplayWord_CorrectlyUpdatesWord()
        {
            // Arrange
            _mainWindow.SetSelectedWord("тест"); // Установка слова для теста
            _mainWindow.SetDisplayWord("____"); // Начальное слово в виде подчеркиваний

            // Act
            var updatedWord = _mainWindow.UpdateDisplayWord("т");

            // Assert
            Assert.AreEqual("т___", updatedWord);
        }
    }
}
