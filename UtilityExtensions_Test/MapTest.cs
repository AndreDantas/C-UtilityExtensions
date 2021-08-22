using NUnit.Framework;
using System.Collections.Generic;
using UtilityExtensions.Core;

namespace UtilityExtensions_Test
{
    public class MapTest
    {
        private class MapConvertionTest
        {
            public string StringTest { get; set; } = "test";
            public int IntTest { get; set; } = 42;
            public decimal DecimalTest { get; set; } = 12.34m;
            public NestedClass NestedClassTest { get; set; } = new NestedClass();
            public List<decimal> ListTest { get; set; } = new List<decimal> { 1.2m, 32 };

            [MapIgnore]
            public int Ignore { get; set; }
        }

        private class NestedClass
        {
            public string NestedStringTest { get; set; } = "test2";
            public int NestedIntTest { get; set; } = 21;
            public decimal NestedDecimalTest { get; set; } = 100.1m;
            public List<decimal> NestedListTest { get; set; } = new List<decimal> { 1.2m, 32 };
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test, Order(0)]
        public void ConvertObjectToMap()
        {
            MapConvertionTest obj = new MapConvertionTest();
            Map convertMap = obj.ToMap();
            Map checkMap = new Map() {
                { "StringTest" , "test" },
                { "IntTest" , 42 },
                { "DecimalTest" , 12.34m },
                { "ListTest" , new List<decimal> { 1.2m, 32 } },
                { "NestedClassTest" ,  new Map(){
                    {"NestedStringTest", "test2" },
                    {"NestedIntTest", 21 },
                    {"NestedDecimalTest", 100.1m },
                    {"NestedListTest", new List<decimal> { 1.2m, 32 }},
                } }
            };

            Assert.IsTrue(convertMap == checkMap);
        }

        [Test]
        public void ConvertMapToObject()
        {
            Map convertMap = new Map() {
                { "StringTest" , null},
                { "IntTest" , 1 },
                { "DecimalTest" , 2m },
                { "NestedClassTest" ,  new Map(){
                    {"NestedStringTest", "test" },
                    {"NestedIntTest", 3 },
                    {"NestedDecimalTest", 4m },
                } }
            };
            MapConvertionTest obj = convertMap.FromMap<MapConvertionTest>();
            Assert.IsNotNull(obj);
            Assert.IsTrue(obj is MapConvertionTest);
            Assert.IsNull(obj.StringTest);
            Assert.IsTrue(obj.IntTest == 1);
            Assert.IsTrue(obj.DecimalTest == 2);
            Assert.IsNotNull(obj.NestedClassTest);
            Assert.IsTrue(obj.NestedClassTest.NestedStringTest == "test");
            Assert.IsTrue(obj.NestedClassTest.NestedIntTest == 3);
            Assert.IsTrue(obj.NestedClassTest.NestedDecimalTest == 4);
        }

        [Test]
        public void Clone()
        {
            Map checkMap = new Map() {
                { "StringTest" , "test" },
                { "IntTest" , 42 },
                { "DecimalTest" , 12.34m },
                { "ListTest" , new List<decimal> { 1.2m, 32 } },
                { "NestedClassTest" ,  new Map(){
                    {"NestedStringTest", "test2" },
                    {"NestedIntTest", 21 },
                    {"NestedDecimalTest", 100.1m },
                    {"NestedListTest", new List<decimal> { 1.2m, 32 }},
                } }
            };
            Map cloneMap = checkMap.Clone() as Map;
            Assert.IsTrue(cloneMap == checkMap);
        }

        [Test]
        public void MapToString()
        {
            Map map = new Map()
            {
                {"field1", 10 },
                {"field2", null },
                {"field3", 134.12 },
                {"field4", "test" },
                {"field5", new List<int>{1, 2} },
                {"field6", new Map(){
                    {"field1", 3 }
                } },
            };
            string checkString = "{\"field1\":10,\"field2\":null,\"field3\":134.12,\"field4\":\"test\",\"field5\":[1,2],\"field6\":{\"field1\":3}}";
            string mapString = map.ToString();

            Assert.IsTrue(checkString == mapString);
        }

        [Test]
        public void CompareMaps_DifferentFieldValue_ReturnsFalse()
        {
            Map map1 = new Map()
            {
                {"field1", 10 },
                {"field2", null },
            };
            Map map2 = new Map()
            {
                {"field1", 10 },
                {"field2", "test" },
            };

            Assert.IsFalse(map1 == map2);
        }

        [Test]
        public void CompareMaps_SameFieldValues_ReturnsTrue()
        {
            Map map1 = new Map()
            {
                {"field1", 10 },
                {"field2", "test" },
            };
            Map map2 = new Map()
            {
                {"field1", 10 },
                {"field2", "test" },
            };

            Assert.IsTrue(map1 == map2);
        }

        [Test]
        public void CompareMaps_DifferentFields_ReturnsFalse()
        {
            Map map1 = new Map()
            {
                {"field1", 10 },
                {"field2", "test" },
            };
            Map map2 = new Map()
            {
                {"field1", 10 },
                {"field3", "test" },
            };

            Assert.IsFalse(map1 == map2);
        }

        [Test]
        public void CompareMaps_MissingFields_ReturnsFalse()
        {
            Map map1 = new Map()
            {
                {"field1", 10 },
            };
            Map map2 = new Map()
            {
                {"field1", 10 },
                {"field2", "test" },
            };

            Assert.IsFalse(map1 == map2);
        }
    }
}