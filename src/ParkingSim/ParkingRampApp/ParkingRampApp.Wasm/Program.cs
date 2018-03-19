using System;
using Ooui;
using Xamarin.Forms;

namespace ParkingRampApp.Wasm
{
  class Program
  {
    static void Main(string[] args)
    {
      // Initialize Xamarin.Forms
      Forms.Init();

      // Create the UI
      var page = new ParkingRampApp.MainPage();

      // Publish a root element to be displayed
      UI.Publish("/", page.GetOouiElement());
    }
  }
}
