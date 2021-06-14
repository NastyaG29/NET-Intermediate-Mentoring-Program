// Decompiled with JetBrains decompiler
// Type: MyCalculatorv1.Properties.Resources
// Assembly: MyCalculatorv1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E4247A5-25E4-47A6-84F4-A414933F7536
// Assembly location: C:\Workspace\Mentoring\NET_In\NET-Intermediate-Mentoring-Program\Analyzing and profiling tools\DumpHomework\MyCalculator.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace MyCalculatorv1.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (MyCalculatorv1.Properties.Resources.resourceMan == null)
          MyCalculatorv1.Properties.Resources.resourceMan = new ResourceManager("MyCalculatorv1.Properties.Resources", typeof (MyCalculatorv1.Properties.Resources).Assembly);
        return MyCalculatorv1.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return MyCalculatorv1.Properties.Resources.resourceCulture;
      }
      set
      {
        MyCalculatorv1.Properties.Resources.resourceCulture = value;
      }
    }
  }
}
