using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
	public enum ChartIndicatorType
	{
		//Simple indicators
		RegressionLine,
		TrendLine,
		
		//Price indicators
		MedianPrice,
		TypicalPrice,
		WeightedClose,

		//Fibonacci indicators
		Fibonacci,

		//Moving Average and Envelope
		EMA,
		ExponentialMovingAverage,
		SMA,	
		SimpleMovingAverage,
		TMA,
		TriangularMovingAverage,
		TEMA,
		TripleExponentialMovingAverageTema,
		WMA,
		WeightedMovingAverage,

		//Trend indicators
		BollingerBands,
		MassIndex,
		StandardDeviation,	//separate panel

		//Oscillator indicators - separate panel indicators
		ATR,
		AverageTrueRange,
		CHV,
		ChaikinsVolatility,
		CCI,
		CommodityChannelIndex,
		DPO,
		DetrendedPriceOscillator,
		MACD,
		MovingAverageConvergenceDivergence,
		ROC,
		RateOfChange,
		RSI,
		RelativeStrengthIndex,
		TRIX,
		TripleExponentialMovingAverageTrix,
		WilliamsR,

		//Error Bars
		DataSourceBasedErrorBars,
		FixedValueErrorBars,
		PercentageErrorBars,
		StandardDeviationErrorBars,
		StandardErrorBars
	}
}
