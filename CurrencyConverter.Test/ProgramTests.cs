namespace CurrencyConverter.Test;

public class CurrencyConverterTests
{
  private static decimal Convert(decimal amount, decimal rate) => amount * rate;

  [Test]
  public void Convert_PositiveAmountAndRate_ReturnsCorrectResult()
  {
    decimal result = Convert(100m, 1.08m);

    Assert.That(result, Is.EqualTo(108m));
  }

  [Test]
  public void Convert_ZeroAmount_ReturnsZero()
  {
    decimal result = Convert(0m, 1.08m);

    Assert.That(result, Is.EqualTo(0m));
  }

  [Test]
  public void Convert_AmountWithRateOne_ReturnsSameAmount()
  {
    decimal result = Convert(250m, 1m);

    Assert.That(result, Is.EqualTo(250m));
  }
}