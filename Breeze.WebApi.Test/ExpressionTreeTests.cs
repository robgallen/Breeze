﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.ObjectModel;

namespace Breeze.WebApi.Test {

  public class FakeInstance {
    public int ID { get; set; }
    public String StringValue { get; set; }
    public Int32 IntValue { get; set; }
    public Double DoubleValue { get; set; }
    public Decimal DecimalValue { get; set; }
    public DateTime DateValue { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTimeOffset PointInTime { get; set; }
    public TestChoice ChoiceValue { get; set; }
    public ICollection<FakeChildItem> Children;
  }

  public class FakeChildItem {
		private readonly Collection<FakeGrandChildItem> _children = new Collection<FakeGrandChildItem>();
		public int ID { get; set; }
		public string ChildStringValue { get; set; }
		public ICollection<FakeGrandChildItem> Children {
			get { return _children; }
		}
	}

  public class FakeGrandChildItem {
		public string GrandChildStringValue { get; set; }
	}

  [Flags]
  public enum TestChoice {
    Choice1 = 1,
    Choice2 = 2,
    BothChoices = 3
  }


  [TestClass]
  public class ExpressionTreeTests {

    [TestInitialize]
    public void Init() {
      _factory = new ExpressionTreeBuilder();
    }

    [TestMethod]
    public void ExprTest1() {
      bool error = false;
      __testCases.ForEach(tpl => {
        var input = tpl.Item1;
        var expected = tpl.Item2;
        
        try {
          var expr = _factory.Parse(typeof (FakeInstance), input);
          if (expr.ToString() != expected) {
            error = true;
            Console.WriteLine("Error - Input: " + input + "Output: " + expr.ToString() + " Expected: " + expected);
          } else {
            Console.WriteLine("Ok - Input: " + input);
          }
        } catch (Exception e) {
          error = true;
          Console.WriteLine("Error - Input: " + input + " but got error: " + e.Message);
        }
      });
      if (error) {
        Assert.Fail("failed");
      }
    }

    public static string ToAggregateString(IEnumerable items, string delimiter) {
      StringBuilder sb = null;
      foreach (object aObject in items) {
        if (sb == null) {
          sb = new StringBuilder();
        } else {
          sb.Append(delimiter);
        }
        sb.Append(aObject.ToString());
      }
      if (sb == null) return String.Empty;
      return sb.ToString();
    }

    private ExpressionTreeBuilder _factory;
    private static TestCases __testCases = new TestCases();
  };

  public class TestCases : List<Tuple<String, String>> {

    public TestCases() {
      Add("substringof('text', StringValue) eq true", "x => (x.StringValue.Contains(\"text\") == True)");
      Add("StringValue eq 'a \"quote\"'", "x => (x.StringValue == \"a \"quote\"\")");
      Add("StringValue eq 'a \"quote\" within the text'", "x => (x.StringValue == \"a \"quote\" within the text\")");
      Add("StringValue eq 'a ''single quote within the text'", "x => (x.StringValue == \"a 'single quote within the text\")");
      Add("StringValue eq '''single quotes'' within the text'", "x => (x.StringValue == \"'single quotes' within the text\")");
      Add("true", "x => True");
      //Add("ChoiceValue eq Choice1", "x => ((Convert(x.ChoiceValue) & Convert(Choice1)) == Convert(Choice1))");
      Add("IntValue eq 1", "x => (x.IntValue == 1)");
      Add("(IntValue eq 1) and DoubleValue lt 2", "x => ((x.IntValue == 1) AndAlso (x.DoubleValue < 2))");
      Add("IntValue eq (10 mod 2)", "x => (x.IntValue == (10 % 2))");
      Add("(10 mod 2) eq IntValue", "x => ((10 % 2) == x.IntValue)");
      Add("IntValue ne 1", "x => (x.IntValue != 1)");
      Add("IntValue gt 1", "x => (x.IntValue > 1)");
      Add("-IntValue lt 1", "x => (-x.IntValue < 1)");
      Add("IntValue ge 1", "x => (x.IntValue >= 1)");
      Add("IntValue lt 1", "x => (x.IntValue < 1)");
      Add("IntValue le 1", "x => (x.IntValue <= 1)");
      Add("DoubleValue eq 1.2", "x => (x.DoubleValue == 1.2)");
      Add("DoubleValue eq (10 mod 2)", "x => (x.DoubleValue == Convert((10 % 2)))");
      Add("(10 mod 2) eq DoubleValue", "x => (Convert((10 % 2)) == x.DoubleValue)");
      Add("(DoubleValue mod 2) eq 10", "x => ((x.DoubleValue % 2) == 10)");
      Add("DoubleValue mod 2 eq 10", "x => ((x.DoubleValue % 2) == 10)");
      Add("DoubleValue ne 1.2", "x => (x.DoubleValue != 1.2)");
      Add("DoubleValue gt 1.2", "x => (x.DoubleValue > 1.2)");
      Add("DoubleValue ge 1.2", "x => (x.DoubleValue >= 1.2)");
      Add("DoubleValue lt 1.2", "x => (x.DoubleValue < 1.2)");
      Add("DoubleValue le 1.2", "x => (x.DoubleValue <= 1.2)");
      Add("DoubleValue eq 1", "x => (x.DoubleValue == 1)");
      Add("DoubleValue ne 1", "x => (x.DoubleValue != 1)");
      Add("DoubleValue gt 1", "x => (x.DoubleValue > 1)");
      Add("DoubleValue ge 1", "x => (x.DoubleValue >= 1)");
      Add("DoubleValue lt 1", "x => (x.DoubleValue < 1)");
      Add("DoubleValue le 1", "x => (x.DoubleValue <= 1)");
      Add("(DoubleValue add 2) eq 3", "x => ((x.DoubleValue + 2) == 3)");
      Add("(DoubleValue sub 2) eq 3", "x => ((x.DoubleValue - 2) == 3)");
      Add("(DoubleValue mul 2) eq 3", "x => ((x.DoubleValue * 2) == 3)");
      Add("(DoubleValue div 2) eq 3", "x => ((x.DoubleValue / 2) == 3)");
      Add("DoubleValue add 2 eq 3", "x => ((x.DoubleValue + 2) == 3)");
      Add("DoubleValue sub 2 eq 3", "x => ((x.DoubleValue - 2) == 3)");
      Add("DoubleValue mul 2 eq 3", "x => ((x.DoubleValue * 2) == 3)");
      Add("DoubleValue div 2 eq 3", "x => ((x.DoubleValue / 2) == 3)");
      Add("(DoubleValue div 2) mod 2 eq 3", "x => (((x.DoubleValue / 2) % 2) == 3)");
      Add("StringValue eq 1", "x => (x.StringValue == \"1\")");
      Add("StringValue eq '1'", "x => (x.StringValue == \"1\")");
      Add("StringValue eq 'something'", "x => (x.StringValue == \"something\")");
      Add("StringValue eq 'this and that'", "x => (x.StringValue == \"this and that\")");
      Add("StringValue eq 'Group1 foo Group2'", "x => (x.StringValue == \"Group1 foo Group2\")");
      Add("StringValue eq 'Group1 and Group2'", "x => (x.StringValue == \"Group1 and Group2\")");
      Add("StringValue eq 'Group1 or Group2'", "x => (x.StringValue == \"Group1 or Group2\")");
      Add("StringValue eq 'Group1 not Group2'", "x => (x.StringValue == \"Group1 not Group2\")");
      Add("StringValue ne 1", "x => (x.StringValue != \"1\")");
      Add("StringValue/Length eq 1", "x => (x.StringValue.Length == 1)");
      Add("StringValue/Length ne 1", "x => (x.StringValue.Length != 1)");
      Add("substringof('text', StringValue) eq true", "x => (x.StringValue.Contains(\"text\") == True)");
      Add("substringof('text', StringValue) ne true", "x => (x.StringValue.Contains(\"text\") != True)");
      Add("substringof('text', StringValue) eq false", "x => (x.StringValue.Contains(\"text\") == False)");
      Add("substringof('text', StringValue) ne false", "x => (x.StringValue.Contains(\"text\") != False)");
      Add("endswith(StringValue, 'text') eq true", "x => (x.StringValue.EndsWith(\"text\") == True)");
      Add("endswith(StringValue, 'text') ne true", "x => (x.StringValue.EndsWith(\"text\") != True)");
      Add("endswith(StringValue, 'text') eq false", "x => (x.StringValue.EndsWith(\"text\") == False)");
      Add("endswith(StringValue, 'text') ne false", "x => (x.StringValue.EndsWith(\"text\") != False)");
      Add("startswith(StringValue, 'text') eq true", "x => (x.StringValue.StartsWith(\"text\") == True)");
      Add("startswith(StringValue, 'text') ne true", "x => (x.StringValue.StartsWith(\"text\") != True)");
      Add("startswith(StringValue, 'text') eq false", "x => (x.StringValue.StartsWith(\"text\") == False)");
      Add("startswith(StringValue, 'text') ne false", "x => (x.StringValue.StartsWith(\"text\") != False)");
      Add("not length(StringValue) eq 1", "x => Not((x.StringValue.Length == 1))");
      Add("length(StringValue) eq 1", "x => (x.StringValue.Length == 1)");
      Add("length(StringValue) ne 1", "x => (x.StringValue.Length != 1)");
      Add("length(StringValue) gt 1", "x => (x.StringValue.Length > 1)");
      Add("length(StringValue) ge 1", "x => (x.StringValue.Length >= 1)");
      Add("length(StringValue) lt 1", "x => (x.StringValue.Length < 1)");
      Add("length(StringValue) le 1", "x => (x.StringValue.Length <= 1)");
      Add("length(concat(StringValue, 'text')) le 4", "x => (Concat(x.StringValue, \"text\").Length <= 4)");
      Add("replace(StringValue, 'text', 'zzz') eq 'foo'", "x => (x.StringValue.Replace(\"text\", \"zzz\") == \"foo\")");
      Add("indexof(StringValue, 'text') eq 1", "x => (x.StringValue.IndexOf(\"text\") == 1)");
      Add("indexof(StringValue, 'text') ne 1", "x => (x.StringValue.IndexOf(\"text\") != 1)");
      Add("indexof(StringValue, 'text') gt 1", "x => (x.StringValue.IndexOf(\"text\") > 1)");
      Add("indexof(StringValue, 'text') ge 1", "x => (x.StringValue.IndexOf(\"text\") >= 1)");
      Add("indexof(StringValue, 'text') lt 1", "x => (x.StringValue.IndexOf(\"text\") < 1)");
      Add("indexof(StringValue, 'text') le 1", "x => (x.StringValue.IndexOf(\"text\") <= 1)");
      Add("indexof('text', StringValue) eq 1", "x => (\"text\".IndexOf(x.StringValue) == 1)");
      Add("indexof('text', StringValue) ne 1", "x => (\"text\".IndexOf(x.StringValue) != 1)");
      Add("indexof('text', StringValue) gt 1", "x => (\"text\".IndexOf(x.StringValue) > 1)");
      Add("indexof('text', StringValue) ge 1", "x => (\"text\".IndexOf(x.StringValue) >= 1)");
      Add("indexof('text', StringValue) lt 1", "x => (\"text\".IndexOf(x.StringValue) < 1)");
      Add("indexof('text', StringValue) le 1", "x => (\"text\".IndexOf(x.StringValue) <= 1)");
      Add("substring(StringValue, 1) eq 'text'", "x => (x.StringValue.Substring(1) == \"text\")");
      Add("substring(StringValue, 1) ne 'text'", "x => (x.StringValue.Substring(1) != \"text\")");
      Add("substring(StringValue, 1) ne 'text' and IntValue eq 25",
          "x => ((x.StringValue.Substring(1) != \"text\") AndAlso (x.IntValue == 25))");
      Add("substring(StringValue, 1) ne 'text' and IntValue eq 25 and DoubleValue le 10",
          "x => (((x.StringValue.Substring(1) != \"text\") AndAlso (x.IntValue == 25)) AndAlso (x.DoubleValue <= 10))");
      Add("tolower(StringValue) ne 'text'", "x => (x.StringValue.ToLower() != \"text\")");
      Add("tolower(StringValue) eq 'text' and substring(StringValue, 1) ne 'text'",
          "x => ((x.StringValue.ToLower() == \"text\") AndAlso (x.StringValue.Substring(1) != \"text\"))");
      Add("toupper(StringValue) ne 'text'", "x => (x.StringValue.ToUpper() != \"text\")");
      Add("toupper(StringValue) eq 'text' and substring(StringValue, 1) ne 'text'",
          "x => ((x.StringValue.ToUpper() == \"text\") AndAlso (x.StringValue.Substring(1) != \"text\"))");
      Add("trim(StringValue) ne 'text'", "x => (x.StringValue.Trim() != \"text\")");
      Add("trim(StringValue) eq 'text' and substring(StringValue, 1) ne 'text'",
          "x => ((x.StringValue.Trim() == \"text\") AndAlso (x.StringValue.Substring(1) != \"text\"))");
      Add("hour(DateValue) eq 2", "x => (x.DateValue.Hour == 2)");
      Add("minute(DateValue) eq 2", "x => (x.DateValue.Minute == 2)");
      Add("second(DateValue) eq 2", "x => (x.DateValue.Second == 2)");
      Add("day(DateValue) eq 2", "x => (x.DateValue.Day == 2)");
      Add("month(DateValue) eq 2", "x => (x.DateValue.Month == 2)");
      Add("year(DateValue) eq 2011", "x => (x.DateValue.Year == 2011)");
      Add("round(DoubleValue) gt 1", "x => (Round(x.DoubleValue) > 1)");
      Add("floor(DoubleValue) gt 1", "x => (Floor(x.DoubleValue) > 1)");
      Add("ceiling(DoubleValue) gt 1", "x => (Ceiling(x.DoubleValue) > 1)");
      Add("round(DecimalValue) gt 1", "x => (Round(x.DecimalValue) > 1)");
      Add("floor(DecimalValue) gt 1", "x => (Floor(x.DecimalValue) > 1)");
      Add("ceiling(DecimalValue) gt 1", "x => (Ceiling(x.DecimalValue) > 1)");
      Add("(StringValue ne 'text') or IntValue gt 2", "x => ((x.StringValue != \"text\") OrElse (x.IntValue > 2))");
      Add(
        "(startswith(tolower(StringValue),'foo') eq true and endswith(tolower(StringValue),'1') eq true) and (tolower(StringValue) eq 'bar03')",
        "x => (((x.StringValue.ToLower().StartsWith(\"foo\") == True) AndAlso (x.StringValue.ToLower().EndsWith(\"1\") == True)) AndAlso (x.StringValue.ToLower() == \"bar03\"))");
      Add(
        "(startswith(tolower(StringValue),'foo') and endswith(tolower(StringValue),'1')) and (tolower(StringValue) eq 'bar03')",
        "x => ((x.StringValue.ToLower().StartsWith(\"foo\") AndAlso x.StringValue.ToLower().EndsWith(\"1\")) AndAlso (x.StringValue.ToLower() == \"bar03\"))");
      Add("startswith(tolower(StringValue),'foo')", "x => x.StringValue.ToLower().StartsWith(\"foo\")");
      Add("Children/any(a: a/ChildStringValue eq 'foo')", "x => x.Children.Any(a => (a.ChildStringValue == \"foo\"))");
      Add("Children/all(y: y/Children/all(z: z/GrandChildStringValue eq 'foo'))",
          "x => x.Children.All(y => y.Children.All(z => (z.GrandChildStringValue == \"foo\")))");
      Add("Children/all(y: y/Children/any(z: z/GrandChildStringValue eq 'foo'))",
          "x => x.Children.All(y => y.Children.Any(z => (z.GrandChildStringValue == \"foo\")))");
      Add("Children/any(y: y/Children/all(z: z/GrandChildStringValue eq 'foo'))",
          "x => x.Children.Any(y => y.Children.All(z => (z.GrandChildStringValue == \"foo\")))");
      Add("Children/any(a: startswith(tolower(a/ChildStringValue), 'foo'))",
          "x => x.Children.Any(a => a.ChildStringValue.ToLower().StartsWith(\"foo\"))");
      Add("Children/all(a: startswith(tolower(a/ChildStringValue), 'foo'))",
          "x => x.Children.All(a => a.ChildStringValue.ToLower().StartsWith(\"foo\"))");
      Add(
        "Children/all(a: startswith(tolower(a/ChildStringValue), 'foo') and endswith(tolower(a/ChildStringValue), 'foo'))",
        "x => x.Children.All(a => (a.ChildStringValue.ToLower().StartsWith(\"foo\") AndAlso a.ChildStringValue.ToLower().EndsWith(\"foo\")))");
      Add("Children/any(a: a/Children/any(b: startswith(tolower(b/GrandChildStringValue), 'foo')))",
          "x => x.Children.Any(a => a.Children.Any(b => b.GrandChildStringValue.ToLower().StartsWith(\"foo\")))");
      Add("Children/any(a: startswith(tolower(a/ChildStringValue), StringValue))",
          "x => x.Children.Any(a => a.ChildStringValue.ToLower().StartsWith(x.StringValue))");
      Add("Children/all(y: y/ID eq 2 add ID)", "x => x.Children.All(y => (y.ID == (2 + x.ID)))");
      Add("DateValue eq datetime'2012-05-06T16:11:00Z'", "x => (x.DateValue == 5/6/2012 4:11:00 PM)");
      Add("DateValue eq datetime'2012-05-06T16:11:00Z'", "x => (x.DateValue == 5/6/2012 4:11:00 PM)");
      Add("Duration eq time'PT2H15M'", "x => (x.Duration == 02:15:00)");
      Add("PointInTime eq datetimeoffset'2012-05-06T18:10:00+02:00'",
          "x => (x.PointInTime == 5/6/2012 6:10:00 PM +02:00)");
    }

    

    public void Add(String input, String output) {
      this.Add(Tuple.Create(input, output));
    }

  }
}

    //[TestCase("StringValue eq 'a \"quote\"'", "x => (x.StringValue == \"a \"quote\"\")")]
    //[TestCase("StringValue eq 'a \"quote\"'", "x => (x.StringValue == \"a \"quote\"\")")]
    //[TestCase("StringValue eq 'a \"quote\" within the text'", "x => (x.StringValue == \"a \"quote\" within the text\")")]
    //[TestCase("StringValue eq 'a 'single quote' within the text'", "x => (x.StringValue == \"a 'single quote' within the text\")")]
    //[TestCase("true", "x => True")]
    //[TestCase("ChoiceValue eq This", "x => ((Convert(x.ChoiceValue) & Convert(This)) == Convert(This))")]
    //[TestCase("IntValue eq 1", "x => (x.IntValue == 1)")]
    //[TestCase("(IntValue eq 1) and DoubleValue lt 2", "x => ((x.IntValue == 1) AndAlso (x.DoubleValue < 2))")]
    //[TestCase("IntValue eq (10 mod 2)", "x => (x.IntValue == (10 % 2))")]
    //[TestCase("(10 mod 2) eq IntValue", "x => ((10 % 2) == x.IntValue)")]
    //[TestCase("IntValue ne 1", "x => (x.IntValue != 1)")]
    //[TestCase("IntValue gt 1", "x => (x.IntValue > 1)")]
    //[TestCase("-IntValue lt 1", "x => (-x.IntValue < 1)")]
    //[TestCase("IntValue ge 1", "x => (x.IntValue >= 1)")]
    //[TestCase("IntValue lt 1", "x => (x.IntValue < 1)")]
    //[TestCase("IntValue le 1", "x => (x.IntValue <= 1)")]
    //[TestCase("DoubleValue eq 1.2", "x => (x.DoubleValue == 1.2)")]
    //[TestCase("DoubleValue eq (10 mod 2)", "x => (x.DoubleValue == (10 % 2))")]
    //[TestCase("(10 mod 2) eq DoubleValue", "x => ((10 % 2) == x.DoubleValue)")]
    //[TestCase("(DoubleValue mod 2) eq 10", "x => ((x.DoubleValue % 2) == 10)")]
    //[TestCase("DoubleValue mod 2 eq 10", "x => ((x.DoubleValue % 2) == 10)")]
    //[TestCase("DoubleValue ne 1.2", "x => (x.DoubleValue != 1.2)")]
    //[TestCase("DoubleValue gt 1.2", "x => (x.DoubleValue > 1.2)")]
    //[TestCase("DoubleValue ge 1.2", "x => (x.DoubleValue >= 1.2)")]
    //[TestCase("DoubleValue lt 1.2", "x => (x.DoubleValue < 1.2)")]
    //[TestCase("DoubleValue le 1.2", "x => (x.DoubleValue <= 1.2)")]
    //[TestCase("DoubleValue eq 1", "x => (x.DoubleValue == 1)")]
    //[TestCase("DoubleValue ne 1", "x => (x.DoubleValue != 1)")]
    //[TestCase("DoubleValue gt 1", "x => (x.DoubleValue > 1)")]
    //[TestCase("DoubleValue ge 1", "x => (x.DoubleValue >= 1)")]
    //[TestCase("DoubleValue lt 1", "x => (x.DoubleValue < 1)")]
    //[TestCase("DoubleValue le 1", "x => (x.DoubleValue <= 1)")]
    //[TestCase("(DoubleValue add 2) eq 3", "x => ((x.DoubleValue + 2) == 3)")]
    //[TestCase("(DoubleValue sub 2) eq 3", "x => ((x.DoubleValue - 2) == 3)")]
    //[TestCase("(DoubleValue mul 2) eq 3", "x => ((x.DoubleValue * 2) == 3)")]
    //[TestCase("(DoubleValue div 2) eq 3", "x => ((x.DoubleValue / 2) == 3)")]
    //[TestCase("DoubleValue add 2 eq 3", "x => ((x.DoubleValue + 2) == 3)")]
    //[TestCase("DoubleValue sub 2 eq 3", "x => ((x.DoubleValue - 2) == 3)")]
    //[TestCase("DoubleValue mul 2 eq 3", "x => ((x.DoubleValue * 2) == 3)")]
    //[TestCase("DoubleValue div 2 eq 3", "x => ((x.DoubleValue / 2) == 3)")]
    //[TestCase("(DoubleValue div 2) mod 2 eq 3", "x => (((x.DoubleValue / 2) % 2) == 3)")]
    //[TestCase("StringValue eq 1", "x => (x.StringValue == \"1\")")]
    //[TestCase("StringValue eq '1'", "x => (x.StringValue == \"1\")")]
    //[TestCase("StringValue eq 'something'", "x => (x.StringValue == \"something\")")]
    //[TestCase("StringValue eq 'this and that'", "x => (x.StringValue == \"this and that\")")]
    //[TestCase("StringValue eq 'Group1 foo Group2'", "x => (x.StringValue == \"Group1 foo Group2\")")]
    //[TestCase("StringValue eq 'Group1 and Group2'", "x => (x.StringValue == \"Group1 and Group2\")")]
    //[TestCase("StringValue eq 'Group1 or Group2'", "x => (x.StringValue == \"Group1 or Group2\")")]
    //[TestCase("StringValue eq 'Group1 not Group2'", "x => (x.StringValue == \"Group1 not Group2\")")]
    //[TestCase("StringValue ne 1", "x => (x.StringValue != \"1\")")]
    //[TestCase("StringValue/Length eq 1", "x => (x.StringValue.Length == 1)")]
    //[TestCase("StringValue/Length ne 1", "x => (x.StringValue.Length != 1)")]
    //[TestCase("substringof('text', StringValue) eq true", "x => (x.StringValue.Contains(\"text\") == True)")]
    //[TestCase("substringof('text', StringValue) ne true", "x => (x.StringValue.Contains(\"text\") != True)")]
    //[TestCase("substringof('text', StringValue) eq false", "x => (x.StringValue.Contains(\"text\") == False)")]
    //[TestCase("substringof('text', StringValue) ne false", "x => (x.StringValue.Contains(\"text\") != False)")]
    //[TestCase("endswith(StringValue, 'text') eq true", "x => (x.StringValue.EndsWith(\"text\", OrdinalIgnoreCase) == True)")]
    //[TestCase("endswith(StringValue, 'text') ne true", "x => (x.StringValue.EndsWith(\"text\", OrdinalIgnoreCase) != True)")]
    //[TestCase("endswith(StringValue, 'text') eq false", "x => (x.StringValue.EndsWith(\"text\", OrdinalIgnoreCase) == False)")]
    //[TestCase("endswith(StringValue, 'text') ne false", "x => (x.StringValue.EndsWith(\"text\", OrdinalIgnoreCase) != False)")]
    //[TestCase("startswith(StringValue, 'text') eq true", "x => (x.StringValue.StartsWith(\"text\", OrdinalIgnoreCase) == True)")]
    //[TestCase("startswith(StringValue, 'text') ne true", "x => (x.StringValue.StartsWith(\"text\", OrdinalIgnoreCase) != True)")]
    //[TestCase("startswith(StringValue, 'text') eq false", "x => (x.StringValue.StartsWith(\"text\", OrdinalIgnoreCase) == False)")]
    //[TestCase("startswith(StringValue, 'text') ne false", "x => (x.StringValue.StartsWith(\"text\", OrdinalIgnoreCase) != False)")]
    //[TestCase("not length(StringValue) eq 1", "x => Not((x.StringValue.Length == 1))")]
    //[TestCase("length(StringValue) eq 1", "x => (x.StringValue.Length == 1)")]
    //[TestCase("length(StringValue) ne 1", "x => (x.StringValue.Length != 1)")]
    //[TestCase("length(StringValue) gt 1", "x => (x.StringValue.Length > 1)")]
    //[TestCase("length(StringValue) ge 1", "x => (x.StringValue.Length >= 1)")]
    //[TestCase("length(StringValue) lt 1", "x => (x.StringValue.Length < 1)")]
    //[TestCase("length(StringValue) le 1", "x => (x.StringValue.Length <= 1)")]
    //[TestCase("indexof(StringValue, 'text') eq 1", "x => (x.StringValue.IndexOf(\"text\", OrdinalIgnoreCase) == 1)")]
    //[TestCase("indexof(StringValue, 'text') ne 1", "x => (x.StringValue.IndexOf(\"text\", OrdinalIgnoreCase) != 1)")]
    //[TestCase("indexof(StringValue, 'text') gt 1", "x => (x.StringValue.IndexOf(\"text\", OrdinalIgnoreCase) > 1)")]
    //[TestCase("indexof(StringValue, 'text') ge 1", "x => (x.StringValue.IndexOf(\"text\", OrdinalIgnoreCase) >= 1)")]
    //[TestCase("indexof(StringValue, 'text') lt 1", "x => (x.StringValue.IndexOf(\"text\", OrdinalIgnoreCase) < 1)")]
    //[TestCase("indexof(StringValue, 'text') le 1", "x => (x.StringValue.IndexOf(\"text\", OrdinalIgnoreCase) <= 1)")]
    //[TestCase("indexof('text', StringValue) eq 1", "x => (\"text\".IndexOf(x.StringValue, OrdinalIgnoreCase) == 1)")]
    //[TestCase("indexof('text', StringValue) ne 1", "x => (\"text\".IndexOf(x.StringValue, OrdinalIgnoreCase) != 1)")]
    //[TestCase("indexof('text', StringValue) gt 1", "x => (\"text\".IndexOf(x.StringValue, OrdinalIgnoreCase) > 1)")]
    //[TestCase("indexof('text', StringValue) ge 1", "x => (\"text\".IndexOf(x.StringValue, OrdinalIgnoreCase) >= 1)")]
    //[TestCase("indexof('text', StringValue) lt 1", "x => (\"text\".IndexOf(x.StringValue, OrdinalIgnoreCase) < 1)")]
    //[TestCase("indexof('text', StringValue) le 1", "x => (\"text\".IndexOf(x.StringValue, OrdinalIgnoreCase) <= 1)")]
    //[TestCase("substring(StringValue, 1) eq 'text'", "x => (x.StringValue.Substring(1) == \"text\")")]
    //[TestCase("substring(StringValue, 1) ne 'text'", "x => (x.StringValue.Substring(1) != \"text\")")]
    //[TestCase("substring(StringValue, 1) ne 'text' and IntValue eq 25", "x => ((x.StringValue.Substring(1) != \"text\") AndAlso (x.IntValue == 25))")]
    //[TestCase("substring(StringValue, 1) ne 'text' and IntValue eq 25 and DoubleValue le 10", "x => (((x.StringValue.Substring(1) != \"text\") AndAlso (x.IntValue == 25)) AndAlso (x.DoubleValue <= 10))")]
    //[TestCase("tolower(StringValue) ne 'text'", "x => (x.StringValue.ToLowerInvariant() != \"text\")")]
    //[TestCase("tolower(StringValue) eq 'text' and substring(StringValue, 1) ne 'text'", "x => ((x.StringValue.ToLowerInvariant() == \"text\") AndAlso (x.StringValue.Substring(1) != \"text\"))")]
    //[TestCase("toupper(StringValue) ne 'text'", "x => (x.StringValue.ToUpperInvariant() != \"text\")")]
    //[TestCase("toupper(StringValue) eq 'text' and substring(StringValue, 1) ne 'text'", "x => ((x.StringValue.ToUpperInvariant() == \"text\") AndAlso (x.StringValue.Substring(1) != \"text\"))")]
    //[TestCase("trim(StringValue) ne 'text'", "x => (x.StringValue.Trim() != \"text\")")]
    //[TestCase("trim(StringValue) eq 'text' and substring(StringValue, 1) ne 'text'", "x => ((x.StringValue.Trim() == \"text\") AndAlso (x.StringValue.Substring(1) != \"text\"))")]
    //[TestCase("hour(DateValue) eq 2", "x => (x.DateValue.Hour == 2)")]
    //[TestCase("minute(DateValue) eq 2", "x => (x.DateValue.Minute == 2)")]
    //[TestCase("second(DateValue) eq 2", "x => (x.DateValue.Second == 2)")]
    //[TestCase("day(DateValue) eq 2", "x => (x.DateValue.Day == 2)")]
    //[TestCase("month(DateValue) eq 2", "x => (x.DateValue.Month == 2)")]
    //[TestCase("year(DateValue) eq 2011", "x => (x.DateValue.Year == 2011)")]
    //[TestCase("round(DoubleValue) gt 1", "x => (Round(x.DoubleValue) > 1)")]
    //[TestCase("floor(DoubleValue) gt 1", "x => (Floor(x.DoubleValue) > 1)")]
    //[TestCase("ceiling(DoubleValue) gt 1", "x => (Ceiling(x.DoubleValue) > 1)")]
    //[TestCase("round(DecimalValue) gt 1", "x => (Round(x.DecimalValue) > 1)")]
    //[TestCase("floor(DecimalValue) gt 1", "x => (Floor(x.DecimalValue) > 1)")]
    //[TestCase("ceiling(DecimalValue) gt 1", "x => (Ceiling(x.DecimalValue) > 1)")]
    //[TestCase("(StringValue ne 'text') or IntValue gt 2", "x => ((x.StringValue != \"text\") OrElse (x.IntValue > 2))")]
    //[TestCase("(startswith(tolower(StringValue),'foo') eq true and endswith(tolower(StringValue),'1') eq true) and (tolower(StringValue) eq 'bar03')", "x => (((x.StringValue.ToLowerInvariant().StartsWith(\"foo\", OrdinalIgnoreCase) == True) AndAlso (x.StringValue.ToLowerInvariant().EndsWith(\"1\", OrdinalIgnoreCase) == True)) AndAlso (x.StringValue.ToLowerInvariant() == \"bar03\"))")]
    //[TestCase("(startswith(tolower(StringValue),'foo') and endswith(tolower(StringValue),'1')) and (tolower(StringValue) eq 'bar03')", "x => ((x.StringValue.ToLowerInvariant().StartsWith(\"foo\", OrdinalIgnoreCase) AndAlso x.StringValue.ToLowerInvariant().EndsWith(\"1\", OrdinalIgnoreCase)) AndAlso (x.StringValue.ToLowerInvariant() == \"bar03\"))")]
    //[TestCase("startswith(tolower(StringValue),'foo')", "x => x.StringValue.ToLowerInvariant().StartsWith(\"foo\", OrdinalIgnoreCase)")]
    //[TestCase("Children/any(a: a/ChildStringValue eq 'foo')", "x => x.Children.Any(a => (a.ChildStringValue == \"foo\"))")]
    //[TestCase("Children/all(y: y/Children/all(z: z/GrandChildStringValue eq 'foo'))", "x => x.Children.All(y => y.Children.All(z => (z.GrandChildStringValue == \"foo\")))")]
    //[TestCase("Children/all(y: y/Children/any(z: z/GrandChildStringValue eq 'foo'))", "x => x.Children.All(y => y.Children.Any(z => (z.GrandChildStringValue == \"foo\")))")]
    //[TestCase("Children/any(y: y/Children/all(z: z/GrandChildStringValue eq 'foo'))", "x => x.Children.Any(y => y.Children.All(z => (z.GrandChildStringValue == \"foo\")))")]
    //[TestCase("Children/any(a: startswith(tolower(a/ChildStringValue), 'foo'))", "x => x.Children.Any(a => a.ChildStringValue.ToLowerInvariant().StartsWith(\"foo\", OrdinalIgnoreCase))")]
    //[TestCase("Children/all(a: startswith(tolower(a/ChildStringValue), 'foo'))", "x => x.Children.All(a => a.ChildStringValue.ToLowerInvariant().StartsWith(\"foo\", OrdinalIgnoreCase))")]
    //[TestCase("Children/all(a: startswith(tolower(a/ChildStringValue), 'foo') and endswith(tolower(a/ChildStringValue), 'foo'))", "x => x.Children.All(a => (a.ChildStringValue.ToLowerInvariant().StartsWith(\"foo\", OrdinalIgnoreCase) AndAlso a.ChildStringValue.ToLowerInvariant().EndsWith(\"foo\", OrdinalIgnoreCase)))")]
    //[TestCase("Children/any(a: a/Children/any(b: startswith(tolower(b/GrandChildStringValue), 'foo')))", "x => x.Children.Any(a => a.Children.Any(b => b.GrandChildStringValue.ToLowerInvariant().StartsWith(\"foo\", OrdinalIgnoreCase)))")]
    //[TestCase("Children/any(a: startswith(tolower(a/ChildStringValue), StringValue))", "x => x.Children.Any(a => a.ChildStringValue.ToLowerInvariant().StartsWith(x.StringValue, OrdinalIgnoreCase))")]
    //[TestCase("Children/all(y: y/ID eq 2 add ID)", "x => x.Children.All(y => (y.ID == (2 + x.ID)))")]
    //[TestCase("DateValue eq datetime'2012-05-06T16:11:00Z'", "x => (x.DateValue == 5/6/2012 4:11:00 PM)")]
    //[TestCase("DateValue eq datetime'2012-05-06T16:11:00Z'", "x => (x.DateValue == 5/6/2012 4:11:00 PM)")]
    //[TestCase("Duration eq time'PT2H15M'", "x => (x.Duration == 02:15:00)")]
    //[TestCase("PointInTime eq datetimeoffset'2012-05-06T18:10:00+02:00'", "x => (x.PointInTime == 5/6/2012 6:10:00 PM +02:00)")]
  