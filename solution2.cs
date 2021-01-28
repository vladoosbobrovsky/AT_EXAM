
using InputSimulatorStandard;
using NUnit.Framework;
using SikuliSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Ski
{
    public class Tests
    {
        string projectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        InputSimulator keyboard = new InputSimulator();
        Process wordpad;
        ISikuliSession session;

        private IPattern ResolveScreenshot(string screenshotName, double similiraty = 0.7) => Patterns.FromFile($"{projectDir}\\screenshots\\{screenshotName}", (float)similiraty);

        [OneTimeSetUp]
        public void Setup()
        {
            session = Sikuli.CreateSession();
            wordpad = Process.Start(@"C:\\Program Files\\Windows NT\\Accessories\\wordpad.exe");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            session.Dispose();
            wordpad.Kill();
        }

        [Test, TestCase("D:\\AT-shared", "test.txt"), Order(1)]
        public void OpenFileTest(string folderPath, string fileName)
        {
            var fileButton = ResolveScreenshot("fileButton.png");
            session.Click(fileButton);

            var openButton = ResolveScreenshot("openButton.png");
            session.Click(openButton);

            var folderInput = ResolveScreenshot("folderInputIcon.png");
            var fileInput = ResolveScreenshot("fileInputTitle.png");
            var dialogOpenButton = ResolveScreenshot("dialogOpenButton.png");

            session.Click(folderInput);
            Thread.Sleep(1000);
            keyboard.Keyboard.TextEntry(folderPath);
            session.Click(fileInput, new Point(100, 0));
            keyboard.Keyboard.TextEntry(fileName);
            session.Click(dialogOpenButton);

            var expectResult = ResolveScreenshot("expectHeader1.png");
            Assert.IsTrue(session.Exists(expectResult), "File have not been opened");
        }
        [Test, TestCase("За екзамен 60 балів"), Order(2)]
        public void InputTest(string input)
        {
            var header = ResolveScreenshot("expectHeader1.png");
            session.DoubleClick(header, new Point(0, 150));
            Thread.Sleep(1000);
            keyboard.Keyboard.TextEntry(input);
            var expectResult = ResolveScreenshot("inEditHeader.png");
            Assert.IsTrue(session.Exists(expectResult), "File have not been edited");
        }

        [Test, TestCase("D:\\AT-shared", "newTest.txt"), Order(3)]
        public void SaveFileTest(string folderPath, string newName, bool overrideFile = true)
        {
            var fileButton = ResolveScreenshot("fileButton.png");
            session.Click(fileButton);

            var saveButton = ResolveScreenshot("saveAsButton.png");
            session.Click(saveButton);

            var folderInput = ResolveScreenshot("folderInputIcon.png");
            var fileInput = ResolveScreenshot("fileInputTItle.png");

            session.Click(folderInput);
            Thread.Sleep(1000);
            keyboard.Keyboard.TextEntry(folderPath);
            session.Click(fileInput, new Point(100, 0));
            Thread.Sleep(1000);
            keyboard.Keyboard.TextEntry(newName);

            var dialogSaveButton = ResolveScreenshot("dialogSaveButton.png");
            session.Click(dialogSaveButton);

            var warnSign = ResolveScreenshot("warnSign.png");
            if (session.Exists(warnSign))
            {
                var yesButton = ResolveScreenshot("yesButton.png");
                if (overrideFile)
                    session.Click(yesButton);
                else
                    throw new Exception("override file is not enabled!");
            }

            var expectResult = ResolveScreenshot("expectHeader2.png");
            Assert.IsTrue(session.Exists(expectResult), "File have not been saved");
        }
    }
}
