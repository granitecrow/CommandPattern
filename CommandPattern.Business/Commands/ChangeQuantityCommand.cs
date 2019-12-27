using CommandPattern.Business.Models;
using CommandPattern.Business.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPattern.Business.Commands
{
    public class ChangeQuantityCommand : ICommand
    {
        public enum Operation
        {
            Increase,
            Decrease
        }
        private readonly IShoppingCartRepository shoppingCartRepo;
        private readonly IProductRepository productRepo;
        private readonly Product product;
        private readonly Operation operation;


        public ChangeQuantityCommand(IShoppingCartRepository shoppingCartRepo, IProductRepository productRepo, Product product, Operation operation)
        {
            this.shoppingCartRepo = shoppingCartRepo;
            this.productRepo = productRepo;
            this.product = product;
            this.operation = operation;
        }

        public bool CanExecute()
        {
            switch (operation)
            {
                case Operation.Increase:
                    return (productRepo.GetStockFor(product.ArticleId) >= 1);
                case Operation.Decrease:
                    return shoppingCartRepo.Get(product.ArticleId).Quantity != 0;
            }
            return false;
        }

        public void Execute()
        {
            switch(operation)
            {
                case Operation.Increase:
                    productRepo.DecreaseStockBy(product.ArticleId, 1);
                    shoppingCartRepo.IncreaseQuantity(product.ArticleId);
                    break;
                case Operation.Decrease:
                    productRepo.IncreaseStockBy(product.ArticleId, 1);
                    shoppingCartRepo.DecreaseQuantity(product.ArticleId);
                    break;
            }
        }

        public void Undo()
        {
            switch (operation)
            {
                case Operation.Increase:
                    productRepo.IncreaseStockBy(product.ArticleId, 1);
                    shoppingCartRepo.DecreaseQuantity(product.ArticleId);
                    break;
                case Operation.Decrease:
                    productRepo.DecreaseStockBy(product.ArticleId, 1);
                    shoppingCartRepo.IncreaseQuantity(product.ArticleId);
                    break;
            }
        }
    }
}
