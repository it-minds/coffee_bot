using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Common.Linq;

namespace Application.Common
{
  public class WordStrings
  {
    private string[][] sets = {
      new string[]{ "Bird","Cat","Cow","Dog","Ferret","Horse","Pub","Rabbit","Hares","Racoon","Whale","Dolphin","Porpoise","Fish","Squirrel","Snake","Lizard","Chicken",".1",".2"},
      new string[]{"Blue","Black","Silver","Gray","White","Red","Brown","Maroon","Orange","Gold","Beige","Yellow","Lime","Turquoise","Teal","Green","Indigo","Purple","Magenta", "Violet"},
      new string[]{"One","Two","Three","Four","Five","Six","Seven","Eight","Nine","Ten","Eleven","Twelve","Twenty","Hundred","Thousand","@1","@2","@3","@4","@5"},
      new string[]{"Rice","Sesame","Quinoa","Peanut","Grape","Rhubarb","Apple","Avocado","Banana","Cherry","Lemon","Melon","Potato","Cabbage","Carrot","Biscuits","Maize","Bread","Chocolate","Soup"}
     };

    private Random rng;

    public int MaxCount { get => sets.RunningSum((set) => set.Count(), (int a, int b) => a == 0 ? b : a * b); }

    public string GetPredeterminedStringFromInt(int x)
    {
      rng = new Random(608750231); //hardcoded seed to make it predetermined.

      if (x < 0) throw new ArgumentOutOfRangeException("Less Than 0");
      if (x >= MaxCount) throw new ArgumentOutOfRangeException("More than " + MaxCount);

      var resultBuilder = new StringBuilder();

      for (int i = 0; i < sets.Count(); i++)
      {
        var remainingCount = sets.Skip(i + 1).RunningSum(x => x.Count(), (int a, int b) => a == 0 ? b : a * b);
        var set = sets[i].OrderBy(_ignore => this.rng.Next()).ToList();
        var result = GetModuloIndexIfValueInRange(set, remainingCount, ref x);

        resultBuilder.Append(result);
      }

      return resultBuilder.ToString();
    }

    private string GetModuloIndexIfValueInRange(in IList<string> set, int dividend, ref int value) {
      if (value < dividend) return ""; // aka the result of
      if (dividend == 0) {
        var result = set[value];
        value = 0;
        return result;
      }

      var index = (int)Math.Floor((decimal)value / (dividend)) - 1;
      value = value % dividend;
      return set[index];
    }
  }
}
