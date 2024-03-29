﻿using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.HelperClasses;

namespace TradingBotCS.IBApi_OverRide
{
    public class OrderOverride : Order
    {
        private static string Name = "OrderOverride";


        public DateTime DateTime { get; set; }

        public OrderOverride()
        {
            Order order = new Order();
            try
            {
                DateTime = DateTime.Now;
                LmtPrice = order.LmtPrice;
                AuxPrice = order.AuxPrice;
                ActiveStartTime = order.ActiveStartTime;
                ActiveStopTime = order.ActiveStopTime;
                OutsideRth = order.OutsideRth;
                OpenClose = order.OpenClose;
                Origin = order.Origin;
                Transmit = order.Transmit;
                DesignatedLocation = order.DesignatedLocation;
                ExemptCode = order.ExemptCode;
                MinQty = order.MinQty;
                PercentOffset = order.PercentOffset;
                NbboPriceCap = order.NbboPriceCap;
                OptOutSmartRouting = order.OptOutSmartRouting;
                StartingPrice = order.StartingPrice;
                StockRefPrice = order.StockRefPrice;
                Delta = order.Delta;
                StockRangeLower = order.StockRangeLower;
                StockRangeUpper = order.StockRangeUpper;
                Volatility = order.Volatility;
                VolatilityType = order.VolatilityType;
                DeltaNeutralOrderType = order.DeltaNeutralOrderType;
                DeltaNeutralAuxPrice = order.DeltaNeutralAuxPrice;
                DeltaNeutralConId = order.DeltaNeutralConId;
                DeltaNeutralSettlingFirm = order.DeltaNeutralSettlingFirm;
                DeltaNeutralClearingAccount = order.DeltaNeutralClearingAccount;
                DeltaNeutralClearingIntent = order.DeltaNeutralClearingIntent;
                DeltaNeutralOpenClose = order.DeltaNeutralOpenClose;
                DeltaNeutralShortSale = order.DeltaNeutralShortSale;
                DeltaNeutralShortSaleSlot = order.DeltaNeutralShortSaleSlot;
                DeltaNeutralDesignatedLocation = order.DeltaNeutralDesignatedLocation;
                ReferencePriceType = order.ReferencePriceType;
                TrailStopPrice = order.TrailStopPrice;
                TrailingPercent = order.TrailingPercent;
                BasisPoints = order.BasisPoints;
                BasisPointsType = order.BasisPointsType;
                ScaleInitLevelSize = order.ScaleInitLevelSize;
                ScaleSubsLevelSize = order.ScaleSubsLevelSize;
                ScalePriceIncrement = order.ScalePriceIncrement;
                ScalePriceAdjustValue = order.ScalePriceAdjustValue;
                ScalePriceAdjustInterval = order.ScalePriceAdjustInterval;
                ScaleProfitOffset = order.ScaleProfitOffset;
                ScaleAutoReset = order.ScaleAutoReset;
                ScaleInitPosition = order.ScaleInitPosition;
                ScaleInitFillQty = order.ScaleInitFillQty;
                ScaleRandomPercent = order.ScaleRandomPercent;
                ScaleTable = order.ScaleTable;
                WhatIf = order.WhatIf;
                NotHeld = order.NotHeld;
                Conditions = order.Conditions;
                TriggerPrice = order.TriggerPrice;
                LmtPriceOffset = order.LmtPriceOffset;
                AdjustedStopPrice = order.AdjustedStopPrice;
                AdjustedStopLimitPrice = order.AdjustedStopLimitPrice;
                AdjustedTrailingAmount = order.AdjustedTrailingAmount;
                ExtOperator = order.ExtOperator;
                Tier = order.Tier;
                CashQty = order.CashQty;
                Mifid2DecisionMaker = order.Mifid2DecisionMaker;
                Mifid2DecisionAlgo = order.Mifid2DecisionAlgo;
                Mifid2ExecutionTrader = order.Mifid2ExecutionTrader;
                Mifid2ExecutionAlgo = order.Mifid2ExecutionAlgo;
                DontUseAutoPriceForHedge = order.DontUseAutoPriceForHedge;
                AutoCancelDate = order.AutoCancelDate;
                FilledQuantity = order.FilledQuantity;
                RefFuturesConId = order.RefFuturesConId;
                AutoCancelParent = order.AutoCancelParent;
                Shareholder = order.Shareholder;
                ImbalanceOnly = order.ImbalanceOnly;
                RouteMarketableToBbo = order.RouteMarketableToBbo;
                ParentPermId = order.ParentPermId;
                UsePriceMgmtAlgo = order.UsePriceMgmtAlgo;
            }
            catch (Exception ex)
            {
                Logger.Error(Name, $"{ex}");
            }
        }

        public OrderOverride(Order order)
        {
            try
            {
                DateTime = DateTime.Now;
                LmtPrice = order.LmtPrice;
                AuxPrice = order.AuxPrice;
                ActiveStartTime = order.ActiveStartTime;
                ActiveStopTime = order.ActiveStopTime;
                OutsideRth = order.OutsideRth;
                OpenClose = order.OpenClose;
                Origin = order.Origin;
                Transmit = order.Transmit;
                DesignatedLocation = order.DesignatedLocation;
                ExemptCode = order.ExemptCode;
                MinQty = order.MinQty;
                PercentOffset = order.PercentOffset;
                NbboPriceCap = order.NbboPriceCap;
                OptOutSmartRouting = order.OptOutSmartRouting;
                StartingPrice = order.StartingPrice;
                StockRefPrice = order.StockRefPrice;
                Delta = order.Delta;
                StockRangeLower = order.StockRangeLower;
                StockRangeUpper = order.StockRangeUpper;
                Volatility = order.Volatility;
                VolatilityType = order.VolatilityType;
                DeltaNeutralOrderType = order.DeltaNeutralOrderType;
                DeltaNeutralAuxPrice = order.DeltaNeutralAuxPrice;
                DeltaNeutralConId = order.DeltaNeutralConId;
                DeltaNeutralSettlingFirm = order.DeltaNeutralSettlingFirm;
                DeltaNeutralClearingAccount = order.DeltaNeutralClearingAccount;
                DeltaNeutralClearingIntent = order.DeltaNeutralClearingIntent;
                DeltaNeutralOpenClose = order.DeltaNeutralOpenClose;
                DeltaNeutralShortSale = order.DeltaNeutralShortSale;
                DeltaNeutralShortSaleSlot = order.DeltaNeutralShortSaleSlot;
                DeltaNeutralDesignatedLocation = order.DeltaNeutralDesignatedLocation;
                ReferencePriceType = order.ReferencePriceType;
                TrailStopPrice = order.TrailStopPrice;
                TrailingPercent = order.TrailingPercent;
                BasisPoints = order.BasisPoints;
                BasisPointsType = order.BasisPointsType;
                ScaleInitLevelSize = order.ScaleInitLevelSize;
                ScaleSubsLevelSize = order.ScaleSubsLevelSize;
                ScalePriceIncrement = order.ScalePriceIncrement;
                ScalePriceAdjustValue = order.ScalePriceAdjustValue;
                ScalePriceAdjustInterval = order.ScalePriceAdjustInterval;
                ScaleProfitOffset = order.ScaleProfitOffset;
                ScaleAutoReset = order.ScaleAutoReset;
                ScaleInitPosition = order.ScaleInitPosition;
                ScaleInitFillQty = order.ScaleInitFillQty;
                ScaleRandomPercent = order.ScaleRandomPercent;
                ScaleTable = order.ScaleTable;
                WhatIf = order.WhatIf;
                NotHeld = order.NotHeld;
                Conditions = order.Conditions;
                TriggerPrice = order.TriggerPrice;
                LmtPriceOffset = order.LmtPriceOffset;
                AdjustedStopPrice = order.AdjustedStopPrice;
                AdjustedStopLimitPrice = order.AdjustedStopLimitPrice;
                AdjustedTrailingAmount = order.AdjustedTrailingAmount;
                ExtOperator = order.ExtOperator;
                Tier = order.Tier;
                CashQty = order.CashQty;
                Mifid2DecisionMaker = order.Mifid2DecisionMaker;
                Mifid2DecisionAlgo = order.Mifid2DecisionAlgo;
                Mifid2ExecutionTrader = order.Mifid2ExecutionTrader;
                Mifid2ExecutionAlgo = order.Mifid2ExecutionAlgo;
                DontUseAutoPriceForHedge = order.DontUseAutoPriceForHedge;
                AutoCancelDate = order.AutoCancelDate;
                FilledQuantity = order.FilledQuantity;
                RefFuturesConId = order.RefFuturesConId;
                AutoCancelParent = order.AutoCancelParent;
                Shareholder = order.Shareholder;
                ImbalanceOnly = order.ImbalanceOnly;
                RouteMarketableToBbo = order.RouteMarketableToBbo;
                ParentPermId = order.ParentPermId;
                UsePriceMgmtAlgo = order.UsePriceMgmtAlgo;
            }
            catch (Exception ex)
            {
                Logger.Error(Name, $"{ex}");
            }
        }
    }
}
