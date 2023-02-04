using Inventory.Grpc.Protos;

namespace Basket.API.GrpcServices
{
    public class StockItemGrpcService
    {
        private readonly StockProtoService.StockProtoServiceClient _serviceGrpcServiceClient;

        public StockItemGrpcService(StockProtoService.StockProtoServiceClient stockProtoServiceClient)
        {
            _serviceGrpcServiceClient = stockProtoServiceClient ?? throw new ArgumentNullException(nameof(stockProtoServiceClient));
        }

        public async Task<StockModel> GetStock(string itemNo)
        {
            try
            {
                var stockItemRequest = new GetStockRequest { ItemNo = itemNo };
                return await _serviceGrpcServiceClient.GetStockAsync(stockItemRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
