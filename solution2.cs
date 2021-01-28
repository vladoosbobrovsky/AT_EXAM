
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
        string projPATH = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        InputSimulator keyboard = new InputSimulator();
        Process wordpad;
        ISikuliSession current_sesion;

        private IPattern ResolveScreenshot(string screenshotName, double similiraty = 0.7) => Patterns.FromFile($"{projPATH}\\screenshots\\{screenshotName}", (float)similiraty);

        [OneTimeSetUp]
        public void Setup()
        {
            current_sesion = Sikuli.CreateSession();
            wordpad = Process.Start(@"C:\\Program Files\\Windows NT\\Accessories\\wordpad.exe");
        }

        [OneTimeTearDown]
        public void ForceDown()
        {
            current_sesion.Dispose();
            wordpad.Kill();
        }

        [Test, TestCase("D:\\AT-shared", "test.txt"), Order(1)]
        public void OpenFileTest(string folderPath, string fileName)
        {
            var openbuttonfile = ResolveScreenshot("openbuttonfile.png");
            current_sesion.Click(openbuttonfile);

            var ButtonOpen = ResolveScreenshot("ButtonOpen.png");
            current_sesion.Click(ButtonOpen);

            var folderInput = ResolveScreenshot("folderInputIcon.png");
            var fileInput = ResolveScreenshot("fileInputTitle.png");
            var dialogButtonOpen = ResolveScreenshot("dialogButtonOpen.png");

            current_sesion.Click(folderInput);
            Thread.Sleep(1000);
            keyboard.Keyboard.TextEntry(folderPath);
            current_sesion.Click(fileInput, new Point(100, 0));
            keyboard.Keyboard.TextEntry(fileName);
            current_sesion.Click(dialogButtonOpen);

            var expectResult = ResolveScreenshot("expectHeader1.png");
            Assert.IsTrue(current_sesion.Exists(expectResult), "File have not been opened");
        }
        [Test, TestCase("За екзамен 60 балів"), Order(2)]
        public void InputTest(string input)
        {
            var header = ResolveScreenshot("expectHeader1.png");
            current_sesion.DoubleClick(header, new Point(0, 150));
            Thread.Sleep(1000);
            keyboard.Keyboard.TextEntry(input);
            var expectResult = ResolveScreenshot("inEditHeader.png");
            Assert.IsTrue(current_sesion.Exists(expectResult), "File have not been edited");
        }

        [Test, TestCase("D:\\AT-shared", "newTest.txt"), Order(3)]
        public void SaveFileTest(string folderPath, string newName, bool overrideFile = true)
        {
            var openbuttonfile = ResolveScreenshot("openbuttonfile.png");
            current_sesion.Click(openbuttonfile);

            var saveButton = ResolveScreenshot("saveAsButton.png");
            current_sesion.Click(saveButton);

            var folderInput = ResolveScreenshot("folderInputIcon.png");
            var fileInput = ResolveScreenshot("fileInputTItle.png");

            current_sesion.Click(folderInput);
            Thread.Sleep(1000);
            keyboard.Keyboard.TextEntry(folderPath);
            current_sesion.Click(fileInput, new Point(100, 0));
            Thread.Sleep(1000);
            keyboard.Keyboard.TextEntry(newName);

            var dialogSaveButton = ResolveScreenshot("dialogSaveButton.png");
            current_sesion.Click(dialogSaveButton);

            var warnSign = ResolveScreenshot("warnSign.png");
            if (current_sesion.Exists(warnSign))
            {
                var yesButton = ResolveScreenshot("yesButton.png");
                if (overrideFile)
                    current_sesion.Click(yesButton);
                else
                    throw new Exception("override file is not enabled!");
            }

            var expectResult = ResolveScreenshot("expectHeader2.png");
            Assert.IsTrue(current_sesion.Exists(expectResult), "File have not been saved");
        }
    }
}
