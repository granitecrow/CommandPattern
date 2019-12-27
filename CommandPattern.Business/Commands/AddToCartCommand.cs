using CommandPattern.Business.Models;
using CommandPattern.Business.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPattern.Business.Commands
{
    public class AddToCartCommand : ICommand
    {
        private readonly IShoppingCartRepository shoppingCartRepo;
        private readonly IProductRepository productRepo;
        private readonly Product product;

        public AddToCartCommand(IShoppingCartRepository shoppingCartRepo, IProductRepository productRepo, Product product)
        {
            this.shoppingCartRepo = shoppingCartRepo;
            this.productRepo = productRepo;
            this.product = product;
        }


        public bool CanExecute()
        {
            if (product == null) return false;

            return productRepo.GetStockFor(product.ArticleId) > 0;
        }

        public void Execute()
        {
            if (product == null) return;

            productRepo.DecreaseStockBy(product.ArticleId, 1);
            shoppingCartRepo.Add(product);
        }

        public void Undo()
        {
            if (product == null) return;
            
            var lineItem = shoppingCartRepo.Get(product.ArticleId);
            productRepo.IncreaseStockBy(product.ArticleId, lineItem.Quantity);
            shoppingCartRepo.RemoveAll(product.ArticleId);

        }
    }
}
