using review2;

namespace review2NUnit
{
    public class Tests
    {
        [Test]
        public void Execute_AppliesFiltersInOrder()
        {
            var chain = new FilterChain<int>();
            chain.ApplyFilter(value => value > 2, "greater-than-two");
            chain.ApplyFilter(value => value % 2 == 0, "even");

            var result = chain.Execute(new[] { 1, 2, 3, 4, 5, 6 });

            Assert.That(result, Is.EqualTo(new[] { 4, 6 }));
        }

        [Test]
        public void Undo_RemovesLastAddedFilter()
        {
            var chain = new FilterChain<int>();
            chain.ApplyFilter(value => value > 0, "positive");
            chain.ApplyFilter(value => value < 5, "less-than-five");

            chain.Undo();
            var result = chain.Execute(new[] { -1, 1, 5, 10 }); 

            Assert.That(result, Is.EqualTo(new[] { 1, 5, 10 }));
        }

        [Test]
        public void Execute_HandlesEmptyCollection()
        {
            var chain = new FilterChain<string>();
            chain.ApplyFilter(value => value.Contains("credit"), "credit-filter");

            var result = chain.Execute(Array.Empty<string>());

            Assert.That(result, Is.Empty);
        }
    }
}
