using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SquashInfo.Services;

namespace SquashInfo_Test
{
    [TestClass]
    public class SquashService_Test
    {
        StreamReader myFile;
        string respond;
        SquashInfo.Services.SquashService squash;

        [TestInitialize]
        public void Initialize()
        {
            myFile = new StreamReader(@"D:\GitHub\SquashInfo\SquashInfo\Services\sampleResponse.txt");
            respond = myFile.ReadToEnd();
        }

        [TestMethod]
        public void ConvertSquashResponse_Response_Is_Null()
        {
            Assert.Inconclusive();   
        }
        [TestMethod]
        public void ConvertSquashResponse_Response_Is_Empty()
        {
            Assert.Inconclusive();
        }
        [TestMethod]
        public void ConvertSquashResponse_Response_Should_Contian_Courts()
        {
            Assert.Inconclusive();
        }
    }
}
