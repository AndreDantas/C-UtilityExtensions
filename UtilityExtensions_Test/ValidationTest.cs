using NUnit.Framework;
using System;
using System.Linq;
using UtilityExtensions.Core.Validations;

namespace UtilityExtensions_Test
{
    public class ValidationTest
    {
        [Test]
        public void ValidateStrings_EmptyString()
        {
            try
            {
                string string1 = "123";
                string string2 = "321";
                string string3 = "";

                ValidationManager<string>.Add(string1, nameof(string1))
                                         .Add(string2, nameof(string2))
                                         .Add(string3, nameof(string3))
                                         .NotEmpty();
            }
            catch (Exception)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [Test]
        public void ValidateStrings_InvalidLength()
        {
            try
            {
                string string1 = "123";
                string string2 = "321";
                string string3 = "12";

                ValidationManager<string>.UseSettings(new ValidationManager.Settings { throwExceptionOnFail = true, validateImmediately = true })
                                         .Add(string1, nameof(string1))
                                         .Add(string2, nameof(string2))
                                         .Add(string3, nameof(string3))
                                         .ShorterThan(4)
                                         .LongerThan(2);
            }
            catch (Exception)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [Test]
        public void ValidateStrings_ValidEmail()
        {
            string string1 = "validemail@gmail.com";

            var vm = ValidationManager<string>.UseSettings(new ValidationManager.Settings { throwExceptionOnFail = false, validateImmediately = true })
                                     .Add(string1, nameof(string1))
                                     .ValidEmail();

            Assert.IsTrue(vm.FailedParameters().Count == 0);
            Assert.IsTrue(vm.Parameters.First().Results.First().IsSuccess);
        }

        [Test]
        public void ValidateStrings_InvalidEmail()
        {
            string string1 = "invalidmail.cm";

            var vm = ValidationManager<string>.UseSettings(new ValidationManager.Settings { throwExceptionOnFail = false, validateImmediately = true })
                                     .Add(string1, nameof(string1))
                                     .ValidEmail();

            Assert.IsTrue(vm.FailedParameters().Count == 1);
            Assert.IsFalse(vm.Parameters.First().Results.First().IsSuccess);
        }

        [Test]
        public void ValidateDecimal_InRange()
        {
            try
            {
                decimal decim1 = 30.23m;
                decimal decim2 = -13m;
                decimal decim3 = 2000m;

                ValidationManager<decimal>.Add(decim1, nameof(decim1))
                                         .Add(decim2, nameof(decim2))
                                         .Add(decim3, nameof(decim3))
                                         .InRange(-100, 100);
            }
            catch (Exception)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }
    }
}