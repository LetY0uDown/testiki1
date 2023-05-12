using SF2022User01Lib;

Calculations calculations = new Calculations();
var res = calculations.AvailablePeriods(new TimeSpan[] { new(10, 0, 0), new(15, 0, 0) },
                                                                     new int[] { 0 },
                                                                     new(8, 0, 0),
                                                                     new(14, 0, 0),
                                                                     30);

foreach (var r in res)
    Console.WriteLine(r);