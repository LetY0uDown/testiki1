using SF2022User01Lib;

namespace Тестики;

public class Tests
{
    Calculations calculations;

    [SetUp]
    public void Setup()
    {
        calculations = new Calculations();
    }

    [Test]
    public void Periods_ThrowsIfDurationsIsZeroOrNegative()
    {
        Assert.Throws<ArgumentException>(() => calculations.AvailablePeriods(new TimeSpan[] { new(10, 0, 0), new(15, 0, 0) },
                                                                     new int[] { 0, -12 },
                                                                     new(8, 0, 0),
                                                                     new(14, 0, 0),
                                                                     30));
    }
    
    [Test]
    public void Periods_ThrowsIfConsultationTimeIsZeroOrNegative()
    {
        Assert.Throws<ArgumentException>(() => calculations.AvailablePeriods(new TimeSpan[] { new(10, 0, 0), new(15, 0, 0) },
                                                                     new int[] { 45, 11 },
                                                                     new(8, 0, 0),
                                                                     new(14, 0, 0),
                                                                     -10));
    }
    
    [Test]
    public void Periods_ThrowsIfNoStartTimes()
    {
        Assert.Throws<ArgumentException>(() => calculations.AvailablePeriods(Array.Empty<TimeSpan>(),
                                                                             new int[] { 30, 45 },
                                                                             new(8, 0, 0),
                                                                             new(14, 0, 0),
                                                                             30));
    }

    [Test] // вот этот тест не выполняется :(
    public void Periods_ThrowsIfStartTimesAreGreaterThenEndWorkingTime()
    {
        Assert.Throws<Exception>(() => calculations.AvailablePeriods(new TimeSpan[] { new(10, 0, 0), new(15, 0, 0) },
                                                                     new int[] { 10, 30 },
                                                                     new(8, 0, 0),
                                                                     new(14, 0, 0),
                                                                     30));
    }

    [Test]
    public void Periods_IsCorrectWorkingTimes()
    {
        var res = calculations.AvailablePeriods(new TimeSpan[] { new(10, 0, 0), new(12, 0, 0) },
                                        new int[] { 10, 30 },
                                        new(8, 0, 0),
                                        new(16, 0, 0),
                                        30);

        var timeSpans = Parse(res);

        if (timeSpans.Contains(new(10, 10, 0)) && timeSpans.Contains(new(12, 30, 0)))
            Assert.Pass();

        Assert.Fail();
    }

    [Test]
    public void Periods_NoConsultationsAfterEndWorking()
    {
        var res = calculations.AvailablePeriods(new TimeSpan[] { new(10, 0, 0) },
                                                new int[] { 60 },
                                                new(8, 0, 0), new(16, 0, 0),
                                                30);

        var timeSpans = Parse(res);

        Assert.That(timeSpans[^1], Is.Not.GreaterThan(TimeSpan.FromHours(16)));
    }

    [Test]
    public void Periods_Start10Duration60_NoFreeTimeFrom10To11()
    {
        var res = calculations.AvailablePeriods(new TimeSpan[] { new(10, 0, 0) },
                                                new int[] { 60 },
                                                new(8, 0, 0), new(16, 0, 0),
                                                30);

        var timeSpans = Parse(res);

        Assert.That(timeSpans, Does.Not.Contain(new TimeSpan(10, 30, 0)));
    }

    [Test]
    public void Periods_30MinutesDiff()
    {
        var res = calculations.AvailablePeriods(new TimeSpan[] { new(10, 0, 0) },
                                        new int[] { 30 },
                                        new(8, 0, 0),
                                        new(16, 0, 0),
                                        30);

        var timeSpans = Parse(res);

        for (int i = 0; i < timeSpans.Count - 2; i += 2) {
            if ((timeSpans[i + 1] - timeSpans[i]).Minutes != 30)
                Assert.Fail();
        }

        Assert.Pass();
    }

    private List<TimeSpan> Parse (string[] times)
    {
        List<TimeSpan> timesParsed = new();

        foreach (var t in times)
        {
            var bar = t.Split('-');
            timesParsed.Add(TimeSpan.Parse(bar[0].Trim()));
            timesParsed.Add(TimeSpan.Parse(bar[1].Trim()));
        }

        return timesParsed;
    }
}