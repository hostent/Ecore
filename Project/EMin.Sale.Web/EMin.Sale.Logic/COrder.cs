using EMin.Sale.Model.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecore.Frame;
using EMin.Sale.Logic.DataAccess;

namespace EMin.Sale.Logic
{
    public class COrder : IOrder
    {
        public Result CreateTrade(TradeCreatePar par)
        {
            var tradeResponse = SaleDb.Get().Sale_Trade;
            var orderResponse = SaleDb.Get().Sale_Order;
            var tradeRecord = SaleDb.Get().Sale_TradeRecord;

            var trade = new EMin.Model.Collection.Sale_Trade()
            {
                BuyerId = par.MenberId,
                Comment = par.Comment,
                PayState = 10,

            };

            string tradeId = (string)tradeResponse.Add(trade);
            trade.Id = tradeId;

            foreach (var item in par.ItemParList)
            {
                var order = new EMin.Model.Collection.Sale_Order()
                {
                    ItemId = item.ItemId,
                    ActualAmount = item.Count * item.UnitPrice,
                    Count = item.Count,
                    UnitPrice = item.UnitPrice,
                    ItemSkuId = item.ItemSkuId,
                    OrderState = 10,
                    TotalAmount = item.Count * item.UnitPrice,
                    TradeId = tradeId
                };
                orderResponse.Add(order);
                trade.TotalAmount = trade.TotalAmount + order.TotalAmount;
            }

            trade.ActualAmount = trade.TotalAmount;

            tradeResponse.Update(trade);

            tradeRecord.Add(new EMin.Model.Collection.Sale_TradeRecord()
            {
                RecordLog = "下单成功，订单号【" + tradeId + "】"
            });

            return Result.Succeed();

        }
    }
}
