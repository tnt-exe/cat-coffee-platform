namespace DAO.Helper;

public interface IValidateGet
{
    void ValidateGetRequest(ref int startPage, ref int endPage, int? quantity, ref int quantityResult);
}

public class ValidateGet : IValidateGet
{
    private readonly CheckQuantityTaken _checkQuantityTaken = new CheckQuantityTaken();

    public void ValidateGetRequest(ref int startPage, ref int endPage, int? quantity, ref int quantityResult)
    {
        quantityResult = _checkQuantityTaken.CheckQuantity(quantity);

        if (startPage <= 0)
        {
            startPage = 1;
        }

        if (endPage <= 0)
        {
            endPage = 1;
        }

        return;
    }
}

internal class CheckQuantityTaken
{
    private static readonly int MAX_QUANTITY = 50;
    private static readonly int MIN_QUANTITY = 10;
    private static readonly int QUANTITY_PER_PAGE = 20;
    public int CheckQuantity(int? quantity)
    {
        if (quantity is null)
        {
            return QUANTITY_PER_PAGE;
        }

        if (quantity > MAX_QUANTITY)
        {
            return MAX_QUANTITY;
        }

        if (quantity < MIN_QUANTITY)
        {
            return MIN_QUANTITY;
        }

        return quantity ?? QUANTITY_PER_PAGE;
    }

    public int QuantityPerPage
    {
        get => QUANTITY_PER_PAGE;
    }
}
