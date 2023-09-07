// See https://aka.ms/new-console-template for more information
using ASignInSpace;

var dist = new Distances();
dist.Width = 256;
dist.Height = 256;
dist.Run(BitArrayHelper.GetStarmap());
Console.WriteLine("Program finished.");