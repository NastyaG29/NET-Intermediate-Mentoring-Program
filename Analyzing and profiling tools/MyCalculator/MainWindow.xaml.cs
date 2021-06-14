// Decompiled with JetBrains decompiler
// Type: MyCalculatorv1.MainWindow
// Assembly: MyCalculatorv1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E4247A5-25E4-47A6-84F4-A414933F7536
// Assembly location: C:\Workspace\Mentoring\NET_In\NET-Intermediate-Mentoring-Program\Analyzing and profiling tools\DumpHomework\MyCalculator.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace MyCalculatorv1
{
  public partial class MainWindow : Window, IComponentConnector
  {
    public MainWindow()
    {
      this.InitializeComponent();
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      this.tb.Text += ((Button) sender).Content.ToString();
    }

    private void Result_click(object sender, RoutedEventArgs e)
    {
      this.result();
    }

    private void result()
    {
      int num1 = 0;
      if (this.tb.Text.Contains("+"))
        num1 = this.tb.Text.IndexOf("+");
      else if (this.tb.Text.Contains("-"))
        num1 = this.tb.Text.IndexOf("-");
      else if (this.tb.Text.Contains("*"))
        num1 = this.tb.Text.IndexOf("*");
      else if (this.tb.Text.Contains("/"))
        num1 = this.tb.Text.IndexOf("/");
      string str = this.tb.Text.Substring(num1, 1);

      //double num2 = Convert.ToDouble(this.tb.Text.Substring(0, num1));
      //double num3 = Convert.ToDouble(this.tb.Text.Substring(num1 + 1, this.tb.Text.Length - num1 - 1));

      if ( !double.TryParse(this.tb.Text.Substring(0, num1), out var num2) || 
           !double.TryParse(this.tb.Text.Substring(num1 + 1, this.tb.Text.Length - num1 - 1), out var num3))
      {
          return;
      }

      if (str == "+")
      {
        TextBox tb = this.tb;
        tb.Text = tb.Text + "=" + (object) (num2 + num3);
      }
      else if (str == "-")
      {
        TextBox tb = this.tb;
        tb.Text = tb.Text + "=" + (object) (num2 - num3);
      }
      else if (str == "*")
      {
        TextBox tb = this.tb;
        tb.Text = tb.Text + "=" + (object) (num2 * num3);
      }
      else
      {
        TextBox tb = this.tb;
        tb.Text = tb.Text + "=" + (object) (num2 / num3);
      }
    }

    private void Off_Click_1(object sender, RoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }

    private void Del_Click(object sender, RoutedEventArgs e)
    {
      this.tb.Text = "";
    }

    private void R_Click(object sender, RoutedEventArgs e)
    {
      if (this.tb.Text.Length <= 0)
        return;
      this.tb.Text = this.tb.Text.Substring(0, this.tb.Text.Length - 1);
    }
  }
}
