using CommandPattern.Business.Commands;
using CommandPattern.Business.Repositories;
using System;

namespace CommandPattern
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var shoppingCartRepository = new ShoppingCartRepository();
            var productsRepository = new ProductsRepository();
            var product = productsRepository.FindBy("SM7B");
            
            var addSM7BToCartCommand = new AddToCartCommand(shoppingCartRepository, productsRepository, product);
            var increaseSM7Bcommand = new ChangeQuantityCommand(shoppingCartRepository, productsRepository, product, ChangeQuantityCommand.Operation.Increase);

            var manager = new CommandManager();
            manager.Invoke(addSM7BToCartCommand);
            manager.Invoke(increaseSM7Bcommand);
            manager.Invoke(increaseSM7Bcommand);
            manager.Invoke(increaseSM7Bcommand);
            manager.Invoke(increaseSM7Bcommand);


            //shoppingCartRepository.Add(product);
            //shoppingCartRepository.IncreaseQuantity(product.ArticleId);
            //shoppingCartRepository.IncreaseQuantity(product.ArticleId);
            //shoppingCartRepository.IncreaseQuantity(product.ArticleId);
            //shoppingCartRepository.IncreaseQuantity(product.ArticleId);

            PrintCart(shoppingCartRepository);

            manager.Undo();
            PrintCart(shoppingCartRepository);

        }

        static void PrintCart(ShoppingCartRepository shoppingCartRepository)
        {
            var totalPrice = 0m;
            foreach (var lineItem in shoppingCartRepository.LineItems)
            {
                var price = lineItem.Value.Product.Price * lineItem.Value.Quantity;

                Console.WriteLine($"{lineItem.Key} " +
                    $"${lineItem.Value.Product.Price} x {lineItem.Value.Quantity} = ${price}");

                totalPrice += price;
            }

            Console.WriteLine($"Total price:\t${totalPrice}");
        }
    }
}
