using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maximo;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void testSimple()
        {
            Assert.AreEqual(9, Maximo.Maximo.GetMax(new int[] { 3, 7, 9, 8 }));
            
        }

        [TestMethod]
        public void testOrden()
        {
            Assert.AreEqual(9, Maximo.Maximo.GetMax(new int[] { 9, 7, 8 }));
            Assert.AreEqual(9, Maximo.Maximo.GetMax(new int[] { 7, 9, 8 }));
            Assert.AreEqual(9, Maximo.Maximo.GetMax(new int[] { 7, 8, 9 }));
        }

        [TestMethod]
        public void testDuplicados()
        {
            Assert.AreEqual(9, Maximo.Maximo.GetMax(new int[] { 9, 7, 9, 8 }));
        }

        [TestMethod]
        public void testSoloUno()
        {
            Assert.AreEqual(7, Maximo.Maximo.GetMax(new int[] { 7 }));
        }

        [TestMethod]
        public void testTodosNegativos()
        {
            Assert.AreEqual(-4, Maximo.Maximo.GetMax(new int[] { -4, -6, -7, -22 }));
        }
    }
}
