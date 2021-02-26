using IBApi;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.Util;

namespace TradingBotCS.IBApi_OverRide
{
    public class OrderOverride : Order
    {
        private static string Name = "OrderOverride";

        public ObjectId _id { get; set; }

        public DateTime DateTime { get; set; }
        public Contract Contract { get; set; }

        public OrderOverride()
        {
            Order order = new Order();
            try
            {
                _id = new ObjectId();
                DateTime = DateTime.Now;
                Contract = new Contract();
                OrderId = order.OrderId;
                ClientId = order.ClientId;
                PermId = order.PermId;
                TotalQuantity = order.TotalQuantity;
                OcaType = order.OcaType;
                ParentId = order.ParentId;
                DisplaySize = order.DisplaySize;
                TriggerMethod = order.TriggerMethod;
                ShortSaleSlot = order.ShortSaleSlot;
                DiscretionaryAmt = order.DiscretionaryAmt;
                AuctionStrategy = order.AuctionStrategy;
                ContinuousUpdate = order.ContinuousUpdate;
                ReferenceContractId = order.ReferenceContractId;
                PeggedChangeAmount = order.PeggedChangeAmount;
                ReferenceChangeAmount = order.ReferenceChangeAmount;
                AdjustableTrailingUnit = order.AdjustableTrailingUnit;

                Action = order.Action;
                OrderType = order.OrderType;
                Tif = order.Tif;
                OcaGroup = order.OcaGroup;
                OrderRef = order.OrderRef;
                GoodAfterTime = order.GoodAfterTime;
                GoodTillDate = order.GoodTillDate;
                Rule80A = order.Rule80A;
                FaGroup = order.FaGroup;
                FaProfile = order.FaProfile;
                FaMethod = order.FaMethod;
                FaPercentage = order.FaPercentage;
                OpenClose = order.OpenClose;
                DesignatedLocation = order.DesignatedLocation;
                DeltaNeutralSettlingFirm = order.DeltaNeutralSettlingFirm;
                DeltaNeutralClearingAccount = order.DeltaNeutralClearingAccount;
                DeltaNeutralClearingIntent = order.DeltaNeutralClearingIntent;
                DeltaNeutralDesignatedLocation = order.DeltaNeutralDesignatedLocation;
                HedgeType = order.HedgeType;
                HedgeParam = order.HedgeParam;
                Account = order.Account;
                SettlingFirm = order.SettlingFirm;
                ClearingAccount = order.ClearingAccount;
                ClearingIntent = order.ClearingIntent;
                AlgoStrategy = order.AlgoStrategy;
                AlgoParams = order.AlgoParams;
                AlgoId = order.AlgoId;
                SmartComboRoutingParams = order.SmartComboRoutingParams;
                OrderComboLegs = order.OrderComboLegs;
                OrderMiscOptions = order.OrderMiscOptions;
                ModelCode = order.ModelCode;
                ReferenceExchange = order.ReferenceExchange;
                AdjustedOrderType = order.AdjustedOrderType;
                Conditions = order.Conditions;
                Tier = order.Tier;

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
                _id = new ObjectId();
                DateTime = DateTime.Now;
                Contract = new Contract();
                OrderId = order.OrderId;
                ClientId = order.ClientId;
                PermId = order.PermId;
                TotalQuantity = order.TotalQuantity;
                OcaType = order.OcaType;
                ParentId = order.ParentId;
                DisplaySize = order.DisplaySize;
                TriggerMethod = order.TriggerMethod;
                ShortSaleSlot = order.ShortSaleSlot;
                DiscretionaryAmt = order.DiscretionaryAmt;
                AuctionStrategy = order.AuctionStrategy;
                ContinuousUpdate = order.ContinuousUpdate;
                ReferenceContractId = order.ReferenceContractId;
                PeggedChangeAmount = order.PeggedChangeAmount;
                ReferenceChangeAmount = order.ReferenceChangeAmount;
                AdjustableTrailingUnit = order.AdjustableTrailingUnit;

                Action = order.Action;
                OrderType = order.OrderType;
                Tif = order.Tif;
                OcaGroup = order.OcaGroup;
                OrderRef = order.OrderRef;
                GoodAfterTime = order.GoodAfterTime;
                GoodTillDate = order.GoodTillDate;
                Rule80A = order.Rule80A;
                FaGroup = order.FaGroup;
                FaProfile = order.FaProfile;
                FaMethod = order.FaMethod;
                FaPercentage = order.FaPercentage;
                OpenClose = order.OpenClose;
                DesignatedLocation = order.DesignatedLocation;
                DeltaNeutralSettlingFirm = order.DeltaNeutralSettlingFirm;
                DeltaNeutralClearingAccount = order.DeltaNeutralClearingAccount;
                DeltaNeutralClearingIntent = order.DeltaNeutralClearingIntent;
                DeltaNeutralDesignatedLocation = order.DeltaNeutralDesignatedLocation;
                HedgeType = order.HedgeType;
                HedgeParam = order.HedgeParam;
                Account = order.Account;
                SettlingFirm = order.SettlingFirm;
                ClearingAccount = order.ClearingAccount;
                ClearingIntent = order.ClearingIntent;
                AlgoStrategy = order.AlgoStrategy;
                AlgoParams = order.AlgoParams;
                AlgoId = order.AlgoId;
                SmartComboRoutingParams = order.SmartComboRoutingParams;
                OrderComboLegs = order.OrderComboLegs;
                OrderMiscOptions = order.OrderMiscOptions;
                ModelCode = order.ModelCode;
                ReferenceExchange = order.ReferenceExchange;
                AdjustedOrderType = order.AdjustedOrderType;
                Conditions = order.Conditions;
                Tier = order.Tier;

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

        public OrderOverride(Order order, Contract contract)
        {
            try
            {
                _id = new ObjectId();
                DateTime = DateTime.Now;
                OrderId = order.OrderId;
                ClientId = order.ClientId;
                PermId = order.PermId;
                TotalQuantity = order.TotalQuantity;
                OcaType = order.OcaType;
                ParentId = order.ParentId;
                DisplaySize = order.DisplaySize;
                TriggerMethod = order.TriggerMethod;
                ShortSaleSlot = order.ShortSaleSlot;
                DiscretionaryAmt = order.DiscretionaryAmt;
                AuctionStrategy = order.AuctionStrategy;
                ContinuousUpdate = order.ContinuousUpdate;
                ReferenceContractId = order.ReferenceContractId;
                PeggedChangeAmount = order.PeggedChangeAmount;
                ReferenceChangeAmount = order.ReferenceChangeAmount;
                AdjustableTrailingUnit = order.AdjustableTrailingUnit;

                Action = order.Action;
                OrderType = order.OrderType;
                Tif = order.Tif;
                OcaGroup = order.OcaGroup;
                OrderRef = order.OrderRef;
                GoodAfterTime = order.GoodAfterTime;
                GoodTillDate = order.GoodTillDate;
                Rule80A = order.Rule80A;
                FaGroup = order.FaGroup;
                FaProfile = order.FaProfile;
                FaMethod = order.FaMethod;
                FaPercentage = order.FaPercentage;
                OpenClose = order.OpenClose;
                DesignatedLocation = order.DesignatedLocation;
                DeltaNeutralSettlingFirm = order.DeltaNeutralSettlingFirm;
                DeltaNeutralClearingAccount = order.DeltaNeutralClearingAccount;
                DeltaNeutralClearingIntent = order.DeltaNeutralClearingIntent;
                DeltaNeutralDesignatedLocation = order.DeltaNeutralDesignatedLocation;
                HedgeType = order.HedgeType;
                HedgeParam = order.HedgeParam;
                Account = order.Account;
                SettlingFirm = order.SettlingFirm;
                ClearingAccount = order.ClearingAccount;
                ClearingIntent = order.ClearingIntent;
                AlgoStrategy = order.AlgoStrategy;
                AlgoParams = order.AlgoParams;
                AlgoId = order.AlgoId;
                SmartComboRoutingParams = order.SmartComboRoutingParams;
                OrderComboLegs = order.OrderComboLegs;
                OrderMiscOptions = order.OrderMiscOptions;
                ModelCode = order.ModelCode;
                ReferenceExchange = order.ReferenceExchange;
                AdjustedOrderType = order.AdjustedOrderType;
                Conditions = order.Conditions;

                Contract = contract;
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
