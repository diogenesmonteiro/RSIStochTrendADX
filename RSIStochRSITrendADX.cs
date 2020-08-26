#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.Indicators;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

namespace NinjaTrader.NinjaScript.Strategies
{
	public class RSIStochRSITrendADXTimeCode : Strategy
	{
		private RSI RSI1;
		private StochRSI StochRSI1;
		private ADX ADX1;
		private BarSpeed BarSpeed1;
		private EMA EMA1;
		private MACD MACD1;
		private SMA SMA1;

		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description					= @"Enter the description for your new custom Strategy here.";
				Name						= "RSIStochRSITrendADXTimeCode";
				Calculate					= Calculate.OnBarClose;
				EntriesPerDirection				= 1;
				EntryHandling					= EntryHandling.AllEntries;
				IsExitOnSessionCloseStrategy			= true;
				ExitOnSessionCloseSeconds			= 30;
				IsFillLimitOnTouch				= false;
				MaximumBarsLookBack				= MaximumBarsLookBack.TwoHundredFiftySix;
				OrderFillResolution				= OrderFillResolution.Standard;
				Slippage					= 0;
				StartBehavior					= StartBehavior.WaitUntilFlat;
				TimeInForce					= TimeInForce.Gtc;
				TraceOrders					= false;
				RealtimeErrorHandling				= RealtimeErrorHandling.StopCancelClose;
				StopTargetHandling				= StopTargetHandling.PerEntryExecution;
				BarsRequiredToTrade				= 20;
				// Disable this property for performance gains in Strategy Analyzer optimizations
				IsInstantiatedOnEachOptimizationIteration	= true;
				StopLoss					= 8;
				TakeProfit					= 4;
				RSIOverBought					= 65;
				RSIOverSold					= 35;
				RSIMedium					= 50;
				StochRSIOverBought				= 0.9;
				StochRSIOverSold				= 0.1;
				StochRSIMedium					= 0.5;
				MAPeriod					= 200;
			}
			else if (State == State.Configure)
			{
			}
			else if (State == State.DataLoaded)
			{				
				RSI1					= RSI(Close, 14, 3);
				StochRSI1				= StochRSI(Close, 14);
				ADX1					= ADX(Close, 14);
				BarSpeed1				= BarSpeed(Close);
				EMA1					= EMA(Close, Convert.ToInt32(MAPeriod));
				MACD1					= MACD(Close, 12, 26, 9);
				SMA1					= SMA(Close, Convert.ToInt32(MAPeriod));
				ADX1.Plots[0].Brush 			= Brushes.DarkCyan;
				BarSpeed1.Plots[0].Brush		= Brushes.Blue;
				AddChartIndicator(ADX1);
				AddChartIndicator(BarSpeed1);
				SetProfitTarget("", CalculationMode.Ticks, TakeProfit);
				SetStopLoss("", CalculationMode.Ticks, StopLoss, false);
			}
		}

		protected override void OnBarUpdate()
		{
			if (BarsInProgress != 0) 
				return;

			if (CurrentBars[0] < 3)
				return;

			 // Set 1
			if ((CrossAbove(RSI1.Default, RSIOverSold, 1))
				 && (CrossAbove(StochRSI1, StochRSIOverBought, 1))
				 && (ADX1[0] > ADX1[1])
				 && (BarSpeed1[0] < BarSpeed1[1])
				 && (EMA1[0] > EMA1[1]))
			{
				EnterLong(Convert.ToInt32(DefaultQuantity), "");
			}
			
			 // Set 2
			if ((CrossBelow(RSI1.Default, RSIOverBought, 1))
				 && (CrossBelow(StochRSI1, StochRSIOverSold, 1))
				 && (ADX1[0] > ADX1[1])
				 && (BarSpeed1[0] < BarSpeed1[1])
				 && (EMA1[0] < EMA1[1]))
			{
				EnterShort(Convert.ToInt32(DefaultQuantity), "");
			}
			
			 // Set 3
			if (CrossAbove(RSI1.Default, RSIOverSold, 1))
			{
			}
			
			 // Set 4
			if (CrossAbove(StochRSI1, StochRSIOverBought, 1))
			{
			}
			
			 // Set 5
			if (MACD1.Diff[0] > MACD1.Diff[1])
			{
			}
			
			 // Set 6
			if (SMA1[0] > SMA1[2])
			{
			}
			
			 // Set 7
			if (CrossBelow(RSI1.Default, RSIOverBought, 1))
			{
			}
			
			 // Set 8
			if (CrossBelow(StochRSI1, StochRSIOverSold, 1))
			{
			}
			
			 // Set 9
			if (MACD1.Diff[0] < MACD1.Diff[1])
			{
			}
			
			 // Set 10
			if (SMA1[0] < SMA1[2])
			{
			}
			
		}

		#region Properties
		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="StopLoss", Order=1, GroupName="Parameters")]
		public int StopLoss
		{ get; set; }

		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="TakeProfit", Order=2, GroupName="Parameters")]
		public int TakeProfit
		{ get; set; }

		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="RSIOverBought", Order=3, GroupName="Parameters")]
		public int RSIOverBought
		{ get; set; }

		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="RSIOverSold", Order=4, GroupName="Parameters")]
		public int RSIOverSold
		{ get; set; }

		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="RSIMedium", Order=5, GroupName="Parameters")]
		public int RSIMedium
		{ get; set; }

		[NinjaScriptProperty]
		[Range(0, double.MaxValue)]
		[Display(Name="StochRSIOverBought", Order=6, GroupName="Parameters")]
		public double StochRSIOverBought
		{ get; set; }

		[NinjaScriptProperty]
		[Range(0, double.MaxValue)]
		[Display(Name="StochRSIOverSold", Order=7, GroupName="Parameters")]
		public double StochRSIOverSold
		{ get; set; }

		[NinjaScriptProperty]
		[Range(0.5, double.MaxValue)]
		[Display(Name="StochRSIMedium", Order=8, GroupName="Parameters")]
		public double StochRSIMedium
		{ get; set; }

		[NinjaScriptProperty]
		[Range(0, int.MaxValue)]
		[Display(Name="MAPeriod", Order=9, GroupName="Parameters")]
		public int MAPeriod
		{ get; set; }
		#endregion

	}
}
