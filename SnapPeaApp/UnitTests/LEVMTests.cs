using System;
using System.IO;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnapPeaApp;
using SnapPeaApp.ViewModels;

namespace UnitTests
{
    [TestClass]
    public class LEVMTests
    {
        private static string sampleLayout = "{\"Name\":\"test\",\"Regions\":[{\"LeftF\":0.004166667,\"TopF\":0.451485157,\"WidthF\":0.4904762,\"HeightF\":0.480198026,\"Color\":0},{\"LeftF\":0.5005952,\"TopF\":0.0881188139,\"WidthF\":0.483928561,\"HeightF\":0.446534663,\"Color\":0},{\"LeftF\":0.007142857,\"TopF\":0.0118811885,\"WidthF\":0.4857143,\"HeightF\":0.349504948,\"Color\":0}]}";

        [TestMethod]
        public void TestSaveLayout()
        {
            string path = Path.GetTempFileName();

            //save the file we will use for testing
            File.WriteAllText(path, sampleLayout);

            Layout layout = Layout.LoadLayout(path);

            //get rid of the temp file before we proceed
            File.Delete(path);

            Assert.IsNotNull(layout);

            DrawTools.GraphicsList gl = new DrawTools.GraphicsList();

            //create an LEVM
            LayoutEditorViewModel levm = new LayoutEditorViewModel(layout, gl);

            Assert.IsNotNull(levm);

            //save the layout to the file
            levm.SaveLayout(path, "test");

            string contents = File.ReadAllText(path);

            File.Delete(path);

            //ensure that the contents that were saved to the file is equal to the layout we have
            Assert.IsTrue(contents.Trim().Equals(sampleLayout));
        }
    }
}
