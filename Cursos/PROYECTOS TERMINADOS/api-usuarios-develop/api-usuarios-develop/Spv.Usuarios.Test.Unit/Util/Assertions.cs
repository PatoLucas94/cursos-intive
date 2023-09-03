using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Spv.Usuarios.Domain.Utils;

namespace Spv.Usuarios.Test.Unit.Util
{
    [ExcludeFromCodeCoverage]
    public static class Assertions
    {
        public static void AssertAll<T>(this IEnumerable<Result<T>> results, Action<Result<T>> assertion)
        {
            foreach (var result in results)
            {
                assertion(result);
            }
        }

        public static void Equality<T>(T self, T same, T otherEqual, T otherDifferent, T nullObject, object otherType)
        {
            var typeName = self.GetType().Name;
            var otherTypeName = otherType.GetType().Name;
            self.Equals(same).Should().BeTrue($"{typeName}: {nameof(self)}.Equals({nameof(same)})");
            (self.Equals(same) == same.Equals(self)).Should().BeTrue(
                "{typeName}: {nameof(self)}.Equals({nameof(same)}) == {nameof(self)}.Equals({nameof(self)})");
            self.Equals(otherEqual).Should().BeTrue($"{typeName}: {nameof(self)}.Equals({nameof(otherEqual)})");
            (self.Equals(otherEqual) == otherEqual.Equals(self)).Should().BeTrue(
                $"{typeName}: {nameof(self)}.Equals({nameof(otherEqual)}) == {nameof(otherEqual)}.Equals({nameof(self)})");
            self.Equals(otherType).Should()
                .BeFalse($"{typeName}:{nameof(self)}.Equals({otherTypeName}:{nameof(otherType)})");
            (self.Equals(otherType) == otherType.Equals(self)).Should().BeTrue(
                $"{typeName}:{nameof(self)}.Equals({otherTypeName}:{nameof(otherType)}) == {otherTypeName}:{nameof(otherType)}.Equals({typeName}:{nameof(self)})");
            (self.Equals(otherDifferent) == otherDifferent.Equals(self)).Should().BeTrue(
                $"{typeName}: {nameof(self)}.Equals({nameof(otherDifferent)}) == c.Equals({nameof(self)})");
            self.Equals(nullObject).Should().BeFalse($"{typeName} {nameof(self)}.Equals({nullObject})");
            (!self.Equals(otherEqual) || self.GetHashCode() == otherEqual.GetHashCode()).Should()
                .BeTrue($"{typeName}: {nameof(self)}.GetHashCode() == {nameof(otherEqual)}.GetHashCode()");
        }

        public static void Comparability<T>(T self, T otherEqual, T otherBigger, T otherSmaller, T otherNull)
            where T : IComparable<T>
        {
            self.CompareTo(otherEqual).Should().Be(0, $"{self}.CompareTo({otherEqual})");
            self.CompareTo(otherBigger).Should().Be(-1, $"");
            self.CompareTo(otherSmaller).Should().Be(1, $"");
            self.CompareTo(otherNull).Should().Be(1, $"");
        }
    }
}
